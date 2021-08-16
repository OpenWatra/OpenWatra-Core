// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ServerAccess;
    using Watra.Client.Web.ServerAccess.Services;

    /// <summary>
    /// View model for the creation of a <see cref="WatraRoute"/>.
    /// </summary>
    [BindProperties]
    public class CalculatorViewModel : PageModel
    {
        private readonly IWatraRouteServerAccess watraRouteServerAccess;
        private readonly IWatraCalculationServerAccess watraCalculationServerAccess;

        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorViewModel"/> class.
        /// </summary>
        public CalculatorViewModel(IWatraRouteServerAccess watraRouteServerAccess, IWatraCalculationServerAccess watraCalculationServerAccess, IMapper mapper)
        {
            this.watraRouteServerAccess = watraRouteServerAccess;
            this.watraCalculationServerAccess = watraCalculationServerAccess;
            this.mapper = mapper;

            this.ErrorMessages = new List<string>();
            this.WarningMessages = new List<string>();
            this.InfoMessages = new List<string>();
        }

        /// <summary>
        /// Gets or sets list with all active Watra routes.
        /// </summary>
        public List<WatraRouteViewModel> DisplayWatraRoutes { get; set; }

        /// <summary>
        /// Gets or sets the watra calculation objects.
        /// </summary>
        public WatraCalculationViewModel WatraCalculation { get; set; }

        /// <summary>
        /// Gets or sets the Id of the selected Watra.
        /// </summary>
        public int SelectedWatraId { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected section of the currently calculated Watra.
        /// </summary>
        public int SelectedSection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether watreas shoud be displayed without filter.
        /// </summary>
        public bool NoFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a error message is available in the calculation object.
        /// </summary>
        public bool ErrorMessageAvailable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a warnin message is available in the calculation object.
        /// </summary>
        public bool WarningMessageAvailable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a info message is available in the calculation object.
        /// </summary>
        public bool InfoMessageAvailable { get; set; }

        /// <summary>
        /// Gets or sets a list with all error messages in the calculation object.
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// Gets or sets a list with all warning messages in the calculation object.
        /// </summary>
        public List<string> WarningMessages { get; set; }

        /// <summary>
        /// Gets or sets a list with all info messages in the calculation object.
        /// </summary>
        public List<string> InfoMessages { get; set; }

        /// <summary>
        /// Executed on HTTP get.
        /// </summary>
        public void OnGet(bool noFilter)
        {
            this.NoFilter = noFilter;
            try
            {
                var watraRoutes = this.watraRouteServerAccess.GetAllWatraRouteAsViewModelAsync().Result;
                if (this.NoFilter)
                {
                    this.DisplayWatraRoutes = watraRoutes;
                }
                else
                {
                    var watraRoutesFiltered = watraRoutes.Where(watra => watra.IsActiveWatra == true).ToList();
                    this.DisplayWatraRoutes = watraRoutesFiltered;
                }
            }
            catch (ApiException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Fehler beim Verbinden mit dem API.");
            }
            catch (AutoMapperMappingException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Datentypen stimmen nicht mit der API überein.");
            }
            catch (SocketException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Es konnte keine Verbindung hergestellt werden, da der API Server die Verbindung verweigerte.");
            }
            catch (IOException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Interner Fehler aufgetreten.");
            }
            catch (TaskCanceledException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Vorgang konnte nicht ausgeführt werden.");
            }
            catch (TimeoutException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Zeitüberschreitung bei der Anfrage.");
            }
            catch (HttpRequestException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Es konnte keine Verbindung hergestellt werden, da der API Server die Verbindung verweigerte.");
            }
            catch (InvalidOperationException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Die Id des verlangten Wassertransports ist ungültig.");
            }
            catch (AggregateException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("API konnte Anfrage nicht bearbeiten.");
            }

            if (this.DisplayWatraRoutes == null)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Die Wassertransporte konnten nich vom Server geladen werden.");
            }
        }

        /// <summary>
        /// Executed on HTTP post.
        /// </summary>
        public void OnPostAsync()
        {
            try
            {
                var watraRoutes = this.watraRouteServerAccess.GetAllWatraRouteAsViewModelAsync().Result;
                var watraRoutesFiltered = watraRoutes.Where(watra => watra.IsActiveWatra == true).ToList();
                this.DisplayWatraRoutes = watraRoutesFiltered;

                this.WatraCalculation = this.watraCalculationServerAccess.GetWatraCalculationByIdAsyncAsViewModel(this.SelectedWatraId).Result;

                // get messages from calculation
                if (this.WatraCalculation.WatraCalculationMessage != null)
                {
                    foreach (var message in this.WatraCalculation.WatraCalculationMessage)
                    {
                        if (message.Severity.Equals("error"))
                        {
                            this.ErrorMessageAvailable = true;
                            this.ErrorMessages.Add(message.Message);
                        }

                        if (message.Severity.Equals("warning"))
                        {
                            this.WarningMessageAvailable = true;
                            this.WarningMessages.Add(message.Message);
                        }

                        if (message.Severity.Equals("info"))
                        {
                            this.InfoMessageAvailable = true;
                            this.InfoMessages.Add(message.Message);
                        }
                    }
                }
            }
            catch (ApiException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Fehler beim Verbinden mit dem API.");
            }
            catch (AutoMapperMappingException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Datentypen stimmen nicht mit der API überein.");
            }
            catch (ArgumentNullException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("API liefert ungültige Daten.");
            }
            catch (SocketException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Es konnte keine Verbindung hergestellt werden, da der API Server die Verbindung verweigerte.");
            }
            catch (IOException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Interner Fehler aufgetreten.");
            }
            catch (TaskCanceledException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Vorgang konnte nicht ausgeführt werden.");
            }
            catch (TimeoutException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Zeitüberschreitung bei der Anfrage.");
            }
            catch (HttpRequestException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Es konnte keine Verbindung hergestellt werden, da der API Server die Verbindung verweigerte.");
            }
            catch (InvalidOperationException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Die Id des verlangten Wassertransports ist ungültig.");
            }
            catch (AggregateException)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("API konnte Anfrage nicht bearbeiten.");
            }

            if (this.WatraCalculation.WatraCalculationSection == null)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Die Wassertransportberechnung konnte nicht gefunden werden.");
            }

            if (this.WatraCalculation == null)
            {
                this.ErrorMessageAvailable = true;
                this.ErrorMessages.Add("Die Wassertransportberechnung konnte nicht gefunden werden.");
            }
        }
    }
}
