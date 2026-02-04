# .NET MAUI - Complete Documentation & Notes
### Interview Preparation + Development Reference Guide

---

## Table of Contents
1. [Introduction & Overview](#introduction--overview)
2. [Core Definitions](#core-definitions)
3. [Architecture & Project Structure](#architecture--project-structure)
4. [Essential CLI Commands](#essential-cli-commands)
5. [XAML UI Components & Layouts](#xaml-ui-components--layouts)
6. [Navigation Patterns](#navigation-patterns)
7. [Data Binding & MVVM](#data-binding--mvvm)
8. [Dependency Injection](#dependency-injection)
9. [Platform-Specific Code](#platform-specific-code)
10. [Data Persistence](#data-persistence)
11. [Network & Connectivity](#network--connectivity)
12. [App Lifecycle](#app-lifecycle)
13. [Styling & Theming](#styling--theming)
14. [Performance Optimization](#performance-optimization)
15. [Common Interview Questions](#common-interview-questions)
16. [Essential NuGet Packages](#essential-nuget-packages)
17. [Code Snippets & Examples](#code-snippets--examples)

---

## Introduction & Overview

### What is .NET MAUI?

**.NET Multi-platform App UI (.NET MAUI)** is a cross-platform framework for creating native mobile and desktop applications using C# and XAML. It's the evolution of Xamarin.Forms, allowing you to build apps for multiple platforms from a single codebase.

**Supported Platforms:**
- 📱 Android
- 🍎 iOS
- 🪟 Windows (via WinUI 3)
- 🍏 macOS (via Mac Catalyst)

**Key Features:**
-  Single project architecture
-  Unified API across all platforms
-  Hot Reload support (XAML & C#)
-  Native performance
-  MVVM and MVU pattern support
-  Built-in dependency injection
-  Handler-based architecture (replaces Renderers)
-  Shared resources management

**Evolution Path:**
```
Xamarin.Forms → .NET MAUI
- Multiple projects → Single project
- Renderers → Handlers
- .NET Standard → .NET 6+
- Separate resources → Unified resources
```

---

## Core Definitions

### 🔹 MAUI
Cross-platform framework for building native apps with shared UI and business logic across Android, iOS, Windows, and macOS.

### 🔹 Single Project Architecture
One project manages all platforms instead of separate platform-specific projects. Platform code resides in the `Platforms/` folder.

### 🔹 XAML (eXtensible Application Markup Language)
Declarative markup language used to design user interfaces in .NET MAUI.

### 🔹 Handler
Modern replacement for Xamarin Renderers. Maps cross-platform controls to native platform controls.

**Example:**
```
Button → Android: Android.Widget.Button
Button → iOS: UIKit.UIButton
Button → Windows: Microsoft.UI.Xaml.Controls.Button
```

### 🔹 MVVM (Model-View-ViewModel)
Design pattern that separates UI (View/XAML) from business logic (ViewModel) and data (Model).

**Structure:**
```
Model (Data) ← ViewModel (Logic) ← View (UI)
                    ↓
              Data Binding
```

### 🔹 Shell
Simplified navigation container providing:
- URI-based routing
- Flyout menu
- Tab navigation
- Search functionality
- Navigation stack management

### 🔹 Dependency Injection (DI)
Built-in service container for managing object lifetimes and dependencies, configured in `MauiProgram.cs`.

### 🔹 ResourceDictionary
Centralized location for styles, colors, templates, and converters that can be reused throughout the app.

### 🔹 Hot Reload
Feature allowing UI and code changes to be applied to the running app instantly without rebuilding.

### 🔹 Handlers vs Renderers
- **Renderers (Xamarin):** Heavy, platform-specific, hard to customize
- **Handlers (MAUI):** Lightweight, cross-platform with platform hooks, better performance

### 🔹 Platform Channels
Native interfaces for platform-specific functionality (camera, GPS, notifications).

---

## Architecture & Project Structure

### App Lifecycle Events
```
Created → Activated → Deactivated → Resumed → Stopped → Destroying
```

### Project Structure
```
MyMauiApp/
├── Platforms/              # Platform-specific code
│   ├── Android/
│   ├── iOS/
│   ├── Windows/
│   └── MacCatalyst/
├── Resources/              # Shared resources
│   ├── Fonts/
│   ├── Images/
│   ├── Raw/
│   ├── Styles/
│   └── AppIcon/
├── Models/                 # Data models
├── ViewModels/            # Business logic
├── Views/                 # UI pages
├── Services/              # App services
├── App.xaml               # Application-level resources
├── AppShell.xaml          # Shell configuration
├── MainPage.xaml          # Main page
└── MauiProgram.cs         # App configuration & DI
```

### MauiProgram.cs (Entry Point)
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

        // Register services
        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();

        return builder.Build();
    }
}
```

---

## Essential CLI Commands

### Workload Management
```bash
# Install .NET MAUI workload
dotnet workload install maui

# Update MAUI workload
dotnet workload update

# List installed workloads
dotnet workload list

# Repair workload
dotnet workload repair

# Uninstall workload
dotnet workload uninstall maui
```

### Project Creation
```bash
# Create new MAUI app
dotnet new maui -n MyMauiApp

# Create MAUI Class Library
dotnet new mauilib -n MyMauiLibrary

# Create MAUI Blazor Hybrid app
dotnet new maui-blazor -n MyBlazorApp

# Create MAUI app with specific framework
dotnet new maui -n MyApp -f net8.0
```

### Build & Run
```bash
# Restore dependencies
dotnet restore

# Build project (all platforms)
dotnet build

# Build for specific platform
dotnet build -f net8.0-android
dotnet build -f net8.0-ios
dotnet build -f net8.0-maccatalyst
dotnet build -f net8.0-windows10.0.19041.0

# Run application
dotnet run

# Run with specific framework
dotnet run -f net8.0-android

# Clean build outputs
dotnet clean

# Build and run in one command
dotnet build -t:Run -f net8.0-android
```

### Platform-Specific Commands
```bash
# Android
dotnet build -t:Run -f net8.0-android
dotnet build -f net8.0-android -c Release

# iOS (requires Mac)
dotnet build -t:Run -f net8.0-ios
dotnet build -f net8.0-ios -c Release

# Windows
dotnet build -t:Run -f net8.0-windows10.0.19041.0
dotnet build -f net8.0-windows10.0.19041.0 -c Release

# macOS
dotnet build -t:Run -f net8.0-maccatalyst
dotnet build -f net8.0-maccatalyst -c Release
```

### Publishing
```bash
# Publish for Android
dotnet publish -f net8.0-android -c Release

# Publish for iOS
dotnet publish -f net8.0-ios -c Release

# Publish for Windows with runtime
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -r win10-x64

# Publish for macOS
dotnet publish -f net8.0-maccatalyst -c Release
```

### Package Management
```bash
# Add NuGet package
dotnet add package PackageName

# Add specific version
dotnet add package PackageName --version 1.0.0

# Remove package
dotnet remove package PackageName

# Update package
dotnet add package PackageName

# List packages
dotnet list package
```

### Solution Management
```bash
# Create solution
dotnet new sln -n MySolution

# Add project to solution
dotnet sln add MyMauiApp/MyMauiApp.csproj

# Build solution
dotnet build MySolution.sln
```

---

## XAML UI Components & Layouts

### Common Controls

```xml
<!-- Label - Display text -->
<Label Text="Welcome to .NET MAUI!" 
       FontSize="24"
       FontAttributes="Bold"
       TextColor="Blue"
       HorizontalOptions="Center"
       VerticalOptions="Center"/>

<!-- Entry - Single line text input -->
<Entry Placeholder="Enter your name" 
       Text="{Binding Username}"
       Keyboard="Email"
       MaxLength="50"
       IsPassword="False"/>

<!-- Editor - Multi-line text input -->
<Editor Placeholder="Enter description"
        Text="{Binding Description}"
        AutoSize="TextChanges"
        HeightRequest="100"/>

<!-- Button -->
<Button Text="Submit" 
        Command="{Binding SubmitCommand}"
        BackgroundColor="#2196F3"
        TextColor="White"
        CornerRadius="5"
        Padding="20,10"/>

<!-- Image -->
<Image Source="logo.png"
       Aspect="AspectFit"
       HeightRequest="200"
       WidthRequest="200"/>

<!-- CheckBox -->
<CheckBox IsChecked="{Binding IsAccepted}"
          Color="Green"/>

<!-- Switch -->
<Switch IsToggled="{Binding IsEnabled}"
        OnColor="Green"
        ThumbColor="White"/>

<!-- Slider -->
<Slider Minimum="0"
        Maximum="100"
        Value="{Binding Volume}"/>

<!-- Stepper -->
<Stepper Minimum="0"
         Maximum="10"
         Increment="1"
         Value="{Binding Quantity}"/>

<!-- DatePicker -->
<DatePicker Date="{Binding SelectedDate}"
            MinimumDate="2020-01-01"
            MaximumDate="2025-12-31"/>

<!-- TimePicker -->
<TimePicker Time="{Binding SelectedTime}"/>

<!-- Picker (Dropdown) -->
<Picker Title="Select Country"
        ItemsSource="{Binding Countries}"
        SelectedItem="{Binding SelectedCountry}"/>

<!-- ProgressBar -->
<ProgressBar Progress="{Binding LoadProgress}"
             ProgressColor="Blue"/>

<!-- ActivityIndicator (Loading Spinner) -->
<ActivityIndicator IsRunning="{Binding IsLoading}"
                   Color="Blue"/>

<!-- SearchBar -->
<SearchBar Placeholder="Search..."
           Text="{Binding SearchText}"
           SearchCommand="{Binding SearchCommand}"/>

<!-- WebView -->
<WebView Source="https://www.example.com"
         HeightRequest="500"/>

<!-- Frame - Border container -->
<Frame BorderColor="Gray"
       CornerRadius="10"
       Padding="10"
       HasShadow="True">
    <Label Text="Framed Content"/>
</Frame>

<!-- ScrollView -->
<ScrollView>
    <VerticalStackLayout>
        <!-- Long content here -->
    </VerticalStackLayout>
</ScrollView>

<!-- RefreshView -->
<RefreshView IsRefreshing="{Binding IsRefreshing}"
             Command="{Binding RefreshCommand}">
    <CollectionView ItemsSource="{Binding Items}"/>
</RefreshView>
```

### Collection Controls

```xml
<!-- ListView (Legacy - use CollectionView instead) -->
<ListView ItemsSource="{Binding Items}"
          SelectedItem="{Binding SelectedItem}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Label Text="{Binding Name}"/>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>

<!-- CollectionView (Recommended) -->
<CollectionView ItemsSource="{Binding Items}"
                SelectionMode="Single"
                SelectedItem="{Binding SelectedItem}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Grid Padding="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="50"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                
                <Image Source="{Binding ImageUrl}" Grid.Column="0"/>
                <StackLayout Grid.Column="1">
                    <Label Text="{Binding Name}" FontAttributes="Bold"/>
                    <Label Text="{Binding Description}" FontSize="12"/>
                </StackLayout>
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>

<!-- CarouselView -->
<CarouselView ItemsSource="{Binding Items}"
              PeekAreaInsets="50">
    <CarouselView.ItemTemplate>
        <DataTemplate>
            <Frame HasShadow="True" CornerRadius="10">
                <StackLayout>
                    <Image Source="{Binding ImageUrl}"/>
                    <Label Text="{Binding Title}"/>
                </StackLayout>
            </Frame>
        </DataTemplate>
    </CarouselView.ItemTemplate>
</CarouselView>
```

### Layouts

#### StackLayout
```xml
<!-- Vertical Stack (Default) -->
<StackLayout Spacing="10" Padding="20">
    <Label Text="Item 1"/>
    <Label Text="Item 2"/>
    <Label Text="Item 3"/>
</StackLayout>

<!-- Horizontal Stack -->
<StackLayout Orientation="Horizontal" Spacing="15">
    <Button Text="Cancel"/>
    <Button Text="OK"/>
</StackLayout>
```

#### VerticalStackLayout & HorizontalStackLayout
```xml
<!-- Modern vertical stack -->
<VerticalStackLayout Spacing="10" Padding="20">
    <Label Text="Header"/>
    <Entry Placeholder="Name"/>
    <Button Text="Submit"/>
</VerticalStackLayout>

<!-- Modern horizontal stack -->
<HorizontalStackLayout Spacing="10">
    <Image Source="icon.png" WidthRequest="24"/>
    <Label Text="Dashboard" VerticalOptions="Center"/>
</HorizontalStackLayout>
```

#### Grid
```xml
<!-- Basic Grid -->
<Grid RowDefinitions="Auto,*,Auto" 
      ColumnDefinitions="*,*,*"
      RowSpacing="10"
      ColumnSpacing="10">
    
    <!-- Header spanning all columns -->
    <Label Text="Header" 
           Grid.Row="0" 
           Grid.Column="0"
           Grid.ColumnSpan="3"
           BackgroundColor="LightBlue"/>
    
    <!-- Content area -->
    <Label Text="Cell 1,0" Grid.Row="1" Grid.Column="0"/>
    <Label Text="Cell 1,1" Grid.Row="1" Grid.Column="1"/>
    <Label Text="Cell 1,2" Grid.Row="1" Grid.Column="2"/>
    
    <!-- Footer -->
    <Button Text="Submit" 
            Grid.Row="2" 
            Grid.Column="0"
            Grid.ColumnSpan="3"/>
</Grid>

<!-- Proportional Grid -->
<Grid ColumnDefinitions="2*,*,3*">
    <Label Text="Wide" Grid.Column="0"/>
    <Label Text="Narrow" Grid.Column="1"/>
    <Label Text="Wider" Grid.Column="2"/>
</Grid>
```

#### FlexLayout
```xml
<!-- Responsive Wrap Layout -->
<FlexLayout Direction="Row" 
            Wrap="Wrap" 
            JustifyContent="SpaceAround"
            AlignItems="Center">
    <Button Text="Button 1" WidthRequest="100"/>
    <Button Text="Button 2" WidthRequest="100"/>
    <Button Text="Button 3" WidthRequest="100"/>
    <Button Text="Button 4" WidthRequest="100"/>
</FlexLayout>

<!-- Column with Flex Grow -->
<FlexLayout Direction="Column">
    <Label Text="Header" FlexLayout.Grow="0"/>
    <ScrollView FlexLayout.Grow="1">
        <!-- Content -->
    </ScrollView>
    <Button Text="Footer" FlexLayout.Grow="0"/>
</FlexLayout>
```

#### AbsoluteLayout
```xml
<AbsoluteLayout>
    <!-- Position using bounds (x, y, width, height) -->
    <Label Text="Absolute Position" 
           AbsoluteLayout.LayoutBounds="100,50,200,50"/>
    
    <!-- Proportional positioning -->
    <Label Text="Center" 
           AbsoluteLayout.LayoutBounds="0.5,0.5,100,50"
           AbsoluteLayout.LayoutFlags="PositionProportional"/>
    
    <!-- Full screen overlay -->
    <BoxView Color="#80000000"
             AbsoluteLayout.LayoutBounds="0,0,1,1"
             AbsoluteLayout.LayoutFlags="All"/>
</AbsoluteLayout>
```

---

## Navigation Patterns

### Shell Navigation (Recommended)

#### AppShell.xaml Structure
```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:MyApp"
       x:Class="MyApp.AppShell">

    <!-- Flyout Menu -->
    <FlyoutItem Title="Home" Icon="home.png">
        <ShellContent ContentTemplate="{DataTemplate local:HomePage}"/>
    </FlyoutItem>

    <FlyoutItem Title="Settings" Icon="settings.png">
        <ShellContent ContentTemplate="{DataTemplate local:SettingsPage}"/>
    </FlyoutItem>

    <!-- Tab Bar -->
    <TabBar>
        <ShellContent Title="Feed" 
                      Icon="feed.png"
                      ContentTemplate="{DataTemplate local:FeedPage}"/>
        <ShellContent Title="Profile" 
                      Icon="profile.png"
                      ContentTemplate="{DataTemplate local:ProfilePage}"/>
    </TabBar>

</Shell>
```

#### Route Registration
```csharp
// AppShell.xaml.cs
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for pages not in Shell hierarchy
        Routing.RegisterRoute("details", typeof(DetailsPage));
        Routing.RegisterRoute("edit", typeof(EditPage));
        Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
    }
}
```

#### Navigation Commands
```csharp
// Navigate to registered route
await Shell.Current.GoToAsync("details");

// Navigate with parameters
await Shell.Current.GoToAsync($"details?id={itemId}");

// Navigate with multiple parameters
await Shell.Current.GoToAsync($"details?id={itemId}&name={name}");

// Navigate with object parameter
var navigationParameter = new Dictionary<string, object>
{
    { "Item", selectedItem }
};
await Shell.Current.GoToAsync("details", navigationParameter);

// Go back
await Shell.Current.GoToAsync("..");

// Go back multiple levels
await Shell.Current.GoToAsync("../..");

// Navigate to root
await Shell.Current.GoToAsync("//MainPage");

// Navigate with animation
await Shell.Current.GoToAsync("details", animate: false);
```

#### Receiving Navigation Parameters

**Method 1: Query Properties**
```csharp
[QueryProperty(nameof(ItemId), "id")]
[QueryProperty(nameof(ItemName), "name")]
public partial class DetailsPage : ContentPage
{
    private string _itemId;
    public string ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
            LoadData(value);
        }
    }

    public string ItemName { get; set; }
}
```

**Method 2: Navigation Dictionary**
```csharp
public partial class DetailsPage : ContentPage
{
    public DetailsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        // Access parameters from navigation
        if (args.NavigationSource == NavigationSource.Push)
        {
            // Handle navigation
        }
    }
}

// In ViewModel with IQueryAttributable
public class DetailsViewModel : IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Item"))
        {
            Item = query["Item"] as MyItem;
        }
    }
}
```

### Traditional Navigation

```csharp
// Push page onto navigation stack
await Navigation.PushAsync(new DetailsPage());

// Pop current page
await Navigation.PopAsync();

// Push modal page
await Navigation.PushModalAsync(new LoginPage());

// Pop modal page
await Navigation.PopModalAsync();

// Pop to root
await Navigation.PopToRootAsync();

// Insert page before current
Navigation.InsertPageBefore(new HomePage(), this);

// Remove page from stack
Navigation.RemovePage(somePage);
```

---

## Data Binding & MVVM

### Data Binding Modes

| Binding Mode | Description | Use Case |
|--------------|-------------|----------|
| OneWay | Source → Target | Display data |
| TwoWay | Source ↔ Target | Input fields |
| OneTime | Source → Target (once) | Static data |
| OneWayToSource | Target → Source | Rare scenarios |

### Basic Data Binding

```xml
<!-- OneWay binding (default for Label) -->
<Label Text="{Binding Username}"/>

<!-- TwoWay binding (default for Entry) -->
<Entry Text="{Binding Username, Mode=TwoWay}"/>

<!-- OneTime binding -->
<Label Text="{Binding AppVersion, Mode=OneTime}"/>

<!-- Binding with StringFormat -->
<Label Text="{Binding Price, StringFormat='Price: ${0:F2}'}"/>

<!-- Binding with Converter -->
<Label Text="{Binding IsActive, Converter={StaticResource BoolToStringConverter}}"/>

<!-- Binding to Command -->
<Button Text="Save" Command="{Binding SaveCommand}"/>

<!-- Command with parameter -->
<Button Text="Delete" 
        Command="{Binding DeleteCommand}"
        CommandParameter="{Binding ItemId}"/>
```

### ViewModel Implementation

**Method 1: Manual INotifyPropertyChanged**
```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class MainViewModel : INotifyPropertyChanged
{
    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

**Method 2: Using CommunityToolkit.Mvvm (Recommended)**
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<Item> items;

    [RelayCommand]
    private async Task LoadData()
    {
        IsLoading = true;
        try
        {
            Items = new ObservableCollection<Item>(await _dataService.GetItemsAsync());
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteItem(Item item)
    {
        await _dataService.DeleteAsync(item.Id);
        Items.Remove(item);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        await _dataService.SaveAsync(Username);
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Username);
}
```

### Commands

**ICommand Implementation**
```csharp
public class MainViewModel
{
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public MainViewModel()
    {
        SaveCommand = new Command(async () => await Save());
        DeleteCommand = new Command<Item>(async (item) => await Delete(item));
    }

    private async Task Save()
    {
        // Save logic
    }

    private async Task Delete(Item item)
    {
        // Delete logic
    }
}
```

**RelayCommand with CommunityToolkit**
```csharp
[RelayCommand]
private async Task Save()
{
    // Generates SaveCommand automatically
}

[RelayCommand]
private async Task Delete(Item item)
{
    // Generates DeleteCommand automatically
}
```

### Value Converters

```csharp
// Converter class
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive)
        {
            return isActive ? Colors.Green : Colors.Red;
        }
        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Register in App.xaml
<Application.Resources>
    <converters:BoolToColorConverter x:Key="BoolToColorConverter"/>
</Application.Resources>

// Use in XAML
<Label TextColor="{Binding IsActive, Converter={StaticResource BoolToColorConverter}}"/>
```

### Setting BindingContext

```csharp
// In code-behind
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

// In XAML
<ContentPage xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:DataType="vm:MainViewModel">
    <ContentPage.BindingContext>
        <vm:MainViewModel/>
    </ContentPage.BindingContext>
</ContentPage>
```

---

## Dependency Injection

### Service Registration in MauiProgram.cs

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // Singleton - One instance for app lifetime
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        
        // Transient - New instance each time
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<DetailsPage>();
        builder.Services.AddTransient<DetailsViewModel>();
        
        // Scoped - One instance per scope (less common in MAUI)
        builder.Services.AddScoped<IDatabaseService, DatabaseService>();
        
        // Register HttpClient
        builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
        {
            client.BaseAddress = new Uri("https://api.weather.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        return builder.Build();
    }
}
```

### Service Injection Patterns

**In ViewModel**
```csharp
public class MainViewModel
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    public MainViewModel(
        IDataService dataService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }
}
```

**In Page**
```csharp
public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}
```

**Service Interface & Implementation**
```csharp
// Interface
public interface IDataService
{
    Task<List<Item>> GetItemsAsync();
    Task<Item> GetItemByIdAsync(int id);
    Task<bool> SaveItemAsync(Item item);
    Task<bool> DeleteItemAsync(int id);
}

// Implementation
public class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivity _connectivity;

    public DataService(HttpClient httpClient, IConnectivity connectivity)
    {
        _httpClient = httpClient;
        _connectivity = connectivity;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            throw new Exception("No internet connection");
        }

        var response = await _httpClient.GetAsync("api/items");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Item>>();
    }

    public async Task<Item> GetItemByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/items/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Item>();
    }

    public async Task<bool> SaveItemAsync(Item item)
    {
        var response = await _httpClient.PostAsJsonAsync("api/items", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/items/{id}");
        return response.IsSuccessStatusCode;
    }
}
```

---

## Platform-Specific Code

### Method 1: Conditional Compilation

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        #if ANDROID
        ConfigureAndroid();
        #elif IOS
        ConfigureiOS();
        #elif WINDOWS
        ConfigureWindows();
        #elif MACCATALYST
        ConfigureMac();
        #endif
    }

    #if ANDROID
    private void ConfigureAndroid()
    {
        // Android-specific code
        var activity = Platform.CurrentActivity;
    }
    #endif

    #if IOS
    private void ConfigureiOS()
    {
        // iOS-specific code
    }
    #endif
}
```

### Method 2: Platforms Folder

**File Structure:**
```
Platforms/
├── Android/
│   └── MainActivity.cs
├── iOS/
│   └── AppDelegate.cs
├── Windows/
│   └── App.xaml.cs
└── MacCatalyst/
    └── AppDelegate.cs
```

**Platform-Specific Service**
```csharp
// Platforms/Android/PlatformService.cs
public class PlatformService : IPlatformService
{
    public string GetPlatformName() => "Android";
    
    public void ShowToast(string message)
    {
        Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
    }
}

// Platforms/iOS/PlatformService.cs
public class PlatformService : IPlatformService
{
    public string GetPlatformName() => "iOS";
    
    public void ShowToast(string message)
    {
        // iOS toast implementation
    }
}
```

### Method 3: Partial Classes

```csharp
// Shared/MyService.cs
public partial class MyService
{
    public void DoSomething()
    {
        DoPlatformSpecific();
    }

    partial void DoPlatformSpecific();
}

// Platforms/Android/MyService.cs
public partial class MyService
{
    partial void DoPlatformSpecific()
    {
        // Android implementation
    }
}

// Platforms/iOS/MyService.cs
public partial class MyService
{
    partial void DoPlatformSpecific()
    {
        // iOS implementation
    }
}
```

### Method 4: Device.RuntimePlatform (Legacy but still works)

```csharp
if (DeviceInfo.Platform == DevicePlatform.Android)
{
    // Android code
}
else if (DeviceInfo.Platform == DevicePlatform.iOS)
{
    // iOS code
}
else if (DeviceInfo.Platform == DevicePlatform.WinUI)
{
    // Windows code
}
```

### Platform-Specific XAML

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    
    <!-- Platform-specific values -->
    <Label>
        <Label.Text>
            <OnPlatform x:TypeArguments="x:String">
                <On Platform="iOS">iOS Platform</On>
                <On Platform="Android">Android Platform</On>
                <On Platform="WinUI">Windows Platform</On>
            </OnPlatform>
        </Label.Text>
    </Label>

    <!-- Platform-specific margin -->
    <StackLayout>
        <StackLayout.Margin>
            <OnPlatform x:TypeArguments="Thickness">
                <On Platform="iOS">0,20,0,0</On>
                <On Platform="Android">0,0,0,0</On>
            </OnPlatform>
        </StackLayout.Margin>
    </StackLayout>

</ContentPage>
```

---

## Data Persistence

### 1. Preferences (Simple Key-Value Storage)

```csharp
// Save data
Preferences.Set("username", "JohnDoe");
Preferences.Set("age", 25);
Preferences.Set("isLoggedIn", true);
Preferences.Set("lastLogin", DateTime.Now);

// Get data with default value
string username = Preferences.Get("username", "Guest");
int age = Preferences.Get("age", 0);
bool isLoggedIn = Preferences.Get("isLoggedIn", false);
DateTime lastLogin = Preferences.Get("lastLogin", DateTime.MinValue);

// Check if key exists
bool exists = Preferences.ContainsKey("username");

// Remove specific key
Preferences.Remove("username");

// Clear all preferences
Preferences.Clear();

// Save with shared name (for app groups)
Preferences.Set("token", "abc123", "MySharedPrefs");
string token = Preferences.Get("token", null, "MySharedPrefs");
```

### 2. Secure Storage (Sensitive Data)

```csharp
// Store secure data
await SecureStorage.SetAsync("oauth_token", "secret_token_value");
await SecureStorage.SetAsync("password", "user_password");

// Retrieve secure data
string token = await SecureStorage.GetAsync("oauth_token");
string password = await SecureStorage.GetAsync("password");

// Remove secure data
SecureStorage.Remove("oauth_token");

// Remove all
SecureStorage.RemoveAll();

// Note: SecureStorage uses:
// - iOS: Keychain
// - Android: KeyStore
// - Windows: Data Protection API
```

### 3. File System

```csharp
// Get app data directory
string appDataPath = FileSystem.AppDataDirectory;
string cachePath = FileSystem.CacheDirectory;

// Write file
string filePath = Path.Combine(FileSystem.AppDataDirectory, "data.txt");
await File.WriteAllTextAsync(filePath, "Hello World");

// Read file
if (File.Exists(filePath))
{
    string content = await File.ReadAllTextAsync(filePath);
}

// Write JSON
string jsonPath = Path.Combine(FileSystem.AppDataDirectory, "settings.json");
string json = JsonSerializer.Serialize(settingsObject);
await File.WriteAllTextAsync(jsonPath, json);

// Read JSON
if (File.Exists(jsonPath))
{
    string json = await File.ReadAllTextAsync(jsonPath);
    var settings = JsonSerializer.Deserialize<Settings>(json);
}

// Delete file
if (File.Exists(filePath))
{
    File.Delete(filePath);
}

// File picker
var result = await FilePicker.PickAsync(new PickOptions
{
    PickerTitle = "Select a file",
    FileTypes = FilePickerFileType.Images
});

if (result != null)
{
    string fileName = result.FileName;
    string filePath = result.FullPath;
    using var stream = await result.OpenReadAsync();
    // Process file
}
```

### 4. SQLite Database

**Install Package:**
```bash
dotnet add package sqlite-net-pcl
dotnet add package SQLitePCLRaw.bundle_green
```

**Database Setup:**
```csharp
using SQLite;

// Model
public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [MaxLength(100), Unique]
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    [Indexed]
    public DateTime CreatedAt { get; set; }
    
    [Ignore]
    public string FullName { get; set; }
}

// Database Service
public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        InitializeAsync().Wait();
    }

    private async Task InitializeAsync()
    {
        if (_database != null)
            return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myapp.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        
        await _database.CreateTableAsync<User>();
    }

    // Create
    public async Task<int> InsertUserAsync(User user)
    {
        return await _database.InsertAsync(user);
    }

    // Read All
    public async Task<List<User>> GetUsersAsync()
    {
        return await _database.Table<User>().ToListAsync();
    }

    // Read with Where
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _database.Table<User>()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    // Read with complex query
    public async Task<List<User>> SearchUsersAsync(string searchTerm)
    {
        return await _database.Table<User>()
            .Where(u => u.Username.Contains(searchTerm))
            .OrderBy(u => u.Username)
            .ToListAsync();
    }

    // Update
    public async Task<int> UpdateUserAsync(User user)
    {
        return await _database.UpdateAsync(user);
    }

    // Delete
    public async Task<int> DeleteUserAsync(User user)
    {
        return await _database.DeleteAsync(user);
    }

    // Delete by ID
    public async Task<int> DeleteUserByIdAsync(int id)
    {
        return await _database.DeleteAsync<User>(id);
    }

    // Execute raw SQL
    public async Task<List<User>> ExecuteQueryAsync(string query)
    {
        return await _database.QueryAsync<User>(query);
    }

    // Execute non-query
    public async Task<int> ExecuteAsync(string query)
    {
        return await _database.ExecuteAsync(query);
    }
}
```

**Advanced SQLite Queries:**
```csharp
// Count
int count = await _database.Table<User>().CountAsync();

// Max/Min
int maxId = await _database.Table<User>().MaxAsync(u => u.Id);

// OrderBy
var users = await _database.Table<User>()
    .OrderBy(u => u.Username)
    .ThenBy(u => u.CreatedAt)
    .ToListAsync();

// Take/Skip (Pagination)
var pagedUsers = await _database.Table<User>()
    .Skip(10)
    .Take(20)
    .ToListAsync();

// Transaction
await _database.RunInTransactionAsync(tran =>
{
    tran.Insert(user1);
    tran.Insert(user2);
    tran.Update(user3);
});

// Bulk Insert
await _database.InsertAllAsync(usersList);

// Drop Table
await _database.DropTableAsync<User>();

// Table Info
var mapping = _database.GetMapping<User>();
```

---

##  Network & Connectivity

### Connectivity Status

```csharp
// Check current network access
var networkAccess = Connectivity.Current.NetworkAccess;

if (networkAccess == NetworkAccess.Internet)
{
    // Full internet access
}
else if (networkAccess == NetworkAccess.ConstrainedInternet)
{
    // Limited internet access
}
else if (networkAccess == NetworkAccess.Local)
{
    // Local network only
}
else
{
    // No connectivity
}

// Check connection profiles
var profiles = Connectivity.Current.ConnectionProfiles;

if (profiles.Contains(ConnectionProfile.WiFi))
{
    // Connected via WiFi
}
if (profiles.Contains(ConnectionProfile.Cellular))
{
    // Connected via cellular
}

// Monitor connectivity changes
Connectivity.Current.ConnectivityChanged += (sender, e) =>
{
    if (e.NetworkAccess == NetworkAccess.Internet)
    {
        // Connection restored
    }
    else
    {
        // Connection lost
    }
};
```

### HTTP Client Usage

```csharp
// Basic GET request
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.example.com/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    // GET
    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("api/products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>();
    }

    // GET with parameters
    public async Task<Product> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/products/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Product>();
        }
        return null;
    }

    // POST
    public async Task<bool> CreateProductAsync(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/products", product);
        return response.IsSuccessStatusCode;
    }

    // PUT
    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
        return response.IsSuccessStatusCode;
    }

    // DELETE
    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }

    // With headers
    public async Task<string> GetWithAuthAsync(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token");
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // Download file
    public async Task<byte[]> DownloadFileAsync(string url)
    {
        return await _httpClient.GetByteArrayAsync(url);
    }

    // Upload file
    public async Task<bool> UploadFileAsync(string filePath)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        content.Add(fileContent, "file", Path.GetFileName(filePath));

        var response = await _httpClient.PostAsync("api/upload", content);
        return response.IsSuccessStatusCode;
    }
}
```

### Error Handling

```csharp
public async Task<Result<List<Product>>> GetProductsSafeAsync()
{
    try
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            return Result<List<Product>>.Failure("No internet connection");
        }

        var response = await _httpClient.GetAsync("api/products");
        
        if (response.IsSuccessStatusCode)
        {
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            return Result<List<Product>>.Success(products);
        }
        
        return Result<List<Product>>.Failure($"Error: {response.StatusCode}");
    }
    catch (HttpRequestException ex)
    {
        return Result<List<Product>>.Failure($"Request error: {ex.Message}");
    }
    catch (TaskCanceledException)
    {
        return Result<List<Product>>.Failure("Request timeout");
    }
    catch (Exception ex)
    {
        return Result<List<Product>>.Failure($"Unexpected error: {ex.Message}");
    }
}

// Result wrapper class
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}
```

---

## App Lifecycle

### Lifecycle Events

```csharp
// App.xaml.cs
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        // App started for the first time
        Console.WriteLine("App Started");
    }

    protected override void OnSleep()
    {
        // App moved to background
        Console.WriteLine("App Sleeping");
        
        // Save state here
        SaveAppState();
    }

    protected override void OnResume()
    {
        // App brought back to foreground
        Console.WriteLine("App Resumed");
        
        // Restore state or refresh data
        RestoreAppState();
    }

    private void SaveAppState()
    {
        // Save current state
        Preferences.Set("LastActiveTime", DateTime.Now);
    }

    private void RestoreAppState()
    {
        // Check if data needs refresh
        var lastActive = Preferences.Get("LastActiveTime", DateTime.MinValue);
        if (DateTime.Now - lastActive > TimeSpan.FromHours(1))
        {
            // Refresh data
        }
    }
}
```

### Window Lifecycle

```csharp
public partial class App : Application
{
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Created += (s, e) =>
        {
            // Window created
        };

        window.Activated += (s, e) =>
        {
            // Window activated
        };

        window.Deactivated += (s, e) =>
        {
            // Window deactivated
        };

        window.Stopped += (s, e) =>
        {
            // Window stopped
        };

        window.Resumed += (s, e) =>
        {
            // Window resumed
        };

        window.Destroying += (s, e) =>
        {
            // Window being destroyed
        };

        return window;
    }
}
```

### Page Lifecycle

```csharp
public partial class MainPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Page is about to appear
        LoadData();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Page is about to disappear
        SaveChanges();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        // Page was navigated to
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        // Page was navigated away from
    }
}
```

---

## Styling & Theming

### Resource Dictionary

```xml
<!-- App.xaml -->
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.App">
    
    <Application.Resources>
        <ResourceDictionary>
            
            <!-- Colors -->
            <Color x:Key="PrimaryColor">#2196F3</Color>
            <Color x:Key="SecondaryColor">#FFC107</Color>
            <Color x:Key="BackgroundColor">#FFFFFF</Color>
            <Color x:Key="TextColor">#212121</Color>
            
            <!-- Font Sizes -->
            <x:Double x:Key="LargeFontSize">24</x:Double>
            <x:Double x:Key="MediumFontSize">18</x:Double>
            <x:Double x:Key="SmallFontSize">14</x:Double>
            
            <!-- Spacing -->
            <x:Double x:Key="DefaultPadding">20</x:Double>
            <x:Double x:Key="DefaultSpacing">10</x:Double>
            
            <!-- Implicit Styles (apply to all of type) -->
            <Style TargetType="Label">
                <Setter Property="TextColor" Value="{StaticResource TextColor}"/>
                <Setter Property="FontSize" Value="{StaticResource MediumFontSize}"/>
            </Style>
            
            <!-- Explicit Styles (must be referenced by key) -->
            <Style x:Key="HeaderLabel" TargetType="Label">
                <Setter Property="FontSize" Value="{StaticResource LargeFontSize}"/>
                <Setter Property="FontAttributes" Value="Bold"/>
                <Setter Property="TextColor" Value="{StaticResource PrimaryColor}"/>
            </Style>
            
            <Style x:Key="PrimaryButton" TargetType="Button">
                <Setter Property="BackgroundColor" Value="{StaticResource PrimaryColor}"/>
                <Setter Property="TextColor" Value="White"/>
                <Setter Property="CornerRadius" Value="5"/>
                <Setter Property="Padding" Value="20,10"/>
                <Setter Property="FontAttributes" Value="Bold"/>
            </Style>
            
            <!-- Style Inheritance -->
            <Style x:Key="LargeButton" TargetType="Button" BasedOn="{StaticResource PrimaryButton}">
                <Setter Property="FontSize" Value="{StaticResource LargeFontSize}"/>
                <Setter Property="HeightRequest" Value="60"/>
            </Style>
            
        </ResourceDictionary>
    </Application.Resources>
    
</Application>

<!-- Usage in pages -->
<Label Style="{StaticResource HeaderLabel}" Text="Welcome"/>
<Button Style="{StaticResource PrimaryButton}" Text="Click Me"/>
```

### Dark Mode Support

```xml
<!-- App.xaml -->
<Application.Resources>
    <ResourceDictionary>
        
        <!-- Light theme colors -->
        <Color x:Key="BackgroundColor">White</Color>
        <Color x:Key="TextColor">Black</Color>
        
        <!-- Override for dark mode -->
        <ResourceDictionary x:Key="Dark">
            <Color x:Key="BackgroundColor">#1E1E1E</Color>
            <Color x:Key="TextColor">White</Color>
        </ResourceDictionary>
        
    </ResourceDictionary>
</Application.Resources>
```

```csharp
// Detect and change theme
public void ChangeTheme()
{
    Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Dark 
        ? AppTheme.Light 
        : AppTheme.Dark;
}

// Get current theme
var currentTheme = Application.Current.RequestedTheme; // System theme
var userTheme = Application.Current.UserAppTheme;      // User-selected theme
```

### Dynamic Resources

```xml
<!-- Can be changed at runtime -->
<Label Text="Dynamic Color" TextColor="{DynamicResource PrimaryColor}"/>
```

```csharp
// Change resource at runtime
Application.Current.Resources["PrimaryColor"] = Colors.Red;
```

---

## Performance Optimization

### Best Practices

**1. Use CollectionView instead of ListView**
```xml
<!-- Better performance -->
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Label Text="{Binding Name}"/>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

**2. Enable Compiled Bindings**
```xml
<ContentPage xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:DataType="vm:MainViewModel">
    <!-- Compiled binding - faster -->
    <Label Text="{Binding Username}"/>
</ContentPage>
```

**3. Virtualization**
```csharp
// CollectionView automatically virtualizes items
// Only visible items are rendered
```

**4. Image Optimization**
```xml
<!-- Downsampling for better memory usage -->
<Image Source="large_image.jpg">
    <Image.Behaviors>
        <toolkit:ImageTouchBehavior />
    </Image.Behaviors>
</Image>
```

```csharp
// Use FFImageLoading or similar for caching
// Install: CommunityToolkit.Maui
```

**5. Async/Await Properly**
```csharp
// Good
[RelayCommand]
private async Task LoadData()
{
    IsLoading = true;
    try
    {
        Items = await _service.GetItemsAsync();
    }
    finally
    {
        IsLoading = false;
    }
}

// Bad - blocks UI thread
private void LoadData()
{
    Items = _service.GetItemsAsync().Result; // Blocking!
}
```

**6. Lazy Loading**
```csharp
public partial class DetailsPage : ContentPage
{
    private bool _isInitialized;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (!_isInitialized)
        {
            InitializeData();
            _isInitialized = true;
        }
    }
}
```

**7. Avoid Heavy Operations in Constructors**
```csharp
public class MainViewModel
{
    public MainViewModel()
    {
        // Keep constructor light
        LoadDataCommand = new Command(async () => await LoadData());
    }

    private async Task LoadData()
    {
        // Heavy operations here
    }
}
```

**8. Use Handlers for Custom Controls**
```csharp
// More efficient than custom renderers
public class CustomEntryHandler : EntryHandler
{
    protected override void ConnectHandler(Microsoft.Maui.Platform.MauiTextField platformView)
    {
        base.ConnectHandler(platformView);
        // Customize native control
    }
}
```

**9. Reduce Layout Nesting**
```xml
<!-- Bad - deeply nested -->
<StackLayout>
    <StackLayout>
        <Grid>
            <StackLayout>
                <Label Text="Hello"/>
            </StackLayout>
        </Grid>
    </StackLayout>
</StackLayout>

<!-- Good - flatter hierarchy -->
<Grid>
    <Label Text="Hello"/>
</Grid>
```

**10. Cache Data**
```csharp
private List<Item> _cachedItems;

public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
{
    if (_cachedItems != null && !forceRefresh)
        return _cachedItems;

    _cachedItems = await _httpClient.GetFromJsonAsync<List<Item>>("api/items");
    return _cachedItems;
}
```

---

## Common  Questions

### Q1: What is .NET MAUI?
**A:** .NET MAUI (Multi-platform App UI) is a cross-platform framework for building native mobile and desktop applications using C# and XAML. It's the evolution of Xamarin.Forms, supporting Android, iOS, Windows, and macOS from a single codebase with a unified project structure.

### Q2: What's the difference between .NET MAUI and Xamarin.Forms?
**A:** Key differences:
- **Project Structure:** MAUI uses single project vs. Xamarin's multiple projects
- **Performance:** MAUI uses Handlers (lightweight) vs. Renderers (heavy)
- **Framework:** MAUI uses .NET 6+ vs. Xamarin's .NET Standard
- **Resources:** Unified resource management in MAUI
- **Platform Support:** MAUI adds Windows and macOS via Mac Catalyst

### Q3: What are Handlers in MAUI?
**A:** Handlers are the modern replacement for Xamarin Renderers. They map cross-platform controls to native platform controls with better performance and easier customization. For example, a MAUI `Button` maps to `Android.Widget.Button` on Android and `UIKit.UIButton` on iOS.

### Q4: Explain MVVM pattern in MAUI context
**A:** MVVM (Model-View-ViewModel) separates:
- **Model:** Data and business logic
- **View:** UI defined in XAML
- **ViewModel:** Presentation logic and state

Data binding connects View and ViewModel, allowing changes in ViewModel to automatically update the UI and vice versa. This separation enables testability and maintainability.

### Q5: What's the difference between Shell and NavigationPage?
**A:** 
- **Shell:** Comprehensive navigation system with URI-based routing, flyout menus, tabs, and search. Recommended for modern apps.
- **NavigationPage:** Simple stack-based navigation. More basic but limited in features.

### Q6: How do you handle platform-specific code?
**A:** Four methods:
1. Conditional compilation (`#if ANDROID`)
2. Platform-specific folders (`Platforms/Android/`)
3. Partial classes with platform implementations
4. Dependency injection with platform-specific services

### Q7: What are the service lifetimes in MAUI DI?
**A:**
- **Singleton:** One instance for entire app lifetime
- **Transient:** New instance every time requested
- **Scoped:** One instance per scope (less common in MAUI)

### Q8: Difference between ListView and CollectionView?
**A:** CollectionView is:
- Faster and more performant
- More flexible (horizontal, grid layouts)
- Better virtualization
- Replaces ListView in modern apps

### Q9: What's Hot Reload?
**A:** Feature allowing XAML and C# code changes to be instantly applied to the running app without rebuilding or losing app state. Significantly speeds up development.

### Q10: How do you store data in MAUI?
**A:**
- **Preferences:** Simple key-value pairs
- **Secure Storage:** Encrypted sensitive data
- **File System:** Files and JSON
- **SQLite:** Relational database for complex data

### Q11: What binding modes are available?
**A:**
- **OneWay:** Source → Target (default for read-only)
- **TwoWay:** Source ↔ Target (default for inputs)
- **OneTime:** Source → Target once
- **OneWayToSource:** Target → Source

### Q12: How does Shell routing work?
**A:** Shell uses URI-based routing:
```csharp
// Register route
Routing.RegisterRoute("details", typeof(DetailsPage));

// Navigate
await Shell.Current.GoToAsync("details?id=123");

// Receive parameter
[QueryProperty(nameof(ItemId), "id")]
public string ItemId { get; set; }
```

### Q13: What's the purpose of MauiProgram.cs?
**A:** Entry point for configuring the MAUI app:
- Register services for DI
- Configure fonts
- Add community toolkits
- Set up handlers
- Configure logging

### Q14: How do you optimize MAUI app performance?
**A:**
- Use CollectionView over ListView
- Enable compiled bindings
- Implement lazy loading
- Cache data appropriately
- Avoid deep layout nesting
- Use async/await properly
- Optimize images
- Use Handlers for custom controls

### Q15: What's the App lifecycle in MAUI?
**A:** Lifecycle events:
- **OnStart:** App launched
- **OnSleep:** App backgrounded
- **OnResume:** App returned to foreground
- **OnDestroy:** App terminated

### Q16: How do you make HTTP calls in MAUI?
**A:** Using HttpClient with dependency injection:
```csharp
builder.Services.AddHttpClient<IApiService, ApiService>();

// In service
public class ApiService
{
    private readonly HttpClient _httpClient;
    
    public async Task<List<Item>> GetItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Item>>("api/items");
    }
}
```

### Q17: What's the difference between StaticResource and DynamicResource?
**A:**
- **StaticResource:** Resolved once at compile/load time, better performance
- **DynamicResource:** Can change at runtime, used for themes

### Q18: How do you implement Dark Mode?
**A:** Define resources for both themes and detect/change theme:
```csharp
Application.Current.UserAppTheme = AppTheme.Dark;
```

### Q19: What are Value Converters?
**A:** Classes implementing `IValueConverter` to transform binding values:
```csharp
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Colors.Green : Colors.Red;
    }
}
```

### Q20: How do you handle connectivity in MAUI?
**A:** Using the Connectivity API:
```csharp
var networkAccess = Connectivity.Current.NetworkAccess;
if (networkAccess == NetworkAccess.Internet)
{
    // Connected
}

// Monitor changes
Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
```

---

##  Essential NuGet Packages

### MVVM & Toolkit
```bash
# CommunityToolkit.Mvvm - MVVM helpers (ObservableProperty, RelayCommand)
dotnet add package CommunityToolkit.Mvvm

# CommunityToolkit.Maui - Additional controls and behaviors
dotnet add package CommunityToolkit.Maui
```

### Database
```bash
# SQLite for local database
dotnet add package sqlite-net-pcl
dotnet add package SQLitePCLRaw.bundle_green

# Entity Framework Core (alternative)
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

### Networking
```bash
# HTTP client extensions
dotnet add package Microsoft.Extensions.Http

# Refit - Type-safe REST library
dotnet add package Refit
dotnet add package Refit.HttpClientFactory
```

### JSON
```bash
# System.Text.Json (built-in, recommended)
# Newtonsoft.Json (alternative)
dotnet add package Newtonsoft.Json
```

### UI Controls
```bash
# Syncfusion controls (free community license available)
dotnet add package Syncfusion.Maui.Core

# DevExpress controls
dotnet add package DevExpress.Maui.Controls

# Telerik UI for MAUI
dotnet add package Telerik.UI.for.Maui
```

### Media & Images
```bash
# FFImageLoading for MAUI (caching & transformations)
dotnet add package FFImageLoading.Maui

# SkiaSharp for custom graphics
dotnet add package SkiaSharp.Views.Maui.Controls
```

### Maps & Location
```bash
# Microsoft.Maui.Controls.Maps
dotnet add package Microsoft.Maui.Controls.Maps
```

### Analytics & Monitoring
```bash
# Application Insights
dotnet add package Microsoft.ApplicationInsights

# AppCenter
dotnet add package Microsoft.AppCenter.Analytics
dotnet add package Microsoft.AppCenter.Crashes
```

### Testing
```bash
# xUnit
dotnet add package xunit
dotnet add package xunit.runner.visualstudio

# Moq for mocking
dotnet add package Moq
```

---

## Code Snippets & Examples

### Complete MVVM Example

**Model:**
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}
```

**Service:**
```csharp
public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<bool> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("api/products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/products", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }
}
```

**ViewModel:**
```csharp
public partial class ProductListViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IConnectivity _connectivity;

    [ObservableProperty]
    private ObservableCollection<Product> products;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string searchText;

    public ProductListViewModel(IProductService productService, IConnectivity connectivity)
    {
        _productService = productService;
        _connectivity = connectivity;
        Products = new ObservableCollection<Product>();
    }

    [RelayCommand]
    private async Task LoadProducts()
    {
        if (IsLoading)
            return;

        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Error", "No internet connection", "OK");
            return;
        }

        IsLoading = true;
        try
        {
            var products = await _productService.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsRefreshing = true;
        await LoadProducts();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task GoToDetails(Product product)
    {
        if (product == null)
            return;

        await Shell.Current.GoToAsync($"details", new Dictionary<string, object>
        {
            { "Product", product }
        });
    }

    [RelayCommand]
    private async Task DeleteProduct(Product product)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm", 
            $"Delete {product.Name}?", 
            "Yes", 
            "No");

        if (!confirm)
            return;

        IsLoading = true;
        try
        {
            bool success = await _productService.DeleteProductAsync(product.Id);
            if (success)
            {
                Products.Remove(product);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        // Implement search logic
    }
}
```

**View:**
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.Views.ProductListPage"
             x:DataType="vm:ProductListViewModel"
             Title="Products">

    <Grid RowDefinitions="Auto,*" Padding="10">
        
        <!-- Search Bar -->
        <SearchBar Grid.Row="0"
                   Placeholder="Search products..."
                   Text="{Binding SearchText}"
                   SearchCommand="{Binding SearchCommand}"
                   Margin="0,0,0,10"/>

        <!-- Products List -->
        <RefreshView Grid.Row="1"
                     IsRefreshing="{Binding IsRefreshing}"
                     Command="{Binding RefreshCommand}">
            
            <CollectionView ItemsSource="{Binding Products}"
                            SelectionMode="None">
                
                <CollectionView.EmptyView>
                    <StackLayout VerticalOptions="Center" Padding="20">
                        <Label Text="No products found"
                               HorizontalOptions="Center"
                               FontSize="18"
                               TextColor="Gray"/>
                    </StackLayout>
                </CollectionView.EmptyView>

                <CollectionView.ItemTemplate>
                    <DataTemplate x:DataType="vm:Product">
                        <Frame Margin="5" Padding="10" HasShadow="True" CornerRadius="10">
                            <Frame.GestureRecognizers>
                                <TapGestureRecognizer 
                                    Command="{Binding Source={RelativeSource AncestorType={x:Type vm:ProductListViewModel}}, Path=GoToDetailsCommand}"
                                    CommandParameter="{Binding .}"/>
                            </Frame.GestureRecognizers>

                            <Grid ColumnDefinitions="80,*,Auto">
                                
                                <!-- Product Image -->
                                <Image Grid.Column="0"
                                       Source="{Binding ImageUrl}"
                                       Aspect="AspectFill"
                                       HeightRequest="80"
                                       WidthRequest="80"/>

                                <!-- Product Info -->
                                <VerticalStackLayout Grid.Column="1" 
                                                     Padding="10,0"
                                                     VerticalOptions="Center">
                                    <Label Text="{Binding Name}"
                                           FontSize="18"
                                           FontAttributes="Bold"/>
                                    <Label Text="{Binding Description}"
                                           FontSize="14"
                                           TextColor="Gray"
                                           LineBreakMode="TailTruncation"/>
                                    <Label Text="{Binding Price, StringFormat='${0:F2}'}"
                                           FontSize="16"
                                           FontAttributes="Bold"
                                           TextColor="Green"/>
                                </VerticalStackLayout>

                                <!-- Delete Button -->
                                <Button Grid.Column="2"
                                        Text="Delete"
                                        BackgroundColor="Red"
                                        TextColor="White"
                                        VerticalOptions="Center"
                                        Command="{Binding Source={RelativeSource AncestorType={x:Type vm:ProductListViewModel}}, Path=DeleteProductCommand}"
                                        CommandParameter="{Binding .}"/>

                            </Grid>
                        </Frame>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
        </RefreshView>

        <!-- Loading Indicator -->
        <ActivityIndicator Grid.RowSpan="2"
                           IsRunning="{Binding IsLoading}"
                           IsVisible="{Binding IsLoading}"
                           Color="Blue"
                           VerticalOptions="Center"
                           HorizontalOptions="Center"/>

    </Grid>
</ContentPage>
```

**Code-Behind:**
```csharp
public partial class ProductListPage : ContentPage
{
    private readonly ProductListViewModel _viewModel;

    public ProductListPage(ProductListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsCommand.ExecuteAsync(null);
    }
}
```

**Registration in MauiProgram.cs:**
```csharp
// Services
builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com/");
});

// Pages and ViewModels
builder.Services.AddTransient<ProductListPage>();
builder.Services.AddTransient<ProductListViewModel>();
```

---

##  Quick Reference Summary

### Essential Concepts
- **MAUI** = Cross-platform framework (Android, iOS, Windows, macOS)
- **Single Project** = One project for all platforms
- **Handlers** = Map controls to native (replaces Renderers)
- **MVVM** = Model-View-ViewModel pattern
- **Shell** = Modern navigation system
- **DI** = Built-in dependency injection

### Key Commands
```bash
dotnet workload install maui
dotnet new maui -n MyApp
dotnet build -f net8.0-android
dotnet run
```

### Navigation
```csharp
await Shell.Current.GoToAsync("route");
await Shell.Current.GoToAsync($"details?id={id}");
await Shell.Current.GoToAsync("..");
```

### Data Binding
```xml
<Entry Text="{Binding Name, Mode=TwoWay}"/>
<Button Command="{Binding SaveCommand}"/>
```

### Lifecycle
```csharp
OnStart → OnSleep → OnResume → OnDestroy
```

### Storage Options
- **Preferences** - Simple key-value
- **SecureStorage** - Encrypted data
- **FileSystem** - Files & JSON
- **SQLite** - Relational database

### Performance Tips
- Use CollectionView
- Enable compiled bindings
- Implement caching
- Use async/await
- Optimize images
- Reduce layout nesting

---
