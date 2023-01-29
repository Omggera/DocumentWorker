using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentWorker.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using System.Windows.Forms;
using System.Xml;
using System.Windows;
using System.Text;
using System.Data.SqlTypes;
using System.Xml.Linq;
using DocumentWorker.Interfaces;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;
using Wpf.Ui.Controls;
using System.Windows.Controls;
using DocumentWorker.Services;
using DocumentWorker.Views.Pages;
using Wpf.Ui.Controls.Interfaces;

namespace DocumentWorker.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private ObservableCollection<string>? _fileslist = new();

        [ObservableProperty]
        private string? _selectedItem = "";

        [ObservableProperty]
        private string? _saveFolderPath;

        //Выбранный в ComboBox город
        [ObservableProperty]
        private string? _citySet;

        //Список всех городов
        [ObservableProperty]
        private List<string>? _cityList;

        [ObservableProperty]
        private string? _legalEntity;

        [ObservableProperty]
        private string? _legalName;

        [ObservableProperty]
        private string? _phoneNumberSalesDepartment;

        [ObservableProperty]
        private string? _phoneNumberDeliveryService;

        [ObservableProperty]
        private string? _sellersRepresentative;

        [ObservableProperty]
        private int _progressBarValue = 0;

        [ObservableProperty]
        private string _labelProgressBar;

        [ObservableProperty]
        private string _permissionToEdit = "True";


        [ObservableProperty]
        private string _someDialog;


        private readonly IGetCityListFromXml _getCityListFromXml;
        private readonly IGetXmlData _getXmlData;
        private ISaveSettingsXml _saveSettingsXml;

        public DashboardViewModel(
            IGetCityListFromXml getCityListFromXml, 
            IGetXmlData getXmlData,
            ISaveSettingsXml saveSettingsXml)
        {
            _getCityListFromXml = getCityListFromXml;
            _getXmlData = getXmlData;

            CityList = _getCityListFromXml.GetCityList();
            _saveSettingsXml = saveSettingsXml;
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private async void Start()
        {
            ExcelCreater excelCreater = new();
            if((SaveFolderPath != null) & (SaveFolderPath != string.Empty)) 
            {
                SaveFolderPath = SaveFolderPath;

                var progress = new Progress<int>(value =>
                {
                    ProgressBarValue = value;
                    LabelProgressBar = $"{value}%";
                });

                await Task.Run( () => excelCreater.Excel(
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

        [RelayCommand]
        private void OpenFile()
        {
            SelectExcelFiles.AddExcelFilesToList(Fileslist);
            ProgressBarValue = 0;
            LabelProgressBar = " ";
        }

        [RelayCommand]
        private void DeleteFileFromList()
        {
            Fileslist.Remove(SelectedItem);
        }

        [RelayCommand]
        private void DeleteAllFilesFromList()
        {
            Fileslist.Clear();
            ProgressBarValue = 0;
            LabelProgressBar = " ";
        }

        [RelayCommand]
        private void SaveFolder()
        {
            FolderBrowserDialog openFileDlg = new();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {

                SaveFolderPath = openFileDlg.SelectedPath;
            }
        }

        [RelayCommand]
        private void SelectCity()
        {
            LegalEntity = _getXmlData.GetData(CitySet).LegalEntity;
            LegalName = _getXmlData.GetData(CitySet).LegalName;
            PhoneNumberSalesDepartment = _getXmlData.GetData(CitySet).PhoneNumberSalesDepartment;
            PhoneNumberDeliveryService = _getXmlData.GetData(CitySet).PhoneNumberDeliveryService;
            SellersRepresentative = _getXmlData.GetData(CitySet).SellersRepresentative;
        }

        [RelayCommand]
        private void Edit()
        {
            PermissionToEdit = "False";
        }

        [RelayCommand]
        private void Save()
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

        [RelayCommand]
        private void AddNewCity()
        {
            
            
        }

    }
}
