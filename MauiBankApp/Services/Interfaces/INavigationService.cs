namespace MauiBankApp.Services.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync<T>() where T : Page;
        Task NavigateToAsync<T>(object parameter) where T : Page;
        Task NavigateToAsync(Type pageType, object parameter = null);
        Task GoBackAsync();
        Task NavigateToRootAsync();
    }
}
