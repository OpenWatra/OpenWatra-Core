// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;
    using Watra.Client.Desktop.ViewModel.Commands;
    using Watra.Tools;
    using Watra.Tools.Extensions;

    /// <summary>
    /// Basic view model implementation for CRUD of master data objects on the server.
    /// </summary>
    /// <typeparam name="T">Type of the master data entity (API).</typeparam>
    public class CrudViewModel<T> : INotifyPropertyChanged
        where T : class, new()
    {
        /// <summary>
        /// Lock object for <see cref="ModelList"/>.
        /// </summary>
        private static readonly object ModelListLock = new object();
        private readonly IServerAccess<T> serverAccess;
        private readonly IInstanceCreator<T> instanceCreator;

        private string status;
        private T selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudViewModel{T}"/> class.
        /// </summary>
        public CrudViewModel(IServerAccess<T> serverAccess, IInstanceCreator<T> instanceCreator)
        {
            this.serverAccess = serverAccess;
            this.instanceCreator = instanceCreator;

            BindingOperations.EnableCollectionSynchronization(this.ModelList, ModelListLock);

            this.UpdateCommand = new ActionCommand(this.ExecuteUpdateCommandAsync, () => true);
            this.DeleteCommand = new ActionCommand(this.ExecuteDeleteCommandAsync, () => true);
            this.CreateNewCommand = new ActionCommand(this.ExecuteCreateNewCommand, () => true);
        }

        /// <summary>
        /// Delegate used with <see cref="ViewModelUpdate"/>.
        /// </summary>
        public delegate void ViewModelUpdateEventHandler(object sender, EventArgs e);

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event raised when the view model is being updated.
        /// </summary>
        public event ViewModelUpdateEventHandler ViewModelUpdate;

        /// <summary>
        /// Gets a <see cref="ICommand"/> to update the <see cref="SelectedItem"/>.
        /// </summary>
        public ActionCommand UpdateCommand { get; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> to delete the <see cref="SelectedItem"/>.
        /// </summary>
        public ActionCommand DeleteCommand { get; }

        /// <summary>
        /// Gets a <see cref="ICommand"/> to create a new instance for the <see cref="SelectedItem"/>.
        /// </summary>
        public ActionCommand CreateNewCommand { get; }

        /// <summary>
        /// Gets an observable collection of objects which can be displayed in a list.
        /// </summary>
        public ObservableCollection<T> ModelList { get; } = new ObservableCollection<T>();

        /// <summary>
        /// Gets or sets the selected item of <see cref="ModelList"/>.
        /// </summary>
        public T SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedItem)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.EditGridIsEnabled)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether an item is selected.
        /// </summary>
        public bool EditGridIsEnabled => this.SelectedItem != null;

        /// <summary>
        /// Gets a status text which is displayed.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            private set
            {
                this.status = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Status)));
            }
        }

        /// <summary>
        /// Updates/populates the view model with data.
        /// </summary>
        public async void UpdateAsync()
        {
            await Task.Run(async () =>
            {
                this.SelectedItem = null;
                this.Status = "Laden...";
                this.ModelList.Clear();
                var models = await this.serverAccess.GetAll();

                this.ModelList.AddRange(models);

                this.ViewModelUpdate?.Invoke(this, EventArgs.Empty);
                this.Status = "Geladen";
             }).ConfigureAwait(false);
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ExecuteUpdateCommandAsync()
        {
            await Task.Run(async () =>
            {
                var validationErrors = new List<ValidationError>();

                // ToDo GH-35, API enhancement
                try
                {
                    validationErrors = (await this.serverAccess.Validate(this.SelectedItem)).ToList();
                }
                catch (Exception ex)
                {
                    validationErrors.Add(new ValidationError() { Message = ex.Message });
                }

                if (validationErrors.Any())
                {
                    var allErrors = string.Join("\n", validationErrors.Select(valErr => valErr.Message));
                    MessageBox.Show(allErrors, "Speichern nicht möglich", MessageBoxButton.OK);
                    return;
                }

                var updateServerSideTask = this.serverAccess.UpdateAsync(this.SelectedItem);
                this.ModelList.Remove(this.SelectedItem);
                this.SelectedItem = null;
                var updatedModel = await updateServerSideTask;
                this.ModelList.Add(updatedModel);
                this.SelectedItem = updatedModel;
            }).ConfigureAwait(false);
        }

        private async void ExecuteDeleteCommandAsync()
        {
            await Task.Run(async () =>
            {
                var deleteServerSideTask = this.serverAccess.DeleteAsync(this.SelectedItem);
                this.ModelList.Remove(this.SelectedItem);
                this.SelectedItem = null;
                await deleteServerSideTask;
            }).ConfigureAwait(false);
        }

        private void ExecuteCreateNewCommand()
        {
            this.SelectedItem = this.instanceCreator.Create();
        }
    }
}
