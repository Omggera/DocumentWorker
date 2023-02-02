using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentWorker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace DocumentWorker.ViewModels
{
    public partial class PriceViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string? _filePricePath;

        [ObservableProperty]
        private string? _saveFolderPath = string.Empty;

        [ObservableProperty]
        private int _progressBarValue = 0;

        [ObservableProperty]
        private string? _labelProgressBar;

        public PriceViewModel()
        {

        }


        [RelayCommand]
        private void AddPrice()
        {
            FilePricePath = SelectFolderOrFile.AddExcelPriceFile();
            ProgressBarValue = 0;
            LabelProgressBar = " ";
        }

        [RelayCommand]
        private void SaveFolder()
        {
            SaveFolderPath = SelectFolderOrFile.SelectFolderToSave();
        }

        [RelayCommand]
        private async void Start()
        {
            ExcelPriceCreater excelPriceCreater = new ();
            if ((FilePricePath != string.Empty) & (SaveFolderPath != string.Empty))
            {
                SaveFolderPath = SaveFolderPath;

                var progress = new Progress<int>(value =>
                {
                    ProgressBarValue = value;
                    LabelProgressBar = $"{value}%";
                });

                await Task.Run(() => excelPriceCreater.CreatePrice(FilePricePath, SaveFolderPath, progress));
                GC.Collect();
            }
            else
            {
                SaveFolderPath = string.Empty;
                FilePricePath = string.Empty;
            }
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
