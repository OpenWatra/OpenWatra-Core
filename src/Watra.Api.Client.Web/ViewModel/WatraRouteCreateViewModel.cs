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
    using Watra.Client.Web.ServerAccess;
    using Watra.Client.Web.ServerAccess.Services;
    using Watra.Client.Web.Tools;

    /// <summary>
    /// View model for the creation of a <see cref="WatraRoute"/>.
    /// </summary>
    [BindProperties]
    public class WatraRouteCreateViewModel : PageModel
    {
        private readonly IWatraRouteServerAccess watraRouteServerAccess;
        private readonly IPumpSelectionServerAccess pumpSelectionServerAccess;
        private readonly IPumpServerAccess pumpServerAccess;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteCreateViewModel"/> class.
        /// </summary>
        public WatraRouteCreateViewModel(IWatraRouteServerAccess watraRouteServerAccess, IMapper mapper, IPumpSelectionServerAccess pumpSelectionServerAccess, IPumpServerAccess pumpServerAccess)
        {
            this.watraRouteServerAccess = watraRouteServerAccess;
            this.mapper = mapper;
            this.pumpSelectionServerAccess = pumpSelectionServerAccess;
            this.pumpServerAccess = pumpServerAccess;
        }

        /// <summary>
        /// Gets or sets the name of the Watra.
        /// </summary>
        public string Name { get; set; }

        /// <summary>Gets or sets description of the route.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets flow rate in water transport, Unit l/min</summary>
        public double FlowRate { get; set; }

        /// <summary>Gets or sets minimal pressure at the end of hose line, Unit bar</summary>
        public double MinimalOutletPressure { get; set; }

        /// <summary>Gets or sets the safety pressure used during calculation, Unit bar</summary>
        public double SafetyPressure { get; set; }

        /// <summary>
        /// Gets or sets a list of selected pumps.
        /// </summary>
        public List<PumpSelectionViewModel> PumpSelections { get; set; }

        /// <summary>
        /// Executed on HTTP get.
        /// </summary>
        public void OnGet()
        {
            // No await! Reason: If awaiting here, HTML is rendered before objects are fully retrieved...
            this.PumpSelections = this.pumpSelectionServerAccess.GetPossiblePumpSelectionsAsViewModelsAsync().Result;
        }

        /// <summary>
        /// Executed on HTTP post.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // ToDo: this is highly problematic regarding performance.
            // Should instead suffice to deliver the pump's ID to the API.
            foreach (var pumpSelection in this.PumpSelections)
            {
                pumpSelection.Pump = await this.pumpServerAccess.GetPumpByIdAsync(pumpSelection.IdPump);
            }

            if (!this.PumpSelections.Any(selection => selection.IsSelected))
            {
                // Bind this to the name, because list binding with razor is a PITA
                this.ModelState.AddModelError(nameof(this.Name), "Mindestens eine Pumpe muss ausgewählt werden.");
            }

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

            var watraRoute = this.mapper.Map<WatraRouteCreateViewModel, WatraRoute>(this);

            var validationErrors = await this.watraRouteServerAccess.ValidateWatraRouteAsync(watraRoute);
            this.ModelState.AddValidationErrors(validationErrors);

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var createdWatraRoute = await this.watraRouteServerAccess.AddWatraRouteAsync(watraRoute);

            return this.Redirect("Edit?id=" + createdWatraRoute.Id);
        }
    }
}
