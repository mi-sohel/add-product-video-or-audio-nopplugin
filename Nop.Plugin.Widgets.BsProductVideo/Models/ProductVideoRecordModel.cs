using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BsProductVideo.Models
{
    public class ProductVideoRecordModel:BaseNopEntityModel
    {

        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.ProductId")]
        public int ProductId { get; set; }


        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.ProductName")]
        public string ProductName { get; set; }


        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.EmbedVideoHtmlCode")]
    
        public string EmbedVideoHtmlCode { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.Picture")]
        [UIHint("Picture")]
        public int VideoThumbId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.Picture")]
        public string VideoThumbUrl { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.BsProductVideo.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #region nested class

        public partial class AddProductToProductVideoRecordModel : BaseNopModel
        {
            public AddProductToProductVideoRecordModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
         
            public string SearchProductName { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public int SearchCategoryId { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public int SearchManufacturerId { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public int SearchStoreId { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public int SearchVendorId { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }
            public int SelectedProductId { get; set; }
        }
        #endregion
    }
}