reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.GBGPlugin.dll;
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataModels.dll;
reference C:\Program Files (x86)\Green Button Go\Plugins\Biosero.DataServices.RestClient.dll;
reference netstandard.dll;
reference  System.Net.Http.dll;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BioSero.GreenButtonGo.Scripting;
using Biosero.DataServices;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using EdgyCode.ApplicationManager;
using BioSero.GreenButtonGo.Managers;

namespace GreenButtonGo.Scripting
{
    public class Data_Services_Test_Connectivity : BioSero.GreenButtonGo.GBGScript
    {

        public void Run(Dictionary<String, Object> variables, RuntimeInfo runtimeInfo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = BootStrapper.GetInstance<ProgramManager>().GlobalConfigurationManager.GetOrDefault("DataServices.IP", "http://bioserodataservices.pathgroup.com");
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
                     HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
    
                    var response = client.PostAsync("health/ready", content).Result;
    
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(string.Format("{0} {1}", (int)response.StatusCode, response.ReasonPhrase));
                    }
                    
                    var healthResult = response.Content.ReadAsStringAsync().Result;
                    
                    if (healthResult != "Healthy")
                    {
                        StringBuilder errorTextSb = new StringBuilder();
             
                        errorTextSb.Append(string.Format("Data Services Connectivity Test returned a non-successful response: '{0}'\n", healthResult));
                        
                        errorTextSb.Append("\n\nData will not be accessioned to the database throughout this run. It is recommended to stop the run.");
                        variables["Error Text"] = errorTextSb.ToString();
                    }
                    else
                        variables["Error Text"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                StringBuilder error = new StringBuilder();
                error.Append("Failed to connect to Data Services:\n\n");
                error.Append(ex.Message);
                
                Exception current = ex;
                while (current.InnerException != null)
                {
                    error.Append("\n");
                    error.Append(current.InnerException.Message);
                    current = current.InnerException;
                }
                
                error.Append("\n\nData will not be accessioned to the database throughout this run. It is recommended to stop the run.");
                variables["Error Text"] = error.ToString();
            }
        }
    }
}