using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlideShow.TCP_IP.WinForm.Concrete;

namespace SlideShow.TCP_IP.WinForm
{
    public partial class SSlideShow : Form
    {
        #region Properties
        private System.Timers.Timer _NextSlideTimer = new System.Timers.Timer(Constants.SLIDE_SHOW__INTERVAL_TIME);
        private IList<Image> _Images { get; set; }
        private readonly Utility _Utility;
        #endregion


        internal SSlideShow(IList<Image> images)
        {
            InitializeComponent();
            _Utility = new Utility();
            Init(images);
        }


        private void Init(IList<Image> images)
        {
            _Images = images;
            _PictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            SetRandomSlide();
            this.BringToFront();
            _NextSlideTimer.Elapsed += new System.Timers.ElapsedEventHandler(NextSlideTimer_Elapsed);
            _NextSlideTimer.Start();
        }


        internal void PausePlaySlideShow()
        {
            _NextSlideTimer.Enabled = !this._NextSlideTimer.Enabled;
            if (_NextSlideTimer.Enabled)
            {
                SetRandomSlide();
            }
        }


        private void SetRandomSlide()
        {
            if (_Images.Count == 0)
            {
                StopSlideShow();
                return;
            }
            Image image1 = (Image)_Utility.GetRandom(_Images);
            _Images.Remove(image1);
            SetPictureBoxImage(image1);
        }


        delegate void StopSlideShowCallback();
        internal void StopSlideShow()
        {
            if (this.InvokeRequired)
            {
                StopSlideShowCallback d = new StopSlideShowCallback(StopSlideShow);
                this.Invoke(d);
            }
            else
            {
                this.Close();
            }
        }


        delegate void SetPictureBoxImageCallback(Image image);
        public void SetPictureBoxImage(Image image)
        {
            if (_PictureBox.InvokeRequired)
            {
                SetPictureBoxImageCallback d = new SetPictureBoxImageCallback(SetPictureBoxImage);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                if (_PictureBox.Image != null)
                {
                    _PictureBox.Image.Dispose();
                }
                _PictureBox.Image = image;
            }
        }


        internal void Stop()
        {
            _NextSlideTimer.Enabled = false;
            _NextSlideTimer.Stop();
            Close();
        }


        void NextSlideTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetRandomSlide();
        }
    }
}
