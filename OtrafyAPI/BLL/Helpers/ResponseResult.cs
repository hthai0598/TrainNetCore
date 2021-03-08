using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Net;
using BLL.Models;

namespace BLL.Helpers
{
    public class ResponseResult : ObjectResult
    {
        public ResponseResult(HttpStatusCode statusCode, object value = null) : base(value)
        {
            base.StatusCode = (int)statusCode;
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        if (value == null)
                        {
                            base.Value = new OkResponse(value);
                        }
                        else
                        {
                            if (value.GetType().Name.ToLower() == "string")
                            {
                                base.Value = new StatusResponse((int)statusCode, value.ToString());
                            }
                            else
                            {
                                base.Value = new OkResponse(value);
                            }
                        }
                        break;
                    }

                case HttpStatusCode.BadRequest:
                    {
                        if (value == null)
                        {
                            base.Value = new OkResponse(value);
                        }
                        else
                        {
                            if (value.GetType().Name.ToLower() == "string")
                            {
                                base.Value = new StatusResponse((int)statusCode, value.ToString());
                            }
                            else
                            {
                                base.Value = new StatusInvalidParamsResponse((ModelStateDictionary)value);
                            }
                        }
                        break;
                    }

                case HttpStatusCode.InternalServerError:
                    {
                        if (value == null)
                        {
                            base.Value = new OkResponse(value);
                        }
                        else
                        {
                            if (value.GetType().Name.ToLower() == "string")
                            {
                                base.Value = new StatusResponse((int)statusCode, value.ToString());
                            }
                            else
                            {
                                base.Value = new StatusErrorResponse((int)statusCode, "Internal Server Error", (Exception)value);
                            }
                        }
                        break;
                    }

                case HttpStatusCode.Unauthorized:
                    {
                        base.Value = new StatusResponse((int)statusCode, "Unauthorized");
                        break;
                    }

                default:
                    if (value == null)
                    {
                        base.Value = new OkResponse(value);
                    }
                    else
                    {
                        if (value.GetType().Name.ToLower() == "string")
                        {
                            base.Value = new StatusResponse((int)statusCode, value.ToString());
                        }
                        else
                        {
                            base.Value = value;
                        }
                    }
                    break;
            }
        }
    }

    public class OkResponse : Response
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; }

        public OkResponse(object result)
            : base((int)HttpStatusCode.OK)
        {
            Result = result;
        }
    }

    public class Response
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public Response(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    return "Unauthorized";
                case (int)HttpStatusCode.NotFound:
                    return "Resource not found";
                case (int)HttpStatusCode.InternalServerError:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }
    }

    public class StatusErrorResponse
    {
        public StatusErrorResponse(int _code, string _mes, Exception _ex)
        {
            this.StatusCode = _code;
            this.Message = _mes;
            this.Description = _ex.Message;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}