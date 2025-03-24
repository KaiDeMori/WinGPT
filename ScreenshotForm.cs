using System.Drawing.Imaging;

namespace WinGPT;

/// <summary>
/// Unfortunately we can't use this and have to resort to
/// an external tool, since this apporach is
/// not compatible with the dpi-scaling mode DpiUnaware.
/// The snipped image has an offset when the dpi-scaling is not 100%.
/// </summary>
public partial class ScreenshotForm : Form {
   private Point     start_point;
   private Rectangle selection_rectangle;
   private bool      is_drawing;

   // This will store the final snipped image
   public Image? selected_image;

   public ScreenshotForm() {
      // Make the form cover the entire screen (and slightly transparent)
      AutoScaleMode   = AutoScaleMode.None; // Disable autoscaling
      WindowState     = FormWindowState.Maximized;
      FormBorderStyle = FormBorderStyle.None;
      TopMost         = true;
      Opacity         = 0.4;
      Cursor          = Cursors.Cross;
      DoubleBuffered  = true;
   }

   public static void shoot(WinGPT_Form form) {
      var image = capture_screenshot_image(form);
      if (image is null)
         return;

      //save as png image
      var      random_hex_5 = new Random().Next(0x100000).ToString("X5");
      var      filename     = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}_{random_hex_5}.png";
      FileInfo file         = new FileInfo(Path.Join(Config.Screenshot_Directory.FullName, filename));
      image.Save(file.FullName, ImageFormat.Png);
      form.Associated_files.Add(new(file));
   }

   public static Image? capture_screenshot_image(Form main_form) {
      // Hide the main form so it's not captured
      main_form.Hide();

      using var snipper = new ScreenshotForm();
      // Block until user finishes
      var dialog_result = snipper.ShowDialog();

      // Show main form again
      main_form.Show();
      main_form.BringToFront();

      // Get the Image the user snipped, or null if canceled
      return dialog_result == DialogResult.OK ? snipper.selected_image : null;
   }

   private float get_system_dpi_scaling_factor() {
      // We'll grab the primary screen's device context
      using var g = Graphics.FromHwnd(IntPtr.Zero);
      // 96 DPI is considered "standard" with a 1:1 scaling
      return g.DpiX / 96f;
   }


   protected override void OnMouseDown(MouseEventArgs e) {
      base.OnMouseDown(e);
      if (e.Button == MouseButtons.Left) {
         is_drawing = true;
         // Convert local mouse position to an absolute screen point
         // start_point         = PointToScreen(e.Location);
         start_point         = e.Location;
         selection_rectangle = new Rectangle(start_point, new Size(0, 0));
      }
   }

   protected override void OnMouseMove(MouseEventArgs e) {
      base.OnMouseMove(e);
      if (is_drawing) {
         // Convert again on move
         // Point current_screen_point = PointToScreen(e.Location);
         Point current_point = e.Location;

         int x = Math.Min(start_point.X, current_point.X);
         int y = Math.Min(start_point.Y, current_point.Y);
         int w = Math.Abs(start_point.X - current_point.X);
         int h = Math.Abs(start_point.Y - current_point.Y);

         selection_rectangle = new Rectangle(x, y, w, h);
         Invalidate(); // Triggers repaint to show the selection
      }
   }

   protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);
      if (e.Button == MouseButtons.Left) {
         is_drawing = false;
         Hide();

         float scale_factor = get_system_dpi_scaling_factor();
         int   real_x       = (int) (selection_rectangle.Left   * scale_factor);
         int   real_y       = (int) (selection_rectangle.Top    * scale_factor);
         int   real_w       = (int) (selection_rectangle.Width  * scale_factor);
         int   real_h       = (int) (selection_rectangle.Height * scale_factor);

         using var screenshot = new Bitmap(real_w, real_h);
         using var g          = Graphics.FromImage(screenshot);
         g.CopyFromScreen(new Point(real_x, real_y),
            new Point(0,                    0),
            new Size(real_w, real_h));

         selected_image = screenshot;
         DialogResult   = DialogResult.OK;
         Close();
      }
      else {
         DialogResult = DialogResult.Cancel;
         Close();
      }
   }


   protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      // Draw a rectangle where the user is dragging
      using var pen = new Pen(Color.Red, 2);
      e.Graphics.DrawRectangle(pen, selection_rectangle);
   }
}