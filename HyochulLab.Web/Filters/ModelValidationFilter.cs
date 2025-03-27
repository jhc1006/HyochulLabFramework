using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HyochulLab.Core.Results;
using System.Web.Http.Results;

namespace HyochulLab.Web.Filters;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorMessages = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            var result = Result.Fail(string.Join(" | ", errorMessages));
            context.Result = new JsonResult(result) { StatusCode = 400 };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
