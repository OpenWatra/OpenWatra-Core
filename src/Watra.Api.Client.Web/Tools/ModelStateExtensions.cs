// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.Tools
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Watra.Api.Data.ApiClient;

    /// <summary>
    /// Extensions for model state related stuff.
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Adds all <paramref name="validationErrors"/> to the <paramref name="modelStateDic"/>.
        /// </summary>
        public static void AddValidationErrors(this ModelStateDictionary modelStateDic, ICollection<ValidationError> validationErrors)
        {
            foreach (var error in validationErrors)
            {
                // Adding the values twice ensures that errors are shown as
                // overview at the top of the page!
                modelStateDic.AddModelError(error.Property, error.Message);
                modelStateDic.AddModelError(string.Empty, error.Message);
            }
        }
    }
}
