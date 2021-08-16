// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
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
    /// View model to edit a <see cref="WatraRoute"/>.
    /// </summary>
    [BindProperties]
    public class WatraRouteEditViewModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly IWatraRouteServerAccess watraRouteServerAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteEditViewModel"/> class.
        /// </summary>
        public WatraRouteEditViewModel(IMapper mapper, IWatraRouteServerAccess watraRouteServerAccess)
        {
            this.mapper = mapper;
            this.watraRouteServerAccess = watraRouteServerAccess;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this watra route is active.
        /// </summary>
        public bool IsActiveWatra { get; set; }

        /// <summary>
        /// Gets or sets a list of distance/height element combinations.
        /// </summary>
        public IEnumerable<WatraRouteDistanceHeightElement> WatraRouteDistanceHeightElements { get; set; }

        /// <summary>
        /// Executed on HTTP get.
        /// </summary>
        public void OnGet(int id)
        {
            if (id < 1)
            {
                this.ModelState.TryAddModelError(string.Empty, "Ungültige ID, Daten können nicht geladen werden.");
                return;
            }

            // No await! Reason: If awaiting here, HTML is rendered before objects are fully retrieved...
            var watraRoute = this.watraRouteServerAccess.GetWatraRouteByIdAsync(id).Result;
            this.WatraRouteDistanceHeightElements = watraRoute.WatraRouteDistanceHeightElements.OrderBy(element => element.SortOrder).ToList();
            this.mapper.Map(watraRoute, this);
        }

        /// <summary>
        /// Executed on HTTP post.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                this.ModelState.AddModelError(nameof(this.Name), "Name muss eingegeben werden");
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                this.ModelState.AddModelError(nameof(this.Description), "Beschreibung muss eingegeben werden");
            }

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var watraRoute = await this.watraRouteServerAccess.GetWatraRouteByIdAsync(this.Id);

            this.mapper.Map(this, watraRoute);

            var validationErrors = await this.watraRouteServerAccess.ValidateWatraRouteAsync(watraRoute);
            this.ModelState.AddValidationErrors(validationErrors);

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            await this.watraRouteServerAccess.UpdateWatraRouteAsync(watraRoute);

            return this.Redirect("Calculator");
        }
    }
}
