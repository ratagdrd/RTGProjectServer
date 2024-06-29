namespace ratagServerSide.BL
{
    public class Site
    {
            int siteCode;
            string siteName;
            string address;
            string sDescription;
            string phoneNo;
            string webSite;
            string openingHours;

        public Site() { }
        public Site(int siteCode, string siteName, string address, string sDescription, string phoneNo, string webSite, string openingHours)
        {
            SiteCode = siteCode;
            SiteName = siteName;
            Address = address;
            SDescription = sDescription;
            PhoneNo = phoneNo;
            WebSite = webSite;
            OpeningHours = openingHours;
        }

        public int SiteCode { get => siteCode; set => siteCode = value; }
        public string SiteName { get => siteName; set => siteName = value; }
        public string Address { get => address; set => address = value; }
        public string SDescription { get => sDescription; set => sDescription = value; }
        public string PhoneNo { get => phoneNo; set => phoneNo = value; }
        public string WebSite { get => webSite; set => webSite = value; }
        public string OpeningHours { get => openingHours; set => openingHours = value; }

        public List<Site> Read()
        {
            DBServices dbs = new DBServices();
            return dbs.GetSites();
        }

        public Site ReadSiteByCode(int siteCode)
        {
            DBServices dbs = new DBServices();
            return dbs.GetSite(siteCode);

        }


        public int Insert()
        {
            DBServices dbs = new DBServices();
            return dbs.InsertSite(this);
        }

        public int Update()
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateSite(this);
        }


    }



}
