// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Collections.Generic;
    using WaTra.Api.Server;

    /// <summary>
    /// Implementation of the default validator, which does nothing.
    /// </summary>
    /// <typeparam name="T">See <see cref="IValidator{T}"/>.</typeparam>
    public class DoNothingValidator<T> : IValidator<T>
        where T : class
    {
        /// <inheritdoc />
        public ICollection<ValidationError> Validate(T objToValidate)
        {
            return new List<ValidationError>();
        }
    }
}
