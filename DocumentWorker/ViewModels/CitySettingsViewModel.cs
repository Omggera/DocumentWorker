using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentWorker.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Wpf.Ui.Common.Interfaces;

namespace DocumentWorker.ViewModels
{
    public partial class CitySettingsViewModel : ObservableValidator, INavigationAware
    {
        /// <summary> Список всех городов </summary>
        [ObservableProperty]
        private ObservableCollection<string>? _cityList = new();

        /// <summary> Выбранный в списке город </summary>
        [ObservableProperty]
        private string? _selectedItem;

        /// <summary> Название города </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _cityName;

        /// <summary> Юридическое лицо </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _legalEntity;

        /// <summary> Название организации на латинице </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _legalName;

        /// <summary> Телефон отдела продаж </summary>//
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _phoneNumberSalesDepartment;

        /// <summary> Телефон службы доставки </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _phoneNumberDeliveryService;

        /// <summary> Представитель продавца </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _sellersRepresentative;

        public event EventHandler? FormSubmissionCompleted;
        public event EventHandler? FormSubmissionFailed;

        private readonly IGetCityListFromXml _getCityListFromXml;
        private readonly IAddNewCity _addNewCity;
        private readonly IDeleteCity _deleteCity;

        public CitySettingsViewModel(IGetCityListFromXml getCityListFromXml, IAddNewCity addNewCity, IDeleteCity deleteCity)
        {
            _getCityListFromXml = getCityListFromXml;
            _addNewCity = addNewCity;
            _deleteCity = deleteCity;
        }

        /// <summary>
        /// Добавляет новый город
        /// </summary>
        [RelayCommand]
        private void AddCity()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                //Пока не реализованно
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

        /// <summary>
        /// Удаляет выбранный город
        /// </summary>
        [RelayCommand]
        private void DeleteCity()
        {
            _deleteCity.Delete(SelectedItem);

            CityList.Remove(SelectedItem);
        }

        public void OnNavigatedFrom()
        {
            
        }

        /// <summary>
        /// Выполняет действия при навигации на страницу
        /// </summary>
        public void OnNavigatedTo()
        {
            _getCityListFromXml.GetCityList(CityList);
        }
    }
}
