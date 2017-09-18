using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossBore.DeviceTest
{
   public partial class DeviceTraceConfigForm : Form
   {
      #region Properties

      public UInt32 DisplayMask { set; get; }
      public UInt32 TraceMask { set; get; }

      #endregion

      #region User Events

      private void OkButton_Click(object sender, EventArgs e)
      {
         UInt32 result = 0;

         result |= (UInt32)((false != this.SdoCheckBox.Checked) ? 0x00000001 : 0);
         result |= (UInt32)((false != this.HeartbeatCheckBox.Checked) ? 0x00000002 : 0);
         result |= (UInt32)((false != this.Tpdo1CheckBox.Checked) ? 0x00000004 : 0);
         result |= (UInt32)((false != this.Rpdo1CheckBox.Checked) ? 0x00000008 : 0);
         result |= (UInt32)((false != this.Tpdo2CheckBox.Checked) ? 0x00000010 : 0);
         result |= (UInt32)((false != this.Rpdo2CheckBox.Checked) ? 0x00000020 : 0);
         result |= (UInt32)((false != this.Tpdo3CheckBox.Checked) ? 0x00000040 : 0);
         result |= (UInt32)((false != this.Rpdo3CheckBox.Checked) ? 0x00000080 : 0);
         result |= (UInt32)((false != this.Tpdo4CheckBox.Checked) ? 0x00000100 : 0);
         result |= (UInt32)((false != this.Rpdo4CheckBox.Checked) ? 0x00000200 : 0);

         result |= (UInt32)((false != this.Tpdo5CheckBox.Checked) ? 0x00000400 : 0);
         result |= (UInt32)((false != this.Tpdo6CheckBox.Checked) ? 0x00000800 : 0);
         result |= (UInt32)((false != this.Tpdo7CheckBox.Checked) ? 0x00001000 : 0);
         result |= (UInt32)((false != this.Tpdo8CheckBox.Checked) ? 0x00002000 : 0);

         this.TraceMask = result;
         this.DialogResult = System.Windows.Forms.DialogResult.OK;
      }

      private void CancelAButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      }

      #endregion

      #region Form Events

      private void DeviceTraceConfigForm_Shown(object sender, EventArgs e)
      {
         this.SdoCheckBox.Visible = ((this.DisplayMask & 0x00000001) != 0) ? true : false;
         this.HeartbeatCheckBox.Visible = ((this.DisplayMask & 0x00000002) != 0) ? true : false;
         this.Tpdo1CheckBox.Visible = ((this.DisplayMask & 0x00000004) != 0) ? true : false;
         this.Rpdo1CheckBox.Visible = ((this.DisplayMask & 0x00000008) != 0) ? true : false;
         this.Tpdo2CheckBox.Visible = ((this.DisplayMask & 0x00000010) != 0) ? true : false;
         this.Rpdo2CheckBox.Visible = ((this.DisplayMask & 0x00000020) != 0) ? true : false;
         this.Tpdo3CheckBox.Visible = ((this.DisplayMask & 0x00000040) != 0) ? true : false;
         this.Rpdo3CheckBox.Visible = ((this.DisplayMask & 0x00000080) != 0) ? true : false;
         this.Tpdo4CheckBox.Visible = ((this.DisplayMask & 0x00000100) != 0) ? true : false;
         this.Rpdo4CheckBox.Visible = ((this.DisplayMask & 0x00000200) != 0) ? true : false;
         
         this.Tpdo5CheckBox.Visible = ((this.DisplayMask & 0x00000400) != 0) ? true : false;
         this.Tpdo6CheckBox.Visible = ((this.DisplayMask & 0x00000800) != 0) ? true : false;
         this.Tpdo7CheckBox.Visible = ((this.DisplayMask & 0x00001000) != 0) ? true : false;
         this.Tpdo8CheckBox.Visible = ((this.DisplayMask & 0x00002000) != 0) ? true : false;

         this.SyncCheckBox.Visible = ((this.DisplayMask & 0x80000000) != 0) ? true : false;


         this.SdoCheckBox.Checked = ((this.TraceMask & 0x00000001) != 0) ? true : false;
         this.HeartbeatCheckBox.Checked = ((this.TraceMask & 0x00000002) != 0) ? true : false;
         this.Tpdo1CheckBox.Checked = ((this.TraceMask & 0x00000004) != 0) ? true : false;
         this.Rpdo1CheckBox.Checked = ((this.TraceMask & 0x00000008) != 0) ? true : false;
         this.Tpdo2CheckBox.Checked = ((this.TraceMask & 0x00000010) != 0) ? true : false;
         this.Rpdo2CheckBox.Checked = ((this.TraceMask & 0x00000020) != 0) ? true : false;
         this.Tpdo3CheckBox.Checked = ((this.TraceMask & 0x00000040) != 0) ? true : false;
         this.Rpdo3CheckBox.Checked = ((this.TraceMask & 0x00000080) != 0) ? true : false;
         this.Tpdo4CheckBox.Checked = ((this.TraceMask & 0x00000100) != 0) ? true : false;
         this.Rpdo4CheckBox.Checked = ((this.TraceMask & 0x00000200) != 0) ? true : false;

         this.Tpdo5CheckBox.Checked = ((this.TraceMask & 0x00000400) != 0) ? true : false;
         this.Tpdo6CheckBox.Checked = ((this.TraceMask & 0x00000800) != 0) ? true : false;
         this.Tpdo7CheckBox.Checked = ((this.TraceMask & 0x00001000) != 0) ? true : false;
         this.Tpdo8CheckBox.Checked = ((this.TraceMask & 0x00002000) != 0) ? true : false;

         this.SyncCheckBox.Checked = ((this.TraceMask & 0x80000000) != 0) ? true : false;
      }

      #endregion

      #region Constructor

      public DeviceTraceConfigForm()
      {
         InitializeComponent();
      }

      #endregion

   }
}
