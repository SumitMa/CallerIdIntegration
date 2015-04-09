using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CallerIdIntegration.Common
{
  public  class Icon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string CreatedOn { get; set; }

        public object ImageSources
        {
            get
            {
                if (Path == string.Empty)
                {
                    return null;
                }

                BitmapImage image = new BitmapImage();

                try
                {
                    image.BeginInit();
                    image.UriSource = new Uri(Path, UriKind.RelativeOrAbsolute);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.IgnoreImageCache
                      | BitmapCreateOptions.PreservePixelFormat;

                    image.EndInit();
                }
                catch
                {

                }

                return image;
            }
        }
    }
}
