using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QRStudio.Application.Abstractions;
using QRStudio.Application.Services;
using QRStudio.Infrastructure.Generation;
using QRStudio.Infrastructure.Persistence;
using QRStudio.Infrastructure.System;
using QRStudio.Presentation.Services;
using QRStudio.Presentation.ViewModels;

namespace QRStudio.Presentation;

public partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IClock, SystemClock>();
                services.AddSingleton<IAppDataPathProvider, AppDataPathProvider>();
                services.AddSingleton<IQrCodeImageGenerator, QrCodeImageGenerator>();
                services.AddSingleton<IQrHistoryRepository, JsonQrHistoryRepository>();
                services.AddSingleton<IFileExportService, PhysicalFileExportService>();

                services.AddSingleton<QrStudioService>();

                services.AddSingleton<IFileDialogService, FileDialogService>();
                services.AddSingleton<IUserDialogService, UserDialogService>();
                services.AddSingleton<IClipboardService, ClipboardService>();

                services.AddSingleton<CreateQrViewModel>();
                services.AddSingleton<HistoryViewModel>();
                services.AddSingleton<AboutViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync(TimeSpan.FromSeconds(5));
        _host.Dispose();
        base.OnExit(e);
    }
}
