namespace MauiBankApp.Controls
{
    public partial class FingerprintScannerControl : ContentView
    {
        public static readonly BindableProperty IsScanningProperty =
            BindableProperty.Create(nameof(IsScanning), typeof(bool), typeof(FingerprintScannerControl), false, propertyChanged: OnIsScanningChanged);

        public static readonly BindableProperty StatusTextProperty =
            BindableProperty.Create(nameof(StatusText), typeof(string), typeof(FingerprintScannerControl), "Place your finger");

        public static readonly BindableProperty StatusTextColorProperty =
            BindableProperty.Create(nameof(StatusTextColor), typeof(Color), typeof(FingerprintScannerControl), Colors.Gray);

        public static readonly BindableProperty ScannerBackgroundColorProperty =
            BindableProperty.Create(nameof(ScannerBackgroundColor), typeof(Color), typeof(FingerprintScannerControl), Color.FromArgb("#F5F5F5"));

        public static readonly BindableProperty IconOpacityProperty =
            BindableProperty.Create(nameof(IconOpacity), typeof(double), typeof(FingerprintScannerControl), 1.0);

        public static readonly BindableProperty StatusIconTextProperty =
            BindableProperty.Create(nameof(StatusIconText), typeof(string), typeof(FingerprintScannerControl), "");

        public bool IsScanning
        {
            get => (bool)GetValue(IsScanningProperty);
            set => SetValue(IsScanningProperty, value);
        }

        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            set => SetValue(StatusTextProperty, value);
        }

        public Color StatusTextColor
        {
            get => (Color)GetValue(StatusTextColorProperty);
            set => SetValue(StatusTextColorProperty, value);
        }

        public Color ScannerBackgroundColor
        {
            get => (Color)GetValue(ScannerBackgroundColorProperty);
            set => SetValue(ScannerBackgroundColorProperty, value);
        }

        public double IconOpacity
        {
            get => (double)GetValue(IconOpacityProperty);
            set => SetValue(IconOpacityProperty, value);
        }

        public string StatusIconText
        {
            get => (string)GetValue(StatusIconTextProperty);
            set => SetValue(StatusIconTextProperty, value);
        }

        private CancellationTokenSource? _animationCancellation;

        public FingerprintScannerControl()
        {
            InitializeComponent();
        }

        private static void OnIsScanningChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FingerprintScannerControl control)
            {
                if ((bool)newValue)
                {
                    control.StartScanAnimation();
                }
                else
                {
                    control.StopScanAnimation();
                }
            }
        }

        private void StartScanAnimation()
        {
            _animationCancellation?.Cancel();
            _animationCancellation = new CancellationTokenSource();

            StatusText = "Scanning...";
            StatusTextColor = Color.FromArgb("#1E88E5");
            ScannerBackgroundColor = Color.FromArgb("#E3F2FD");

            // Start pulse animation
            AnimatePulseRings(_animationCancellation.Token);

            // Start scan line animation
            AnimateScanLine(_animationCancellation.Token);
        }

        private void StopScanAnimation()
        {
            _animationCancellation?.Cancel();
        }

        private async void AnimatePulseRings(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Pulse ring 1
                    var pulse1 = PulseRing1.ScaleTo(1.15, 1000, Easing.SinInOut);
                    var fade1 = PulseRing1.FadeTo(0.2, 1000, Easing.SinInOut);

                    // Pulse ring 2 (delayed)
                    await Task.Delay(200, cancellationToken);
                    var pulse2 = PulseRing2.ScaleTo(1.15, 1000, Easing.SinInOut);
                    var fade2 = PulseRing2.FadeTo(0.1, 1000, Easing.SinInOut);

                    // Pulse ring 3 (delayed)
                    await Task.Delay(200, cancellationToken);
                    var pulse3 = PulseRing3.ScaleTo(1.15, 1000, Easing.SinInOut);
                    var fade3 = PulseRing3.FadeTo(0.05, 1000, Easing.SinInOut);

                    await Task.WhenAll(pulse1, fade1, pulse2, fade2, pulse3, fade3);

                    // Reset
                    PulseRing1.Scale = 1;
                    PulseRing1.Opacity = 0.6;
                    PulseRing2.Scale = 1;
                    PulseRing2.Opacity = 0.4;
                    PulseRing3.Scale = 1;
                    PulseRing3.Opacity = 0.2;

                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // Animation cancelled, reset rings
                PulseRing1.Scale = 1;
                PulseRing1.Opacity = 0.6;
                PulseRing2.Scale = 1;
                PulseRing2.Opacity = 0.4;
                PulseRing3.Scale = 1;
                PulseRing3.Opacity = 0.2;
            }
        }

        private async void AnimateScanLine(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Move scan line from top to bottom
                    ScanLine.TranslationY = -80;
                    await ScanLine.TranslateTo(0, 80, 2000, Easing.Linear);

                    // Small pause at bottom
                    await Task.Delay(100, cancellationToken);

                    // Reset to top
                    ScanLine.TranslationY = -80;
                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // Animation cancelled
                ScanLine.TranslationY = 0;
            }
        }

        public async Task ShowSuccessAsync()
        {
            IsScanning = false;

            // Change colors
            ScannerBackgroundColor = Color.FromArgb("#E8F5E9");
            StatusTextColor = Color.FromArgb("#4CAF50");
            StatusText = "Success!";

            // Show success icon
            StatusIconText = "✓";
            StatusIcon.IsVisible = true;

            // Animate success
            await FingerprintIcon.FadeTo(0, 200);
            await StatusIcon.FadeTo(1, 300);
            await StatusIcon.ScaleTo(1.2, 200);
            await StatusIcon.ScaleTo(1, 200);

            await Task.Delay(1000);
        }

        public async Task ShowErrorAsync(string message = "Authentication failed")
        {
            IsScanning = false;

            // Change colors
            ScannerBackgroundColor = Color.FromArgb("#FFEBEE");
            StatusTextColor = Color.FromArgb("#F44336");
            StatusText = message;

            // Show error icon
            StatusIconText = "✗";
            StatusIcon.IsVisible = true;

            // Animate error with shake
            await FingerprintIcon.FadeTo(0, 200);
            await StatusIcon.FadeTo(1, 300);

            // Shake animation
            for (int i = 0; i < 3; i++)
            {
                await StatusIcon.TranslateTo(-10, 0, 50);
                await StatusIcon.TranslateTo(10, 0, 50);
            }
            await StatusIcon.TranslateTo(0, 0, 50);

            await Task.Delay(1500);
        }

        public async Task ResetAsync()
        {
            // Reset all properties
            IsScanning = false;
            StatusText = "Place your finger";
            StatusTextColor = Colors.Gray;
            ScannerBackgroundColor = Color.FromArgb("#F5F5F5");
            StatusIconText = "";
            StatusIcon.IsVisible = false;

            // Reset icon visibility
            await StatusIcon.FadeTo(0, 200);
            await FingerprintIcon.FadeTo(1, 300);

            StatusIcon.TranslationX = 0;
            StatusIcon.TranslationY = 0;
            StatusIcon.Scale = 1;
        }
    }
}
