using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nop.Core.Data;
using Nop.Plugin.Widgets.BsProductVideo.Domain;
using Nop.Core;

namespace Nop.Plugin.Widgets.BsProductVideo.Services
{
    public partial class ProductVideoRecordService : IProductVideoRecordService
    {
        #region Fields

        private readonly IRepository<ProductVideoRecord> _productVideoRecordRepository;

        #endregion

        #region Ctor

        public ProductVideoRecordService(IRepository<ProductVideoRecord> productVideoRecordRepository)
        {
            this._productVideoRecordRepository = productVideoRecordRepository;
        }

        #endregion

        #region Utilties

        private string GetEmbeddedFileContent(string resourceName)
        {
            string fullResourceName = string.Format("Nop.Plugin.Feed.Froogle.Files.{0}", resourceName);
            var assem = this.GetType().Assembly;
            using (var stream = assem.GetManifestResourceStream(fullResourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        #endregion

        #region Methods

        public virtual void DeleteProductVideoRecord(ProductVideoRecord productVideoRecord)
        {
            if (productVideoRecord == null)
                throw new ArgumentNullException("productVideoRecord");

            _productVideoRecordRepository.Delete(productVideoRecord);
        }

        public virtual IList<ProductVideoRecord> GetAll()
        {
            var query = from gp in _productVideoRecordRepository.Table
                        orderby gp.Id
                        select gp;
            query = query.OrderBy(x => x.DisplayOrder);
            var records = query.ToList();
            return records;
        }
        public virtual IPagedList<ProductVideoRecord> GetProductVideoRecords(
           int pageIndex = 0, int pageSize = int.MaxValue, int productId = 0)
        {
            var query = _productVideoRecordRepository.Table;
            query = query.OrderBy(x => x.Id);

            if (productId > 0)
            {
                query = query.Where(x => x.ProductId.Equals(productId));
            }
            query = query.OrderBy(x => x.DisplayOrder);
            var products = new PagedList<ProductVideoRecord>(query, pageIndex, pageSize);
            return products;
        }
        public virtual ProductVideoRecord GetById(int productVideoRecordId)
        {
            if (productVideoRecordId == 0)
                return null;

            return _productVideoRecordRepository.GetById(productVideoRecordId);
        }

        public virtual IList<ProductVideoRecord> GetByProductId(int productId)
        {
            if (productId == 0)
                return new List<ProductVideoRecord>();

            var query = from gp in _productVideoRecordRepository.Table
                        where gp.ProductId == productId
                        orderby gp.Id
                        select gp;
            query = query.OrderBy(x => x.DisplayOrder);
            return query.ToList();
        }

        public virtual void InsertProductVideoRecord(ProductVideoRecord productVideoRecord)
        {
            if (productVideoRecord == null)
                throw new ArgumentNullException("productVideoRecord");

            _productVideoRecordRepository.Insert(productVideoRecord);
        }

        public virtual void UpdateProductVideoRecord(ProductVideoRecord productVideoRecord)
        {
            if (productVideoRecord == null)
                throw new ArgumentNullException("productVideoRecord");

            _productVideoRecordRepository.Update(productVideoRecord);
        }

        #endregion
    }
}
