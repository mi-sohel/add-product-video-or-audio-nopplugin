using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Widgets.BsProductVideo.Domain;

namespace Nop.Plugin.Widgets.BsProductVideo.Data
{
    public partial class ProductVideoRecordMap : NopEntityTypeConfiguration<ProductVideoRecord>
    {
       

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductVideoRecord> builder)
        {
            builder.ToTable("Bs_ProductVideoRecord");
            builder.HasKey(record => record.Id);

        }
    }
}