using Nop.Core;

namespace Nop.Plugin.Widgets.BsProductVideo.Domain
{
    /// <summary>
    /// Represents a Google product record
    /// </summary>
    public partial class ProductVideoRecord : BaseEntity
    {
        public int ProductId { get; set; }
        public string EmbedVideoHtmlCode { get; set; }
        public int VideoThumbId { get; set; }
        public int DisplayOrder { get; set; }
    }
}