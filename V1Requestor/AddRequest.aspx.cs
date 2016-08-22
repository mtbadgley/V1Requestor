using System;
using System.Data;
using System.Text;
using System.Web;
using System.Xml;

namespace V1Requestor
{
    public partial class _addrequest : System.Web.UI.Page
    {

        private DataSet _planningleveldata;
        private DataSet _servicetypedata;
        private DataSet _servicetypefiltered;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if we have the email address captured - key to all data
            GetEmailFromCookie();

            //Check to see if we have planning levels available in Session -- if not load
            if (Session["planninglevels"] != null)
            {
                _planningleveldata = (DataSet) Session["planninglevels"];
            }
            else
            {
                //Create structure and populate for Planning Levels
                MakePlanningLevelTable();
                GetPlanningLevels(GetPlanningLevelsUrl());
                Session["planninglevels"] = _planningleveldata;
            }

            //Check to see if we have planning levels available in Session -- if not load
            if (Session["servicetypes"] != null)
            {
                _servicetypedata = (DataSet)Session["servicetypes"];
            }
            else
            {
                //Create structure and populate for Planning Levels
                MakeServiceTypeTable();
                GetServiceTypes(GetServiceTypesUrl());
                Session["servicetypes"] = _servicetypedata;
            }

            if (!IsPostBack)
            {
                //Process Planning Levels
                planninglevel.DataSource = _planningleveldata;
                planninglevel.DataTextField = "name";
                planninglevel.DataValueField = "oid";
                planninglevel.DataBind();

                //Process Service Types
                UpdateServiceTypeControl(0);
            }
        }

        private void GetEmailFromCookie()
        {
            HttpCookie emailCookie = Request.Cookies["RequestorEmailCookie"];

            if (emailCookie == null)
            {
                Helpers.EmailAddress = String.Empty;
                Response.Redirect("SubmitEmail.aspx");
            }
            else
            {
                Helpers.EmailAddress = emailCookie.Values["emailaddress"].ToString();
            }

        }

        protected void PlanningLevelIndexChange(object sender, EventArgs e)
        {
            UpdateServiceTypeControl(planninglevel.SelectedIndex);
        }

        /// <summary>
        ///     Routine executed when user saves/submits the reuquest.
        /// </summary>
        protected void SubmitNewRequest(object sender, EventArgs e)
        {
            string requestedby = Helpers.EmailAddress;
            string scopeid = planninglevel.SelectedValue;
            string sourceid = servicetype.SelectedValue;
            string name = requesttitle.Text;
            string description = requestdescription.Text;

            if (CheckIfProvided(name,"Title"))
            {
                string payload = CreateNewRequestPayload(scopeid, sourceid, requestedby, name, description);
                string newurl = Helpers.DataUrl + "Data/Request";
                string result = Helpers.MakeHttpPost(Helpers.AccessToken, newurl, payload);
                if (string.IsNullOrEmpty(result))
                {
                    DisplayError("Please double check your entries.  If all looks correct please contact the VersionOne Administrator.");
                    return;
                }
                //Close window upon submit
                string close = @"<script type='text/javascript'>
                                window.returnValue = true;
                                window.close();
                                </script>";
                base.Response.Write(close); 
            }
        }

        /// <summary>
        ///     Routine to populate refresh Service Type control based on selection of the Planning Level.
        /// </summary>
        /// <param name="planninglevelindex">The index of the planninglevel dropdown control.</param>
        private void UpdateServiceTypeControl(int planninglevelindex)
        {
            DataRow row = _planningleveldata.Tables["planninglevelTable"].Rows[planninglevelindex];
            string abbr = row["abbr"].ToString();
            GetServiceTypeFiltered(abbr);
            servicetype.DataSource = _servicetypefiltered;
            servicetype.DataValueField = "oid";
            servicetype.DataTextField = "name";
            servicetype.DataBind();
        }

        /// <summary>
        ///     Routine to populate Planning Levels table by querying from VersionOne's Scope
        ///     based on those items with an abbreviation in the ServiceID.
        /// </summary>
        /// <param name="queryurl">The base REST query to retrieve data.</param>
        private void GetPlanningLevels(string queryurl)
        {
            try
            {
                string result = Helpers.MakeHttpRequest(Helpers.AccessToken, queryurl);

                if (result.Length == 0) return;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList assets = doc.SelectNodes("Assets/Asset");

                if (assets.Count > 0)
                {
                    // Add Data to Row
                    foreach (XmlNode asset in assets)
                    {
                        string oid = asset.Attributes["id"].Value;
                        string name = asset.SelectSingleNode("Attribute[@name='Name']").InnerText;
                        string abbr = asset.SelectSingleNode("Attribute[@name='Custom_ServiceID']").InnerText;
                        DataRow row = _planningleveldata.Tables["planninglevelTable"].NewRow();
                        row["oid"] = oid;
                        row["name"] = name;
                        row["abbr"] = abbr;

                        _planningleveldata.Tables["planninglevelTable"].Rows.Add(row);
                    }
                }
            }
            catch
            {
                DisplayError("Error: An invalid value has been provided in request string.  Please verify or perform normal search.");
            }
        }

        /// <summary>
        ///     Routine to populate ServiceTypes table by querying from VersionOne's StorySource.
        /// </summary>
        /// <param name="queryurl">The base REST query to retrieve data.</param>
        private void GetServiceTypes(string queryurl)
        {
            try
            {
                string result = Helpers.MakeHttpRequest(Helpers.AccessToken, queryurl);

                if (result.Length == 0) return;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList assets = doc.SelectNodes("Assets/Asset");

                if (assets.Count > 0)
                {
                    // Add Data to Row
                    foreach (XmlNode asset in assets)
                    {
                        string oid = asset.Attributes["id"].Value;
                        string name = asset.SelectSingleNode("Attribute[@name='Name']").InnerText;
                        string abbr = asset.SelectSingleNode("Attribute[@name='Description']").InnerText;
                        DataRow row = _servicetypedata.Tables["servicetypeTable"].NewRow();
                        row["oid"] = oid;
                        row["name"] = name;
                        row["abbr"] = abbr.Replace("<p>","").Replace("</p>","");

                        _servicetypedata.Tables["servicetypeTable"].Rows.Add(row);
                    }
                }
            }
            catch
            {
                DisplayError("Error: An invalid value has been provided in request string.  Please verify or perform normal search.");
            }
        }

        /// <summary>
        ///     Routine to populate a filtered ServiceTypes table 
        ///     based on the Planning level selected by the user.
        /// </summary>
        /// <param name="abbr">Planning Level abbreviation.  used as key to link between 
        /// Planning Levels and servicestypes.</param>
        private void GetServiceTypeFiltered(string abbr)
        {
            MakeServiceTypeFilteredTable();

            foreach (DataRow servicerow in _servicetypedata.Tables["servicetypeTable"].Rows)
            {
                if (servicerow["abbr"].ToString() == abbr)
                {
                    DataRow row = _servicetypefiltered.Tables["servicetypefilteredTable"].NewRow();
                    row["oid"] = servicerow["oid"];
                    row["name"] = servicerow["name"];
                    _servicetypefiltered.Tables["servicetypefilteredTable"].Rows.Add(row);
                }
            }

        }

        #region MakeDataTables

        private void MakePlanningLevelTable()
        {
            // Create Asset Table Structure
            DataTable table = new DataTable("planninglevelTable");
            DataColumn column;

            // oid Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "oid";
            column.ReadOnly = false;
            column.Unique = true;
            table.Columns.Add(column);
            // name Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "name";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            // abbr Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "abbr";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Instantiate the DataSet variable
            _planningleveldata = new DataSet();
            _planningleveldata.Tables.Add(table);
        }

        private void MakeServiceTypeTable()
        {
            // Create Asset Table Structure
            DataTable table = new DataTable("servicetypeTable");
            DataColumn column;

            // oid Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "oid";
            column.ReadOnly = false;
            column.Unique = true;
            table.Columns.Add(column);
            // name Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "name";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            // abbr Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "abbr";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Instantiate the DataSet variable
            _servicetypedata = new DataSet();
            _servicetypedata.Tables.Add(table);
        }

        private void MakeServiceTypeFilteredTable()
        {
            // Create Asset Table Structure
            DataTable table = new DataTable("servicetypeFilteredTable");
            DataColumn column;

            // oid Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "oid";
            column.ReadOnly = false;
            column.Unique = true;
            table.Columns.Add(column);
            // name Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "name";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Instantiate the DataSet variable
            _servicetypefiltered = new DataSet();
            _servicetypefiltered.Tables.Add(table);
        }

        #endregion

        #region CreateV1UrlsPayload 

        private string GetPlanningLevelsUrl()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/Scope?sel=ID,Name,Custom_ServiceID&where=Custom_ServiceID!=''");

            return sb.ToString();
        }

        private string GetServiceTypesUrl()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/StorySource?sel=ID,Name,Description");
            return sb.ToString();
        }

        private string CreateNewRequestPayload(string scopeid, string sourceid, string requestedby, string name,
            string description)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Asset href='/");
            sb.Append(Helpers.V1Instance);
            sb.Append("/rest-1.v1/New/Request'><Attribute name='Name' act='set'>");
            sb.Append(name);
            sb.Append("</Attribute><Attribute name='RequestedBy' act='set'>");
            sb.Append(requestedby);
            sb.Append("</Attribute><Relation name='Scope' act='set'><Asset href='/");
            sb.Append(Helpers.V1Instance);
            sb.Append("/rest-1.v1/Data/");
            sb.Append(scopeid.Replace(":", "/"));
            sb.Append("' idref='");
            sb.Append(scopeid);
            sb.Append("' /></Relation><Relation name='Source' act='set'><Asset href='/");
            sb.Append(Helpers.V1Instance);
            sb.Append("/rest-1.v1/Data/");
            sb.Append(sourceid.Replace(":", "/"));
            sb.Append("' idref='");
            sb.Append(sourceid);
            sb.Append("' /></Relation><Attribute name='Description' act='set'>");
            sb.Append(description);
            sb.Append("</Attribute></Asset>");
            return sb.ToString();
        }

        #endregion

        #region ValidateForm

        //Simple validation utility
        private bool CheckIfProvided(string value, string field)
        {
            if (string.IsNullOrEmpty(value))
            {
                DisplayError(String.Format("The Request is incomplete. {0} is required.",field));
                return false;
            }
            return true;
        }

        #endregion

        //Page utility to show error.
        private void DisplayError(string errMessage)
        {
            errlabel.Visible = true;
            errlabel.Text = errMessage;
            errsection.Visible = true;
        }
    }
}