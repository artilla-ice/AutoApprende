using Android;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZXing.Mobile;
using ZXing.Net;
using ZXing.QrCode;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using SupportCar.Views;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace SupportCar.ViewModels
{
    public class ScanViewModel:ContentPage
    {
        #region DECLARATIONS
        public string _component;
        
        

        #endregion


        public ScanViewModel()
        {
        }

       
        public string GetComponent()
        {
            return this._component;
        }

        

    }
}
