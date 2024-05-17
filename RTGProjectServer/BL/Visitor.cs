namespace ratagServerSide.BL
{
    public class Visitor
    {
        int visitorCode;
        string nickName;
        int yearOfBirth;
        string gender; //('F', 'M')

        public Visitor(int visitorCode, string nickName, int yearOfBirth, string gender)
        {
            VisitorCode = visitorCode;
            NickName = nickName;
            YearOfBirth = yearOfBirth;
            Gender = gender;
        }

        public int VisitorCode { get => visitorCode; set => visitorCode = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public int YearOfBirth { get => yearOfBirth; set => yearOfBirth = value; }
        public string Gender { get => gender; set => gender = value; }
    }


    //public int Insert()
    //{
    //    DBServices dbs = new DBServices();
    //    return dbs.InsertVisitor(this);

    //}
}
