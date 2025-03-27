using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyochulLab.Core.Results
{
    public class Result : IResult
    {
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; } = string.Empty;

        public static Result Success(string message = "성공") =>
            new Result { IsSuccess = true, Message = message };

        public static Result Fail(string message = "실패") =>
            new Result { IsSuccess = false, Message = message };
    }
}
