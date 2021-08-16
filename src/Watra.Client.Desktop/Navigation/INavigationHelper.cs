// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.Navigation
{
    using System.Windows.Input;

    /// <summary>
    /// Helper to open windows.
    /// </summary>
    public interface INavigationHelper
    {
        /// <summary>
        /// Opens the main window.
        /// </summary>
        public void OpenMainWindow();

        /// <summary>
        /// Creates a command to open the <see cref="EditHoseConnectorsWindow"/>.
        /// </summary>
        public ICommand OpenEditHoseConnectorsWindowCommand();

        /// <summary>
        /// Creates a command to open the <see cref="EditHosesWindow"/>.
        /// </summary>
        public ICommand OpenEditHosesWindowCommand();

        /// <summary>
        /// Creates a command to open the <see cref="EditPumpsWindow"/>.
        /// </summary>
        public ICommand OpenEditPumpsWindowCommand();
    }
}
