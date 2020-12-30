using System;
using System.Data;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SOA_DB_MANAGER
{
    class Program
    {
        // Globalization Variables
        public class Globals
        {
            public static string connStr = "server=localhost;user=root;database=SOA;port=3306;password="; //Change As You Want
            public static string selectedMethod = "";
            public static string sqlQuery = "";
            public static string idType = "";
            public static int versionFisrstNumber = 0; // Before Dot
            public static int versionSecondNumber = 1; // After Dot

            public static string welcomeMessage = $"Welcome to SOA DB MANAGER VER {Globals.versionFisrstNumber}.{Globals.versionSecondNumber} \n\n" +
                "Please Select a Valid option: \n" +
                "";
        }
        private static bool checkTable(string ToCheck)
        {
            string[] tabels = { "answers", "questions", "departments", "tickets", "users" };
            if (tabels.Any(ToCheck.Contains))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void detectMethod(string[] transArg)
        {
            try
            {
                // Detecting Method
                if (transArg.Length > 0)
                {
                    if (transArg[0] == "get")
                    {
                        Globals.selectedMethod = "get";
                    }
                    else if (transArg[0] == "set")
                    {
                        Globals.selectedMethod = "set";
                    }
                    else if (transArg[0] == "getone")
                    {
                        Globals.selectedMethod = "getone";
                    }
                    else if (transArg[0] == "list")
                    {
                        Globals.selectedMethod = "list";
                    }
                    else
                    {
                        Globals.selectedMethod = "none";
                        Console.WriteLine("Please select: set, get, getone or list");
                        System.Environment.Exit(1);
                    }
                }
                else
                {
                    Globals.selectedMethod = "none";
                    Console.WriteLine(Globals.welcomeMessage);
                    System.Environment.Exit(1);
                }
            }
            catch
            {
                Console.WriteLine(Globals.welcomeMessage);
                System.Environment.Exit(1);
            }
        }
        private static void listMethod()
        {
            Globals.sqlQuery = "SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_SCHEMA='soa'";
        }
        private static void getMethod(string[] transArg)
        {
            try
            {
                // Detecting Second Argument
                if (transArg.Length == 2 && checkTable(transArg[1]))
                {
                    Globals.sqlQuery = $"SELECT * FROM {transArg[1]}";
                }
                else
                {
                    Console.WriteLine("get need only one correct argument, check list");
                    System.Environment.Exit(1);
                }
            }
            catch
            {
                Console.WriteLine(Globals.welcomeMessage);
                System.Environment.Exit(1);
            }
        }
        private static void idName(string tableName)
        {
            if (tableName == "users")
            {
                Globals.idType = "id";
            }
            else if (tableName == "tickets")
            {
                Globals.idType = "tktID";
            }
            else if (tableName == "questions")
            {
                Globals.idType = "quesId";
            }
            else if (tableName == "departments")
            {
                Globals.idType = "depID";
            }
            else if (tableName == "answers")
            {
                Globals.idType = "ansID";
            }

        }
        private static void getOneMethod(string[] transArg)
        {
            try
            {
                // Detecting Second Argument
                if (transArg.Length == 3 && checkTable(transArg[1]))
                {
                    idName(transArg[1]);
                    Globals.sqlQuery = $"SELECT * FROM {transArg[1]} WHERE {Globals.idType} = {transArg[2]}";
                }
                else
                {
                    Console.WriteLine("getOne needs two correct argument, check list");
                    System.Environment.Exit(1);
                }
            }
            catch
            {
                Console.WriteLine(Globals.welcomeMessage);
                System.Environment.Exit(1);
            }
        }

        private static void setMethod(string[] transArg)
        {
            try
            {
                // Detecting Second Argument
                if (transArg.Length >= 2 && checkTable(transArg[1]))
                {

                    if (transArg[1] == "tickets")
                    {
                        try
                        {
                            Globals.sqlQuery = $"INSERT INTO tickets(tktID, tktTitle, tktText, tktState, tktSender, tktOwner) VALUES ({transArg[2]},'{transArg[3]}' ,'{transArg[4]}',{transArg[5]},{transArg[6]},{transArg[7]})";
                        }
                        catch
                        {
                            Console.WriteLine("You need 7 arguments");
                            System.Environment.Exit(1);
                        }
                    }

                    if (transArg[1] == "users")
                    {
                        try
                        {
                            Globals.sqlQuery = $"INSERT INTO users(id, name, realName, passWord, depID) VALUES({transArg[2]},'{transArg[3]}','{transArg[4]}','{transArg[5]}',{transArg[6]})";

                        }
                        catch
                        {
                            Console.WriteLine("You need 7 arguments");
                            System.Environment.Exit(1);
                        }
                    }

                    if (transArg[1] == "questions")
                    {
                        try
                        {
                            Globals.sqlQuery = $"INSERT INTO questions(quesId, quesType, quesText, questOwner, ansID) VALUES({ transArg[2]},{ transArg[3]},'{ transArg[4]}',{ transArg[5]},{ transArg[6]})";
                        }
                        catch
                        {
                            Console.WriteLine("You need 6 arguments");
                            System.Environment.Exit(1);
                        }
                    }

                    if (transArg[1] == "departments")
                    {
                        try
                        {
                            Globals.sqlQuery = $"INSERT INTO departments(depID, depName) VALUES({ transArg[2]}, '{ transArg[3]}')";
                        }
                        catch
                        {
                            Console.WriteLine("You need 3 arguments");
                            System.Environment.Exit(1);
                        }
                    }

                    if (transArg[1] == "answers")
                    {
                        try
                        {
                            Globals.sqlQuery = $"INSERT INTO answers(ansID, ansType, nxtQuesID, ansSer, ansText) VALUES({ transArg[2]}, { transArg[3]}, { transArg[4]}, '{ transArg[5]}', '{ transArg[6]}')";
                        }
                        catch
                        {
                            Console.WriteLine("You need 6 arguments");
                            System.Environment.Exit(1);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("set need only one correct argument, check list");
                    System.Environment.Exit(1);
                }
            }
            catch
            {
                Console.WriteLine(Globals.welcomeMessage);
                System.Environment.Exit(1);
            }
        }
        static void Main(string[] args)
        {
            // ConnectionLine
            MySqlConnection conn = new MySqlConnection(Globals.connStr);
            detectMethod(args);
            if (Globals.selectedMethod == "list")
            {
                listMethod();
            }
            else if (Globals.selectedMethod == "get")
            {
                getMethod(args);
            }
            else if (Globals.selectedMethod == "getone")
            {
                getOneMethod(args);
            }
            else if (Globals.selectedMethod == "set")
            {
                setMethod(args);
            }
            else
            {
                Console.Write("wrong option");
                System.Environment.Exit(1);
            }

            try
            {
                conn.Open();
                // Number of Column
                int NoOfCol = 0;
                if(Globals.selectedMethod == "get" || Globals.selectedMethod == "getone")
                {
                    string SqlChkNo = $"SELECT count(*) AS NUMBEROFCOLUMNS FROM information_schema.columns WHERE table_schema = 'soa' AND table_name = '{args[1]}'";
                    MySqlCommand ChkNo = new MySqlCommand(SqlChkNo, conn);
                    MySqlDataReader ChkRdr = ChkNo.ExecuteReader();
                    ChkRdr.Read();
                    NoOfCol = Int32.Parse(ChkRdr[0].ToString());
                    ChkRdr.Close();
                }

                // Excute Query
                MySqlCommand cmd = new MySqlCommand(Globals.sqlQuery, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (Globals.selectedMethod == "list")
                    {
                        Console.WriteLine(rdr[0]);
                    }
                    else if (Globals.selectedMethod == "get" || Globals.selectedMethod == "getone")
                    {
                        foreach (int value in Enumerable.Range(0, NoOfCol))
                        {
                            string prinT = "";
                            string word = rdr[value].ToString();
                            if (value != NoOfCol - 1)
                            {
                                prinT = $"{word} -- ";
                            }
                            else
                            {
                                prinT = $"{word}";
                            }
                            Console.Write(prinT);
                        }
                        Console.WriteLine();
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }
    }
}