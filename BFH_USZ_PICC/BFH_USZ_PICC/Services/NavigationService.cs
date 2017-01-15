﻿using BFH_USZ_PICC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFH_USZ_PICC.Models;
using BFH_USZ_PICC.ViewModels;
using BFH_USZ_PICC.Views;
using BFH_USZ_PICC.ViewModels.JournalEntries;
using BFH_USZ_PICC.Views.JournalEntries;
using Xamarin.Forms;
using BFH_USZ_PICC.Controls;

namespace BFH_USZ_PICC.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> _viewModelmappings;
        protected readonly Dictionary<MenuItemKey, Type> _menuKeymappings;

        public NavigationService()
        {
            _viewModelmappings = new Dictionary<Type, Type>();
            _menuKeymappings = new Dictionary<MenuItemKey, Type>();

            CreatePageViewModelMappings();
            CreateMenuKeyPageMappings();
        }

        #region INavigationService Implementation

        public Task InitializeAsync()
        {
            if (SettingsService.FirstAppExecution)
            {
                // first execution, navigate to onboarding
                // return NavigateToAsync<OnBoardingViewModel>();
                return NavigateToAsync<MainViewModel>();
            } else
            {
                // app has already run at least once, navigate to normal MainView
                return NavigateToAsync<MainViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return NavigateToAsync<TViewModel>(null);
        }

        public Task NavigateToAsync(MenuItemKey pageKey)
        {
            return NavigateToAsync(pageKey, null);
        }

        public Task NavigateToAsync<TViewModel>(List<object> navParams) where TViewModel : ViewModelBase
        {
            Type vmType = typeof(TViewModel);
            MenuItemKey menuKey = MenuItemKey.PICC;
            if (ViewModelIsMenuPage(vmType, ref menuKey))
            {
                return NavigateToAsync(menuKey, navParams);
            }

            return InternalNavigateToAsync(vmType, navParams);
        }

        public Task NavigateToAsync(MenuItemKey pageKey, List<object> navParams)
        {
            return InternalNavigateToMenuEntryAsync(pageKey, navParams);
        }

        public Task DeepNavigateToAsync(Type deepViewModelType, MenuItemKey basePageKey)
        {
            throw new NotImplementedException();
        }

        public Task DeepNavigateToAsync<TViewModel>(MenuItemKey basePageKey) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task DeepNavigateToAsync(Type deepViewModelType, MenuItemKey basePageKey, List<object> deepPagenavParams)
        {
            throw new NotImplementedException();
        }

        public Task DeepNavigateToAsync<TViewModel>(MenuItemKey basePageKey, List<object> deepPagenavParams) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveLastFromBackStackAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the mapping from a ViewModel to its corresponding View.
        /// This allows for navigation by ViewModel and thus loading different
        /// Views for the VM depending on the current Device OS or Idiom
        /// </summary>
        private void CreatePageViewModelMappings()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                _viewModelmappings.Add(typeof(MainViewModel), typeof(MainPage_iOS));
            } else
            {
                _viewModelmappings.Add(typeof(MainViewModel), typeof(MainPage));
            }

            _viewModelmappings.Add(typeof(OnBoardingViewModel), typeof(OnBoardingPage));

            // My PICC
            _viewModelmappings.Add(typeof(MyPICCViewModel), typeof(MyPICCPage));
            _viewModelmappings.Add(typeof(AddPICCViewModel), typeof(AddPICCPage));
            _viewModelmappings.Add(typeof(PICCDetailViewModel), typeof(PICCDetailPage));

            // Knowledge Base
            _viewModelmappings.Add(typeof(KnowledgeEntriesViewModel), typeof(KnowledgeEntriesPage));
            _viewModelmappings.Add(typeof(KnowledgeEntryDetailViewModel), typeof(KnowledgeEntryDetailPage));
            _viewModelmappings.Add(typeof(GlossaryViewModel), typeof(GlossaryPage));

            // Disorders
            _viewModelmappings.Add(typeof(DisorderViewModel), typeof(DisorderPage));
            _viewModelmappings.Add(typeof(DisorderDetailViewModel), typeof(DisorderDetailPage));

            // Journal
            _viewModelmappings.Add(typeof(JournalOverviewViewModel), typeof(JournalOverviewPage));
            _viewModelmappings.Add(typeof(AdministeredDrugViewModel), typeof(AdministeredDrugEntryPage));
            _viewModelmappings.Add(typeof(BandageChangingViewModel), typeof(BandageChangingEntryPage));
            _viewModelmappings.Add(typeof(BloodWithdrawalViewModel), typeof(BloodWithdrawalEntryPage));
            _viewModelmappings.Add(typeof(CatheterFlushViewModel), typeof(CatheterFlushEntryPage));
            _viewModelmappings.Add(typeof(InfusionViewModel), typeof(InfusionEntryPage));
            _viewModelmappings.Add(typeof(MicroClaveChangingViewModel), typeof(MicroClaveChangingEntryPage));
            _viewModelmappings.Add(typeof(StatlockChangingViewModel), typeof(StatlockChangingEntryPage));
            _viewModelmappings.Add(typeof(MaintenanceInstructionViewModel), typeof(MaintenanceInstructionPage));

            // Settings
            _viewModelmappings.Add(typeof(SettingsViewModel), typeof(SettingsPage));
            _viewModelmappings.Add(typeof(MasterDataViewModel), typeof(UserMasterDataPage));
        }

        private void CreateMenuKeyPageMappings()
        {
            _menuKeymappings.Add(MenuItemKey.PICC, typeof(MyPICCPage));
            _menuKeymappings.Add(MenuItemKey.Knowledge, typeof(KnowledgeEntriesPage));
            _menuKeymappings.Add(MenuItemKey.Disorder, typeof(DisorderPage));
            _menuKeymappings.Add(MenuItemKey.Journal, typeof(JournalOverviewPage));
            _menuKeymappings.Add(MenuItemKey.Settings, typeof(SettingsPage));
        }

        protected bool ViewModelIsMenuPage(Type viewModelType, ref MenuItemKey menuKey)
        {
            Type pageType;
            try
            {
                pageType = GetPageTypeForViewModel(viewModelType);
            } catch
            {
                return false;
            }

            if(pageType != null && _menuKeymappings.ContainsValue(pageType))
            {
                menuKey = _menuKeymappings.FirstOrDefault(x => x.Value == pageType).Key;
                return true;
            }

            return false;
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_viewModelmappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _viewModelmappings[viewModelType];
        }

        protected Type GetPageTypeForMenuKey(MenuItemKey key)
        {
            if (!_menuKeymappings.ContainsKey(key))
            {
                throw new KeyNotFoundException($"No map for ${key} was found on navigation mappings");
            }

            return _menuKeymappings[key];
        }

        protected virtual Task InternalNavigateToMenuEntryAsync(MenuItemKey key, List<object> navParams)
        {
            // Check if main navigation Structure exists
            var mainPage = Application.Current.MainPage as MainPage;
            if (mainPage == null)
            {
                throw new Exception("Need MainPage to navigate.");
            }

            // We want to navigate to one of the main menu entries
            // First check if we not already are on this menu entry
            Type pageType = GetPageTypeForMenuKey(key);
            if (pageType != ((mainPage.Detail as NavigationPage)?.CurrentPage as BasePage)?.GetContentType())
            {
                var page = new BasePage(pageType);
                (page.BindingContext as ViewModelBase)?.InitializeAsync(navParams);
                mainPage.Detail = new USZ_PICC_NavigationPage(page);
            }

            mainPage.IsPresented = false;

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, List<object> navParams)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            Page page = null;

            if (viewModelType == typeof(OnBoardingViewModel) || viewModelType == typeof(MainViewModel))
            {
                // We want to navigate to one of the most basic views within the App, set it as main page
                page = Activator.CreateInstance(pageType) as Page;
                Application.Current.MainPage = page;
            } else if (Application.Current.MainPage is MainPage)
            {
                // We don't want to navigate to one of the roots and the current root is the MainPage (Navigation Shell)
                page = new BasePage(pageType);
                var mainPage = Application.Current.MainPage as MainPage;
                var navigationPage = mainPage.Detail as USZ_PICC_NavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }

                // mainPage.IsPresented = false;
            }


            //Page page = CreateAndBindPage(viewModelType, parameter);

            //if (page is MainPage)
            //{
            //    CurrentApplication.MainPage = page;
            //}
            //else if (page is LoginPage)
            //{
            //    CurrentApplication.MainPage = new CustomNavigationPage(page);
            //}
            //else if (CurrentApplication.MainPage is MainPage)
            //{
            //    var mainPage = CurrentApplication.MainPage as MainPage;
            //    var navigationPage = mainPage.Detail as CustomNavigationPage;

            //    if (navigationPage != null)
            //    {
            //        await navigationPage.PushAsync(page);
            //    }
            //    else
            //    {
            //        navigationPage = new CustomNavigationPage(page);
            //        mainPage.Detail = navigationPage;
            //    }

            //    mainPage.IsPresented = false;
            //}
            //else
            //{
            //    var navigationPage = CurrentApplication.MainPage as CustomNavigationPage;

            //    if (navigationPage != null)
            //    {
            //        await navigationPage.PushAsync(page);
            //    }
            //    else
            //    {
            //        CurrentApplication.MainPage = new CustomNavigationPage(page);
            //    }
            //}

            await (page.BindingContext as ViewModelBase)?.InitializeAsync(navParams);
        }

        #endregion
    }
}
