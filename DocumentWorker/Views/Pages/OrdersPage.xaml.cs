using System;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace DocumentWorker.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class OrdersPage : INavigableView<ViewModels.OrdersViewModel>
    {
        public ViewModels.OrdersViewModel ViewModel
        {
            get;
        }

        public OrdersPage(ViewModels.OrdersViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}