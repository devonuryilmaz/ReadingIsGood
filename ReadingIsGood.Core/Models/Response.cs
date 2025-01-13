using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models
{
    public class Response
    {
        public Response(bool success = true)
        {
            Success = success;
            Errors = new List<Error>();
        }

        public bool Success { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Response<T>: Response
    {
        [JsonConstructor]
        public Response()
        {
        }

        public Response(T t)
        {
            Data = t;
        }
        
        public Response(T t, bool success = true)
        {
            Data = t;
            Success = success;
        }

        public T Data { get; set; }
    }

    public class Error
    {
        public string ErrorMessage { get; set; }
    }
}
