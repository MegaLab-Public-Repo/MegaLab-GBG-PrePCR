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
    public class Data_Services_Stamp_cDNA_to_ART : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            string art1Plate = (string)variables["Plate On Mosquito Position 3"];
            string art2Plate = (string)variables["Plate On Mosquito Position 5"];
            string cdnaPlate = (string)variables["Plate On Mosquito Position 4"];

            DataServicesHelper helper = new DataServicesHelper();
            
            try
            {
                helper.PlateStamp("Mosquito", cdnaPlate, art1Plate, 5, (StampType)1);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Mosquito", cdnaPlate, art1Plate, 5, (StampType)1);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Mosquito", cdnaPlate, art1Plate, 5, (StampType)1);
                }
            }
            

            try
            {
                helper.PlateStamp("Mosquito", cdnaPlate, art2Plate, 5, (StampType)1);
            }
            catch
            {
                Thread.Sleep(30000);
                try
                {
                    helper.PlateStamp("Mosquito", cdnaPlate, art2Plate, 5, (StampType)1);
                }
                catch
                {
                    Thread.Sleep(30000);
                    helper.PlateStamp("Mosquito", cdnaPlate, art2Plate, 5, (StampType)1);
                }
            }
        }
    }
}