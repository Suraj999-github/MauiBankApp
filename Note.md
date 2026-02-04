Create default views designs for these pages with default samples in views 

ProfilePage
SendTransactionPage
TransactionHistoryPage
QRCodePage
BalancePage
MobileTopUpPage
WaterBillPage
ElectricityBillPage
SecuritySettingsPage
\

keytool -genkeypair ^
-alias MauiAlias ^
-keyalg RSA ^
-keysize 2048 ^
-validity 10000 ^
-keystore D:\MAUI\MauiBankApp\maui-release.keystore ^
-storetype PKCS12

When prompted:

Keystore password → Test@1212

Key password → press ENTER (same as keystore)

Name / org → anything (not important)

✅ This keystore WILL work with MAUI.

Step 2: Verify the keystore (important)
keytool -list -v -keystore D:\MAUI\MauiBankApp\maui-release.keystore


dotnet clean
dotnet publish -f net8.0-android -c Release

dotnet clean
dotnet publish -f net8.0-android -c Release






keytool -genkeypair -v -keystore mauibank.keystore -alias mauibank \
-keyalg RSA -keysize 2048 -validity 10000


Create a Signed and Publishable .NET MAUI Android App in VS2022
keytool -genkey -v -keystore key.keystore -alias MauiAlias -keyalg RSA -keysize 2048 -validity 10000 
explorer .

dotnet publish -f:net8.0-android -c Release /p:AndroidKeyStore=true /p:AndroidSigningKeyStore="D:\MAUI\MauiBankApp\key.keystore" /p:AndroidSigningKeyAlias="mauibank" /p:AndroidSigningKeyPass="Test@1212" /p:AndroidSigningStorePass="Test@1212" /p:AndroidPackageFormat=apk


dotnet publish -f:net8.0-android -c Release -o ./publish /p:AndroidPackageFormat=aab /p:AndroidSigningKeyStore=mykeystore.keystore /p:AndroidSigningKeyAlias=myalias /p:AndroidSigningKeyPass=1234 /p:AndroidSigningStorePass=1234


cd D:\MAUI\MauiBankApp

dotnet publish -f:net8.0-android -c Release -o ./publish `
/p:AndroidPackageFormat=apk `
/p:AndroidSigningKeyStore=D:\MAUI\MauiBankApp\key.keystore `
/p:AndroidSigningKeyAlias=MauiAlias `
/p:AndroidSigningKeyPass=Test@1212 `
/p:AndroidSigningStorePass=Test@1212

dotnet workload install maui
dotnet workload install android



cd D:\MAUI\MauiBankApp
dotnet publish -f:net8.0-android -c Release -o .\publish /p:AndroidPackageFormat=apk /p:AndroidSigningKeyStore=D:\MAUI\MauiBankApp\key.keystore /p:AndroidSigningKeyAlias=MauiAlias /p:AndroidSigningKeyPass=MauiAlias@1212 /p:AndroidSigningStorePass=Test@1212
