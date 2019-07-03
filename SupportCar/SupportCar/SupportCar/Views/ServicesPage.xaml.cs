using Android.Content.Res;
using SupportCar.Droid;
using SupportCar.Models;
using SupportCar.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SupportCar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesPage : ContentView
    {
        public WebView m_webView;
        public ServicesPage() { }

        public ServicesPage(CarService carService = null, CarSystem carSystem = null, Info info = null)
        {
            try
            {
                InitializeComponent();
                this.m_webView = new WebView();
                if(carService!=null)
                    this.m_webView.Source = "file:///android_asset/CarServices/" + carService.FolderName + "/sitebrand.html";

                if (carSystem != null)
                    this.m_webView.Source = "file:///android_asset/CarSystems/" + carSystem.FolderName + "/sitebrand.html";

                if(carService==null && carSystem==null && info != null)
                    this.m_webView.Source = "file:///android_asset/Info/" + info.FolderPath + "/sitebrand.html";

                if (this.m_webView != null && this.m_webView.Source != null)
                    this.webView.Source = this.m_webView.Source;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            

        }
    }
}