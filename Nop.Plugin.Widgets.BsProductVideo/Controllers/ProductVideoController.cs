using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Core.Plugins;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Security;
using Nop.Plugin.Widgets.BsProductVideo.Models;
using Nop.Plugin.Widgets.BsProductVideo.Services;
using Nop.Plugin.Widgets.BsProductVideo.Domain;
using Nop.Services.Shipping;
using Nop.Services.Customers;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Plugins;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Widgets.BsProductVideo.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class ProductVideoController : BasePluginController
    {
        private readonly IProductVideoRecordService _productVideoRecordService;
        private readonly IProductService _productService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly IStoreService _storeService;
        private readonly ProductVideoSettings _productVideoSettings;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly IShippingService _shippingService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly IProductModelFactory _productModelFactory;

        public ProductVideoController(IProductVideoRecordService productVideoRecordService,
            IProductService productService,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            IPluginFinder pluginFinder,
            ILogger logger,
            IWebHelper webHelper,
            IStoreService storeService,
            ProductVideoSettings productVideoSettings,
            ISettingService settingService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IVendorService vendorService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IWorkContext workContext,
            IShippingService shippingService,
            IProductAttributeService productAttributeService,
            ICustomerService customerService,
            CustomerSettings customerSettings, 
            MediaSettings mediaSettings,
            IProductModelFactory productModelFactory)
        {
            this._productVideoRecordService = productVideoRecordService;
            this._productService = productService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._pluginFinder = pluginFinder;
            this._logger = logger;
            this._webHelper = webHelper;
            this._storeService = storeService;
            this._productVideoSettings = productVideoSettings;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._pictureService = pictureService;

            this._categoryService = categoryService;
            this._vendorService = vendorService;
            this._manufacturerService = manufacturerService;
            this._workContext = workContext;
            this._shippingService = shippingService;
            this._productAttributeService = productAttributeService;
            this._customerService = customerService;
            this._customerSettings = customerSettings;
            _mediaSettings = mediaSettings;
            _productModelFactory = productModelFactory;
        }

        #region Utilities
        [NonAction]
        protected virtual List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var categoriesIds = new List<int>();
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            foreach (var category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id));
            }
            return categoriesIds;
        } 
        #endregion

        #region Methods
 

        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            

          //  var model = new ProductListModel();

            //prepare model
            var model = _productModelFactory.PrepareProductSearchModel(new ProductSearchModel());
            //a vendor should have access only to his products
           

            return View("~/Plugins/Widgets.BsProductVideo/Views/ProductVideo/Configure.cshtml", model);
        }

       
        
        public ActionResult VideoCreate(int id = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var model = new ProductVideoRecordModel();
            model.ProductId = id;

            return View("~/Plugins/Widgets.BsProductVideo/Views/ProductVideo/VideoCreate.cshtml", model);
        }
         
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult VideoCreate(ProductVideoRecordModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            if (!ModelState.IsValid)
            {
                return Configure();
            }

            var productVideoRecord = new ProductVideoRecord
            {
                ProductId = model.ProductId,
                EmbedVideoHtmlCode = model.EmbedVideoHtmlCode,
                VideoThumbId = model.VideoThumbId,
                DisplayOrder = model.DisplayOrder

            };
            _productVideoRecordService.InsertProductVideoRecord(productVideoRecord);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            //redisplay the form
            return View("~/Plugins/Widgets.BsProductVideo/Views/ProductVideo/VideoCreate.cshtml", model);
        }

        [HttpPost]

        [AdminAntiForgery]
        public ActionResult ProductVideoRecordList(DataSourceRequest command, int productId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var productVideoRecords = _productVideoRecordService.GetProductVideoRecords(pageIndex: command.Page - 1,
                pageSize: command.PageSize, productId: productId);
            var productsModel = productVideoRecords
                .Select(x =>
                {
                    var model = new ProductVideoRecordModel()
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        EmbedVideoHtmlCode = x.EmbedVideoHtmlCode,
                        VideoThumbId = x.VideoThumbId,
                        VideoThumbUrl = _pictureService.GetPictureUrl(x.VideoThumbId, 100),
                        DisplayOrder = x.DisplayOrder

                    };
                    var product = _productService.GetProductById(x.ProductId);
                    if (product != null)
                    {
                        model.ProductName = product.Name;

                    }

                    return model;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = productsModel,
                Total = productVideoRecords.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]

        [AdminAntiForgery]
        public ActionResult ProductVideoRecordUpdate(ProductVideoRecordModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return Content("Access denied");

            var productVideoRecor = _productVideoRecordService.GetById(model.Id);
            if (productVideoRecor != null)
            {
                //productVideoRecor.ProductId = model.ProductId;
                productVideoRecor.EmbedVideoHtmlCode = model.EmbedVideoHtmlCode;
                productVideoRecor.DisplayOrder = model.DisplayOrder;

                _productVideoRecordService.UpdateProductVideoRecord(productVideoRecor);

            }

            return new NullJsonResult();
        }

        [HttpPost]

        public ActionResult ProductVideoRecordDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return Content("Access denied"); ;

            var productVideoRecord = _productVideoRecordService.GetById(id);
            if (productVideoRecord == null)
                throw new ArgumentException("No record found with the specified id");
            var picture = _pictureService.GetPictureById(productVideoRecord.VideoThumbId);
            if (picture != null)
                _pictureService.DeletePicture(picture);
            _productVideoRecordService.DeleteProductVideoRecord(productVideoRecord);


            return new NullJsonResult();
        } 
        #endregion

        #region Product List

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return Content("Access denied");

            //  var model = new ProductListModel();

            //prepare model
            var model = _productModelFactory.PrepareProductSearchModel(new ProductSearchModel());
            return View("~/Plugins/Widgets.BsProductVideo/Views/ProductVideo/List.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProductList(DataSourceRequest command, ProductSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedKendoGridJson();

           


            //get parameters to filter comments
            var overridePublished = searchModel.SearchPublishedId == 0 ? null : (bool?)(searchModel.SearchPublishedId == 1);
            if (_workContext.CurrentVendor != null)
                searchModel.SearchVendorId = _workContext.CurrentVendor.Id;
            var categoryIds = new List<int> { searchModel.SearchCategoryId };
            if (searchModel.SearchIncludeSubCategories && searchModel.SearchCategoryId > 0)
            {
                var childCategoryIds = _categoryService.GetChildCategoryIds(parentCategoryId: searchModel.SearchCategoryId, showHidden: true);
                categoryIds.AddRange(childCategoryIds);
            }

            //get products
            var products = _productService.SearchProducts(showHidden: true,
                categoryIds: categoryIds,
                manufacturerId: searchModel.SearchManufacturerId,
                storeId: searchModel.SearchStoreId,
                vendorId: searchModel.SearchVendorId,
                warehouseId: searchModel.SearchWarehouseId,
                productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                overridePublished: overridePublished);

            //prepare list model
            var model = new ProductListModel
            {
                Data = products.Select(product =>
                {
                    //fill in model values from the entity
                    var productModel = product.ToModel<ProductModel>();

                    //little performance optimization: ensure that "FullDescription" is not returned
                    productModel.FullDescription = string.Empty;
                    productModel.DownloadExpirationDays = _productVideoRecordService.GetByProductId(product.Id).Count();
                    //fill in additional values (not existing in the entity)
                    var defaultProductPicture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                    productModel.PictureThumbnailUrl = _pictureService.GetPictureUrl(defaultProductPicture, 75);
                    productModel.ProductTypeName = _localizationService.GetLocalizedEnum(product.ProductType);
                    if (product.ProductType == ProductType.SimpleProduct && product.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
                        productModel.StockQuantityStr = _productService.GetTotalStockQuantity(product).ToString();

                    return productModel;
                }),
                Total = products.TotalCount
            };


            return Json(model);

           
        }

       
        #endregion

        

        
    }
}
