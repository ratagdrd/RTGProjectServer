namespace ratagServerSide.BL
{
    public class Spot
    {
		int spotId;
		string spotName;
		string longT;
		string latT;
		string sDescription;
		string photo;
		string spotType; //('T', 'R', 'G', 'D', 'A')-Toilet, restArea, giftShop, Drinks, Activity
        bool isAccessible;
        bool isBlockd;
        int siteCode; // fk tblSite


        public Spot() { }
        public Spot(int spotId, string spotName, string longT, string latT, string sDescription, string photo, string spotType, bool isAccessible, bool isBlockd, int siteCode)
        {
            SpotId = spotId;
            SpotName = spotName;
            LongT = longT;
            LatT = latT;
            SDescription = sDescription;
            Photo = photo;
            SpotType = spotType;
            IsAccessible = isAccessible;
            IsBlockd = isBlockd;
            SiteCode = siteCode;
        }

        public int SpotId { get => spotId; set => spotId = value; }
        public string SpotName { get => spotName; set => spotName = value; }
        public string LongT { get => longT; set => longT = value; }
        public string LatT { get => latT; set => latT = value; }
        public string SDescription { get => sDescription; set => sDescription = value; }
        public string Photo { get => photo; set => photo = value; }
        public string SpotType { get => spotType; set => spotType = value; }
        public bool IsAccessible { get => isAccessible; set => isAccessible = value; }
        public bool IsBlockd { get => isBlockd; set => isBlockd = value; }
        public int SiteCode { get => siteCode; set => siteCode = value; }



        public List<Spot> ReadSpotsInSite(int sitecode)
        {
            DBServices dbs = new DBServices();
            return dbs.GetSpot(sitecode);

        }
        public int Insert()
        {
            DBServices dbs = new DBServices();
            return dbs.InsertSpot(this);
        }

        public int Update()
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateSpot(this);
        }
         public int delete()
        {
            DBServices dbs = new DBServices();
            return dbs.deleteSpot(this.spotId);
        }

    }
}
