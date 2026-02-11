using MauiBankApp.ViewModels;

namespace MauiBankApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to biometric login events
        _viewModel.BiometricScanStarted += OnBiometricScanStarted;
        _viewModel.BiometricScanCompleted += OnBiometricScanCompleted;
    }

    private void OnBiometricScanStarted(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await LoginFingerprintScanner.ResetAsync();
            LoginFingerprintScanner.IsScanning = true;
        });
    }

    private void OnBiometricScanCompleted(object? sender, (bool success, string message) result)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (result.success)
            {
                await LoginFingerprintScanner.ShowSuccessAsync();
                // Navigation will happen automatically, no need to reset
            }
            else
            {
                await LoginFingerprintScanner.ShowErrorAsync(result.message);
                await Task.Delay(2000);
                await LoginFingerprintScanner.ResetAsync();
            }
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Unsubscribe from events
        _viewModel.BiometricScanStarted -= OnBiometricScanStarted;
        _viewModel.BiometricScanCompleted -= OnBiometricScanCompleted;
    }
}
