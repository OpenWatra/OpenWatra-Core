// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Collections.Generic;
    using WaTra.Api.Server;
    using Watra.Tools;

    /// <summary>
    /// Implementation of the <see cref="IValidator{T}"/>
    /// for <see cref="HoseConnector"/> objects.
    /// </summary>
    public class WatraRouteValidator : IValidator<WatraRoute>
    {
        private readonly IInstanceCreator<ValidationError> validationErrorCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteValidator"/> class.
        /// </summary>
        public WatraRouteValidator(IInstanceCreator<ValidationError> validationErrorCreator)
        {
            this.validationErrorCreator = validationErrorCreator;
        }

        /// <inheritdoc />
        public ICollection<ValidationError> Validate(WatraRoute objToValidate)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(objToValidate.Name))
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(WatraRoute.Name);
                validationError.Message = "Name muss eingegeben werden.";
                validationErrors.Add(validationError);
            }

            if (string.IsNullOrWhiteSpace(objToValidate.Description))
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(WatraRoute.Description);
                validationError.Message = "Beschreibung muss eingegeben werden.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.FlowRate <= 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(WatraRoute.FlowRate);
                validationError.Message = "Flussrate muss grösser als 1 l/min sein.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.MinimalOutletPressure <= 0)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(WatraRoute.MinimalOutletPressure);
                validationError.Message = "Minimaler Abgangsdruck muss grösser als  0 bar sein.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.SafetyPressure <= 0)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(WatraRoute.MinimalOutletPressure);
                validationError.Message = "Safety pressure muss grösser 0 bar sein.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.PumpSelections == null)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.PumpPressureFlowModel);
                validationError.Message = "Pumpenselektion fehlt.";
                validationErrors.Add(validationError);
            }
            else if (objToValidate.PumpSelections.Count < 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.PumpPressureFlowModel);
                validationError.Message = "Keine Pumpen ausgewählt.";
                validationErrors.Add(validationError);
            }
            else
            {
                foreach (var pumpSelection in objToValidate.PumpSelections)
                {
                    if (pumpSelection.NumberOfHoseLines < 1 || pumpSelection.NumberOfHoseLines > 2)
                    {
                        var validationError = this.validationErrorCreator.Create();
                        validationError.Property = nameof(Pump.PumpPressureFlowModel);
                        validationError.Message = "Nur einer oder zwei parallele Schlauchstränge sind erlaubt.";
                        validationErrors.Add(validationError);
                    }
                }
            }

            if (objToValidate.WatraRouteDistanceHeightElements == null)
            {
                return validationErrors;
            }

            foreach (var distHeightElement in objToValidate.WatraRouteDistanceHeightElements)
            {
                if (distHeightElement.Length < 0)
                {
                    var validationError = this.validationErrorCreator.Create();
                    validationError.Property = nameof(Pump.PumpPressureFlowModel);
                    validationError.Message = "Mindestens ein Distanz-/Höhenelement hat eine negative Länge.";
                    validationErrors.Add(validationError);
                }
            }

            return validationErrors;
        }
    }
}
