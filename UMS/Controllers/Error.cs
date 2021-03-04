using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UMS.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var errorDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"Error Path: {errorDetails.Path}, Error Message: {errorDetails.Error}");
            return View("Error");
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var statusDetail = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if(statusCode == 404)
            {
                _logger.LogError($"Message: Page not found!, Error path: {statusDetail.OriginalPath}, Error Query string: {statusDetail.OriginalQueryString}");
            }

            return View("NotFound");
        }
    }
}
