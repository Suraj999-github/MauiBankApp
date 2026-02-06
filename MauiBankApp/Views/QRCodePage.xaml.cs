using ZXing.Net.Maui;
namespace MauiBankApp.Views;

public partial class QRCodePage : ContentPage
{
    private bool _hasScanned;
    private string _scannedData = string.Empty;
    private bool _isAnimating;

    public QRCodePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Start scanning animation
        StartScanningAnimation();

        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            ShowError("Camera permission is required.");
            return;
        }

        _hasScanned = false;
        CameraView.IsDetecting = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraView.IsDetecting = false;
        _isAnimating = false;
    }

    private async void StartScanningAnimation()
    {
        _isAnimating = true;
        ScanLine.TranslationY = 0;
        ScanLine.IsVisible = true;

        while (_isAnimating)
        {
            // Animate scan line from top to bottom
            await ScanLine.TranslateTo(ScanLine.X, 270, 2000, Easing.Linear);

            // Small pause at bottom
            await Task.Delay(300);

            // Reset to top (invisible reset)
            ScanLine.TranslationY = 0;

            // Small pause at top
            await Task.Delay(300);
        }
    }

    private void StopScanningAnimation()
    {
        _isAnimating = false;
        ScanLine.IsVisible = false;
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_hasScanned)
            return;

        var barcode = e.Results.FirstOrDefault();
        if (barcode == null)
            return;

        _hasScanned = true;
        _scannedData = barcode.Value;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            CameraView.IsDetecting = false;
            StopScanningAnimation();
            ShowSuccess(barcode.Value, barcode.Format.ToString());
        });
    }

    private async void ShowSuccess(string data, string format)
    {
        SuccessHeader.IsVisible = true;
        ErrorHeader.IsVisible = false;

        InfoLabel.Text = "Scanned Data:";
        InfoLabel.IsVisible = true;

        ResultLabel.Text = data;

        FormatLayout.IsVisible = true;
        FormatLabel.Text = format;

        CopyButton.IsVisible = true;

        // Animate modal appearance
        ModalOverlay.Scale = 0.8;
        ModalOverlay.Opacity = 0;
        ModalOverlay.IsVisible = true;

        await Task.WhenAll(
            ModalOverlay.FadeTo(1, 200, Easing.CubicOut),
            ModalOverlay.ScaleTo(1, 300, Easing.SpringOut)
        );
    }

    private async void ShowError(string message)
    {
        ErrorHeader.IsVisible = true;
        SuccessHeader.IsVisible = false;

        InfoLabel.Text = message;
        InfoLabel.IsVisible = true;

        ResultLabel.Text = "";
        FormatLayout.IsVisible = false;
        CopyButton.IsVisible = false;

        // Animate modal appearance
        ModalOverlay.Scale = 0.8;
        ModalOverlay.Opacity = 0;
        ModalOverlay.IsVisible = true;

        await Task.WhenAll(
            ModalOverlay.FadeTo(1, 200, Easing.CubicOut),
            ModalOverlay.ScaleTo(1, 300, Easing.SpringOut)
        );
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_scannedData))
        {
            await Clipboard.SetTextAsync(_scannedData);

            // Visual feedback for copy action
            var originalText = CopyButton.Text;
            var originalColor = CopyButton.BackgroundColor;
            CopyButton.Text = "✓ Copied!";
            CopyButton.BackgroundColor = Color.FromArgb("#2E7D32");

            await Task.Delay(1500);

            CopyButton.Text = originalText;
            CopyButton.BackgroundColor = originalColor;
        }
    }

    private async void OnCloseModalClicked(object sender, EventArgs e)
    {
        // Animate modal dismissal
        await Task.WhenAll(
            ModalOverlay.FadeTo(0, 200, Easing.CubicIn),
            ModalOverlay.ScaleTo(0.8, 200, Easing.CubicIn)
        );

        ModalOverlay.IsVisible = false;
        ModalOverlay.Opacity = 1;
        ModalOverlay.Scale = 1;

        _hasScanned = false;
        CameraView.IsDetecting = true;

        // Restart scanning animation
        StartScanningAnimation();
    }

    private async void GoBackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}