﻿using BFH_USZ_PICC.Models;
using BFH_USZ_PICC.Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static BFH_USZ_PICC.Models.JournalEntry;

namespace BFH_USZ_PICC.ViewModels.JournalEntries
{
    class InfusionViewModel : INotifyPropertyChanged
    {
        private InfusionEntry _selectedEntry;
        
        public InfusionViewModel(InfusionEntry entry)
        {
            if (entry == null)
            {
                IsEnabledOrVisible = true;
                ProcedureDate = DateTime.Now;
            }
            else
            {
                Person = entry.Person;
                Institution = entry.Institution;
                ProcedureDate = entry.ProcedureDateTime;
                InfusionType = entry.Type;
                AntibioticName = entry.TypeAntibioticName;
                InfusionAdministration = entry.Administration;

                _selectedEntry = entry;
                
                IsEnabledOrVisible = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a binded property has been changed and fires the event
        /// </summary>
        /// <param name="propertyname"></param>
        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private HealthPerson _person;
        public HealthPerson Person
        {
            get { return _person; }
            set
            {
                if (_person != value)
                {
                    _person = value;
                    OnPropertyChanged("Person");
                }
            }
        }

        private HealthInstitution _institution;
        public HealthInstitution Institution
        {
            get { return _institution; }
            set
            {
                if (_institution != value)
                {
                    _institution = value;
                    OnPropertyChanged("Institution");
                }
            }
        }

        private DateTime _procedureDate;
        public DateTime ProcedureDate
        {
            get { return _procedureDate; }
            set
            {
                if (_procedureDate != value)
                {
                    _procedureDate = value;
                    OnPropertyChanged("ProcedureDate");
                }
            }
        }
        
        private InfusionType _infusionType;
        public InfusionType InfusionType
        {
            get { return _infusionType; }
            set
            {
                if (_infusionType != value)
                {
                    _infusionType = value;
                    
                    if(value == InfusionType.Antibiotic)
                    {
                        IsTypeAntibiotic = true;
                    }else { IsTypeAntibiotic = false; }

                    OnPropertyChanged("InfusionType");
                }
            }
        }

        private string _antibioticName;
        public string AntibioticName
        {
            get { return _antibioticName; }
            set
            {
                if (_antibioticName != value)
                {
                    _antibioticName = value;
                    OnPropertyChanged("AntibioticName");
                }
            }
        }

        private InfusionAdministration _infusionAdministration;
        public InfusionAdministration InfusionAdministration
        {
            get { return _infusionAdministration; }
            set
            {
                if (_infusionAdministration != value)
                {
                    _infusionAdministration = value;
                    OnPropertyChanged("InfusionAdministration");
                }
            }
        }

        private bool _isEnabledOrVisible;
        public bool IsEnabledOrVisible
        {
            get { return _isEnabledOrVisible; }
            set
            {
                _isEnabledOrVisible = value;
                OnPropertyChanged("IsEnabledOrVisible");
            }
        }

        private bool _isTypeAntibiotic;
        public bool IsTypeAntibiotic
        {
            get { return _isTypeAntibiotic; }
            set
            {
                if (!value)
                {
                    AntibioticName = null;
                }
                _isTypeAntibiotic = value;
                OnPropertyChanged("IsTypeAntibiotic");
            }
        }

        private ICommand _saveButtonCommand;
        public ICommand SaveButtonCommand => _saveButtonCommand ?? (_saveButtonCommand = new Command(async () => {
            // create a new PICCAppliedDrugEntry with the user entered information
            InfusionEntry infusionEntry = new InfusionEntry(DateTime.Now, ProcedureDate, Institution, Person, InfusionType, 
                InfusionAdministration, AntibioticName);
            //Add the object to the collection of JournalEntries
            JournalEntry.AllEnteredJournalEntries.Add(infusionEntry);
            //close the page
            await ((Shell)Application.Current.MainPage).Detail.Navigation.PopAsync();
        }));

        private ICommand _cancelButtonCommand;
        public ICommand CancelButtonCommand => _cancelButtonCommand ?? (_cancelButtonCommand = new Command(async () =>
        {
            //Check if the user really wants to leave the page
            if (await Application.Current.MainPage.DisplayAlert("Warnung!", "Wollen Sie die Eingabe wirklich abbrechen?", "Ja", "Nein"))
            {
                await ((Shell)Application.Current.MainPage).Detail.Navigation.PopAsync();
            }
        }));

        private ICommand _deleteButtonCommand;
        public ICommand DeleteButtonCommand => _deleteButtonCommand ?? (_deleteButtonCommand = new Command(async () => {
            if (await Application.Current.MainPage.DisplayAlert("Warnung!", "Wollen Sie den Eintrag wirklich löschen?", "Ja", "Nein"))
            {
                JournalEntry.AllEnteredJournalEntries.Remove(_selectedEntry);
                await((Shell)Application.Current.MainPage).Detail.Navigation.PopAsync();
            }
        }));

    }
}