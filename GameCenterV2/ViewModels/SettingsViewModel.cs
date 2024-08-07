using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameCenterV2.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> availablePrinters;
        [ObservableProperty]
        private string selectedPrinter = null;
       
        public SettingsViewModel()
        {
            AvailablePrinters = new ObservableCollection<string>();
            LoadPrinters();
            LoadSavedPrinter();
        }
        partial void OnSelectedPrinterChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                SaveSelectedPrinter(value);
            }
        }
        [RelayCommand]
        private void LoadPrinters()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                AvailablePrinters.Clear();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    AvailablePrinters.Add(printer);
                }
            }
            //else
            //    AvailablePrinters.Add("هذه الميزة فقط لنظام التشغيل ويندوز");
        }

        private void SaveSelectedPrinter(string printerName)
        {
            Preferences.Set("SelectedPrinter", printerName);
        }

        private void LoadSavedPrinter()
        {
            var printername = Preferences.Get("SelectedPrinter", string.Empty);
            if (!string.IsNullOrEmpty(printername))
            {
                SelectedPrinter = printername;
            }
        }

        public static string GetSavedPrinterName()
        {
            return Preferences.Get("SelectedPrinter", string.Empty);
        }
    }
}
