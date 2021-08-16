// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Model for the index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        public IndexModel(ILogger<IndexModel> logger)
        {
            this.logger = logger;
        }
    }
}
