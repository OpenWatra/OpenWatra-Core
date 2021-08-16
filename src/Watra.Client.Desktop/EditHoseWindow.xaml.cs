// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop
{
    using System.Windows;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Desktop.ViewModel;

    /// <summary>
    /// Interaction logic for EditHoseWindow.xaml.
    /// </summary>
    public partial class EditHosesWindow : Window
    {
        private readonly CrudViewModel<Hose> viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHosesWindow"/> class.
        /// </summary>
        public EditHosesWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHosesWindow"/> class.
        /// </summary>
        public EditHosesWindow(CrudViewModel<Hose> viewModel)
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
