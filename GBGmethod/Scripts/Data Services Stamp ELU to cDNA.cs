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
using System.Threading;

namespace GreenButtonGo.Scripting
{
    public class Data_Services_Stamp_ELU_to_cDNA : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            string eluPlate1 = (string)variables["Plate On Bravo Position 2"];
            string eluPlate2 = (string)variables["Plate On Bravo Position 3"];
            string eluPlate3 = (string)variables["Plate On Bravo Position 8"];
            string eluPlate4 = (string)variables["Plate On Bravo Position 9"];
            string cdnaPlate = (string)variables["Plate On Bravo Position 6"];

            DataServicesHelper helper = new DataServicesHelper();
            
            try
            {
                helper.PlateStamp("Bravo", eluPlate1, cdnaPlate, 8.5, (StampType)2);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Bravo", eluPlate1, cdnaPlate, 8.5, (StampType)2);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Bravo", eluPlate1, cdnaPlate, 8.5, (StampType)2);
                }
            }
            
            try
            {
                helper.PlateStamp("Bravo", eluPlate2, cdnaPlate, 8.5, (StampType)3);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Bravo", eluPlate2, cdnaPlate, 8.5, (StampType)3);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Bravo", eluPlate2, cdnaPlate, 8.5, (StampType)3);
                }
            }
            
            try
            {
                helper.PlateStamp("Bravo", eluPlate3, cdnaPlate, 8.5, (StampType)4);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Bravo", eluPlate3, cdnaPlate, 8.5, (StampType)4);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Bravo", eluPlate3, cdnaPlate, 8.5, (StampType)4);
                }
            }
            
            try
            {
                helper.PlateStamp("Bravo", eluPlate4, cdnaPlate, 8.5, (StampType)5);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Bravo", eluPlate4, cdnaPlate, 8.5, (StampType)5);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Bravo", eluPlate4, cdnaPlate, 8.5, (StampType)5);
                }
            }
        }
    }
}