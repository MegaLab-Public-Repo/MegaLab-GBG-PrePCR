using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;

namespace GreenButtonGo.Scripting
{
    public class Check_if_Complete : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            var database = EdgyCode.ApplicationManager.BootStrapper.GetInstance<BioSero.GreenButtonGo.Data.RuntimeDataManager>();
            string[] headers = {"BARCODE"};
			List<string[]> worklist1 = database.GetAllRows("ART1_Worklist",headers).ToList();
                    List<string[]> worklist2 = database.GetAllRows("ART2_Worklist",headers).ToList();

                    //MessageBox.Show(worklist1.Count().ToString());
                    //MessageBox.Show(worklist2.Count().ToString());

                    //see if any plates in worklist
			if(worklist1.Count() + worklist2.Count() > 2)	
			{return;}

			// if check on all Plate On variables until all of them are empty
			
			string[] instruments = new string[] {    //need to edit this list to reflect current system
			"PF-400",
			"Dragonfly 1",
			"ODTC 1",            
                    "ODTC 2",
                    "Cytomat",            
                    "a4S Sealer",
                    "XPeel",			
			"Bravo Position 2",
                    "Bravo Position 3",            
                    "Bravo Position 6",
                    "Bravo Position 8",
                    "Bravo Position 9",
                    "Mosquito Position 3",
                    "Mosquito Position 4",
                    "Mosquito Position 5",
                    "Regrip"};
			
			foreach(string nest in instruments)
			{
				string check_var = (string)variables["Plate On " + nest];
				if(!String.IsNullOrEmpty(check_var))
				{return;}
			}
			
			//if it gets this far, the run should be complete so flip the variable
			variables["All Workcell Processes Complete"] = true; // if this doesn't work but the evaulation does, we can set it run the GBG 'End Program' task as well
        }
    }
}