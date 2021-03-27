using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;
using System.Text;
using System.Text.RegularExpressions;

namespace GreenButtonGo.Scripting
{
    public class Setup_Cytomat_Verify_Barcodes_Intermediate : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            // This script analyzes the contents of the intermediate plates of the hotel and compares against the rules
            // Hotel 2 must be empty
            // Hotel 3-7 must only contain ART1/ART2 pairs.  Pair count > Minimum (Default 8 indicating 4 pairs)
            // Hotel 8-10 must only contain cDNA plates.  cDNA count > Minimum (Default 4)
            // A pass condition will have Error Text variable == string.Empty
            
            // Get RegEx Values
            Regex art1Regex = new Regex((string)variables["Recipe.COV1 Plate Validation Regex"]);
            Regex art2Regex = new Regex((string)variables["Recipe.COV2 Plate Validation Regex"]);
            Regex cdnaRegex = new Regex((string)variables["Recipe.Intermediate Plate Validation Regex"]);
            
            // Get Minimum Plate Count Values
            int hotel2PlateCountRequired = Convert.ToInt32((double)variables["Recipe.Hotel 2 Plate Count"]);
            int artPlateCountMinimum = Convert.ToInt32((double)variables["Recipe.ART Plate Count Minimum"]);
            int cdnaPlateCountMinimum = Convert.ToInt32((double)variables["Recipe.CDNA Plate Count Minimum"]); //DONE: CDNA Plate Count is set to 4 in recipe file -BT      
            
            StringBuilder errorSb = new StringBuilder();
            StringBuilder debug = new StringBuilder();              //Debug string
            
            //Get all Plates in hotel 3,4,5,6,7 to determine if ART pairs
            var artPlates = Database.GetAllRows("Cytomat_Hotels", new[] { "Hotel", "Barcode" })
                .Where(r => r[0] == "Hotel 3"  && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 4" && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 5" && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 6" && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 7" && !string.IsNullOrWhiteSpace(r[1]));

             //Get count of all Plates in hotel 2.  Compare against required count (default 0)
             int hotel2PlateCount = Database.GetAllRows("Cytomat_Hotels", new[] { "Hotel", "Barcode" })
                .Where(r => r[0] == "Hotel 2" && !string.IsNullOrWhiteSpace(r[1])).Count();
             if (hotel2PlateCount != hotel2PlateCountRequired)
                   errorSb.Append(string.Format("Unexpected plate count in Hotel 2: {0}. Expected {1}.\n",
                        hotel2PlateCount,
                        hotel2PlateCountRequired));
                        
             //Get all Plates in hotel 8,9,10 to determine if cDNA plates
             var cdnaPlates = Database.GetAllRows("Cytomat_Hotels", new[] { "Hotel", "Barcode" })
                .Where(r => r[0] == "Hotel 8"  && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 9" && !string.IsNullOrWhiteSpace(r[1])
                    || r[0] == "Hotel 10" && !string.IsNullOrWhiteSpace(r[1]));
            
             int art1PlateCount = 0;
             int art2PlateCount = 0;
             int artPlateCountTotal = 0;
             //Verify that ART1/2 plates match RegEx, have a matching pair, and count ART1 and ART2 plates
            foreach (string[] plate in artPlates)
            {
                 debug.Append(string.Format("Found ART Barcode: {0} ({1}).\n", plate[1], plate[0]));    //List of plates to debug
                 
                 if (plate[1].Contains('-'))
                 {
                 
                     string[] barcodeSegments = plate[1].Split('-');
    
                    bool isArt1 = barcodeSegments[1] == "ART1";
                    string[] matchingPlate;
    
                    if (isArt1)
                    {
                        if (!art1Regex.IsMatch(plate[1]))
                            errorSb.Append(string.Format("Barcode has incorrect format: {0} ({1}).\n", plate[1], plate[0]));
    
                        art1PlateCount++;
    
                        matchingPlate = artPlates.FirstOrDefault(p => p[0] == plate[0]
                            && art2Regex.IsMatch(p[1])
                            && p[1].Split('-')[0] == barcodeSegments[0]);
                    }
                    else
                    {
                        if (!art2Regex.IsMatch(plate[1]))
                            errorSb.Append(string.Format("Barcode has incorrect format: {0} ({1}).\n", plate[1], plate[0]));
    
                        art2PlateCount++;
    
                        matchingPlate = artPlates.FirstOrDefault(p => p[0] == plate[0]
                            && art1Regex.IsMatch(p[1])
                            && p[1].Split('-')[0] == barcodeSegments[0]);
                    }
    
                    if (matchingPlate == null)
                        errorSb.Append(string.Format("No plate barcode was found matching '{0}' in '{1}'\n", plate[1], plate[0]));
            }
            else
            {
               errorSb.Append(string.Format("Incorrect plate barcode: {0}\n", plate[1]));
            }
        }
            artPlateCountTotal = artPlates.Count();
            //Verify ART pairs plate count is greater than or equal to minimum.
            if (artPlateCountTotal < artPlateCountMinimum) 
                errorSb.Append(string.Format("Unexpected ART plate count: {0} total ART1 and ART 2 plates. Expected minimum {1}. Verify that only ART 1 an 2 plates are contained in Hotels 3 through 6.\n", artPlateCountTotal, artPlateCountMinimum));
            
           //Verify ART plate barcodes are all unique
            if (artPlates.Distinct().Count() != artPlates.Count())
                errorSb.Append(string.Format("Found duplicate ART plate barcodes: {0}. All barcodes must be unique.\n", artPlates.GroupBy(r => r[1]).OrderByDescending(bc => bc.Count()).Select(grp => grp.Key).First()));

            
            //Verify cDNA matches regex and count cDNA plates
            int cdnaPlateCount = 0;
            foreach (string[] plate in cdnaPlates)
            {
                debug.Append(string.Format("Found cDNA Barcode: {0} ({1}).\n", plate[1], plate[0])); //List of plates to debug
                
                if (!cdnaRegex.IsMatch(plate[1])) 
                    errorSb.Append(string.Format("Barcode has incorrect format: {0} ({1}).\n", plate[1], plate[0]));
                else
                    cdnaPlateCount++;
            }
            
            //Verify cDNA count is greater than or equal to minimum
            if (cdnaPlateCount < cdnaPlateCountMinimum) 
                errorSb.Append(string.Format("Unexpected CDNA plate count: {0}. Expected minimum {1}. Verify that only CDNA plates are contained in Hotels 8 through 10.\n", cdnaPlateCount, cdnaPlateCountMinimum));
            
            //Verify cDNA plate barcodes are all unique
            if (cdnaPlates.Distinct().Count() != cdnaPlates.Count())
                errorSb.Append(string.Format("Found duplicate CDNA plate barcodes: {0}. All barcodes must be unique.\n", cdnaPlates.GroupBy(r => r[1]).OrderByDescending(bc => bc.Count()).Select(grp => grp.Key).First()));

            debug.Append(string.Format("Hotel 2 plate count: {0}.  ART total plate count: {1}.  cDNA plate count : {2}", hotel2PlateCount, artPlateCountTotal, cdnaPlateCount));
            
            variables["DebugLog"] = debug.ToString();
            variables["Error Text"] = errorSb.ToString();
            
            variables["Process.ART Plate Pair Qty"] = artPlateCountTotal;
            variables["Process.cDNA Plate Qty"] = cdnaPlateCount;
        }
    }
}