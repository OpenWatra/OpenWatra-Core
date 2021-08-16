// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ServerAccess.Services;

    /// <summary>
    /// View model to edit a <see cref="WatraRoute"/>.
    /// </summary>
    [BindProperties]
    public class WatraRouteDeleteViewModel : PageModel
    {
        private readonly IMapper mapper;
        private readonly IWatraRouteServerAccess watraRouteServerAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteDeleteViewModel"/> class.
        /// </summary>
        public WatraRouteDeleteViewModel(IMapper mapper, IWatraRouteServerAccess watraRouteServerAccess)
        {
            this.mapper = mapper;
            this.watraRouteServerAccess = watraRouteServerAccess;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Watra.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Executed on HTTP get.
        /// </summary>
        /// <param name="id">Id of the Watra to delete</param>
        public void OnGet(int id)
        {
            this.Id = id;
            if (this.Id < 1)
            {
                this.ModelState.TryAddModelError(string.Empty, "Ungültige ID, Daten können nicht geladen werden.");
                return;
            }

            // No await! Reason: If awaiting here, HTML is rendered before objects are fully retrieved...
            var watraRoute = this.watraRouteServerAccess.GetWatraRouteByIdAsync(id).Result;
            this.mapper.Map(watraRoute, this);
        }

        /// <summary>
        /// Executed on HTTP post.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.Id < 1)
            {
                this.ModelState.TryAddModelError(string.Empty, "Ungültige ID, Daten können nicht geladen werden.");
                return this.Page();
            }

            await this.watraRouteServerAccess.DeleteWatraRoute(this.Id);

            return this.Redirect("Calculator");
        }
    }
}
