using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlideShow.TCP_IP.WinForm.Concrete;

namespace SlideShow.TCP_IP.WinForm.Business
{
    class ImageBusiness 
    {
        public ImageBusiness()
        {
        }

        public IList<System.Drawing.Image> GetImagesForSlideShow()
        {
            IList<Image> images = new List<Image>();
            images.Add(SlideShow.TCP_IP.WinForm.Properties.Resources.Beach1);
            images.Add(SlideShow.TCP_IP.WinForm.Properties.Resources.Beach2);
            images.Add(SlideShow.TCP_IP.WinForm.Properties.Resources.Beach3);
            images.Add(SlideShow.TCP_IP.WinForm.Properties.Resources.Beach4);
            images.Add(SlideShow.TCP_IP.WinForm.Properties.Resources.Beach5);
            return images;
        }
    }
}
