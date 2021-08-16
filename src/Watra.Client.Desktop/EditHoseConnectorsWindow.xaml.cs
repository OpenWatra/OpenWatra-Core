// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop
{
    using System.Windows;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Desktop.ViewModel;

    /// <summary>
    /// Interaction logic for EditHoseConnectorsWindow.xaml.
    /// </summary>
    public partial class EditHoseConnectorsWindow : Window
    {
        private readonly CrudViewModel<HoseConnector> viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHoseConnectorsWindow"/> class.
        /// </summary>
        public EditHoseConnectorsWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHoseConnectorsWindow"/> class.
        /// </summary>
        public EditHoseConnectorsWindow(CrudViewModel<HoseConnector> viewModel)
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
