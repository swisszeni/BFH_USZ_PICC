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
    public sealed partial class BandageChangingEntryPage : BaseContentPage
    {
        public BandageChangingEntryPage(ContentPage contained) : base(contained)
        {
            InitializeComponent();
            AddHealthInstitutionsAndHealthPeopleToPicker();
            BindingContext = new BandageChangingViewModel(null);
            
        }

        public BandageChangingEntryPage(ContentPage contained, BandageChangingEntry entry) : base(contained)
        {
            InitializeComponent();
            AddHealthInstitutionsAndHealthPeopleToPicker();
            BindingContext = new BandageChangingViewModel(entry);

        }

        void AddHealthInstitutionsAndHealthPeopleToPicker()
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

            BandageChangingReasonPicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonRoutineText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonPunctureNotCoveredText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonBandageWetText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonBandageDoesNotStickAnymoreText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonSecondaryBleedingText);
            BandageChangingReasonPicker.Items.Add(AppResources.BandageChangingReasonPainText);

            BandageChangingAreaPicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            BandageChangingAreaPicker.Items.Add(AppResources.BandageChangingAreaCompleteText);
            BandageChangingAreaPicker.Items.Add(AppResources.BandageChangingAreaOnlyBandageText);
            BandageChangingAreaPicker.Items.Add(AppResources.BandageChangingAreaOnlyStatlockText);

            BandageChangingPuncturePicker.Items.Add(AppResources.JournalEntryNotSpecifiedText);
            BandageChangingPuncturePicker.Items.Add(AppResources.BandagePunctureSituationSkinNotIrritantText);
            BandageChangingPuncturePicker.Items.Add(AppResources.BandagePunctureSituationReddenedPunctureText);
            BandageChangingPuncturePicker.Items.Add(AppResources.BandagePunctureSituationSwollenPunctureText);
            BandageChangingPuncturePicker.Items.Add(AppResources.BandagePunctureSituationPainfulPunctureText);
            BandageChangingPuncturePicker.Items.Add(AppResources.BandagePunctureSituationLiquidDischargeText);

        }

    }
}
