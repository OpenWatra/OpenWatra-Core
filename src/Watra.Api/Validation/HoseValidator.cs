// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Collections.Generic;
    using WaTra.Api.Server;
    using Watra.Tools;

    /// <summary>
    /// Implementation of the <see cref="IValidator{T}"/>
    /// for <see cref="Hose"/> objects.
    /// </summary>
    public class HoseValidator : IValidator<Hose>
    {
        private readonly IInstanceCreator<ValidationError> validationErrorCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoseValidator"/> class.
        /// </summary>
        public HoseValidator(IInstanceCreator<ValidationError> validationErrorCreator)
        {
            this.validationErrorCreator = validationErrorCreator;
        }

        /// <inheritdoc />
        public ICollection<ValidationError> Validate(Hose objToValidate)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(objToValidate.Name))
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Hose.Name);
                validationError.Message = "Name muss eingegeben werden.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.ElementLengthInMetres < 1)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Hose.ElementLengthInMetres);
                validationError.Message = "Länge muss mindestens 1.0 betragen.";
                validationErrors.Add(validationError);
            }

            if (objToValidate.HoseConnector == null)
            {
                var validationError = this.validationErrorCreator.Create();
                validationError.Property = nameof(Hose.HoseConnector);
                validationError.Message = "Kupplung muss angegben werden.";
                validationErrors.Add(validationError);
            }

            return validationErrors;
        }
    }
}
