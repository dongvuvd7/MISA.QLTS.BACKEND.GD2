using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Exceptions
{
    public class HttpResponseExceptionFilter : Microsoft.AspNetCore.Mvc.Filters.IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is MISAValidateException validateException)
                {
                    context.Result = new ObjectResult(validateException.Data)
                    {
                        StatusCode = 400
                    };
                    context.ExceptionHandled = true;
                }
                else
                {
                    var result = new
                    {
                        devMsg = context.Exception.Message,
                        userMsg = Properties.Resources.ExceptionMsg
                    };
                    context.Result = new ObjectResult(result)
                    {
                        StatusCode = 500
                    };
                    context.ExceptionHandled = true;
                }
            }
        }
    }
}
