namespace RTGProjectServer.BL
{
    public class Employee
    {
        string username;
        string password;
        string role;

        public Employee() { }

        public Employee(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }

        public int LogIn()
        {
            DBServices dbs = new DBServices();
            int logInStatus = dbs.EmpLogIn(Username,Password);
            return logInStatus;
        }
    }
}
