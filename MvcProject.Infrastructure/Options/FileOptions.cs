using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Options
{
    public class FileOptions
    {
        public string WebRootPath { get; set; }
        public string ImagesFolder { get; set; } = "img";
        public Dictionary<string, string> ImageAcceptedContentTypes { get; set; } = new Dictionary<string, string>
        {
            { "image/png", ".png"},
            {"image/jpeg", ".jpg" },
        };

        public int MaxBytesfileSize { get; set; } = 1024 * 1024 * 8;

    }
}
