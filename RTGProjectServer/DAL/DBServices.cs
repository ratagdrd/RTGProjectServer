using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ratagServerSide.BL;
using System.Net;
using RTGProjectServer.BL;

public class DBServices
    {


    public DBServices() { }


    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {
        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //Insert Group- insert that returns groupCode
    public int InsertGroup(Group group)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertGroupWithStoredProcedure("sp_InsertGroup", con, group); // create the command
        try
        {
            // Execute the command (numEffected)
            cmd.ExecuteNonQuery();
         int groupCode = Convert.ToInt32(cmd.Parameters["@groupCode"].Value);
                return groupCode;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // Close the database connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateInsertGroupWithStoredProcedure(String spName, SqlConnection con, Group group)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object

        cmd.Connection = con; // Assign the connection to the command object

        cmd.CommandText = spName; // Set the stored procedure name

        cmd.CommandTimeout = 10; // Time to wait for the execution. The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // Set the command type to stored procedure

        // Add parameters
        cmd.Parameters.AddWithValue("@groupName", group.GroupName);
        cmd.Parameters.AddWithValue("@roadType", group.RoadType);
        cmd.Parameters.AddWithValue("@maxAge", group.MaxAge);
        cmd.Parameters.AddWithValue("@minAge", group.MinAge);
        cmd.Parameters.AddWithValue("@totalPoints", group.TotalPoints);
        cmd.Parameters.AddWithValue("@photo", group.Photo);
        cmd.Parameters.AddWithValue("@numOfParticipants", group.NumOfParticipants);

        // Add output parameter for groupCode
        SqlParameter groupCodeParameter = new SqlParameter("@groupCode", SqlDbType.Int);
        groupCodeParameter.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(groupCodeParameter);

        return cmd;
    }


    //get groups
    public List<Group> GetGroups()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Group> groupList = new List<Group>();

        cmd = buildReadGroupsStoredProcedureCommand(con, "sp_GetGroups");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Group g = new Group();
            g.GroupCode= Convert.ToInt32(dataReader["groupCode"]);
            g.GroupName = dataReader["groupName"].ToString();
            g.CreateDate = Convert.ToDateTime(dataReader["createdDate"]);
            g.Photo = dataReader["photo"].ToString(); 
            g.RoadType = dataReader["roadType"].ToString();
            g.MaxAge=Convert.ToInt32(dataReader["maxAge"]);
            g.MinAge= Convert.ToInt32(dataReader["minAge"]);
            g.TotalPoints=Convert.ToInt32(dataReader["totalPoints"]);
            g.NumOfParticipants = Convert.ToInt32(dataReader["numOfParticipants"]);

            groupList.Add(g);
        }

        if (con != null)
        {
            con.Close();
        }

        return groupList;

    }

    SqlCommand buildReadGroupsStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }


    //get group by groupCode
    public Group GetGroupByGroupCode(int groupCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildGetGroupByGroupCodeStoredProcedureCommand(con, "sp_GetGroupByCode", groupCode);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        Group g = new Group();

        while (dataReader.Read())
        {
            g.GroupCode = Convert.ToInt32(dataReader["groupCode"]);
            g.GroupName = dataReader["groupName"].ToString();
            g.CreateDate = Convert.ToDateTime(dataReader["createdDate"]);
            g.Photo = dataReader["photo"].ToString();
            g.RoadType = dataReader["roadType"].ToString();
            g.MaxAge = Convert.ToInt32(dataReader["maxAge"]);
            g.MinAge = Convert.ToInt32(dataReader["minAge"]);
            g.TotalPoints = Convert.ToInt32(dataReader["totalPoints"]);
            g.NumOfParticipants = Convert.ToInt32(dataReader["numOfParticipants"]);

        }

        if (con != null)
        {
            con.Close();
        }
        return g;


    }

    SqlCommand buildGetGroupByGroupCodeStoredProcedureCommand(SqlConnection con, string spName, int groupCode)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@groupCode", groupCode);


        return cmd;

    }



    //update photo fileName at the photo field in group table

    public int UpdateImageName(int groupCode, string imageName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdatePhotoWithStoredProcedure("sp_UpdatePhotoByGroupCode", con, groupCode, imageName);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdatePhotoWithStoredProcedure(String spName, SqlConnection con, int groupCode, string imageName)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@groupCode", groupCode);
        cmd.Parameters.AddWithValue("@photoName", imageName);
   
        return cmd;
    }


    //get photo by groupCode- get the photo path from server
    //public string GetPhoto(int groupCode)
    //{
    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }


    //    cmd = buildGetPhotoStoredProcedureCommand(con, "sp_GetPhotoByGroupCode", groupCode);

    //    SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //    string photo = null; //for exception- groupCode not exist
    //    while (dataReader.Read())
    //    {
    //        photo = dataReader["photo"].ToString();
    //    }

    //    if (con != null)
    //    {
    //        con.Close();

    //        if (photo == ""||photo==null) // if there is no photo the data reader return "", if it didnt entered the while- groupCode not exist
    //        {
    //            return photo;
    //        }
    //    }
    //    //return System.IO.Directory.GetCurrentDirectory() + "\\uploadedFiles\\" + photo;
    //    return "/Images/" + photo;

    //}

    //SqlCommand buildGetPhotoStoredProcedureCommand(SqlConnection con, string spName, int groupCode)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
    //    cmd.Parameters.AddWithValue("@groupCode", groupCode);


    //    return cmd;

    //}

    //update group points

    public int UpdateGroupPoints(int groupCode, int pointsToAdd)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateGroupPointsWithStoredProcedure("sp_UpdateTotalPoints", con, groupCode, pointsToAdd);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    
    

    //public string GetEmoji(int groupCode)
    //{
    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }


    //    cmd = buildGetEmojiStoredProcedureCommand(con, "sp_GetPhotoByGroupCode", groupCode);

    //    SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //    string emoji = null;
    //    while (dataReader.Read())
    //    {
    //        emoji = dataReader["photo"].ToString();
    //    }

    //    if (con != null)
    //    {
    //        con.Close();
    //        if (emoji == "" || emoji == null) // if there is no photo the data reader return "", if it didnt entered the while- groupCode not exist
    //        {
    //            return emoji;
    //        }
    //    }
    //    return emoji;


    //}
    //SqlCommand buildGetEmojiStoredProcedureCommand(SqlConnection con, string spName, int groupCode)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
    //    cmd.Parameters.AddWithValue("@groupCode", groupCode);


    //    return cmd;

    //}

    private SqlCommand CreateUpdateGroupPointsWithStoredProcedure(String spName, SqlConnection con, int groupCode, int pointsToAdd)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@groupCode", groupCode);
        cmd.Parameters.AddWithValue("@newTotalPoints", pointsToAdd);

        return cmd;
    }

    //delete Group by groupCode
    public int deleteGroup(int groupCode)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateDeleteGroupWithStoredProcedure("sp_DeleteGroup", con, groupCode);   // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    private SqlCommand CreateDeleteGroupWithStoredProcedure(String spName, SqlConnection con, int groupCode)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@groupCode", groupCode);


        return cmd;
    }

    //Question for activity
    //get question
    public List<QuestionForActivity> GetQuestion(int activitiCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<QuestionForActivity> questionList = new List<QuestionForActivity>();

        cmd = buildReadquesStoredProcedureCommand(con, "sp_GetQuestionForActivity", activitiCode);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            QuestionForActivity q = new QuestionForActivity();

            q.Activitycode = activitiCode;
            q.QuestionNo = Convert.ToInt32(dataReader["questionNo"]);
            q.Question = dataReader["question"].ToString();
            q.Answer1 = dataReader["answer1"].ToString();
            q.Answer2 = dataReader["answer2"].ToString();
            q.Answer3 = dataReader["answer3"].ToString();
            q.Answer4 = dataReader["answer4"].ToString();
            q.CorrectedAnswer = Convert.ToInt32(dataReader["CorrectedAnswer"]);
            q.NoOfPoints = Convert.ToInt32(dataReader["NoOfPoints"]);


            questionList.Add(q);
        }

        if (con != null)
        {
            con.Close();
        }

        return questionList;

    }

    SqlCommand buildReadquesStoredProcedureCommand(SqlConnection con, string spName, int activitiCode)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@activitycode", activitiCode);


        return cmd;

    }


    public int InsertQues(QuestionForActivity ques)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertQuesWithStoredProcedure("sp_InsertQuestionForActivity", con, ques);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (SqlException ex)
        {
            if (ex.Number == 547) // 547 is the error code for foreign key constraint violation
            {
                // Handle the foreign key constraint violation here
                throw new Exception("The provided activity code does not exist.", ex);
            }
            else
            {
                // Other SQL exceptions
                throw ex;
            }
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private SqlCommand CreateInsertQuesWithStoredProcedure(String spName, SqlConnection con, QuestionForActivity ques)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", ques.Activitycode);
        cmd.Parameters.AddWithValue("@questionNo", ques.QuestionNo);
        cmd.Parameters.AddWithValue("@question", ques.Question);
        cmd.Parameters.AddWithValue("@answer1", ques.Answer1);
        cmd.Parameters.AddWithValue("@answer2", ques.Answer2);
        cmd.Parameters.AddWithValue("@answer3", ques.Answer3);
        cmd.Parameters.AddWithValue("@answer4", ques.Answer4);
        cmd.Parameters.AddWithValue("@CorrectedAnswer", ques.CorrectedAnswer);
        cmd.Parameters.AddWithValue("@NoOfPoints", ques.NoOfPoints);

        return cmd;
    }

    //update question for activity 
    public int UpdateQuestionForActivity(QuestionForActivity question)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateQuestionForActivityWithStoredProcedure("sp_UpdateQuestionForActivity", con, question); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateQuestionForActivityWithStoredProcedure(String spName, SqlConnection con, QuestionForActivity question)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", question.Activitycode);
        cmd.Parameters.AddWithValue("@questionNo", question.QuestionNo);
        cmd.Parameters.AddWithValue("@question", question.Question);
        cmd.Parameters.AddWithValue("@answer1", question.Answer1);
        cmd.Parameters.AddWithValue("@answer2", question.Answer2);
        cmd.Parameters.AddWithValue("@answer3", question.Answer3);
        cmd.Parameters.AddWithValue("@answer4", question.Answer4);
        cmd.Parameters.AddWithValue("@correctedAnswer", question.CorrectedAnswer);
        cmd.Parameters.AddWithValue("@noOfPoints", question.NoOfPoints);
        return cmd;
    }


    //Site
    //get site by code
    public Site GetSite(int siteCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildReadSiteStoredProcedureCommand(con, "sp_GetSite", siteCode);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        Site s = new Site();

        while (dataReader.Read())
        {

            s.SiteCode = Convert.ToInt32(dataReader["siteCode"]);
            s.SiteName = dataReader["siteName"].ToString();
            s.PhoneNo = dataReader["phoneNo"].ToString();
            s.Address = dataReader["address"].ToString();
            s.SDescription = dataReader["sDescription"].ToString();
            s.WebSite = dataReader["webSite"].ToString();
            s.OpeningHours = dataReader["openingHours"].ToString();

        }

        if (con != null)
        {
            con.Close();
        }

        return s;

    }

    SqlCommand buildReadSiteStoredProcedureCommand(SqlConnection con, string spName, int siteCode)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@siteCode", siteCode);
        

        return cmd;

    }


    //get all sites

    public List<Site> GetSites()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildReadSitesStoredProcedureCommand(con, "sp_GetSites");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        List<Site> sites = new List<Site>();
        while (dataReader.Read())
        {
            Site s = new Site();

            s.SiteCode = Convert.ToInt32(dataReader["siteCode"]);
            s.SiteName = dataReader["siteName"].ToString();
            s.PhoneNo = dataReader["phoneNo"].ToString();
            s.Address = dataReader["address"].ToString();
            s.SDescription = dataReader["sDescription"].ToString();
            s.WebSite = dataReader["webSite"].ToString();
            s.OpeningHours = dataReader["openingHours"].ToString();
            sites.Add(s);
        }

        if (con != null)
        {
            con.Close();
        }

        return sites;

    }

    SqlCommand buildReadSitesStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        return cmd;

    }

    public int InsertSite(Site site)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertSiteWithStoredProcedure("sp_InsertSite", con, site);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateInsertSiteWithStoredProcedure(String spName, SqlConnection con, Site site)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@siteName", site.SiteName);
        cmd.Parameters.AddWithValue("@address", site.Address);
        cmd.Parameters.AddWithValue("@sDescription", site.SDescription);
        cmd.Parameters.AddWithValue("@phoneNo", site.PhoneNo);
        cmd.Parameters.AddWithValue("@webSite", site.WebSite);
        cmd.Parameters.AddWithValue("@openingHours", site.OpeningHours);


        return cmd;
    }

    //update Site
    public int UpdateSite(Site site)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateSiteWithStoredProcedure("sp_UpdateSite", con,  site); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateSiteWithStoredProcedure(String spName, SqlConnection con, Site site)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@siteCode", site.SiteCode);
        cmd.Parameters.AddWithValue("@siteName", site.SiteName);
        cmd.Parameters.AddWithValue("@address", site.Address);
        cmd.Parameters.AddWithValue("@sDescription", site.SDescription);
        cmd.Parameters.AddWithValue("@phoneNo", site.PhoneNo);
        cmd.Parameters.AddWithValue("@webSite", site.WebSite);
        cmd.Parameters.AddWithValue("@openingHours", site.OpeningHours);

        return cmd;
    }


    //Insert Spot
    public int InsertSpot(Spot spot)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertSpotWithStoredProcedure("sp_InsertSpot", con, spot);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateInsertSpotWithStoredProcedure(String spName, SqlConnection con, Spot spot)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@spotName", spot.SpotName);
        cmd.Parameters.AddWithValue("@longT", spot.LongT);
        cmd.Parameters.AddWithValue("@latT", spot.LatT);
        cmd.Parameters.AddWithValue("@SDescription", spot.SDescription);
        cmd.Parameters.AddWithValue("@photo", spot.Photo);
        cmd.Parameters.AddWithValue("@spotType", spot.SpotType);
        cmd.Parameters.AddWithValue("@IsAccessible", spot.IsAccessible);
        cmd.Parameters.AddWithValue("@isBlockd", spot.IsBlockd);
        cmd.Parameters.AddWithValue("@siteCode", spot.SiteCode);

        return cmd;
    }

    //get Spot

    public List<Spot> GetSpot(int siteCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Spot> spotsList = new List<Spot>();

        cmd = buildReadSpotStoredProcedureCommand(con, "sp_GetSpots", siteCode);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Spot s= new Spot();
            s.SpotId = Convert.ToInt32(dataReader["spotId"]);
            s.SpotName = dataReader["spotName"].ToString();
            s.LongT = dataReader["longT"].ToString();
            s.LatT = dataReader["latT"].ToString();
            s.SDescription = dataReader["SDescription"].ToString();
            s.Photo = dataReader["photo"].ToString();
            s.SpotType = dataReader["spotType"].ToString();
            s.IsAccessible = Convert.ToBoolean(dataReader["IsAccessible"]);
            s.IsBlockd = Convert.ToBoolean(dataReader["isBlockd"]);
            s.SiteCode = Convert.ToInt32(dataReader["siteCode"]);

            spotsList.Add(s);
        }

        if (con != null)
        {
            con.Close();
        }

        return spotsList;

    }
    SqlCommand buildReadSpotStoredProcedureCommand(SqlConnection con, string spName, int siteCode)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@siteCode", siteCode);


        return cmd;

    }

    //updateSpot

    public int UpdateSpot(Spot spot)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateSpotWithStoredProcedure("sp_UpdateSpot", con, spot); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateSpotWithStoredProcedure(String spName, SqlConnection con, Spot spot)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@spotId", spot.SpotId);
        cmd.Parameters.AddWithValue("@spotName", spot.SpotName);
        cmd.Parameters.AddWithValue("@longT", spot.LongT);
        cmd.Parameters.AddWithValue("@latT", spot.LatT);
        cmd.Parameters.AddWithValue("@SDescription", spot.SDescription);
        cmd.Parameters.AddWithValue("@photo", spot.Photo);
        cmd.Parameters.AddWithValue("@spotType", spot.SpotType);
        cmd.Parameters.AddWithValue("@IsAccessible", spot.IsAccessible);
        cmd.Parameters.AddWithValue("@isBlockd", spot.IsBlockd);
        cmd.Parameters.AddWithValue("@siteCode", spot.SiteCode);
        return cmd;
    }

    public int deleteSpot(int spotId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateDeleteSpotWithStoredProcedure("sp_DeleteSpot", con, spotId);   // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //delete spot
    private SqlCommand CreateDeleteSpotWithStoredProcedure(String spName, SqlConnection con, int spotId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@spotId", spotId);


        return cmd;
    }

    //Insert Activity
    public int InsertActivity(Activity activity)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertActivityWithStoredProcedure("sp_InsertActivity", con, activity);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateInsertActivityWithStoredProcedure(String spName, SqlConnection con, Activity activity)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activityName", activity.Activityname);
        cmd.Parameters.AddWithValue("@instructions", activity.Instruction);
        cmd.Parameters.AddWithValue("@rate", activity.Rate);
        cmd.Parameters.AddWithValue("@numOfRates", activity.NumOfRates);

        return cmd;
    }


    //update Activity


    public int UpdateRateActivity(int activitycode, int rateToAdd)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateRateActivityWithStoredProcedure("sp_UpdateActivityRate", con, activitycode, rateToAdd);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateRateActivityWithStoredProcedure(String spName, SqlConnection con, int activitycode, int rateToAdd)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", activitycode);
        cmd.Parameters.AddWithValue("@newRate", rateToAdd);
       
        return cmd;
    }


    //get Activity
    public List<Activity> GetActivity()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Activity> activityList = new List<Activity>();

        cmd = buildReadActivityStoredProcedureCommand(con, "sp_GetActivities");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Activity a = new Activity();

            a.Activitycode = Convert.ToInt32(dataReader["activitycode"]);
            a.Activityname = dataReader["activityName"].ToString();
            a.Instruction = dataReader["instructions"].ToString();
            a.Rate = Convert.ToInt32(dataReader["rate"]); 
            a.NumOfRates = Convert.ToInt32(dataReader["numOfRates"]);
            activityList.Add(a);
        }

        if (con != null)
        {
            con.Close();
        }

        return activityList;

    }

    SqlCommand buildReadActivityStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }

    //get Activity by code 
    public Activity GetActivityById(int id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Activity a = new Activity();

        cmd = buildReadActivityByCodeStoredProcedureCommand(con, "sp_GetActivityByCode", id);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            a.Activitycode = Convert.ToInt32(dataReader["activitycode"]);
            a.Activityname = dataReader["activityName"].ToString();
            a.Instruction = dataReader["instructions"].ToString();
            a.Rate = Convert.ToInt32(dataReader["rate"]);
            a.NumOfRates = Convert.ToInt32(dataReader["numOfRates"]);
        }

        if (con != null)
        {
            con.Close();
        }

        return a;

    }

    //update Activity
    public int UpdateActivity(int activityCode, string activityname, string instruction)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateActivityWithStoredProcedure("sp_UpdateActivity", con, activityCode, activityname, instruction); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateActivityWithStoredProcedure(String spName, SqlConnection con, int activitycode, string activityname, string instruction)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", activitycode);

        cmd.Parameters.AddWithValue("@newActivityName", activityname);
        cmd.Parameters.AddWithValue("@newInstructions", instruction);
        return cmd;
    }

    SqlCommand buildReadActivityByCodeStoredProcedureCommand(SqlConnection con, string spName, int id)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", id);

        return cmd;

    }


    //get Activity status by activity code 
    public ActivityStatus GetActivityStatusByCode(int activitycode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
       

        cmd = buildGetActivityStatusByCodeStoredProcedureCommand(con, "sp_GetActivityStatus", activitycode);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        ActivityStatus status = new ActivityStatus();
        while (dataReader.Read())
        {
            status.Activitycode = Convert.ToInt32(dataReader["activitycode"]);
            status.IsAccessible = Convert.ToBoolean(dataReader["isAccessible"]);
            status.IsBlocked = Convert.ToBoolean(dataReader["isBlocked"]);

        }

        if (con != null)
        {
            con.Close();
        }

        return status;

    }

    SqlCommand buildGetActivityStatusByCodeStoredProcedureCommand(SqlConnection con, string spName, int id)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", id);

        return cmd;

    }

    //update status
    public int UpdateStatus(int activityCode, bool isAccessible,bool isBlocked)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateStatusWithStoredProcedure("sp_UpdateActivityStatus", con, activityCode, isAccessible, isBlocked); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateStatusWithStoredProcedure(String spName, SqlConnection con, int activitycode, bool isAccessible, bool isBlocked)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@activitycode", activitycode);
       
        cmd.Parameters.AddWithValue("@isAccessible", isAccessible);
        cmd.Parameters.AddWithValue("@isBlocked", isBlocked);
        return cmd;
    }


    //Employee Log In

    public int EmpLogIn(string username, string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildLogInStoredProcedureCommand(con, "sp_LoginEmployee", username, password);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        int loginStatus=2;
        while (dataReader.Read())
        {
            loginStatus = dataReader.GetInt32(0);
            //Convert.ToInt32(dataReader["loginStatus"])
        }

        if (con != null)
        {
            con.Close();
        }

        return loginStatus;

    }

    SqlCommand buildLogInStoredProcedureCommand(SqlConnection con, string spName, string username, string password)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        return cmd;

    }

}



