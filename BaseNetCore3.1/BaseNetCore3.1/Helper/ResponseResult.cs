using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BaseNetCore3._1.Helper
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

    public class StatusResponse
    {
        public StatusResponse()
        {

        }
        public StatusResponse(int _code, string _mes)
        {
            this.StatusCode = _code;
            this.Message = _mes;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class StatusInvalidParamsResponse : StatusResponse
    {
        public StatusInvalidParamsResponse() { }
        public StatusInvalidParamsResponse(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary _modelState, string _message = "Params Invalid")
        {
            Dictionary<string, List<string>> errorList = _modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => (string.IsNullOrWhiteSpace(e.ErrorMessage) || string.IsNullOrEmpty(e.ErrorMessage)) ? e.Exception.InnerException.Message : e.ErrorMessage).ToList()
                );

            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.Message = _message;
            this.Description = errorList;
        }
        public Dictionary<string, List<string>> Description { get; set; }
    }
}
