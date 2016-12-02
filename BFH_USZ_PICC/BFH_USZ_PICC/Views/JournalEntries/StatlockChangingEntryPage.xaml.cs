﻿using BFH_USZ_PICC.Models;
using System;
using Xamarin.Forms;
using static BFH_USZ_PICC.Models.JournalEntry;
using BFH_USZ_PICC.Resx;
using BFH_USZ_PICC.ViewModels.JournalEntries;



// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace BFH_USZ_PICC.Views.JournalEntries
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class StatlockChangingEntryPage : BaseContentPage
    {
        public StatlockChangingEntryPage(ContentPage contained) : base(contained)
        {
            InitializeComponent();
            AddPickers();
            BindingContext = new StatlockChangingViewModel(null);
            
        }

        public StatlockChangingEntryPage(ContentPage contained, StatlockChangingEntry entry) : base(contained)
        {
            InitializeComponent();
            AddPickers();
            BindingContext = new StatlockChangingViewModel(entry);

        }

        void AddPickers()
        {
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryInstitutionHospitalText);
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryInstitutionOutpatienClinicText);
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryInstitutionRehabilitationText);
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryInstitutionHomeCareText);
            HealthInstitutionPicker.Items.Add(AppResources.JournalEntryOthersText);

            HealthPersonPicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonFamilyDoctorText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonSpecialistText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonNursingStaffText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonXrayStaffText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonMedicalTechnicalAssistantText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonHealthExpertStaffText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonRelativeText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryPersonAffectedPersonText);
            HealthPersonPicker.Items.Add(AppResources.JournalEntryOthersText);

            StatlockChangingReasonPicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            StatlockChangingReasonPicker.Items.Add(AppResources.StatlockChangingEntryRoutineText);
            StatlockChangingReasonPicker.Items.Add(AppResources.StatlockChangingEntrySticksUnsatisfactorilyText);
            StatlockChangingReasonPicker.Items.Add(AppResources.StatlockChangingEntryPollutionText);
            StatlockChangingReasonPicker.Items.Add(AppResources.StatlockChangingEntryDamagedWingsText);

        }

    }
}