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
        /// <summary> Абсолютный путь к файлу прайс-листа </summary>
        [ObservableProperty]
        private string? _filePricePath;

        /// <summary> Абсолютный путь к папке куда сохранится созданный файл </summary>
        [ObservableProperty]
        private string? _saveFolderPath = string.Empty;

        /// <summary> Значение шкалы progress bar </summary>
        [ObservableProperty]
        private int _progressBarValue = 0;

        /// Значение шкалы progress bar для числового отображения в label </summary>
        [ObservableProperty]
        private string? _labelProgressBar;

        /// <summary>
        /// Добавляет xlsx файл прайс-листа
        /// </summary>
        [RelayCommand]
        private void AddPrice()
        {
            FilePricePath = SelectFolderOrFile.WorkingWithTheFileDialog();
            ProgressBarValue = 0;
            LabelProgressBar = " ";
        }

        /// <summary>
        /// Выбирает папку для сохранения созданного прайс-листа
        /// </summary>
        [RelayCommand]
        private void SaveFolder()
        {
            SaveFolderPath = SelectFolderOrFile.SelectFolderToSave();
        }

        /// <summary>
        /// Запускает набор прайс-листа
        /// </summary>
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
