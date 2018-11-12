﻿using System;
using System.Linq;
using AutoMapper;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.Routing;

namespace Umbraco.Web.Models.Mapping
{
    internal class ContentUrlResolver : IValueResolver<IContent, ContentItemDisplay, UrlInfo[]>
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly PublishedRouter _publishedRouter;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedTextService _textService;
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly ILogger _logger;

        public ContentUrlResolver(
            IUmbracoContextAccessor umbracoContextAccessor,
            PublishedRouter publishedRouter,
            ILocalizationService localizationService,
            ILocalizedTextService textService,
            IContentService contentService,
            IContentTypeService contentTypeService,
            ILogger logger)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new System.ArgumentNullException(nameof(umbracoContextAccessor));
            _publishedRouter = publishedRouter ?? throw new System.ArgumentNullException(nameof(publishedRouter));
            _localizationService = localizationService ?? throw new System.ArgumentNullException(nameof(localizationService));
            _textService = textService ?? throw new System.ArgumentNullException(nameof(textService));
            _contentService = contentService ?? throw new System.ArgumentNullException(nameof(contentService));
            _contentTypeService = contentTypeService ?? throw new ArgumentNullException(nameof(contentTypeService));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public UrlInfo[] Resolve(IContent source, ContentItemDisplay destination, UrlInfo[] destMember, ResolutionContext context)
        {
            var umbracoContext = _umbracoContextAccessor.UmbracoContext;

            var urls = umbracoContext == null
                ? new[] { UrlInfo.Message("Cannot generate urls without a current Umbraco Context") }
                : source.GetContentUrls(_publishedRouter, umbracoContext, _localizationService, _textService,  _contentService, _contentTypeService, _logger).ToArray();

            return urls;
        }
    }
}
