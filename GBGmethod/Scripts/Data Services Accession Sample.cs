reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.GBGPlugin.dll;
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataModels.dll;
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.RestClient.dll;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;
using Biosero.DataServices;
using System.Text;

namespace GreenButtonGo.Scripting
{
    public class Data_Services_Accession_Sample : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            string sample = (string)variables["Data Services.Identity"];
            string source = (string)variables["Data Services.Source"];
            
            try
            {
                DataServicesHelper helper = new DataServicesHelper();
                helper.CreateSample(sample, source);
                
                variables["Error Text"] = string.Empty;
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.Append("Failed to accession plate to Data Services:\n\n");
                error.Append(ex.Message);
                
                Exception current = ex;
                while (current.InnerException != null)
                {
                    error.Append("\n");
                    error.Append(current.InnerException.Message);
                    current = current.InnerException;
                }
            
                variables["Error Text"] = error.ToString();
            }
        }
    }
}