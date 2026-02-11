using MauiBankApp.Services.Interfaces;
using MauiBankApp.ViewModels;

namespace MauiBankApp.Views;

public partial class SecuritySettingsPage : ContentPage
{
    private readonly SecuritySettingsViewModel _viewModel;

    // Primary constructor with dependency injection (recommended)
    public SecuritySettingsPage(SecuritySettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Parameterless constructor for backward compatibility with NavigationService
    // This will be used if NavigationService creates page without ViewModel
    public SecuritySettingsPage() : this(CreateDefaultViewModel())
    {
    }

    // Helper method to create ViewModel when using parameterless constructor
    private static SecuritySettingsViewModel CreateDefaultViewModel()
    {
        // Get the service from the app's service provider
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
        // Execute the toggle command when switch is toggled, passing the new value
        if (_viewModel.ToggleBiometricCommand.CanExecute(e.Value))
        {
            await _viewModel.ToggleBiometricCommand.ExecuteAsync(e.Value);
        }
    }
}