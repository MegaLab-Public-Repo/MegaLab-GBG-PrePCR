using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;

namespace GreenButtonGo.Scripting
{
    public class Setup_Get_PCR_Lid_Quantity : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
             int lidCountMinimum = Convert.ToInt32((double)variables["Recipe.PCR Lid Load Quantity Minimum"]);     
             int pcrLidQuantity = Convert.ToInt32((double)variables["Process.PCR Lid Quantity"]);
             
             if (pcrLidQuantity < lidCountMinimum)
                variables["Error Text"] = string.Format("Insufficient PCR Lids were loaded into stack. Actual: {0}. Expected: {1}.", pcrLidQuantity,  lidCountMinimum);
             else
                variables["Error Text"] = string.Empty;
                
            Database.UpdateRow("Plate_Storage_Stacks","STACK","Stack 1","PLATES_IN_STACK",pcrLidQuantity.ToString());
             
       }
    }
}