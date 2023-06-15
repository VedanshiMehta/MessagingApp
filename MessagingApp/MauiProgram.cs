
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using CommunityToolkit.Maui;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Android.Content.Res;
#endif

namespace MessagingApp;

public static class MauiProgram
{
    [Obsolete]
    public static MauiApp CreateMauiApp()
	{


        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCompatibility()
            .ConfigureMauiHandlers(handlers => {


                Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(PrefixEntry), ((handler, view) =>
                {
#if ANDROID
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());

#endif
                }));
                Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", ((handler, view) =>
                {
#if ANDROID
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());

#endif
                }));
                Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("NoUnderline", ((handler, view) =>
                {
#if ANDROID
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());

#endif
                }));

            });
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", ((handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());

#endif
        }));



#if ANDROID
        builder.Services.AddSingleton<IAuthenticationServices, AuthenticationService>();
        builder.Services.AddSingleton<WelcomeScreen>();
#endif



#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        return builder.Build();
        
    }
}
