using System;
using System.Windows.Forms;

namespace Raytracing.Test {
    public partial class FrmRenderOutput : Form {
        public Engine Tracer { get; private set; }

        public event EventHandler TraceCompleted;

        public FrmRenderOutput() {
            InitializeComponent();

            Tracer = new Engine();
            Tracer.SetTarget(800, 600);

            picRenderTarget.Image = Tracer.FrameBuffer;
        }

        public void StartTracing() {
            lblRenderTime.Text = "";
            lblRaysCast.Text = "";
            lblIntersections.Text = "";

            bool done = false;

            while (!done)
                done = Tracer.Scene.InitScene();

            Tracer.InitRenderer(new Vector3D(-.2f, 0, -2), new Vector3D(-.2f, .8f, 5));
            
            DateTime start = DateTime.Now;
            done = false;
            while (!done) {
                done = Tracer.RenderTiles();
                picRenderTarget.Refresh();
            }

            DateTime end = DateTime.Now;
            TimeSpan elapsed = end - start;

            lblRenderTime.Text = string.Format("{0:00}:{1:00}.{2:000}", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds); ;
            lblRaysCast.Text = Tracer.RaysCast.ToString("N0");
            lblIntersections.Text = Tracer.Intersections.ToString("N0");

            picRenderTarget.Refresh();
            OnRenderCompleted(this, EventArgs.Empty);
        }

        protected virtual void OnRenderCompleted(object sender, EventArgs e) {
            if (TraceCompleted != null)
                TraceCompleted(sender, e);
        }
    }
}
