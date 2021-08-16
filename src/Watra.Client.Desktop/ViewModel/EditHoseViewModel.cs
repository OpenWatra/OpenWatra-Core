// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Data;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;
    using Watra.Tools;
    using Watra.Tools.Extensions;

    /// <summary>
    /// Specific implementation / extension for <see cref="CrudViewModel{Hose}"/>.
    /// </summary>
    public class EditHoseViewModel : CrudViewModel<Hose>
    {
        private readonly object allHoseConnectorsLock = new object();
        private readonly IServerAccess<HoseConnector> hoseConnectorServerAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHoseViewModel"/> class.
        /// </summary>
        public EditHoseViewModel(IServerAccess<Hose> serverAccess, IInstanceCreator<Hose> instanceCreator, IServerAccess<HoseConnector> hoseConnectorServerAccess)
            : base(serverAccess, instanceCreator)
        {
            this.hoseConnectorServerAccess = hoseConnectorServerAccess;
            BindingOperations.EnableCollectionSynchronization(this.AllHoseConnectors, this.allHoseConnectorsLock);

            this.ViewModelUpdate += this.EditHoseViewModel_ViewModelUpdate;
            this.PropertyChanged += this.EditHoseViewModel_PropertyChanged;
        }

        /// <summary>
        /// Gets a list with all hose connectors available for selection when editing
        /// <see cref="CrudViewModel{Hose}.SelectedItem"/>.
        /// </summary>
        public ObservableCollection<HoseConnector> AllHoseConnectors { get; } = new ObservableCollection<HoseConnector>();

        /// <summary>
        /// See GH-13. This issue needs adressing, but I fear that this won't happen without a major structural API overhaul.
        /// </summary>
        private void EditHoseViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedItem) && this.SelectedItem != null)
            {
                this.SelectedItem.HoseConnector = this.AllHoseConnectors.SingleOrDefault(
                    connector => connector.Id == this.SelectedItem.HoseConnector.Id);
            }
        }

        private void EditHoseViewModel_ViewModelUpdate(object sender, EventArgs e)
        {
            var allHoseConnectors = this.hoseConnectorServerAccess.GetAll();
            var allHoseConnectorsAwaiter = allHoseConnectors.ConfigureAwait(true).GetAwaiter();

            allHoseConnectorsAwaiter.OnCompleted(() =>
            {
                this.AllHoseConnectors.Clear();
                this.AllHoseConnectors.AddRange(allHoseConnectorsAwaiter.GetResult());
            });
        }
    }
}
