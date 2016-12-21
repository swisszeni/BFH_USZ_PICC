﻿using BFH_USZ_PICC.Models;
using BFH_USZ_PICC.Resx;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFH_USZ_PICC.Interfaces;
using Realms;
using Xamarin.Forms;
using Microsoft.Practices.ServiceLocation;

namespace BFH_USZ_PICC.ViewModels
{
    public class MasterDataViewModel : ViewModelBase
    {
        private ILocalUserDataService _dataService;
        private UserMasterData _displayingmasterData;

        public MasterDataViewModel()
        {
            // Getting the dataservice
            _dataService = ServiceLocator.Current.GetInstance<ILocalUserDataService>();

            PopulateMasterDataAsync();
        }

        private async void PopulateMasterDataAsync()
        {
            var masterDataEntries = await _dataService.GetMasterDataAsync();

            if (masterDataEntries.Count > 0)
            {
                _displayingmasterData = masterDataEntries.First();
            } else
            {
                // There is no existing MasterData, create one!
                _displayingmasterData = new UserMasterData();
            }

            LoadFromModel();
        }

        //private void GetOrCreateMasterData_Realm()
        //{
        //    _realm = Realm.GetInstance();

        //    // Get the MasterData. There should be only one instance of it. If the App is started for the first time, there is none.
        //    var masterDataEntries = _realm.All<UserMasterData>();
        //    if (masterDataEntries.Count() > 0)
        //    {
        //        _displayingmasterData = masterDataEntries.First();
        //    }
        //    else
        //    {
        //        // There is no existing MasterData, create one!
        //        _displayingmasterData = new UserMasterData();
        //        _realm.Write(() =>
        //        {
        //            _realm.Add(_displayingmasterData);
        //        });
        //    }
        //}

        //private void GetOrCreateMasterData_SQLite()
        //{
        //    _displayingmasterData = new UserMasterData();
        //}

        //private void DeleteMasterData_Realm()
        //{
        //    _realm.Write(() =>
        //    {
        //        _realm.RemoveAll<UserMasterData>();
        //    });
        //}

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter is List<object> && ((List<object>)parameter).Count > 0)
            {
                // Passes if should go in edit mode or not
                if((bool)((List<object>)parameter).First())
                {
                    StartEditing();
                }
            }

            LoadFromModel();

            // Return "fake task" since Task.CompletedTask is not supported in this PCL
            return Task.FromResult(false);
        }

        public override Task OnNavigatedFromAsync()
        {
            EndEditing();

            // Return "fake task" since Task.CompletedTask is not supported in this PCL
            return Task.FromResult(false);
        }

        private void StartEditing()
        {
            IsUserInputEnabled = true;
        }

        private void EndEditing()
        {
            IsUserInputEnabled = false;
        }

        private void LoadFromModel()
        {
            Gender = _displayingmasterData == null ? 0 : _displayingmasterData.Gender;
            Name = _displayingmasterData?.Name;
            Surname = _displayingmasterData?.Surname;
            Street = _displayingmasterData?.Street;
            ZIP = _displayingmasterData?.Zip;
            City = _displayingmasterData?.City;
            Email = _displayingmasterData?.Email;
            Phone = _displayingmasterData?.Phone;
            Mobile = _displayingmasterData?.Mobile;

            if(_displayingmasterData?.Birthdate == null)
            {
                Birthdate = DateTimeOffset.Now;
                IsBirthdateSet = false;
            } else
            {
                Birthdate = (DateTimeOffset)_displayingmasterData.Birthdate;
                IsBirthdateSet = true;
            }

            // Actually, this shoudln't be needed... but because of some weird timing, it is.
            RaisePropertyChanged("");
        }

        private void SaveToModel()
        {
            _displayingmasterData.Gender = Gender;
            _displayingmasterData.Name = Name;
            _displayingmasterData.Surname = Surname;
            _displayingmasterData.Street = Street;
            _displayingmasterData.Zip = ZIP;
            _displayingmasterData.City = City;
            _displayingmasterData.Email = Email;
            _displayingmasterData.Phone = Phone;
            _displayingmasterData.Mobile = Mobile;

            if (IsBirthdateSet)
            {
                _displayingmasterData.Birthdate = Birthdate;
            }
            else
            {
                _displayingmasterData.Birthdate = null;
            }

            // Save to DB
            _dataService.SaveMasterDataAsync(_displayingmasterData);
        }

        private bool _isUserInputEnabled;
        public bool IsUserInputEnabled
        {
            get { return _isUserInputEnabled; }
            set
            {
                Set(ref _isUserInputEnabled, value);
                RaisePropertyChanged(() => IsBirthdateDisplayed);
            }
        }


        private Gender _gender;
        public Gender Gender
        {
            get { return _gender; }
            set { Set(ref _gender, value); }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set { Set(ref _surname, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _street;
        public string Street
        {
            get { return _street; }
            set { Set(ref _street, value); }
        }

        private string _zip;
        public string ZIP
        {
            get { return _zip; }
            set { Set(ref _zip, value); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { Set(ref _city, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { Set(ref _mobile, value); }
        }

        private DateTimeOffset _birthdate;
        public DateTimeOffset Birthdate
        {
            get { return (!IsBirthdateSet || _birthdate.Year >= DateTimeOffset.Now.Year) ? DateTimeOffset.Now : _birthdate; }
            set
            {
                if (value.Year >= DateTimeOffset.Now.Year)
                {
                    Set(ref _birthdate, value);
                    return;
                }

                IsBirthdateSet = true;
                Set(ref _birthdate, value);
            }
        }

        public bool IsBirthdateDisplayed
        {
            get { return IsBirthdateSet || IsUserInputEnabled; }
        }

        private bool _isBirthdateSet;
        public bool IsBirthdateSet
        {
            get { return _isBirthdateSet; }
            set {
                Set(ref _isBirthdateSet, value);
                RaisePropertyChanged(() => IsBirthdateDisplayed);
            }
        }

        private RelayCommand _startEditCommand;
        public RelayCommand StartEditCommand => _startEditCommand ?? (_startEditCommand = new RelayCommand(() =>
        {
            StartEditing();
        }));

        private RelayCommand _cancelEditCommand;
        public RelayCommand CancelEditCommand => _cancelEditCommand ?? (_cancelEditCommand = new RelayCommand(async () =>
        {
            if (await Application.Current.MainPage.DisplayAlert(AppResources.WarningText, AppResources.CancelButtonPressedConfirmationText, AppResources.YesButtonText, AppResources.NoButtonText))
            {
                LoadFromModel();
                EndEditing();
            }
        }));

        private RelayCommand _saveEditCommand;
        public RelayCommand SaveEditCommand => _saveEditCommand ?? (_saveEditCommand = new RelayCommand(async () =>
        {
            bool saveInput = true;

            if (Birthdate.Year >= DateTimeOffset.Now.Year && IsBirthdateSet)
            {   
                if (!await Application.Current.MainPage.DisplayAlert(AppResources.WarningText, AppResources.MasterDataViewModelBirthdateNotValidText, AppResources.YesButtonText, AppResources.NoButtonText))
                {
                    saveInput = false;
                    IsBirthdateSet = false;
                };
            }
            if (saveInput) {
                EndEditing();
                SaveToModel();
            }
        }));

        private RelayCommand _resetMasterDataCommand;
        public RelayCommand ResetMasterDataCommand => _resetMasterDataCommand ?? (_resetMasterDataCommand = new RelayCommand(async () =>
        {
            if (await Application.Current.MainPage.DisplayAlert(AppResources.WarningText, AppResources.UserMasterDataPageDelteAllPersonalDataText, AppResources.YesButtonText, AppResources.NoButtonText))
            {
                await _dataService.DeleteAllMasterDataAsync();
                _displayingmasterData = new UserMasterData();
                LoadFromModel();
            }
        }));
    }
}
