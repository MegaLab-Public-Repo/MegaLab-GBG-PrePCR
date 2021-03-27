using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;

namespace GreenButtonGo.Scripting
{
    public class Setup_Update_Consumables_Count : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            variables["Process.a4S Heat Seal Counter"] = (double)(variables["Process.a4S Heat Seal Counter"]) + 1;
            variables["Process.XPeel Tape Counter"] = (double)(variables["Process.XPeel Tape Counter"]) + 1;
            variables["Process.Waste Count"] = (double)(variables["Process.Waste Count"]) + 1;
            variables["Process.BenchCel Pipette Counter"] = (double)(variables["Process.BenchCel Pipette Counter"]) + 1;
            variables["Process.Mosquito Pipette Counter"] = (double)(variables["Process.Mosquito Pipette Counter"]) + 1; 
            
            int artPlateCountMinimum = Convert.ToInt32((double)variables["Recipe.ART Plate Count Minimum"]);
            int cdnaPlateCountMinimum = Convert.ToInt32((double)variables["Recipe.CDNA Plate Count Minimum"]); 
            int lidCountMinimum  = Convert.ToInt32((double)variables["Recipe.PCR Lid Load Quantity Minimum"]);     
            int artQty = Convert.ToInt32((double)variables["Process.ART Plate Pair Qty"]);
            int cdnaQty = Convert.ToInt32((double)variables["Process.cDNA Plate Qty"]); 
            int lidQty = Convert.ToInt32((double)variables["Process.PCR Lid Quantity"]);
            
            variables["Process.ART Plate Pair Qty"] = artQty - artPlateCountMinimum;
            variables["Process.cDNA Plate Qty"] = cdnaQty - cdnaPlateCountMinimum;
            variables["Process.PCR Lid Quantity"] = lidQty - lidCountMinimum;
        }
    }
}