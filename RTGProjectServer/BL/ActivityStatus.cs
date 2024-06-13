using ratagServerSide.BL;

namespace RTGProjectServer.BL
{
    public class ActivityStatus
    {
        int statusId;
        int activitycode;
        bool isAccessible;
        bool isBlocked;

        public ActivityStatus(){}
        public ActivityStatus(int statusId, int activitycode, bool isAccessible, bool isBlocked)
        {
            StatusId = statusId;
            Activitycode = activitycode;
            IsAccessible = isAccessible;
            IsBlocked = isBlocked;
        }

        public int StatusId { get => statusId; set => statusId = value; }
        public int Activitycode { get => activitycode; set => activitycode = value; }
        public bool IsAccessible { get => isAccessible; set => isAccessible = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }


        public ActivityStatus GetStat(int activitycode)
        {
            DBServices dbs = new DBServices();
            return dbs.GetActivityStatusByCode(activitycode);

        }
        public int Update(int activityCode, bool isAccessible, bool isBlocked)
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateStatus(activityCode, isAccessible, isBlocked);
        }
    }
}
