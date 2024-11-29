using BudgetExpenseSystem.Domain.Exceptions;
using BudgetExpenseSystem.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BudgetExpenseSystem.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ErrorModel errorModel;

        switch (context.Exception)
        {
            case NotFoundException notFoundException:
                errorModel = new ErrorModel(404, notFoundException.Message);
                context.Result = new JsonResult(errorModel)
                {
                    StatusCode = 404
                };
                break;

            default:
                errorModel = new ErrorModel(
                    StatusCodes.Status500InternalServerError, 
                    context.Exception.Message, 
                    context.Exception.StackTrace);
                context.Result = new JsonResult(errorModel)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
        }
    }
}