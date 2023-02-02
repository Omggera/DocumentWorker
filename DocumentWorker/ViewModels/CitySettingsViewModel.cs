using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentWorker.Interfaces;
using DocumentWorker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wpf.Ui.Common.Interfaces;

namespace DocumentWorker.ViewModels
{
    public partial class CitySettingsViewModel : ObservableValidator, INavigationAware
    {
        [ObservableProperty]
        private ObservableCollection<string>? _cityList = new();

        [ObservableProperty]
        private string? _selectedItem;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _cityName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _legalEntity;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _legalName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _phoneNumberSalesDepartment;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _phoneNumberDeliveryService;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _sellersRepresentative;

        public event EventHandler? FormSubmissionCompleted;
        public event EventHandler? FormSubmissionFailed;

        private readonly IGetCityListFromXml _getCityListFromXml;
        private IAddNewCity _addNewCity;
        private IDeleteCity _deleteCity;

        public CitySettingsViewModel(IGetCityListFromXml getCityListFromXml, IAddNewCity addNewCity, IDeleteCity deleteCity)
        {
            _getCityListFromXml = getCityListFromXml;
            _getCityListFromXml.GetCityList(CityList);

            _addNewCity = addNewCity;
            _deleteCity = deleteCity;
        }

        [RelayCommand]
        private void AddCity()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                FormSubmissionFailed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                FormSubmissionCompleted?.Invoke(this, EventArgs.Empty);
                _addNewCity.Add(
                CityName,
                LegalEntity,
                LegalName,
                PhoneNumberSalesDepartment,
                PhoneNumberDeliveryService,
                SellersRepresentative);

                CityList.Add(CityName);

                CityName = null;
                LegalEntity = null;
                LegalName = null;
                PhoneNumberSalesDepartment = null;
                PhoneNumberDeliveryService = null;
                SellersRepresentative = null;
            }
            
        }

        [RelayCommand]
        private void DeleteCity()
        {
            _deleteCity.Delete(SelectedItem);

            CityList.Remove(SelectedItem);
        }

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo()
        {
            
        }

        
    }
}
