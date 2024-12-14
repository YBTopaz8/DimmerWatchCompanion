using CommunityToolkit.Maui;
using DevExpress.Maui;
using Microsoft.Extensions.Logging;

namespace DimmerWatchCompanion;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
        .UseDevExpress(useLocalization: false)
        .UseDevExpressCollectionView()
        .UseDevExpressControls()
        .UseMauiCommunityToolkit(options =>
        {
            options.SetShouldSuppressExceptionsInAnimations(true);
            options.SetShouldSuppressExceptionsInBehaviors(true);
            options.SetShouldSuppressExceptionsInConverters(true);

        });
#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<MainPageVM>();
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}
