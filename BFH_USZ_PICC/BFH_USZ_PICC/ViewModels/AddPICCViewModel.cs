﻿using BFH_USZ_PICC.Models;
using BFH_USZ_PICC.Resx;
using BFH_USZ_PICC.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace BFH_USZ_PICC.ViewModels
{
    public class AddPICCViewModel : ViewModelBase
    {
        /// <summary>
        /// Get all possible PICC models. If the user searches a model, the getter compares the input string with all models
        /// and provides a selection of all models which contain the entered input.
        /// </summary>
        private ObservableCollection<PICCModel> _piccModels = new ObservableCollection<PICCModel>(PICCModel.AllModels());
        public ObservableCollection<PICCModel> PICCModels
        {
            get
            {
                ObservableCollection<PICCModel> theCollection = new ObservableCollection<PICCModel>();
                List<PICCModel> entities = (from e in _piccModels
                                            where e.PICCName.ToLower().Contains(SearchedPICCText.ToLower())
                                            select e).ToList();
                if (entities != null && entities.Any())
                {
                    theCollection = new ObservableCollection<PICCModel>(entities);
                }
                return theCollection;
            }

        }

        /// <summary>
        /// Propterty for the search bar.
        /// </summary>
        private string _searchedPICCText = string.Empty;
        public string SearchedPICCText
        {
            get { return _searchedPICCText; }
            set
            {
                if (Set(ref _searchedPICCText, value ?? string.Empty))
                {
                    RaisePropertyChanged("");
                }
            }
        }

        private PICCModel _selectedPicc;
        public PICCModel SelectedPicc
        {
            get { return _selectedPicc; }
            set
            {
                if (value != null)
                {
                    Set(ref _selectedPicc, value);
                    ((Shell)Application.Current.MainPage).Detail.Navigation.PushAsync(new BasePage(typeof(PICCDetailPage), new List<object> { value }));
                }
            }
        }

        private RelayCommand _piccSearchButtonCommand;
        public RelayCommand PiccSearchButtonCommand => _piccSearchButtonCommand ?? (_piccSearchButtonCommand = new RelayCommand(() =>
       {
           searchForAPiccModel(SearchedPICCText);

       }));


        private RelayCommand _addPiccManualButtonCommand;
        public RelayCommand AddPiccManualButtonCommand => _addPiccManualButtonCommand ?? (_addPiccManualButtonCommand = new RelayCommand(async () =>
        {
            PICCModel model = new PICCModel(null, 0, 0, 0, null, null, null, null);
            await ((Shell)Application.Current.MainPage).Detail.Navigation.PushAsync(new BasePage(typeof(PICCDetailPage), new List<object> { model }));
        }));

        private RelayCommand _scanButtonCommand;
        public RelayCommand ScanButtonCommand => _scanButtonCommand ?? (_scanButtonCommand = new RelayCommand(async () =>
        {
            var scanPage = new ZXingScannerPage();

            //Disable the FlashButton
            scanPage.DefaultOverlayShowFlashButton = false;

            //If the scaner has a result: stop scanning, close the lpage, check if the result is not null and give the result to the "searchForAPiccModel" method. 
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    ((Shell)Application.Current.MainPage).Detail.Navigation.PopAsync();
                    if (result != null)
                    {
                        searchForAPiccModel(result.Text);
                    }
                });
            };

            //Opens the scanPage with the parameters set above

            await ((Shell)Application.Current.MainPage).Detail.Navigation.PushAsync(scanPage);


        }));


        private async void searchForAPiccModel(string nameOrBarcode)
        {
            foreach (PICCModel piccModel in PICCModels)
            {
                // if either the picc name or the barcode could be found in the database
                if ((string.Compare(piccModel.PICCName, nameOrBarcode, StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(piccModel.Barcode, nameOrBarcode, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    await ((Shell)Application.Current.MainPage).Detail.Navigation.PushAsync(new BasePage(typeof(PICCDetailPage), new List<object> { piccModel }));
                    return;
                }
            }

            if (await ((Shell)Application.Current.MainPage).DisplayAlert(AppResources.InformationText, AppResources.AddPICCPagePICCNotFoundInformationText, AppResources.AddPICCPageAddPiccManualButtonText, AppResources.OkButtonText))
            {

                // User wants to create model manually, create a new model with the searchterm preset as text
                PICCModel model = new PICCModel(null, 0, 0, 0, null, null, null, null);
                await ((Shell)Application.Current.MainPage).Detail.Navigation.PushAsync(new BasePage(typeof(PICCDetailPage), new List<object> { model }));
            }
        }

    }
}
