using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Filters;

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult
            && EsRespuestaExitosa(objectResult)
            && !YaEsApiResponse(objectResult.Value)
            && objectResult.Value is not ProblemDetails)
        {
            objectResult.Value = new ApiResponse<object>
            {
                Success = true,
                Message = "Operacion exitosa",
                Data = objectResult.Value
            };
        }

        await next();
    }

    private static bool EsRespuestaExitosa(ObjectResult objectResult)
    {
        int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
        return statusCode >= StatusCodes.Status200OK
            && statusCode < StatusCodes.Status300MultipleChoices;
    }

    private static bool YaEsApiResponse(object? value)
    {
        if (value is null)
            return false;

        Type type = value.GetType();

        return type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(ApiResponse<>);
    }
}
