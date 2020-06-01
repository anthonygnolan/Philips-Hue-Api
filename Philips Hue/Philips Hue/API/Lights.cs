using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhilipsHue.Helpers;
using PhilipsHue.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipsHue.APIs
{
    public class Lights
    {
        /// <summary>
        /// Documentation
        /// https://www.developers.meethue.com/documentation/lights-api
        /// </summary>
        static string ipBase = "http://";
        static string apiBase = "/api/";
        static string apiGroup = "/lights";

        /// <summary>
        /// 1.1 Get all lights
        /// URL: http://<bridgeipaddress>/api/<username>/lights
        /// Method: GET
        /// Version: 1.0
        /// Permission: Whitelist 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<List<Light>> GetAllLights(string ipAddress, string userName)
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup;
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetAllLights - Url to be used: " + url);

            string response = await RESTHelper.Get(url);
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetAllLights - Response recieved: : " + response);

            List<Light> LightList = new List<Light>();
            JToken token = JToken.Parse(response);

            if (token.Type == JTokenType.Object)
            {
                var jsonResult = (JObject)token;

                foreach (var prop in jsonResult.Properties())
                {
                    Light newLight = JsonConvert.DeserializeObject<Light>(prop.Value.ToString());
                    //newLight.bridgeid = prop.Name;
                    LightList.Add(newLight);
                    Debug.WriteLine("<Philips Hue - APIs - Lights> GetAllLights - Light added to list. Light name: " + prop.Name);
                }
            }
            return LightList;
        }

        /// <summary>
        /// 1.2 Get new lights
        /// URL: http://<bridgeipaddress>/api/<username>/lights/new
        /// Method: GET
        /// Version: 1.0
        /// Permission: Whitelist 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<List<Light>> GetNewLights(string ipAddress, string userName)
        {
            string api = "/new";
            string url = ipBase + ipAddress + apiBase + userName + apiGroup + api;
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetNewLights - Url to be used: " + url);

            string response = await RESTHelper.Get(url);
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetNewLights - Response recieved: : " + response);

            List<Light> LightList = new List<Light>();
            JToken token = JToken.Parse(response);

            if (token.Type == JTokenType.Object)
            {
                var jsonResult = (JObject)token;
                var lastscan = jsonResult.First;
                jsonResult.First.Remove();

                foreach (var prop in jsonResult.Properties())
                {
                    Light newLight = JsonConvert.DeserializeObject<Light>(prop.Value.ToString());
                    //newLight.bridgeid = prop.Name;
                    LightList.Add(newLight);
                    Debug.WriteLine("<Philips Hue - APIs - Lights> GetNewLights - Light added to list. Light name: " + newLight.name);
                }
            }
            return LightList;
        }

        /// <summary>
        /// 1.3 Search for new lights
        /// URL: http://<bridgeipaddress>/api/<username>/lights
        /// Method: POST
        /// Version: 1.0
        /// Permission: Whitelist
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<bool> SearchForNewLights(string ipAddress, string userName, string body = "")
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup;
            Debug.WriteLine("<Philips Hue - APIs - Lights> SearchForNewLights - Url to be used: " + url);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SearchForNewLights - Body to be used: " + body);

            bool success = false;
            if (body == null)
            {
                body = "";
            }
            string response = await RESTHelper.Post(url, body);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SearchForNewLights - Response recieved: : " + response);

            success = ErrorHelper.CheckForError(response, success);
            
            Debug.WriteLine("<Philips Hue - APIs - Lights> SearchForNewLights - Sucess: " + success.ToString());
            return success;
        }

        /// <summary>
        /// 1.4 Get light attributes and state
        /// URL: http://<bridgeipaddress>/api/<username>/lights/<id>
        /// Method: GET
        /// Version: 1.0
        /// Permission: Whitelist
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Light> GetLightsAttributesAndState(string ipAddress, string userName, string id)
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup + "/" + id;
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetLightsAttributesAndState - Url to be used: " + url);

            string response = await RESTHelper.Get(url);
            Debug.WriteLine("<Philips Hue - APIs - Lights> GetLightsAttributesAndState - Response recieved: : " + response);
            Light light;
            try
            {
                light = JsonConvert.DeserializeObject<Light>(response);
                Debug.WriteLine("<Philips Hue - APIs - Lights> GetLightsAttributesAndState - Light Name: : " + light.name);
            }
            catch(Exception e)
            {
                Debug.WriteLine("<Philips Hue - APIs - Lights> Error - : " + e.Message + response );
                light = new Light();
            }
            return light;
        }

        /// <summary>
        /// 1.5 Set light attributes
        /// URL: http://<bridgeipaddress>/api/<username>/lights/<id>        
        /// Method: PUT
        /// Version: 1.0
        /// Permission: Whitelist
        /// </summary>
        /// <param name="url"></param>
        /// <param name="newlightname"></param>
        /// <returns></returns>
        public static async Task<bool> SetLightsAttributes(string ipAddress, string userName, string id, string attribute)
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup + "/" + id;
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightsAttributes - Url to be used: " + url);

            string body = $"{{\"name\":\"{attribute}\"}}";
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightsAttributes - Body to be used: " + body);

            bool success = false;
            string response = await RESTHelper.Put(url, body);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightsAttributes - Response recieved: : " + response);
            success = ErrorHelper.CheckForError(response, success);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightsAttributes - Sucess: " + success.ToString());

            return success;
        }

        /// <summary>
        /// 1.6 Set light state
        /// URL: http://<bridgeipaddress>/api/<username>/lights/<id>/state
        /// Method: PUT
        /// Version: 1.0
        /// Permission: Whitelist
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task<bool> SetLightState(string ipAddress, string userName, string id, State state)
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup + "/" + id + "/state";
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightState - Url to be used: " + url);
            string body = JsonConvert.SerializeObject(state);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightState - Body to be used: " + body);

            bool success = false;
            string response = await RESTHelper.Put(url, body);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightState - Response recieved: : " + response);
            success = ErrorHelper.CheckForError(response, success);
            Debug.WriteLine("<Philips Hue - APIs - Lights> SetLightState - Sucess: " + success.ToString());

            return success;
        }

        /// <summary>
        /// 1.7 Delete lights
        /// URL: http://<bridgeipaddress>/api/<username>/lights/<id>
        /// Method: DELETE
        /// Version: 1.0
        /// Permission: Whitelist
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteLight(string ipAddress, string userName, string id)
        {
            string url = ipBase + ipAddress + apiBase + userName + apiGroup + "/" + id;
            Debug.WriteLine("<Philips Hue - APIs - Lights> DeleteLight - Url to be used: " + url);

            bool success = false;
            string response = await RESTHelper.Delete(url);
            Debug.WriteLine("<Philips Hue - APIs - Lights> DeleteLight - Response recieved: : " + response);
            success = ErrorHelper.CheckForError(response, success);
            Debug.WriteLine("<Philips Hue - APIs - Lights> DeleteLight - Sucess: " + success.ToString());

            return success;
        }
    }
}
