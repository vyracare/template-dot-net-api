using Microsoft.AspNetCore.Mvc;
using [assembly-generic].Common.Results;

namespace [assembly-generic].Common.Http;

public static class ControllerBaseExtensions
{
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        UseCaseResult<T> result,
        Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value!);
        }

        var payload = new { message = result.Message };

        return result.ErrorType switch
        {
            UseCaseErrorType.Validation => controller.BadRequest(payload),
            UseCaseErrorType.NotFound => controller.NotFound(payload),
            _ => controller.BadRequest(payload)
        };
    }
}
