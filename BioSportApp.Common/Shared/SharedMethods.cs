namespace BioSportApp.Common.Shared
{
    public class SharedMethods
    {
        public static TService? GetService<TService>() where TService : class
        {
            var app = Application.Current;

            if (app?.Handler?.MauiContext != null)
            {
                var service = app.Handler.MauiContext.Services.GetService(typeof(TService)) as TService;
                return service;
            }

            return null;
        }
    }
}
