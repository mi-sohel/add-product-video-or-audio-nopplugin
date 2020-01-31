using System.Collections.Generic;
using Nop.Plugin.Widgets.BsProductVideo.Domain;
using Nop.Core;

namespace Nop.Plugin.Widgets.BsProductVideo.Services
{
    public partial interface IProductVideoRecordService
    {
        void DeleteProductVideoRecord(ProductVideoRecord productVideoRecord);

        IList<ProductVideoRecord> GetAll();

        /// <summary>
        /// Gets ProductVideoRecord
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Products</returns>
        IPagedList<ProductVideoRecord> GetProductVideoRecords(
            int pageIndex = 0, int pageSize = int.MaxValue, int productId = 0);
        ProductVideoRecord GetById(int googleProductRecordId);

        IList<ProductVideoRecord> GetByProductId(int productId);

        void InsertProductVideoRecord(ProductVideoRecord productVideoRecord);

        void UpdateProductVideoRecord(ProductVideoRecord productVideoRecord);
    }
}
