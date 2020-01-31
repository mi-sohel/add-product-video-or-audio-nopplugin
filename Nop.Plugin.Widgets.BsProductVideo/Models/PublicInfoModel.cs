using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc;


namespace Nop.Plugin.Widgets.BsProductVideo.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public PublicInfoModel()
        {
            EmbedVideoRecordModels=new List<EmbedVideoModel>();
        }
        public int ProductId { get; set; }

        public IList<EmbedVideoModel> EmbedVideoRecordModels { get; set; }

        public class EmbedVideoModel
        {
            public int Id { get; set; }
            public string VideoThumbUrl { get; set; }
            public string EmbedVideoHtmlCode { get; set; }

        }
    }
}