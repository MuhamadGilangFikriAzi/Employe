using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EmployeAPI.Models
{
    public class ApiMessage<TApiMessage> : ApiMessageAbstract
    {
        private const int DefaultStatusCode = StatusCodes.Status200OK;
        public ApiMessage(TApiMessage value)
        {
            Data = value;
            IsSucces = true;
        }

        public ApiMessage()
        {
            IsSucces = true;
        }

        public TApiMessage? Data { get; set; }
        public bool IsSucces { get; set; }

    }
}
