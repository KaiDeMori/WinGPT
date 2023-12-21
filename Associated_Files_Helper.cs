using System.ComponentModel;

namespace WinGPT;

internal class Associated_Files_Helper {
   public static void handle_associated_files_list_event(
      object?              sender,
      ListChangedEventArgs args,
      Action               callback) {
      if (!Config.Active.UIable.Show_Live_Token_Count)
         return;
      if (sender is not BindingList<AssociatedFile> list)
         return;

      var listChangedType = args.ListChangedType;
      switch (listChangedType) {
         case ListChangedType.Reset:
            break;
         case ListChangedType.ItemAdded:
            break;
         case ListChangedType.ItemDeleted:
            break;
         case ListChangedType.ItemMoved:
            break;
         case ListChangedType.ItemChanged:
            break;
         case ListChangedType.PropertyDescriptorAdded:
            break;
         case ListChangedType.PropertyDescriptorDeleted:
            break;
         case ListChangedType.PropertyDescriptorChanged:
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }

      callback();

      //uploaded_files_comboBox.DataSource = null;
      //uploaded_files_comboBox.DataSource = Associated_files;
      //uploaded_files_comboBox.DisplayMember = "Name";
      //uploaded_files_comboBox.ValueMember   = "FullName";
   }

}