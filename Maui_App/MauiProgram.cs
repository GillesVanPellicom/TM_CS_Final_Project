using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Logging;
using Maui_App.Views;
using Maui_App.ViewModels;
using Maui_App.Services;

namespace Maui_App;

public static class MauiProgram {
  public static MauiApp CreateMauiApp() {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
      .ConfigureFonts(fonts => {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
      });

    builder.Services.AddSingleton<NotesViewModel>();
    builder.Services.AddSingleton<NotesPage>();

    builder.Services.AddTransient<AddEditNoteViewModel>();
    builder.Services.AddTransient<AddEditNotePage>();

    // Platform-aware API base URL provider
    builder.Services.AddSingleton<Func<HttpClient>>(() => {
      var platformName = DeviceInfo.Platform.ToString();
      var baseUrl = platformName switch {
        "Android" => "http://10.0.2.2:8080", // Android emulator host alias for localhost
        "WinUI" => "http://localhost:8080",
        "MacCatalyst" => "http://localhost:8080",
        "iOS" => "http://localhost:8080",
        _ => "http://localhost:8080"
      };
      return new HttpClient { BaseAddress = new Uri(baseUrl) };
    });

    builder.Services.AddSingleton<INotesApiService>(sp => {
      var httpFactory = sp.GetRequiredService<Func<HttpClient>>();
      return new NotesApiService(httpFactory());
    });

    Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
    Routing.RegisterRoute(nameof(AddEditNotePage), typeof(AddEditNotePage));


// #if DEBUG
//     builder.Logging.AddDebug();
// #endif

    return builder.Build();
  }
}