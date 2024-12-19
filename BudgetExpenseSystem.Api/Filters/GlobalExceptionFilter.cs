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
				errorModel = new ErrorModel
				{
					StatusCode = StatusCodes.Status404NotFound,
					Message = notFoundException.Message
				};
				context.Result = new JsonResult(errorModel)
				{
					StatusCode = StatusCodes.Status404NotFound
				};
				break;
			case BadRequestException notActiveException:
				errorModel = new ErrorModel
				{
					StatusCode = StatusCodes.Status409Conflict,
					Message = notActiveException.Message,
					Details = context.Exception.StackTrace
				};
				context.Result = new JsonResult(errorModel)
				{
					StatusCode = StatusCodes.Status409Conflict
				};
				break;
			case InsufficientFundsException insufficientFundsException:
				errorModel = new ErrorModel
				{
					StatusCode = StatusCodes.Status400BadRequest,
					Message = insufficientFundsException.Message,
					Details = context.Exception.StackTrace
				};
				context.Result = new JsonResult(errorModel)
				{
					StatusCode = StatusCodes.Status400BadRequest
				};
				break;
			default:
				errorModel = new ErrorModel
				{
					StatusCode = StatusCodes.Status500InternalServerError,
					Message = context.Exception.Message,
					Details = context.Exception.StackTrace
				};
				context.Result = new JsonResult(errorModel)
				{
					StatusCode = StatusCodes.Status500InternalServerError
				};
				break;
		}
	}
}
