// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ViewModel
{
    using System.Windows.Input;
    using Watra.Client.Desktop.Navigation;

    /// <summary>
    /// Main view model of the application.
    /// </summary>
    public class MainViewModel
    {
        private readonly INavigationHelper navigationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <remarks>C'tor for design-time data.</remarks>
        public MainViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(INavigationHelper navigationHelper)
        {
            this.navigationHelper = navigationHelper;
        }

        /// <summary>
        /// Gets the command to edit pumps.
        /// </summary>
        public ICommand EditPumpsCommand => this.navigationHelper?.OpenEditPumpsWindowCommand();

        /// <summary>
        /// Gets the command to edit hose connectors.
        /// </summary>
        public ICommand EditHoseConnectorsCommand => this.navigationHelper?.OpenEditHoseConnectorsWindowCommand();

        /// <summary>
        /// Gets the command to edit hose connectors.
        /// </summary>
        public ICommand EditHosesCommand => this.navigationHelper?.OpenEditHosesWindowCommand();
    }
}
