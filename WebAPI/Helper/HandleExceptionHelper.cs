using Microsoft.AspNetCore.Mvc;

namespace Application.Helper
{
    public class HandleExceptionHelper : ControllerBase
    {
        public IActionResult HandleException(Exception ex)
        {
            string errorMessage = $"An error occurred while processing your request: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
            }
            return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
        }

    }
}
