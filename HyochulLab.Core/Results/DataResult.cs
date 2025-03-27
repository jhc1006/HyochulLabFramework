using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyochulLab.Core.Results
{
    public class DataResult<T> : Result
    {
        public T Data { get; private set; } = default!;

        public static DataResult<T> Success(T data, string message = "성공") =>
            new DataResult<T> { IsSuccess = true, Message = message, Data = data };

        public static new DataResult<T> Fail(string message = "실패") =>
            new DataResult<T> { IsSuccess = false, Message = message };
    }
}
