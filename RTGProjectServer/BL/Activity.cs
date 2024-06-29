using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ratagServerSide.BL
{
    public class Activity
    {
        int activitycode;
        string activityname;
        string instruction;
        int rate;
        int numOfRates;
        static List<QuestionForActivity> questionForActivityList = new List<QuestionForActivity>();

       
        public Activity() { }

        public Activity(int activitycode, string activityname, string instruction, int rate, int numOfRates)
        {
            Activitycode = activitycode;
            Activityname = activityname;
            Instruction = instruction;
            Rate = rate;
            NumOfRates = numOfRates;
        }

        public int Activitycode { get => activitycode; set => activitycode = value; }
        public string Activityname { get => activityname; set => activityname = value; }
        public string Instruction { get => instruction; set => instruction = value; }
        public int Rate { get => rate; set => rate = value; }
        public int NumOfRates { get => numOfRates; set => numOfRates = value; }

        public int Insert()
        {
            DBServices dbs = new DBServices();
            return dbs.InsertActivity(this);
        }

        public List<Activity> Read()
        {
            DBServices dbs = new DBServices();
            return dbs.GetActivity();
        }

        public int UpdateRate(int activitycode, int rateToAdd)
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateRateActivity(activitycode, rateToAdd);
        }

        public int Update(int activityCode, string activityname, string instruction)
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateActivity(activityCode, activityname, instruction);
        }

        public Activity getActivityByCode(int code)
        {
            DBServices dbs = new DBServices();
            return dbs.GetActivityById(code);
        }
    }

   
}
