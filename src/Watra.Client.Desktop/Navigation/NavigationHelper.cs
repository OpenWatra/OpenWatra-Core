// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.Navigation
{
    using System;
    using System.Windows.Input;
    using Microsoft.Extensions.DependencyInjection;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Desktop.ViewModel;
    using Watra.Client.Desktop.ViewModel.Commands;

    /// <inheritdoc/>
    public class NavigationHelper : INavigationHelper
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationHelper"/> class.
        /// </summary>
        public NavigationHelper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public void OpenMainWindow()
        {
            var mainViewModel = this.serviceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }

        /// <inheritdoc/>
        public ICommand OpenEditHoseConnectorsWindowCommand()
        {
            return new ActionCommand(
                () =>
                {
                    var editHoseConnectorsViewModel = this.serviceProvider.GetRequiredService<CrudViewModel<HoseConnector>>();
                    var editHoseConnectorsWindow = new EditHoseConnectorsWindow(editHoseConnectorsViewModel);
                    editHoseConnectorsWindow.ShowDialog();
                },
                () => { return true; });
        }

        /// <inheritdoc/>
        public ICommand OpenEditHosesWindowCommand()
        {
            return new ActionCommand(
                () =>
                {
                    var editHoseViewModel = this.serviceProvider.GetRequiredService<CrudViewModel<Hose>>();
                    var editHoseWindow = new EditHosesWindow(editHoseViewModel);
                    editHoseWindow.ShowDialog();
                },
                () => { return true; });
        }

        /// <inheritdoc/>
        public ICommand OpenEditPumpsWindowCommand()
        {
            return new ActionCommand(
                () =>
                {
                    var editPumpsViewModel = this.serviceProvider.GetRequiredService<CrudViewModel<Pump>>();
                    var editPumpsWindow = new EditPumpsWindow(editPumpsViewModel);
                    editPumpsWindow.ShowDialog();
                },
                () => { return true; });
        }
    }
}
