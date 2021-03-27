using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;

namespace GreenButtonGo.Scripting
{
    public class Validate_RVT_MM : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            bool barcode = Regex.IsMatch(variables["Input.RVT MM Barcode"] as string, 
                                                                variables["Recipe.RVT MM Barcode Validation RegEx"] as string);
            bool rack = Regex.IsMatch(variables["Input.RVT MM Rack"] as string, 
                                                         variables["Recipe.RVT MM Rack Validation RegEx"] as string);
                                                        
            variables["Error Text"] = string.Empty;
            if (!rack && !barcode) 
            { 
                variables["Error Text"] = "RVT MM Barcode and Rack are incorrect. Please check them and rescan.";
                return;
            }
            if (!rack) 
            { 
                variables["Error Text"] = "RVT MM Rack is incorrect. Please check the rack and rescan.";
                return;
            }
            if (!barcode) 
            { 
                variables["Error Text"] = "RVT MM Barcode is incorrect. Please check the barcode and rescan.";
                return;
            }

        }
    }
}