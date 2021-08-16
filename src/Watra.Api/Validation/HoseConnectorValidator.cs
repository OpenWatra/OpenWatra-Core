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
    public class HoseConnectorValidator : IValidator<HoseConnector>
    {
        private readonly IInstanceCreator<ValidationError> validationErrorCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoseConnectorValidator"/> class.
        /// </summary>
        public HoseConnectorValidator(IInstanceCreator<ValidationError> validationErrorCreator)
        {
            this.validationErrorCreator = validationErrorCreator;
        }

        /// <inheritdoc />
        public ICollection<ValidationError> Validate(HoseConnector objToValidate)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(objToValidate.Name))
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(HoseConnector.Name);
                validationError.Message = "Name muss eingegeben werden.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.Diameter < 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(HoseConnector.Diameter);
                validationError.Message = "Durchmesser muss mindestens 1.0 betragen.";
                validationErrors.Add(validationError);
            }

            return validationErrors;
        }
    }
}
