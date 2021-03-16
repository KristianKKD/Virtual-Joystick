using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Joystick {
    public class CentreStick : PictureBox {

        public bool active;
        private Point eOriginalPos;
        private Point defaultPos;

        public int angle = 0;
        public int distance = 0;
        public Point coords = new Point(0, 0);

        public float maxDist = 63;

        public CentreStick() {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.White;
            SetDefaultPos();
        }

        async Task SetDefaultPos() {
            await Task.Delay(100).ConfigureAwait(false);
            defaultPos = Location;
        }

        protected override void OnMouseEnter(EventArgs e) {
            Cursor = Cursors.SizeAll;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            Cursor = Cursors.Default;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            active = true;
            eOriginalPos = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            active = false;
            Location = defaultPos;
            Location = defaultPos;
            angle = 0;
            distance = 0;
            coords = new Point(0, 0);
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            var newLocation = Location;
            if (active) {
                newLocation.Offset(e.Location.X - eOriginalPos.X, e.Location.Y - eOriginalPos.Y);

                float dist = Distance(defaultPos, newLocation);
                Point dir = new Point(newLocation.X - defaultPos.X, newLocation.Y - defaultPos.Y);

                if (dist > maxDist) {
                    dir = new Point (Convert.ToInt32(dir.X * (maxDist / dist)), Convert.ToInt32(dir.Y * (maxDist / dist)));
                }

                Location = new Point(defaultPos.X + dir.X, defaultPos.Y + dir.Y);
                angle = Convert.ToInt32((float)Math.Atan2(dir.Y, dir.X) * (180f / 3.1415926535f) + 180);
                distance = Distance(defaultPos, Location);

                coords = new Point(Location.X - defaultPos.X, defaultPos.Y - Location.Y);
            }

            base.OnMouseMove(e);
        }


        int Distance(Point p1, Point p2) {
            return Convert.ToInt32((float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
        }

    }

}
