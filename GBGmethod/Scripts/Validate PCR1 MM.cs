using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;

namespace GreenButtonGo.Scripting
{
    public class Validate_PCR1_MM : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            bool barcode = Regex.IsMatch(variables["Input.PCR1 MM Barcode"] as string, 
                                                                variables["Recipe.PCR1 MM Barcode Validation RegEx"] as string);
            bool rack = Regex.IsMatch(variables["Input.PCR1 MM Rack"] as string, 
                                                         variables["Recipe.PCR1 MM Rack Validation RegEx"] as string);
                                                        
            variables["Error Text"] = string.Empty;
            if (!rack && !barcode) 
            { 
                variables["Error Text"] = "PCR 1 MM Barcode and Rack are incorrect. Please check them and rescan.";
                return;
            }
            if (!rack) 
            { 
                variables["Error Text"] = "PCR 1 MM Rack is incorrect. Please check the rack and rescan.";
                return;
            }
            if (!barcode) 
            { 
                variables["Error Text"] = "PCR 1 MM Barcode is incorrect. Please check the barcode and rescan.";
                return;
            }
        }
    }
}