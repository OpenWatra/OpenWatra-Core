// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Collections.Generic;
    using WaTra.Api.Server;

    /// <summary>
    /// A validator for the given <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the object which is validated.</typeparam>
    public interface IValidator<T>
        where T : class
    {
        /// <summary>
        /// Validates <paramref name="objToValidate"/> and returns <see cref="ValidationError"/>
        /// if data is invalid.
        /// </summary>
        ICollection<ValidationError> Validate(T objToValidate);
    }
}
