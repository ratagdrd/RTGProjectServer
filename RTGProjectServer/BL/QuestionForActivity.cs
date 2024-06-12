namespace ratagServerSide.BL
{
    public class QuestionForActivity
    {
        int activitycode;
        int questionNo;
        string question;
        string answer1;
        string answer2;
        string answer3;
        string answer4;
        int correctedAnswer;
        int noOfPoints;

        public int Activitycode { get => activitycode; set => activitycode = value; }
        public int QuestionNo { get => questionNo; set => questionNo = value; }
        public string Question { get => question; set => question = value; }
        public string Answer1 { get => answer1; set => answer1 = value; }
        public string Answer2 { get => answer2; set => answer2 = value; }
        public string Answer3 { get => answer3; set => answer3 = value; }
        public string Answer4 { get => answer4; set => answer4 = value; }
        public int CorrectedAnswer { get => correctedAnswer; set => correctedAnswer = value; }
        public int NoOfPoints { get => noOfPoints; set => noOfPoints = value; }

        public QuestionForActivity() { }

        public QuestionForActivity(int activitycode, int questionNo, string question, string answer1, string answer2, string answer3, string answer4, int correctedAnswer, int noOfPoints)
        {
            this.activitycode = activitycode;
            this.questionNo = questionNo;
            this.question = question;
            this.answer1 = answer1;
            this.answer2 = answer2;
            this.answer3 = answer3;
            this.answer4 = answer4;
            this.correctedAnswer = correctedAnswer;
            this.noOfPoints = noOfPoints;
        }

        public List<QuestionForActivity> ReadQuestion(int activitiCode)
        {
            DBServices dbs = new DBServices();
            return dbs.GetQuestion(activitiCode);

        }

        public int Insert()
        {
            DBServices dbs = new DBServices();
            return dbs.InsertQues(this);
        }

        public int Update()
        {
            DBServices dbs = new DBServices();
            return dbs.UpdateQuestionForActivity(this);
        }
    }

}
