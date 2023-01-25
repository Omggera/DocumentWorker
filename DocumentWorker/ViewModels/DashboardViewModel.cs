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
        private string? _citySet = "Владимир";

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

        private readonly IGetCityListFromXml _getCityListFromXml;
        private readonly IGetXmlData _getXmlData;

        public DashboardViewModel(IGetCityListFromXml getCityListFromXml, IGetXmlData getXmlData)
        {
            _getCityListFromXml = getCityListFromXml;
            _getXmlData = getXmlData;

            CityList = _getCityListFromXml.GetCityList();

            LegalEntity = _getXmlData.GetData(CitySet).LegalEntity;
            LegalName = _getXmlData.GetData(CitySet).LegalName;
            PhoneNumberSalesDepartment = _getXmlData.GetData(CitySet).PhoneNumberSalesDepartment;
            PhoneNumberDeliveryService = _getXmlData.GetData(CitySet).PhoneNumberDeliveryService;
            SellersRepresentative = _getXmlData.GetData(CitySet).SellersRepresentative;
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void Start()
        {
            ExcelCreater excelCreater = new();
            if((SaveFolderPath != null) & (SaveFolderPath != string.Empty)) 
            {
                SaveFolderPath = SaveFolderPath;
                foreach(var file in Fileslist)
                {
                    excelCreater.Excel(
                    SaveFolderPath,
                    file,
                    CitySet,
                    LegalEntity,
                    LegalName,
                    PhoneNumberSalesDepartment,
                    PhoneNumberDeliveryService,
                    SellersRepresentative);

                    ProgressBarValue += 100/Fileslist.Count;
                }
                
            }
            else SaveFolderPath = "Выберите папку для сохранения";
        }

        [RelayCommand]
        private void OpenFile()
        {
            SelectExcelFiles.AddExcelFilesToList(Fileslist);
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
    }
}
