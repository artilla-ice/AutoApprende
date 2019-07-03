using SupportCar.Services;
using SupportCar.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SupportCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage: ContentPage
    {
        ScanViewModel viewModel;
        public string _component;
        public readonly string _cameraPermission;
        //private IDataBase DataBase { get; set; }

        ZXingDefaultOverlay overlay = new ZXingDefaultOverlay();
        ZXingScannerView scanView = new ZXingScannerView();

        public IDataBase DataBase;

        public ScanPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ScanViewModel();
            this.DataBase = new DataBase(true);

        }



        public async void OnScan(object sender, EventArgs e)
        {
            try
            {
                

                scanView = new ZXingScannerView()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    AutomationId = "zxingScannerView",
                };

                scanView.Options = new MobileBarcodeScanningOptions()
                {
                    UseFrontCameraIfAvailable = false,
                    TryHarder = true,
                    AutoRotate = false,
                    TryInverted = true,
                    DelayBetweenContinuousScans = 2000
                };

                overlay = new ZXingDefaultOverlay
                {
                    TopText = "AUTOAPPRENDE",
                    BottomText = "Sostén la cámara frente al código QR",
                    ShowFlashButton = true,
                    AutomationId = "zxingDefaultOverlay",
                    
                };

                var scanPage = new ZXingScannerPage(scanView.Options, overlay);

                overlay.FlashButtonClicked += (s, ed) =>
                {
                    scanPage.ToggleTorch();
                };

                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = true;
                    
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if(result!= null)
                        {
                            await Navigation.PopAsync();
                            this._component = result.Text;

                            var conn = this.DataBase.GetConnectionAsync();
                            var item = this.DataBase.GetItem(this._component, conn);

                            if (item == null)
                            {
                                var newItem = this.DataBase.GetInfo(result.Text, conn);

                                
                                 if(newItem==null)
                                    await DisplayAlert("Error", "Código no válido", "INTENTAR DE NUEVO");
                                 else
                                {
                                    await DisplayAlert("Encontrado", newItem.Name, "VER INFORMACIÓN");
                                    var page = new ServicesPage(null, null, newItem);
                                    var contentPage = new ContentPage()
                                    {
                                        Content = page
                                    };
                                    if (contentPage != null)
                                        await Navigation.PushModalAsync(contentPage);

                                }
                            }
                            else
                            {
                                await DisplayAlert("Encontrado", this._component, "VER INFORMACIÓN");
                                ScanItemPage scanItemPage = new ScanItemPage(item)
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    VerticalOptions = LayoutOptions.FillAndExpand
                                };
                                var page = new ContentPage()
                                {
                                    Content = scanItemPage
                                };
                                await Navigation.PushModalAsync(page);
                            }

                        }else
                            await DisplayAlert("Error", "Código no válido", "INTENTAR DE NUEVO");
                    });
                };
                await Navigation.PushAsync(scanPage);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async void OnTest(object sender, EventArgs e)
        {
            var testItem = "Condensador (Aire Acondicionado)";

            var conn = this.DataBase.GetConnectionAsync();
            var item = this.DataBase.GetItem(testItem, conn);

            if(item != null)
            {
                await DisplayAlert("Encontrado", item.Name, "VER INFORMACIÓN");
                ScanItemPage scanItemPage = new ScanItemPage(item)
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var page = new ContentPage()
                {
                    Content = scanItemPage
                };
                await Navigation.PushModalAsync(page);
            }
            

        }
    }

}