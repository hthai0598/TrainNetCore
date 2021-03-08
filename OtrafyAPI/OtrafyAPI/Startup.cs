using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BLL.JwtHelpers;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OtrafyAPI.Persistence;
using Swashbuckle.AspNetCore.Swagger;
using DAL;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OtrafyAPI.Helpers;
using BLL.Services.Interfaces;
using BLL.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using BLL.Helpers;
using DAL.Core.Settings;
namespace OtrafyAPI
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();
            Configuration = builder.Build();
            _hostingEnvironment = env;
            
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = Configuration.GetSection("Authentication:Issuer").Value,
                            ValidAudience = Configuration.GetSection("Authentication:Audience").Value,
                            IssuerSigningKey = JwtSecurityKey.Create(Configuration.GetSection("Authentication:SecurityKey").Value)
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Member",
                    policy => policy.RequireClaim("MembershipId"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // The port to use for https redirection in production
            if (!_hostingEnvironment.IsDevelopment() && !string.IsNullOrWhiteSpace(Configuration["HttpsRedirectionPort"]))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = int.Parse(Configuration["HttpsRedirectionPort"]);
                });
            }

            MongoDbPersistence.Configure();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Otrafy - API",
                    Description = "Swagger surface",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Otrafy", Email = "otrafy@gmail.com", Url = "https://www.otrafy.com" }
                });

                // XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                s.OperationFilter<AuthorizeCheckOperationFilter>();
                s.OperationFilter<FileUploadOperation>();
                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Please Enter Authentication Token",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

            });
            RegisterServices(services);

        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                context.ApiDescription.TryGetMethodInfo(out var methodInfo);

                if (methodInfo == null)
                    return;

                var hasAuthorizeAttribute = false;

                if (methodInfo.MemberType == MemberTypes.Method)
                {
                    // NOTE: Check the controller itself has Authorize attribute
                    hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                    // NOTE: Controller has Authorize attribute, so check the endpoint itself.
                    //       Take into account the allow anonymous attribute
                    if (hasAuthorizeAttribute)
                        hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                    else
                        hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
                }

                if (!hasAuthorizeAttribute)
                    return;

                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new Swashbuckle.AspNetCore.Swagger.Response { Description = "Unauthorized" });
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new Swashbuckle.AspNetCore.Swagger.Response { Description = "Forbidden" });
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //if (!env.IsDevelopment())
            //{
            //    app.UseHttpsRedirection();
            //}

            app.UseCors("CorsPolicy");
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Otrafy API v1.0");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {

            // Configurations
            services.Configure<LocalSettings>(Configuration);
            // Business Services
            services.AddScoped<ISendGridSender, SendGridSender>();
            services.AddScoped<IEmailTemplates, EmailTemplates>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJWTTokenServices, JWTTokenServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IRefreshTokenServices, RefreshTokenServices>();
            services.AddScoped<ICompanyServices, CompanyServices>();
            services.AddScoped<IBuyersServices, BuyersServices>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<ISuppliersServices, SuppliersServices>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

    }
}
