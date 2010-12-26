using System;
using System.Windows.Forms;
//using System.Drawing.Imaging;

namespace Raytracing.Test {
    public partial class FrmMain : Form {
        private int _traceDepth = 4;
        private int _tileSize = 32;
        private int _samples = 32;

        private bool _ignoreEvent;

        private FrmRenderOutput _output;

        public FrmMain() {
            InitializeComponent();
        }

        /*
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (picRenderTarget.Image == null || !_bRenderComplete) {
                MessageBox.Show("Please render an image first.", "Raytracing", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.AddExtension = true;
                sfd.AutoUpgradeEnabled = true;
                sfd.CheckPathExists = true;
                sfd.DefaultExt = "png";
                sfd.Filter = "JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|Windows Bitmap (*.bmp)|*.bmp||";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                sfd.OverwritePrompt = true;
                
                if (sfd.ShowDialog(this) == DialogResult.OK) {
                    int nPos = sfd.FileName.LastIndexOf('.');
                    string sExtension = "";
                    if (nPos > -1)
                        sExtension = sfd.FileName.ToUpperInvariant().Substring(sfd.FileName.LastIndexOf('.') + 1);

                    ImageFormat selectedImageFormat = ImageFormat.Bmp;

                    switch (sExtension) {
                        case "BMP":
                            break; //already selected
                        case "JPG":
                        case "JPEG":
                            selectedImageFormat = ImageFormat.Jpeg;
                            break;
                        case "PNG":
                            selectedImageFormat = ImageFormat.Jpeg;
                            break;
                        default:
                            if (MessageBox.Show("Unrecognized image format. The image will be saved as a Windows Bitmap file.\r\nDo you want to continue saving the image?", "Raytracing", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                return;
                            break;
                    }

                    Bitmap bmp = new Bitmap(_tracer.FrameBuffer);
                    bmp.Save(sfd.FileName, selectedImageFormat);
                }
            }
        }
        */

        private void TrackBar_ValueChanged(object sender, EventArgs e) {
            if (_ignoreEvent)
                return;

            TrackBar tb = sender as TrackBar;
            if (tb == null)
                return;

            MaskedTextBox target = txtSamples;
            if (tb == tbTraceDepth)
                target = txtTraceDepth;
            else if (tb == tbTileSize)
                target = txtTileSize;

            target.Text = tb.Value.ToString();
        }

        private void TraceDepth_TextChanged(object sender, EventArgs e) {
            int value;

            if (!RetrieveAndTestValue(txtTraceDepth, tbTraceDepth, out value))
                return;

            //Only update the internal value if the range is respected
            _traceDepth = value;
        }

        private void Samples_TextChanged(object sender, EventArgs e) {
            int value;

            if (!RetrieveAndTestValue(txtSamples, tbSamples, out value))
                return;

            //Only update the internal value if the range is respected
            _samples = value;
        }

        private void TileSize_TextChanged(object sender, EventArgs e) {
            int value;

            if (!RetrieveAndTestValue(txtTileSize, tbTileSize, out value))
                return;

            //Only update the internal value if the range is respected
            _tileSize = value;
        }

        private bool ValueInRange(int value, int min, int max) {
            return value >= min && value <= max;
        }

        private bool RetrieveAndTestValue(MaskedTextBox source, TrackBar valueBar, out int value) {
            value = -1;

            if (source.TextLength == 0)
                return false;

            int.TryParse(source.Text, out value);
            if (!ValueInRange(value, valueBar.Minimum, valueBar.Maximum))
                return false;

            _ignoreEvent = true;
            valueBar.Value = value;
            _ignoreEvent = false;

            return true;
        }

        private void CloseButtonClick(object sender, EventArgs e) {
            Close();
        }

        private void RenderButtonClick(object sender, EventArgs e) {
            Constants.TileSize = _tileSize;
            Constants.TraceDepth = _traceDepth;
            Constants.Samples = _samples;
            Constants.ReflectionsEnabled = chkReflections.Checked;
            Constants.RefractionsEnabled = chkRefraction.Checked;

            if (_output == null) {
                _output = new FrmRenderOutput();
                _output.TraceCompleted += OutputWindow_TraceCompleted;
                _output.FormClosed += OutputWindow_FormClosed;
                _output.Show();
            }
            else
                _output.Activate();
            
            Enabled = false;

            _output.StartTracing();
        }

        private void OutputWindow_TraceCompleted(object sender, EventArgs e) {
            Enabled = true;
        }

        private void OutputWindow_FormClosed(object sender, EventArgs e) {
            _output.TraceCompleted -= OutputWindow_TraceCompleted;
            _output.FormClosed -= OutputWindow_FormClosed;
            _output.Dispose();
            _output = null;

            Enabled = true;
        }
    }
}
