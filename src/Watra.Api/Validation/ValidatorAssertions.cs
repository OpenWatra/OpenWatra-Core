// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Linq;

    /// <summary>
    /// Assertions for <see cref="IValidator{T}"/>.
    /// </summary>
    public static class ValidatorAssertions
    {
        /// <summary>
        /// Throws an exception if the <paramref name="validator"/>
        /// finds <paramref name="objToValidate"/> invalid.
        /// The exception message contains the <see cref="ValidationError"/>
        /// objects created by the <paramref name="validator"/> as message.
        /// </summary>
        /// <typeparam name="T">Type which is validated.</typeparam>
        /// <param name="validator">The validator to use.</param>
        /// <param name="objToValidate">The object which is validated.</param>
        public static void AssertIsValid<T>(this IValidator<T> validator, T objToValidate)
            where T : class
        {
            var validationErrors = validator.Validate(objToValidate);

            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join("\n", validationErrors.Select(error => error.Message));
                throw new WatraValidationException(exceptionMessage);
            }
        }
    }
}
