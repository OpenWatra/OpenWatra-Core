// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop
{
    using System.Windows;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Desktop.ViewModel;

    /// <summary>
    /// Interaction logic for EditPumpWindow.xaml
    /// </summary>
    public partial class EditPumpsWindow : Window
    {
        private readonly CrudViewModel<Pump> viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPumpsWindow"/> class.
        /// </summary>
        public EditPumpsWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPumpsWindow"/> class.
        /// </summary>
        public EditPumpsWindow(CrudViewModel<Pump> viewModel)
            : this()
        {
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.UpdateAsync();
        }
    }
}
