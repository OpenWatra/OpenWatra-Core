// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ServerAccess.Services;
    using Watra.Client.Web.Tools;

    /// <summary>
    /// View Model to edit the distance / height elements.
    /// </summary>
    [BindProperties]
    public class WatraRouteHeightDistanceElementViewModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly IWatraRouteServerAccess distanceHeightElementServerAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteHeightDistanceElementViewModel"/> class.
        /// </summary>
        public WatraRouteHeightDistanceElementViewModel(IMapper mapper, IWatraRouteServerAccess distanceHeightElementServerAccess)
        {
            this.mapper = mapper;
            this.distanceHeightElementServerAccess = distanceHeightElementServerAccess;
        }

        /// <summary>
        /// Gets or sets the ID of the watra route.
        /// </summary>
        public int IdRoute { get; set; }

        /// <summary>
        /// Gets or sets the ID of element being edited, 0 = new element being created.
        /// </summary>
        public int Id { get; set; } = -1;

        /// <summary>
        /// Gets or sets the Sort Order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets Length of the Element
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the Height Difference.
        /// </summary>
        public int HeightDifference { get; set; }

        /// <summary>
        /// Executed on HTTP get.
        /// </summary>
        public void OnGet(int id, int idWatraRoute)
        {
            if (idWatraRoute <= 0)
            {
                this.ModelState.TryAddModelError(string.Empty, "Ungültige ID der WatraRoute, Daten können nicht geladen werden.");
                return;
            }

            this.IdRoute = idWatraRoute;

            if (id > 0)
            {
                // No await! Reason: If awaiting here, HTML is rendered before objects are fully retrieved...
                var route = this.distanceHeightElementServerAccess.GetWatraRouteByIdAsync(idWatraRoute).Result;
                var distanceHeightElement = route.WatraRouteDistanceHeightElements.SingleOrDefault(element => element.Id == id);
                if (distanceHeightElement != null)
                {
                    this.mapper.Map(distanceHeightElement, this);
                }
            }
        }

        /// <summary>
        /// Executed on HTTP post.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.Length < 1)
            {
                this.ModelState.AddModelError(nameof(this.Length), "Länge muss mindestens 1 betragen.");
            }

            if (this.SortOrder < 1)
            {
                this.ModelState.AddModelError(nameof(this.SortOrder), "Sortierung muss mindestens 1 betragen.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var watraRoute = await this.distanceHeightElementServerAccess.GetWatraRouteByIdAsync(this.IdRoute);

            var element = watraRoute.WatraRouteDistanceHeightElements.SingleOrDefault(element => element.Id == this.Id);

            if (element == null)
            {
                element = new WatraRouteDistanceHeightElement();
                watraRoute.WatraRouteDistanceHeightElements.Add(element);
            }

            this.mapper.Map(this, element);

            var validationErrors = await this.distanceHeightElementServerAccess.ValidateWatraRouteAsync(watraRoute);
            this.ModelState.AddValidationErrors(validationErrors);

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            await this.distanceHeightElementServerAccess.UpdateWatraRouteAsync(watraRoute);

            return this.Redirect("Edit?id=" + this.IdRoute);
        }
    }
}
