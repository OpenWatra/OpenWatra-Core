// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;
    using Watra.Client.Desktop.ViewModel.Commands;
    using Watra.Tools;
    using Watra.Tools.Extensions;

    /// <summary>
    /// Specific implementation / extension for <see cref="CrudViewModel{Hose}"/>.
    /// </summary>
    public class EditPumpViewModel : CrudViewModel<Pump>
    {
        private readonly object allHoseConnectorsLock = new object();
        private readonly IServerAccess<HoseConnector> hoseConnectorServerAccess;
        private readonly object allHosesLock = new object();
        private readonly IServerAccess<Hose> hoseServerAccess;

        private PumpPressureFlowModelCoefficient selectedPumpPressureFlowModelCoefficient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPumpViewModel"/> class.
        /// </summary>
        public EditPumpViewModel(
            IServerAccess<Pump> pumpServerAccess,
            IInstanceCreator<Pump> instanceCreator,
            IServerAccess<HoseConnector> hoseConnectorServerAccess,
            IServerAccess<Hose> hoseServerAccess)
            : base(pumpServerAccess, instanceCreator)
        {
            this.hoseConnectorServerAccess = hoseConnectorServerAccess;
            this.hoseServerAccess = hoseServerAccess;

            BindingOperations.EnableCollectionSynchronization(this.AllHoseConnectors, this.allHoseConnectorsLock);
            BindingOperations.EnableCollectionSynchronization(this.AllHoses, this.allHosesLock);

            this.ViewModelUpdate += this.EditHoseViewModel_ViewModelUpdate;
            this.PropertyChanged += this.EditPumpViewModel_PropertyChanged;
        }

        /// <summary>
        /// Gets a list with all hose connectors available for selection when editing
        /// <see cref="CrudViewModel{Pump}.SelectedItem"/>.
        /// </summary>
        public ObservableCollection<HoseConnector> AllHoseConnectors { get; } = new ObservableCollection<HoseConnector>();

        /// <summary>
        /// Gets a list with all hose connectors available for selection when editing
        /// <see cref="CrudViewModel{Pump}.SelectedItem"/>.
        /// </summary>
        public ObservableCollection<Hose> AllHoses { get; } = new ObservableCollection<Hose>();

        /// <summary>
        /// Gets a list of <see cref="PumpPressureFlowModelCoefficient"/>.
        /// List is newly generated each time so the UI updates:
        /// https://stackoverflow.com/questions/40769941/itemscontrol-itemsource-binding-not-updating
        /// </summary>
        public List<PumpPressureFlowModelCoefficient> PumpPressureFlowModelCoefficientsList => new List<PumpPressureFlowModelCoefficient>(this.SelectedItem?.PumpPressureFlowModel ?? new List<PumpPressureFlowModelCoefficient>());

        /// <summary>
        /// Gets or sets the selected <see cref="PumpPressureFlowModelCoefficient"/>.
        /// </summary>
        public PumpPressureFlowModelCoefficient SelectedItemPumpPressureFlowModel
        {
            get
            {
                return this.selectedPumpPressureFlowModelCoefficient;
            }

            set
            {
                this.selectedPumpPressureFlowModelCoefficient = value;
                this.RaisePropertyChanged(nameof(this.SelectedItemPumpPressureFlowModel));
            }
        }

        /// <summary>
        /// Gets a command to add a <see cref="PumpPressureFlowModelCoefficient"/>.
        /// </summary>
        public ICommand AddPressureFlowModelCommand => new ActionCommand(
            () =>
                {
                    if (this.SelectedItem?.PumpPressureFlowModel == null)
                    {
                        MessageBox.Show("Kein Element ausgewählt");
                    }

                    var pFlowModel = new PumpPressureFlowModelCoefficient();
                    this.SelectedItem.PumpPressureFlowModel.Add(pFlowModel);
                    this.SelectedItemPumpPressureFlowModel = pFlowModel;
                    this.RaisePropertyChanged(nameof(this.PumpPressureFlowModelCoefficientsList));
                },
            () => true);

        /// <summary>
        /// Gets a command to remove a <see cref="PumpPressureFlowModelCoefficient"/>.
        /// </summary>
        public ICommand RemovePressureFlowModelCommand => new ActionCommand(
            () =>
            {
                if (this.SelectedItemPumpPressureFlowModel == null)
                {
                    MessageBox.Show("Kein FlowModel ausgewählt");
                }

                this.SelectedItem.PumpPressureFlowModel.Remove(this.SelectedItemPumpPressureFlowModel);
                this.SelectedItemPumpPressureFlowModel = null;
                this.RaisePropertyChanged(nameof(this.PumpPressureFlowModelCoefficientsList));
            },
            () => true);

        /// <summary>
        /// See GH-13. This issue needs adressing, but I fear that this won't happen without a major structural API overhaul.
        /// </summary>
        private void EditPumpViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedItem) && this.SelectedItem != null)
            {
                var selectedConnectorId = this.SelectedItem.HoseConnector?.Id;
                this.SelectedItem.HoseConnector = this.AllHoseConnectors.SingleOrDefault(
                    connector => connector.Id == selectedConnectorId);

                var selectedHoseId = this.SelectedItem.Hose?.Id;
                this.SelectedItem.Hose = this.AllHoses.SingleOrDefault(
                    hose => hose.Id == selectedHoseId);
                this.RaisePropertyChanged(nameof(this.PumpPressureFlowModelCoefficientsList));
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

            var allHoses = this.hoseServerAccess.GetAll();
            var allHosesAwaiter = allHoses.ConfigureAwait(true).GetAwaiter();

            allHosesAwaiter.OnCompleted(() =>
            {
                this.AllHoses.Clear();
                this.AllHoses.AddRange(allHosesAwaiter.GetResult());
            });
        }
    }
}
