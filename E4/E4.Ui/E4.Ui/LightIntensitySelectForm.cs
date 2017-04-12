
namespace E4.Ui
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Data;
   using System.Drawing;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Windows.Forms;

   public partial class LightIntensitySelectForm : Form
   {
      #region Properties

      public string LocationText { set; get; }
      public ValueParameter IntensityValue { set; get; }
      public Controls.CameraLocations Camera { set; get; }

      #endregion

      #region Constructor

      public LightIntensitySelectForm()
      {
         this.InitializeComponent();
      }

      #endregion
   }
}
