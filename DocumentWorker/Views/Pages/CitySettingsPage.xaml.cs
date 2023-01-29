using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;

namespace DocumentWorker.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CitySettings.xaml
    /// </summary>
    public partial class CitySettingsPage : INavigableView<ViewModels.CitySettingsViewModel>
    {
        public ViewModels.CitySettingsViewModel ViewModel
        {
            get;
        }

        public CitySettingsPage(ViewModels.CitySettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }


    }
}
