using Wpf.Ui.Common.Interfaces;

namespace DocumentWorker.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PricePage : INavigableView<ViewModels.PriceViewModel>
    {
        public ViewModels.PriceViewModel ViewModel
        {
            get;
        }

        public PricePage(ViewModels.PriceViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
