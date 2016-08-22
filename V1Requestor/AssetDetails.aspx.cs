using System;
using System.Text;
using System.Xml;

namespace V1Requestor
{
    public partial class AssetDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryurl = "";

            // determine asset type for display behaviors
            bool isPrimaryWorkitem = false;
            bool isEpic = false;
            bool isRequest = false;

            // capture versionone asset number from url
            string argAssetId = Request["id"];

            // set label on page
            assetId.Text = argAssetId;

            // hide sections on page
            pwdetails.Visible = false;
            rqdetails.Visible = false;
            epdetails.Visible = false;

            // determine asset type based on the first letter of asset, 
            // set the queryurl appropriately
            switch (argAssetId.Substring(0,1))
            {
                case "B":
                    queryurl = GetPrimaryWorkitemUrl(argAssetId);
                    isPrimaryWorkitem = true;
                    break;
                case "D":
                    queryurl = GetPrimaryWorkitemUrl(argAssetId);
                    isPrimaryWorkitem = true;
                    break;
                case "E":
                    queryurl = GetEpicUrl(argAssetId);
                    isEpic = true;
                    break;
                case "R":
                    queryurl = GetRequestUrl(argAssetId);
                    isRequest = true;
                    break;
            }

            // query asset details
            string resultset = Helpers.MakeHttpRequest(Helpers.AccessToken, queryurl);

            if (resultset.Length > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(resultset);
                XmlNodeList assets = doc.SelectNodes("Assets/Asset");
                if (assets != null && assets.Count > 0)
                {
                    
                    assetTitle.Text = assets[0].SelectSingleNode("Attribute[@name='Name']").InnerText;

                    //Common Values
                    string planninglevel = assets[0].SelectSingleNode("Attribute[@name='Scope.Name']").InnerText;
                    string statusname = assets[0].SelectSingleNode("Attribute[@name='Status.Name']").InnerText;
                    string servicename = assets[0].SelectSingleNode("Attribute[@name='Source.Name']").InnerText;
                    string description = assets[0].SelectSingleNode("Attribute[@name='Description']").InnerText;

                    //Set page labels, text containing
                    if (isPrimaryWorkitem)
                    {
                        pwPlanningLevel.Text = planninglevel;
                        pwTeamName.Text = assets[0].SelectSingleNode("Attribute[@name='Team.Name']").InnerText;
                        pwSprintName.Text = assets[0].SelectSingleNode("Attribute[@name='Timebox.Name']").InnerText;
                        pwStatusName.Text = statusname;
                        pwServiceName.Text = servicename;
                        pwEpicName.Text = assets[0].SelectSingleNode("Attribute[@name='Super.Name']").InnerText;
                        pwDescription.Text = description;
                        pwdetails.Visible = true;
                    }

                    if (isEpic)
                    {
                        epPlanningLevel.Text = planninglevel;
                        epTeamName.Text = assets[0].SelectSingleNode("Attribute[@name='Team.Name']").InnerText;
                        epStatusName.Text = statusname;
                        epServiceName.Text = servicename;
                        epParentName.Text = assets[0].SelectSingleNode("Attribute[@name='Super.Name']").InnerText;
                        epDescription.Text = description;
                        epdetails.Visible = true;
                    }

                    if (isRequest)
                    {
                        rqPlanningLevel.Text = planninglevel;
                        rqStatusName.Text = statusname;
                        rqServiceName.Text = servicename;
                        rqDescription.Text = description;
                        rqOwnerName.Text = assets[0].SelectSingleNode("Attribute[@name='Owner.Name']").InnerText;
                        rqdetails.Visible = true;
                    }
                    
                }
            }
        }

        #region AssetUrls
        /// <summary>
        ///     Set of functions used to create an asset specific URL to handle
        ///     primaryworkitem (story, defect), epic (portfolio item), and request.
        /// </summary>
        /// <param name="assetid">The display id or VersionOne number.</param>
        /// <returns>A string that returns the Data rest query including sel and where.</returns>
        protected string GetPrimaryWorkitemUrl(string assetid)
        {
            if (assetid.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/PrimaryWorkitem?sel=Name,Scope.Name,Team.Name,Status.Name,");
            sb.Append("Timebox.Name,Source.Name,Super.Name,Owners.Name,Description");
            sb.Append("&where=Number='");
            sb.Append(assetid);
            sb.Append("'");
            return sb.ToString();
        }

        protected string GetEpicUrl(string assetid)
        {
            if (assetid.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/Epic?sel=Name,Scope.Name,Team.Name,Status.Name,");
            sb.Append("Source.Name,Super.Name,Owners.Name,Description");
            sb.Append("&where=Number='");
            sb.Append(assetid);
            sb.Append("'");
            
            return sb.ToString();
        }

        protected string GetRequestUrl(string assetid)
        {
            if (assetid.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/Request?sel=Name,Scope.Name,Status.Name,");
            sb.Append("Source.Name,Owner.Name,Description");
            sb.Append("&where=Number='");
            sb.Append(assetid);
            sb.Append("'");

            return sb.ToString();
        }

        #endregion
    }
}