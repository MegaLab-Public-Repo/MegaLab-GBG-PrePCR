reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.GBGPlugin.dll;
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataModels.dll
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.RestClient.dll
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
    public class Data_Services_Publish_Transfer : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            string instrument = (string)variables["Data Services.Instrument"];
            string source = (string)variables["Data Services.Source"];
            string destination = (string)variables["Data Services.Destination"];
            double volume = (double)variables["Data Services.Volume (uL)"];
            
            try
            {
                DataServicesHelper helper = new DataServicesHelper();
                helper.SingleSourceToFull384WellPlateTransfer(instrument, source ,destination, volume);
                
                variables["Error Text"] = string.Empty;
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.Append("Failed to publish to Data Services:\n\n");
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