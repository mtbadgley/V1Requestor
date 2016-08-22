using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace V1Requestor
{
    public partial class _Default : Page
    {
        private DataSet _assetData;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetEmailFromCookie();
            if (!String.IsNullOrEmpty(Helpers.EmailAddress))
            {
                ShowAssets();
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
                Helpers.EmailAddress = emailCookie.Values["emailaddress"];
            }

        }

        private void ShowAssets()
        {
            // Query based on the email address
            errsection.Visible = false;
            MakeAssetTable();
            string reqSearchFor = Helpers.EmailAddress;
            GetAssets(GetRequestUrl(reqSearchFor,includeClosed.Checked));
            assetsview.DataSource = _assetData;
            assetsview.DataBind();
            requestlisting.Visible = true;
        }

        private void MakeAssetTable()
        {
            // Create Asset Table Structure
            DataTable table = new DataTable("assetTable");
            DataColumn column;

            // ID Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ID";
            column.ReadOnly = false;
            column.Unique = true;
            table.Columns.Add(column);

            // Title Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Title";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Status Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Status";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // GroupTeam Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "GroupTeam";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // ServiceRequested Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ServiceRequested";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // LastUpdated Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "LastUpdated";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // RelatedWorkitems Column
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "RelatedWorkitems";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Instantiate the DataSet variable
            _assetData = new DataSet();
            _assetData.Tables.Add(table);
        }

        private void GetAssets(string queryurl)
        {
            try
            {
                
                string result = Helpers.MakeHttpRequest(Helpers.AccessToken,queryurl);

                if (result.Length == 0) return;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList assets = doc.SelectNodes("Assets/Asset");

                if (assets != null && assets.Count > 0)
                {
                    // Add Data to Row
                    foreach (XmlNode asset in assets)
                    {
                        string assetState = GetAssetState(asset.SelectSingleNode("Attribute[@name='AssetState']").InnerText);
                        string assetID = asset.SelectSingleNode("Attribute[@name='Number']").InnerText;
                        string statusValue = asset.SelectSingleNode("Attribute[@name='Status.Name']").InnerText;
                        string statusSpanopening = Helpers.GetStatusStyle(statusValue,assetState);
                        DataRow row = _assetData.Tables["assetTable"].NewRow();
                        string spanopening = "<span>";
                        if (assetState != "Active")
                            spanopening = "<span style='text-decoration: line-through;'>";
                        row["ID"] = spanopening + "<a href='AssetDetails.aspx?id=" + assetID + "' target='_blank'>" + assetID + "</a></span>";
                        row["Title"] = spanopening + asset.SelectSingleNode("Attribute[@name='Name']").InnerText + "</span>";
                        row["Status"] = statusSpanopening + asset.SelectSingleNode("Attribute[@name='Status.Name']").InnerText + "</span>";
                        row["GroupTeam"] = spanopening + asset.SelectSingleNode("Attribute[@name='Scope.Name']").InnerText + "</span>";
                        row["ServiceRequested"] = spanopening + asset.SelectSingleNode("Attribute[@name='Source.Name']").InnerText + "</span>";
                        row["LastUpdated"] = spanopening + asset.SelectSingleNode("Attribute[@name='ChangeDate']").InnerText + "</span>";
                        XmlNodeList relatedworkitems = asset.SelectNodes("Attribute[@name='PrimaryWorkitems.Number']/Value");
                        XmlNodeList relatedepics = asset.SelectNodes("Attribute[@name='Epics.Number']/Value");

                        string relateditems = GetXmlValues(relatedworkitems,string.Empty);
                        relateditems = GetXmlValues(relatedepics, relateditems);


                        row["RelatedWorkitems"] = spanopening + Helpers.Getv1List(relateditems.Split(',')) + "</span>";

                       if (!includeClosed.Checked && assetState == "Active")
                            _assetData.Tables["assetTable"].Rows.Add(row);
                       else if (includeClosed.Checked)
                            _assetData.Tables["assetTable"].Rows.Add(row);
                    }
                }
            }
            catch
            {
                DisplayError("Error: An invalid value has been provided in request string.  Please verify or perform normal search.");
            }
        }

        private void DisplayError(string errMessage)
        {
            errlabel.Visible = true;
            errlabel.Text = errMessage;
            errsection.Visible = true;
        }

        protected void AssetsviewPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            assetsview.PageIndex = e.NewPageIndex;
            assetsview.DataBind();
        }

        protected string GetRequestUrl(string requestedby, bool includeclosed)
        {
            if (requestedby.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Helpers.DataUrl);
            sb.Append("Data/Request?sel=Number,Name,Scope.Name,Status.Name,");
            sb.Append("Source.Name,ChangeDate,AssetState,PrimaryWorkitems.Number,Epics.Number");
            sb.Append("&where=RequestedBy='");
            sb.Append(requestedby);
            sb.Append("';AssetState='64'");
            if (includeclosed)
            {
                sb.Append(",'128'");
            }

            return sb.ToString();
        }

        protected string GetAssetState(string assetstate)
        {
            if (assetstate == "64")
                return "Active";

            if (assetstate == "128")
                return "Closed";

            return string.Empty;
        }

        protected string GetXmlValues(XmlNodeList items, string currentlist)
        {
            string listofitems = currentlist;
            foreach (XmlNode item in items)
            {
                if (listofitems.Length == 0)
                {
                    listofitems = item.InnerText;
                }
                else
                {
                    listofitems = listofitems + "," + item.InnerText;
                }
            }
            return listofitems;
        }


    }
}