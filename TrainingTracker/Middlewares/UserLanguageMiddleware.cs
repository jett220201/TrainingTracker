using System.Globalization;

namespace TrainingTracker.API.Middlewares
{
    public class UserLanguageMiddleware
    {
        private readonly RequestDelegate _next;
        public UserLanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Default culture to use if no language is specified
            var defaultCulture = new CultureInfo("en-US");
            // Set the default culture for the application
            var culture = defaultCulture;
            // Check if the Accept-Language header is present
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var langClaim = context.User.FindFirst("lang")?.Value;
                if (!string.IsNullOrEmpty(langClaim))
                {
                    try
                    {
                        culture = new CultureInfo(langClaim);
                    }
                    catch(CultureNotFoundException)
                    {
                        culture = defaultCulture; // Fallback to default if the culture is not found
                    }
                }
            }

            // Set the current culture and UI culture
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
