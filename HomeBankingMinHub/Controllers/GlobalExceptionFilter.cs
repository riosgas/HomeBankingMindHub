using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static HomeBankingMindHub.Services.ServiceExceptions;

namespace HomeBankingMindHub.Controllers
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is MaxAccountsException 
                or ClientNotFoundException 
                or InvalidRequest 
                or EmailAlreadyUsed
            )
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = 403,
                    DeclaredType = typeof(string)
                };
            }
            else
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(string)
                };
            }
            //switch (context.Exception)
            //{
            //    case MaxAccountsException ex:
            //        context.Result = new ObjectResult(ex.Message)
            //        {
            //            StatusCode = 403,
            //            DeclaredType = typeof(string)
            //        };
            //        break;

            //    case ClientNotFoundException ex:
            //        context.Result = new ObjectResult(ex.Message)
            //        {
            //            StatusCode = 403,
            //            DeclaredType = typeof(string)
            //        };
            //        break;
            //    case InvalidRequest ex:
            //        context.Result = new ObjectResult(ex.Message)
            //        {
            //            StatusCode = 403,
            //            DeclaredType = typeof(string)
            //        };
            //        break;

            //    case EmailAlreadyUsed ex:
            //        context.Result = new ObjectResult(ex.Message)
            //        {
            //            StatusCode = 403,
            //            DeclaredType = typeof(string)
            //        };
            //        break;

            //    default:
            //        context.Result = new ObjectResult(context.Exception.Message)
            //        {
            //            StatusCode = 500,
            //            DeclaredType = typeof(string)
            //        };
            //        break;
            //}
        }
    }
}
