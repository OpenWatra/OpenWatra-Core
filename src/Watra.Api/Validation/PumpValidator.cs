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
    public class PumpValidator : IValidator<Pump>
    {
        private readonly IInstanceCreator<ValidationError> validationErrorCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpValidator"/> class.
        /// </summary>
        public PumpValidator(IInstanceCreator<ValidationError> validationErrorCreator)
        {
            this.validationErrorCreator = validationErrorCreator;
        }

        /// <inheritdoc />
        public ICollection<ValidationError> Validate(Pump objToValidate)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(objToValidate.Name))
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.Name);
                validationError.Message = "Name muss eingegeben werden.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.HoseConnector == null)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.HoseConnector);
                validationError.Message = "Keine Kupplungen angegeben.";
                validationErrors.Add(validationError);
            }
            else if (objToValidate.Hose == null)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.Hose);
                validationError.Message = "Pumpe hat keinen Schlauchtyp.";
                validationErrors.Add(validationError);
            }
            else
            {
                var hose = objToValidate.Hose;
                if (hose.HoseConnector == null)
                {
                    var validationError = this.validationErrorCreator.Create();
                    validationError.Property = nameof(Pump.Hose);
                    validationError.Message = $"Mindestens ein Schlauch {hose.Name} hat keine Kupplung.";
                    validationErrors.Add(validationError);
                }
                else
                {
                    var comp = new HoseConnectorComparer();
                    var hasMatch = comp.Equals(objToValidate.HoseConnector, hose.HoseConnector);

                    if (!hasMatch)
                    {
                        var validationError = this.validationErrorCreator.Create();
                        validationError.Property = nameof(Pump.Hose);
                        validationError.Message = $"Der Schlauch {hose.HoseConnector.Name} passt auf keine Kupplung der Pumpe.";
                        validationErrors.Add(validationError);
                    }
                }
            }

            if (objToValidate.MaxFlowRateLitresPerMinute < 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.MaxFlowRateLitresPerMinute);
                validationError.Message = "Maximale Durchflussrate muss mindestens 1.0 betragen.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.MaxOutletPressureBar < 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Pump.MaxOutletPressureBar);
                validationError.Message = "Maximaler Abgangsdruck muss mindestens 1.0 betragen.";
                validationErrors.Add(validationError);
            }

            return validationErrors;
        }
    }
}
