using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class ServicesResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        private ServicesResult(bool success, string message , T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ServicesResult<T> Successed(T data, string message = "")
        {
            return new ServicesResult<T>(true, message, data);
        }

        public static ServicesResult<T> Failure(string message)
        {
            return new ServicesResult<T>(false, message, default);
        }
    }
}
