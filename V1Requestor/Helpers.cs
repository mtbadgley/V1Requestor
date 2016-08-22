using System;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace V1Requestor
{
    public static class Helpers
    {
        private static string _accesstoken;
        public static string AccessToken
        {
            get
            {
                _accesstoken = ConfigurationManager.AppSettings["AccessToken"];
                return _accesstoken;
            }
        }

        private static string _url;
        public static string Url
        {
            get
            {
                _url = ConfigurationManager.AppSettings["V1Server"];
                return _url;
            }
        }

        private static string _v1Instance;
        public static string V1Instance
        {
            get
            {
                _v1Instance = ConfigurationManager.AppSettings["V1Instance"];
                return _v1Instance;
            }
        }

        private static string _metaurl;
        public static string MetaUrl
        {
            get
            {
                _metaurl = Url + "/meta.v1/";
                return _metaurl;
            }
        }

        private static string _dataurl;
        public static string DataUrl
        {
            get
            {
                _dataurl = Url + "/rest-1.v1/";
                return _dataurl;
            }
        }

        private static string _emailaddress;
        public static string EmailAddress
        {
            get
            {
                return _emailaddress;
            }
            set { _emailaddress = value; }
        }

        /// <summary>
        ///     Function that returns a string containing a list of VersionOne Asset numbers that can be viewed.
        ///     The string returned is formatted.
        /// </summary>
        /// <param name="v1List">List object of different display ids/Numbers returned from VersionOne.</param>
        /// <returns>A string containing a comma delimited list of links to related asset numbers.</returns>
        public static string Getv1List(IList v1List)
        {
            string result = "";
            if (v1List.Count > 0)
            {
                foreach (string s in v1List)
                {
                    string listvalue = s;
                    if (Regex.IsMatch(s, "[A-Z]{1,2}-[0-9]+"))
                    {
                        listvalue = "<a href='AssetDetails.aspx?id=" + listvalue + "' target='_blank'>" + listvalue + "</a>";
                    }
                    result = result + listvalue + ", ";
                }
                if (result.EndsWith(", "))
                    result = result.Substring(0, result.Length - 2);
            }
            return result;
        }

        /// <summary>
        ///     Formatting utility function for providing the lozenge element around the status.
        ///     NOTE - uses Bootstrap 3, see Labels for more options.
        /// </summary>
        /// <param name="statusValue">The text name of the Status from VersionOne</param>
        /// <param name="assetState">Either Active or Closed.</param>
        /// <returns>A string containing the necessary html/css to form the appropriate lozenge.</returns>
        public static string GetStatusStyle(string statusValue, string assetState)
        {
            string spantext = string.Empty;

            if (statusValue.Length > 0)
            {
                switch (statusValue)
                {
                    case "Approved":
                        // Green
                        spantext = "class='label label-success'";
                        break;
                    case "Reviewed":
                        // Blue
                        spantext = "class='label label-primary'";
                        break;
                    case "Rejected":
                        // Grey
                        spantext = "class='label label-default'";
                        break;
                    default:
                        spantext = "";
                        break;
                }
            }
            // if closed, strikethru the text
            if (assetState != "Active")
            {
                spantext += " style='text-decoration: line-through;'";
            }

            spantext = "<span " + spantext + ">";
            return spantext;
        }

        /// <summary>
        ///     Utility function to remove the Moment identifier from the object id.
        /// </summary>
        /// <param name="oid">Asset Object Id with Moment</param>
        /// <returns>Asset Object Id</returns>
        public static string RemoveOidMoment(string oid)
        {
            string[] oidparts = oid.Split(Convert.ToChar(":"));
            string momentlessoid = oidparts[0] + ":" + oidparts[1];
            return momentlessoid;
        }


        #region HTTPCalls

        /// <summary>
        ///     Perform a REST API HTTP GET call.
        ///     Uses VersionOne Access token to authenticate.
        /// </summary>
        /// <param name="accesstoken">The security/application access token used to authenticate.</param>
        /// <param name="queryurl">Fully qualified URL to perform a GET</param>
        /// <returns>
        ///     Returns response from API Call, XML or JSON. Will return
        ///     empty string for exception.
        /// </returns>
        public static string MakeHttpRequest(string accesstoken, string queryurl)
        {
            try
            {
                string result;
                using (var webclient = new WebClient())
                {
                    webclient.Headers.Add("Authorization", "Bearer " + accesstoken);
                    result = webclient.DownloadString(queryurl);
                }
                return result;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        /// <summary>
        ///     Perform a REST API HTTP POST call.
        ///     Uses VersionOne Access Token authentication.
        /// </summary>
        /// <param name="accesstoken">The security/application access token used to authenticate.</param>
        /// <param name="queryurl">A fully qualified URL</param>
        /// <param name="payload">Either XML or JSON payload based on the call</param>
        /// <returns>
        ///     Response from call, XML or JSON.  Will return an empty
        ///     string if call fails.
        /// </returns>
        public static string MakeHttpPost(string accesstoken, string queryurl, string payload = "")
        {
            try
            {
                string result;
                using (var webclient = new WebClient())
                {
                    webclient.Headers.Add("Authorization", "Bearer " + accesstoken);
                    result = webclient.UploadString(queryurl, "POST", payload);
                }
                return result;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        #endregion HTTPCalls

        /// <summary>
        ///     Utlility function to test the connectivity to VersionOne.
        /// </summary>
        /// <param name="accesstoken">The security/application access token used to authenticate.</param>
        /// <param name="systemurl">The Base VersionOne instance URL.</param>
        /// <returns>True if connection is successful.</returns>
        public static bool ValidateV1Connection(string accesstoken, string systemurl)
        {
            string query = systemurl + "/rest-1.v1/Data/Member/20?sel=Name";
            string result = MakeHttpRequest(accesstoken, query);
            if (!String.IsNullOrEmpty(result))
            {
                //Logger.Info("Connection to VersionOne Successful.");
                return true;
            }
            return false;
        }



    }
}