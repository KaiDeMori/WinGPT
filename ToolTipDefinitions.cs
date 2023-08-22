using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

class ToolTipDefinitions {
   private ToolTip toolTip;

   public ToolTipDefinitions(Form form, Dictionary<string, string> tooltips) {
      toolTip = new ToolTip();

      foreach (KeyValuePair<string, string> pair in tooltips) {
         var control = FindControl(form, pair.Key);
         if (control != null) {
            toolTip.SetToolTip(control, pair.Value);
         }
      }
   }

   private Control FindControl(Control container, string name) {
      if (container.Name == name) {
         return container;
      }

      foreach (Control child in container.Controls) {
         var result = FindControl(child, name);
         if (result != null) {
            return result;
         }
      }

      return null;
   }
}