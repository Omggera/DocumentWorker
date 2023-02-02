using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentWorker.Models;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common.Interfaces;
using DocumentWorker.Interfaces;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using System.ComponentModel.DataAnnotations;

namespace DocumentWorker.ViewModels
{
    public partial class OrdersViewModel : ObservableValidator, INavigationAware
    {
        /// <summary> Список файлов (заказов) </summary>
        [ObservableProperty]
        private ObservableCollection<string>? _fileslist = new();

        /// <summary> Выбранный в списке файл </summary>
        [ObservableProperty]
        private string? _selectedItem = "";

        /// <summary> Абсолютный путь к папке, куда сохранится созданный файл(ы) </summary>
        [ObservableProperty]
        private string? _saveFolderPath;

        /// <summary> Выбранный в ComboBox город </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [MinLength(1)]
        private string? _citySet;

        /// <summary> Список всех городов </summary>
        [ObservableProperty]
        private ObservableCollection<string>? _cityList = new();

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

        /// <summary> Значение шкалы progress bar </summary>
        [ObservableProperty]
        private int? _progressBarValue = 0;

        /// <summary> Значение шкалы progress bar для числового отображения в label </summary>
        [ObservableProperty]
        private string? _labelProgressBar;

        /// <summary>
        /// Отключает или включает возможность редактировать
        /// textBox для изменения данных города
        /// </summary>
        [ObservableProperty]
        private string _permissionToEdit = "True";

        private readonly IGetCityListFromXml _getCityListFromXml;
        private readonly IGetXmlData _getXmlData;
        private readonly ISaveSettingsXml _saveSettingsXml;

        public OrdersViewModel(
            IGetCityListFromXml getCityListFromXml, 
            IGetXmlData getXmlData,
            ISaveSettingsXml saveSettingsXml)
        {
            _getCityListFromXml = getCityListFromXml;
            _getXmlData = getXmlData;
            _saveSettingsXml = saveSettingsXml;
        }

        /// <summary>
        /// При навигации на эту страницу загрузится список городов в comboBox
        /// </summary>
        public void OnNavigatedTo()
        {
            CityList.Clear();
            _getCityListFromXml.GetCityList(CityList);
        }

        public void OnNavigatedFrom()
        {
        }

        /// <summary>
        /// Запускает набор заказов
        /// </summary>
        [RelayCommand]
        private async void Start()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                ExcelOrdersCreater excelOrdersCreater = new();

                if ((SaveFolderPath != null) & (SaveFolderPath != string.Empty))
                {
                    var progress = new Progress<int>(value =>
                    {
                        ProgressBarValue = value;
                        LabelProgressBar = $"{value}%";
                    });

                    await Task.Run(() => excelOrdersCreater.CreateNewExcelDocument(
                        SaveFolderPath,
                        Fileslist,
                        CitySet,
                        LegalEntity,
                        LegalName,
                        PhoneNumberSalesDepartment,
                        PhoneNumberDeliveryService,
                        SellersRepresentative,
                        progress));
                }
                else SaveFolderPath = "Выберите папку для сохранения";
            }
            else
            {
                //Пока не реализованно
            }  
        }

        /// <summary>
        /// Открывает файловый диалог для добавления 
        /// файлов(заказов) в список
        /// </summary>
        [RelayCommand]
        private void OpenFile()
        {
            SelectFolderOrFile.WorkingWithTheFileDialog(Fileslist);
            Default();
        }

        /// <summary>
        /// Удаляет выбранный в списке файл(заказ)
        /// </summary>
        [RelayCommand]
        private void DeleteFileFromList()
        {
            Fileslist.Remove(SelectedItem);
            Default();
        }

        /// <summary>
        /// Удаляет все файлы(заказы) из списка
        /// </summary>
        [RelayCommand]
        private void DeleteAllFilesFromList()
        {
            Fileslist.Clear();
            Default();
        }

        /// <summary>
        /// Выбирает папкуу для сохранения созданных файлов
        /// </summary>
        [RelayCommand]
        private void SaveFolder()
        {
            SaveFolderPath = SelectFolderOrFile.SelectFolderToSave();
        }

        /// <summary>
        /// При выборе города в comboBox заполняет данными 
        /// все связанные с ним textBox 
        /// </summary>
        [RelayCommand]
        private void SelectCity()
        {
            LegalEntity = _getXmlData.GetData(CitySet).LegalEntity;
            LegalName = _getXmlData.GetData(CitySet).LegalName;
            PhoneNumberSalesDepartment = _getXmlData.GetData(CitySet).PhoneNumberSalesDepartment;
            PhoneNumberDeliveryService = _getXmlData.GetData(CitySet).PhoneNumberDeliveryService;
            SellersRepresentative = _getXmlData.GetData(CitySet).SellersRepresentative;
        }

        /// <summary>
        /// Разрешает и запрещает редактирование textBox
        /// с данными города
        /// </summary>
        [RelayCommand]
        private void Edit()
        {
            if (PermissionToEdit == "True")
                PermissionToEdit = "False";

            else if (PermissionToEdit == "False")
                PermissionToEdit = "True";
        }

        /// <summary>
        /// Сохраняет в xml измененные в textBox данные
        /// </summary>
        [RelayCommand]
        private void Save()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                _saveSettingsXml.SaveSettings(
                    CitySet,
                    LegalEntity,
                    LegalName,
                    PhoneNumberSalesDepartment,
                    PhoneNumberDeliveryService,
                    SellersRepresentative);

                PermissionToEdit = "True";
            }
            else
            {
                //Пока не реализованно
            }
        }

        /// <summary>
        /// Сброс значений Progress Bar до ноля
        /// </summary>
        private void Default()
        {
            ProgressBarValue = 0;
            LabelProgressBar = "";
        }
    }
}
