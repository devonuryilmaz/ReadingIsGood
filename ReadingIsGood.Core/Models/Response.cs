using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models
{
    public class Response
    {
        public Response(bool success)
        {
            Success = success;
            Errors = new List<Error>();
        }

        public bool Success { get; set; } = true;
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string ErrorMessage { get; set; }
    }
}
