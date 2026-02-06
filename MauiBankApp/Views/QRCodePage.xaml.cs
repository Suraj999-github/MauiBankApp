using ZXing.Net.Maui;

namespace MauiBankApp.Views;

public partial class QRCodePage : ContentPage
{
    private bool _hasScanned;
    private string _scannedData = string.Empty;

    public QRCodePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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
            ShowSuccess(barcode.Value, barcode.Format.ToString());
        });
    }

    private void ShowSuccess(string data, string format)
    {
        SuccessHeader.IsVisible = true;
        ErrorHeader.IsVisible = false;

        InfoLabel.Text = "Scanned Data:";
        InfoLabel.IsVisible = true;

        ResultLabel.Text = data;

        FormatLayout.IsVisible = true;
        FormatLabel.Text = format;

        CopyButton.IsVisible = true;
        ModalOverlay.IsVisible = true;
    }

    private void ShowError(string message)
    {
        ErrorHeader.IsVisible = true;
        SuccessHeader.IsVisible = false;

        InfoLabel.Text = message;
        InfoLabel.IsVisible = true;

        ModalOverlay.IsVisible = true;
    }

    private async void OnCopyClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_scannedData))
            await Clipboard.SetTextAsync(_scannedData);
    }

    private void OnCloseModalClicked(object sender, EventArgs e)
    {
        ModalOverlay.IsVisible = false;
        _hasScanned = false;
        CameraView.IsDetecting = true;
    }

    private async void GoBackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
