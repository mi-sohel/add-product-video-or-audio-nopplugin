using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
using Nop.Plugin.Widgets.BsProductVideo.Models;
using Nop.Services.Catalog;
using Nop.Plugin.Widgets.BsProductVideo.Services;
using System.Linq;
using Nop.Core.Domain.Media;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.BsProductVideo.Components
{
    [ViewComponent(Name = "WidgetsProductVideo")]
    public class WidgetsProductVideoViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly IProductVideoRecordService _productVideoRecordService;
        private readonly MediaSettings _mediaSettings;

        public WidgetsProductVideoViewComponent(IStoreContext storeContext, 
            IStaticCacheManager cacheManager, 
            ISettingService settingService, 
            IPictureService pictureService, 
            ILogger logger,
            IProductService productService,
            IProductVideoRecordService productVideoRecordService,
            MediaSettings mediaSettings)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._pictureService = pictureService;
            _logger = logger;
            _productService = productService;
            _productVideoRecordService = productVideoRecordService;
            _mediaSettings = mediaSettings;
        }

        public IViewComponentResult Invoke(string widgetZone, ProductDetailsModel additionalData)
        {

            var wZone = widgetZone;

            if (wZone == "productdetails_after_pictures" && additionalData != null)
            {
                var product = _productService.GetProductById(additionalData.Id);
                var model = new PublicInfoModel()
                {
                    ProductId = product.Id
                };
                var productVideoRecords = _productVideoRecordService.GetByProductId(product.Id);
                if (productVideoRecords.Count == 0)
                    return Content("");
                foreach (var embedVideoModel in productVideoRecords.Select(productVideoRecord => new PublicInfoModel.EmbedVideoModel()
                {
                    Id = productVideoRecord.Id,
                    VideoThumbUrl = _pictureService.GetPictureUrl(productVideoRecord.VideoThumbId, _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage),
                    EmbedVideoHtmlCode = productVideoRecord.EmbedVideoHtmlCode
                }))
                {
                    model.EmbedVideoRecordModels.Add(embedVideoModel);
                }

                return View("~/Plugins/Widgets.BsProductVideo/Views/ProductVideo/PublicInfo.cshtml", model);
            }

            return Content("");
        }

        

       
    }
}
