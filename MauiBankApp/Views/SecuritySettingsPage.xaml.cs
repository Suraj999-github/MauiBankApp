using MauiBankApp.Services.Interfaces;
using MauiBankApp.ViewModels;

namespace MauiBankApp.Views;

public partial class SecuritySettingsPage : ContentPage
{
    private readonly SecuritySettingsViewModel _viewModel;

    public SecuritySettingsPage(SecuritySettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to ViewModel events for scanner animations
        _viewModel.ScanStarted += OnScanStarted;
        _viewModel.ScanCompleted += OnScanCompleted;
    }

    public SecuritySettingsPage() : this(CreateDefaultViewModel())
    {
    }

    private static SecuritySettingsViewModel CreateDefaultViewModel()
    {
        var serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
        if (serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider not available");
        }

        var biometricService = serviceProvider.GetService(typeof(IBiometricAuthService)) as IBiometricAuthService;
        if (biometricService == null)
        {
            throw new InvalidOperationException("IBiometricAuthService not registered in DI container");
        }

        var authService = serviceProvider.GetService(typeof(IAuthService)) as IAuthService;
        if (authService == null)
        {
            throw new InvalidOperationException("IAuthService not registered in DI container");
        }

        return new SecuritySettingsViewModel(biometricService, authService);
    }

    private async void OnBiometricToggled(object sender, ToggledEventArgs e)
    {
        if (_viewModel.ToggleBiometricCommand.CanExecute(e.Value))
        {
            // If enabling, show scanning animation
            if (e.Value)
            {
                await FingerprintScanner.ResetAsync();
                FingerprintScanner.IsScanning = true;
            }

            await _viewModel.ToggleBiometricCommand.ExecuteAsync(e.Value);
        }
    }

    private void OnScanStarted(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await FingerprintScanner.ResetAsync();
            FingerprintScanner.IsScanning = true;
        });
    }

    private void OnScanCompleted(object? sender, (bool success, string message) result)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (result.success)
            {
                await FingerprintScanner.ShowSuccessAsync();
            }
            else
            {
                await FingerprintScanner.ShowErrorAsync(result.message);
            }

            // Reset scanner after a delay
            await Task.Delay(2000);
            await FingerprintScanner.ResetAsync();
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Unsubscribe from events
        _viewModel.ScanStarted -= OnScanStarted;
        _viewModel.ScanCompleted -= OnScanCompleted;
    }
}
