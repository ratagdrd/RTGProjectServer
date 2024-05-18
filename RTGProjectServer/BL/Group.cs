using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace ratagServerSide.BL
{
    public class Group
    {
		int groupCode;
        string groupName;
		DateTime createDate;
		string photo;
		string roadType; // ('R', 'A', 'B')
		int numOfParticipants;
        int maxAge;
        int minAge;
        int totalPoints;

        public Group() { }
        public Group(int groupCode, string groupName, DateTime createDate, string photo, string roadType, int numOfParticipants, int maxAge, int minAge, int totalPoints)
        {
            GroupCode = groupCode;
            GroupName = groupName;
            CreateDate = createDate;
            Photo = photo;
            RoadType = roadType;
            NumOfParticipants = numOfParticipants;
            MaxAge = maxAge;
            MinAge = minAge;
            TotalPoints = totalPoints;
        }

        public int GroupCode { get => groupCode; set => groupCode = value; }
        public string GroupName { get => groupName; set => groupName = value; }

        public DateTime CreateDate { get => createDate; set => createDate = value; }
        public string Photo { get => photo; set => photo = value; }
        public string RoadType { get => roadType; set => roadType = value; }
        public int NumOfParticipants { get => numOfParticipants; set => numOfParticipants = value; }
        public int MaxAge { get => maxAge; set => maxAge = value; }
        public int MinAge { get => minAge; set => minAge = value; }
        public int TotalPoints { get => totalPoints; set => totalPoints = value; }


        public int Insert() //returns groupCode
        {
            DBServices dbs = new DBServices();
            return dbs.InsertGroup(this);
        }
        //get all groups in db
        public List<Group> Read()
        {
            DBServices dbs = new DBServices();
            return dbs.GetGroups();
        }

        public Group GetGroupByGroupCode(int groupCode)
        {
            DBServices dbs = new DBServices();
            return dbs.GetGroupByGroupCode(groupCode);
        }

        //public string GetPhoto(int groupCode)
        //{
        //    DBServices dbs = new DBServices();
        //    return dbs.GetPhoto(groupCode);
        //}
        //public string GetEmoji(int groupCode)
        //{
        //    DBServices dbs = new DBServices();
        //    return dbs.GetEmoji(groupCode);
        //}
        public int UpdatePhoto(int groupCode, string imageName)
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateImageName(groupCode, imageName);

        }

        public int UpdateTotalPoints(int groupCode, int pointsToAdd)
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateGroupPoints(groupCode, pointsToAdd);
        }
    }

}
