using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Utils
{
    public class JsonSerialize
    {
        private static string Time => DateTime.Now.ToString("G");
        private static bool Success(object data) => data is not null;

        public static object MessageText(string message) => new
        {
            message,
            success = Success(message),
            Time,
        };

        public static object ErrorMessageText(string message) => new
        {
            errors = new
            {
                message,
            },
            success = false,
            Time,
        };
        public static object Data(object data, bool _success = true) => new
        {
            data,
            success = Success(data) && _success,
            Time,
        };

        public static object Data(object data, string message, bool _success = true) => new
        {
            data,
            message,
            success = Success(data) && _success,
            Time,
        };

        public static IActionResult ResponseTemplate(Func<IActionResult> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception e)
            {
                return new JsonResult(ErrorMessageText(e.Message))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
    }
}
