
namespace Pattern_of_life
{
    public static class ExceptionHandler
    {
        public static string GetErrorMessage(Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += " Inner Exception: " + GetErrorMessage(ex.InnerException);
            
            return errorMessage;
        }
    }
}
