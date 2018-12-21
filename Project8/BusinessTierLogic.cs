//
// BusinessTier:  business logic, acting as interface between UI and data store.
//

using System;
using System.Collections.Generic;
using System.Data;


//
/***********************
***Jakub****
* Project8 Final
 * ********/

namespace BusinessTier
{

  //
  // Business:
  //
  public class Business
  {
    //
    // Fields:
    //
    private string _DBFile;
    private DataAccessTier.Data dataTier;


    //
    // Constructor:
    //
    public Business(string DatabaseFilename)
    {
      _DBFile = DatabaseFilename;

      dataTier = new DataAccessTier.Data(DatabaseFilename);
    }


    //
    // TestConnection:
    //
    // Returns true if we can establish a connection to the database, false if not.
    //
    public bool TestConnection()
    {
      return dataTier.TestConnection();
    }


    //
    // GetNamedUser:
    //
    // Retrieves User object based on USER NAME; returns null if user is not
    // found.
    //
    // NOTE: there are "named" users from the Users table, and anonymous users
    // that only exist in the Reviews table.  This function only looks up "named"
    // users from the Users table.
    //
    public User GetNamedUser(string UserName)
    {
            //We must escape the string by using replace, just in case
            UserName = UserName.Replace("'", "''");

            //SQL statement to fetch the data
            string STRINGSQL = string.Format(@"Select *
                                      FROM Users
                                      WHERE  UserName = '{0}';", UserName);

            //The result gets stored back here, though execution of string
            DataSet TheResults = dataTier.ExecuteNonScalarQuery(STRINGSQL);

            //If the result is > 0, then we create a new user object and return the user
            if(TheResults.Tables[0].Rows.Count != 0)
            {
                DataRowCollection AllRows = TheResults.Tables["Table"].Rows;
                DataRow FirstRow = AllRows[0];

                User NewUser = new BusinessTier.User(Convert.ToInt32(FirstRow["UserID"]), Convert.ToString(FirstRow["UserName"]), Convert.ToString(FirstRow["Occupation"]));
                return NewUser;
            }
            else
            {//otherwise return null
                return null;
            }
            //
            // TODO!
            //

            //
            //return null;
    }


    //
    // GetAllNamedUsers:
    //
    // Returns a list of all the users in the Users table ("named" users), sorted 
    // by user name.
    //
    // NOTE: the database also contains lots of "anonymous" users, which this 
    // function does not return.
    //
    public IReadOnlyList<User> GetAllNamedUsers()
    {
            //List creation of users
      List<User> users = new List<User>();

            //SQL statement to retrieve users and order them
            string STRINGSQL = string.Format(@"Select *
                                        FROM Users 
                                        Order BY UserName");

            //The result of execution of SQL string
            DataSet TheResult = dataTier.ExecuteNonScalarQuery(STRINGSQL);

            //FOreach loop to go through all rows, converts userid,name, and occupation and add the user to list
            foreach (DataRow SingleRow in TheResult.Tables["Table"].Rows)
            {
                User AddUser = new BusinessTier.User(Convert.ToInt32(SingleRow["UserID"]), Convert.ToString(SingleRow["UserName"]), Convert.ToString(SingleRow["Occupation"]));
                users.Add(AddUser);
            }
      //
      // TODO!
      //
      //return the users
      return users;
    }


    //
    // GetMovie:
    //
    // Retrieves Movie object based on MOVIE ID; returns null if movie is not
    // found.
    //

     
    public Movie GetMovie(int MovieID)
    {
           // MovieID = MovieID.Replace("'", "''");

            //Using SQL to return movies by movieID
            string STRINGSQL = string.Format(@"Select *
                                               FROM Movies
                                                WHERE MovieID = '{0}';", MovieID);

            //running sql statement against database and storing the result
            DataSet STRINGSQLEXECUTE = dataTier.ExecuteNonScalarQuery(STRINGSQL);
            
            //If its not 0, then we know its nto empty
            if (STRINGSQLEXECUTE.Tables[0].Rows.Count != 0)
            {   //GO throught the rows of the table 
                DataRowCollection collectingRows = STRINGSQLEXECUTE.Tables["Table"].Rows;
                DataRow SingleRow = collectingRows[0];
                //create new movie object
                Movie newMovieObj = new BusinessTier.Movie(MovieID, Convert.ToString(SingleRow["MovieName"]));
                //reutrn object
                return newMovieObj;
            }
            else
            {//otherwise null
                return null;
            }

            //Otherwise 

            // TODO!
            //

          //  return null;      
    }


    //
    // GetMovie:
    //
    // Retrieves Movie object based on MOVIE NAME; returns null if movie is not
    // found.
    //
    public Movie GetMovie(string MovieName)
    {
            MovieName = MovieName.Replace("'", "''");
        //SQL to get data from database
        string STRINGSQL = string.Format(@"Select *
                                            FROM Movies
                                            WHERE MovieName = '{0}';", MovieName);
        //Execute STRING SQL agianst datatier and store it
        DataSet STRINGSQLEXECUTE = dataTier.ExecuteNonScalarQuery(STRINGSQL);

        //create new object for the movie and return it
        if (STRINGSQLEXECUTE.Tables[0].Rows.Count != 0)
        {
                DataRowCollection DataRowsCollection = STRINGSQLEXECUTE.Tables["Table"].Rows;
                DataRow SingleRow = DataRowsCollection[0];

                Movie NewMovieObj = new BusinessTier.Movie(Convert.ToInt32(SingleRow["MovieID"]), MovieName);

                return NewMovieObj;
            }//IF sql fails, return null
        else
        {
            return null;
        }

            //
            // TODO!
            //

            //return null;
    }


    //
    // AddReview:
    //
    // Adds review based on MOVIE ID, returning a Review object containing
    // the review, review's id, etc.  If the add failed, null is returned.
    //
    public Review AddReview(int MovieID, int UserID, int Rating)
    {
            //Runnnig sql against database to inster into the reviews
            string STRINGSQL = string.Format(@"Insert Into Reviews(MovieID, UserID, Rating)
                                             Values ('{0}','{1}', '{2}');
                                              Select ReviewID
                                               FROM Reviews
                                               WHERE ReviewID = SCOPE_IDENTITY();",
                                               MovieID, UserID, Rating);
            object TheResult = dataTier.ExecuteScalarQuery(STRINGSQL);
            //If it is NOT NULL
            if(TheResult != null)
            {   //create the review and return it
                Review NewReview = new BusinessTier.Review(Convert.ToInt32(TheResult), MovieID, UserID, Rating);
                return NewReview;
            }
            else
            {//otherwise return null
                return null;
            }
      //
      // TODO!
      //
      
      //return null;
    }


    //
    // GetMovieDetail:
    //
    // Given a MOVIE ID, returns detailed information about this movie --- all
    // the reviews, the total number of reviews, average rating, etc.  If the 
    // movie cannot be found, null is returned.
    //
    public MovieDetail GetMovieDetail(int MovieID)
    {
            //Running SQL to execute against database
            string SQLSTRING1 = string.Format(@"Select *
                                               FROM Movies
                                                WHERE MovieID = '{0}';",
                                                MovieID);
            //Storing result that returns table
            DataSet TheResult = dataTier.ExecuteNonScalarQuery(SQLSTRING1);

            
            //If table isn't 0
            if (TheResult.Tables[0].Rows.Count != 0)
            {
                //Getting the rows of the tables
                DataRowCollection FirstRow = TheResult.Tables["Table"].Rows;
                //Getting single Rows
                DataRow SingleRow = FirstRow[0];

                //Creating a new movie
                Movie NewMovie = new Movie(MovieID, Convert.ToString(SingleRow["MovieName"]));
                //Executing SQL against databse
                string SQLSTRING2 = string.Format(@"Select ROUND(AVG(CONVERT(float, Rating)), 4)
                                                  FROM Reviews
                                                  WHERE MovieID = '{0}';", MovieID);

                //Creating new object for rating
                object CreateAvgRating = dataTier.ExecuteScalarQuery(SQLSTRING2);
                
                //
                if (CreateAvgRating.Equals(0)) { return null; }

                string SQLSTRING3 = string.Format(@"Select Count(*) as RevTotals 
                                                  FROM Reviews
                                                  WHERE MovieID = '{0}';", MovieID);
                object CreateTotRev = dataTier.ExecuteScalarQuery(SQLSTRING3);
                
                //If table isn't 0
                if (CreateTotRev.Equals(0)) { return null; }
                //Running SQL against the database
                string SQLSTRING4 = string.Format(@"Select * 
                                                  FROM Reviews 
                                                  WHERE MovieID = '{0}' 
                                                  Order by Rating DESC, UserID; 
                                                  ", MovieID);
                DataSet CreateAllRevs = dataTier.ExecuteNonScalarQuery(SQLSTRING4);

                if (CreateAllRevs.Equals(0)) { return null; }

                //Creating a list of reviews
                List<Review> THEReviews = new List<Review>();

                
                //Foreach loop to go throught rows in the tables
                foreach (DataRow secondRow in CreateAllRevs.Tables["Table"].Rows)
                {
                    //Creating review
                    Review AddRev = new BusinessTier.Review(Convert.ToInt32(secondRow["ReviewID"]), MovieID, Convert.ToInt32(secondRow["UserID"]), Convert.ToInt32(secondRow["Rating"]));
                    //adding review
                    THEReviews.Add(AddRev);

                }
               //Creating movie detail and returning it
                MovieDetail movie = new BusinessTier.MovieDetail(NewMovie, Convert.ToDouble(CreateAvgRating), Convert.ToInt32(CreateTotRev), THEReviews);
                    return movie;
            }
            else
            {//Else null
                return null;
            }
        }


    //
    // GetUserDetail:
    //
    // Given a USER ID, returns detailed information about this user --- all
    // the reviews submitted by this user, the total number of reviews, average 
    // rating given, etc.  If the user cannot be found, null is returned.
    //
    public UserDetail GetUserDetail(int UserID)
    {
            //Creating string to execute against the database
            string SQLSTRING = string.Format(@"Select * 
                                             FROM Users 
                                             WHERE UserID = '{0}';", UserID);

            //The resulsts get stored into variable
            DataSet fINDINGUSER = dataTier.ExecuteNonScalarQuery(SQLSTRING);

            //If it doesnt return 0 then
            if (fINDINGUSER.Tables[0].Rows.Count != 0)
            {
                //Getting ROws from tables
                DataRowCollection EachRow = fINDINGUSER.Tables["Table"].Rows;
                DataRow SingleRow = EachRow[0]; //Getting each row

                //Adding user to the businessterr.User
                User AddingNewUser = new BusinessTier.User(UserID, Convert.ToString(SingleRow["UserName"]), Convert.ToString(SingleRow["Occupation"]));

                //Running another string against database 
                string STRINGSQL2 = string.Format(@"Select ROUND(AVG(CONVERT(float,Rating)),4) 
                                                  FROM Reviews 
                                                  WHERE UserID = '{0}';", UserID);

                //store object into TheAvgRate
                object TheAvgRate = dataTier.ExecuteScalarQuery(STRINGSQL2);
                //check and return null if 0
                if (TheAvgRate.Equals(0)) { return null;  }
                //StringSQL satetment to run against database
                string STRINGSQL3 = string.Format(@"Select Count(*) 
                                                    AS totalReview 
                                                    FROM Reviews 
                                                    WHERE UserID = '{0}';", UserID);
                //store object into NumTotRev
                object NumTotRev = dataTier.ExecuteScalarQuery(STRINGSQL3);
                //check and return null if 0
                if (NumTotRev.Equals(0)) { return null;  }

                //StringSQL satetment to run against database
                string sql4 = string.Format(@"Select * FROM Reviews, Movies 
                                            WHERE Reviews.MovieID = Movies.MovieID AND 
                                            UserID = '{0}' 
                                            ORDER BY Movies.MovieName, Reviews.Rating 
                                            DESC ;", UserID);

                //store object into AllToTalRev
                DataSet AllToTalRev = dataTier.ExecuteNonScalarQuery(sql4);

                //check and return null if 0
                if (AllToTalRev.Equals(0)) { return null;  }

                //CReating a list of reviews
                List<Review> reviews = new List<Review>();

                //foreach loop to go through all this
                foreach (DataRow EveryRow in AllToTalRev.Tables["Table"].Rows)
                {
                    //Creating review to add
                    Review AddingNewRev = new BusinessTier.Review(Convert.ToInt32(EveryRow["ReviewID"]), Convert.ToInt32(EveryRow["MovieID"]), UserID, Convert.ToInt32(EveryRow["Rating"]));
                    //add review
                    reviews.Add(AddingNewRev);
                }
                //Creating User
                UserDetail NewUser = new BusinessTier.UserDetail(AddingNewUser, Convert.ToDouble(TheAvgRate), Convert.ToInt32(NumTotRev), reviews);

                //Returing User
                return NewUser;
                
            }
            else //get user details, avg rating, number of reviews, and list of reviews and return UserDetail object
            {
                //return null;
                return null;
            }

            //return null;
    }


    //
    // GetTopMoviesByAvgRating:
    //
    // Returns the top N movies in descending order by average rating.  If two
    // movies have the same rating, the movies are presented in ascending order
    // by name.  If N < 1, an EMPTY LIST is returned.
    //
    public IReadOnlyList<Movie> GetTopMoviesByAvgRating(int N)
    {
            //Create list of movies
      List<Movie> movies = new List<Movie>();
            //Runnning sql agaianst database to fetch the top N movies
            string SQLSTRING = string.Format(@"Select TOP {0} MovieName, ROUND(AVG(CONVERT(float, Rating)), 4) as Averagee
                                             FROM Reviews, Movies
                                             WHERE Reviews.MovieID = Movies.MovieID
                                             GROUP BY Movies.MovieName
                                             ORDER BY  Averagee DESC, MovieName ASC;",Convert.ToInt32(N));
            //Execeuting the string against database
            DataSet TheResult = dataTier.ExecuteNonScalarQuery(SQLSTRING);

            //If its not 0 or if N>= 1 then we go through the table
            if(TheResult.Tables[0].Rows.Count != 0 || N >= 1)
            {
                foreach(DataRow EachRow in TheResult.Tables["Table"].Rows)
                {
                    EachRow["MovieName"] = Convert.ToString(EachRow["MovieName"]).Replace("'", "''");
                    //run another sql to select movieID
                    string SQLSTRING2 = string.Format(@"Select MovieID
                                                      FROM Movies
                                                      WHERE MovieName = '{0}';", Convert.ToString(EachRow["MovieName"]));
                    //create movieid obj and execute string
                    object MovieID = dataTier.ExecuteScalarQuery(SQLSTRING2);
                    //then ccreate full obj to addmovie
                    Movie AddMovie = new BusinessTier.Movie(Convert.ToInt32(MovieID), Convert.ToString(EachRow["MovieName"]));
                    movies.Add(AddMovie); //adds movie
                }
            }
            else
            {//once  we are finish we return the obj back to the user
                return movies;
            }
      //
      // TODO!
      //

      return movies;
    }


  }//class
}//namespace
