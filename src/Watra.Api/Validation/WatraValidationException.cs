// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System;

    /// <summary>
    /// Exception which may be thrown if continuing the program
    /// execution is not possible because of validation errors.
    /// </summary>
    [System.Serializable]
    public class WatraValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatraValidationException"/> class.
        /// </summary>
        public WatraValidationException(string message)
            : base(message)
        {
        }
    }
}
