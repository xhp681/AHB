﻿using AHB.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;

namespace AHB.Web
{
    public class RsRedirectResultExecutor: RedirectResultExecutor
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly SecuritySettings _securitySettings;
        private readonly IWebHelper _webHelper;

        public RsRedirectResultExecutor(IActionContextAccessor actionContextAccessor,
            ILoggerFactory loggerFactory,
            IUrlHelperFactory urlHelperFactory,
            SecuritySettings securitySettings,
            IWebHelper webHelper) : base(loggerFactory, urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _securitySettings = securitySettings;
            _webHelper = webHelper;
        }


        /// <summary>
        /// Execute passed redirect result
        /// </summary>
        /// <param name="context">Action context</param>
        /// <param name="result">Redirect result</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override Task ExecuteAsync(ActionContext context, RedirectResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (_securitySettings.AllowNonAsciiCharactersInHeaders)
            {
                //passed redirect URL may contain non-ASCII characters, that are not allowed now (see https://github.com/aspnet/KestrelHttpServer/issues/1144)
                //so we force to encode this URL before processing
                var url = WebUtility.UrlDecode(result.Url);
                var urlHelper = result.UrlHelper ?? _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                var isLocalUrl = urlHelper.IsLocalUrl(url);

                var uri = new Uri(isLocalUrl ? $"{_webHelper.GetStoreLocation().TrimEnd('/')}{url}" : url, UriKind.Absolute);

                result.Url = isLocalUrl ? uri.PathAndQuery : $"{uri.GetLeftPart(UriPartial.Query)}{uri.Fragment}";
            }

            return base.ExecuteAsync(context, result);
        }
    }
}
