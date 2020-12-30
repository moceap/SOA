using System;
using System.Diagnostics;

namespace SOA
{
    class Program
    {
        public class Department
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string RealName { get; set; }
            public string Password { get; set; }
            public int DepId { get; set; }
        }

        public class Ticket
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public int State { get; set; }
            public int SenderId { get; set; }
            public int OwnerId { get; set; }
        }

        public class Question
        {
            public int Id { get; set; }
            public int Type { get; set; }
            public string Text { get; set; }
            public int OwnerId { get; set; }
            public int AnswerId { get; set; }
        }

        public class Answer
        {
            public int Id { get; set; }
            public int Type { get; set; }
            public int[] NextQuestionId { get; set; }
            public string[] AnswerSeries { get; set; }
            public string[] Text { get; set; }
        }
        private static Answer getAnswerById(int id)
        {
            var tempAnswer = new Answer();
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet.exe",
                    Arguments = $"SOA_DB_MANAGER.dll getone answers {id}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string line = "";
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
            }
            tempAnswer.Id = id;
            string[] tempAnswerArray = line.Split(" -- ");
            string[] tempNxtQuesArray = tempAnswerArray[2].Split("|");
            tempAnswer.NextQuestionId = Array.ConvertAll<string, int>(tempNxtQuesArray, int.Parse);
            tempAnswer.AnswerSeries = tempAnswerArray[3].Split("|");
            tempAnswer.Text = tempAnswerArray[4].Split("|");
            return tempAnswer;
        }
        private static Question getQuestionById(int id)
        {
            var tempQuestion = new Question();
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet.exe",
                    Arguments = $"SOA_DB_MANAGER.dll getone questions {id}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string line = "";
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
            }
            string[] tempQuestionArray = line.Split(" -- ");
            tempQuestion.Id = id;
            tempQuestion.Text = tempQuestionArray[2];
            tempQuestion.AnswerId = Convert.ToInt32(tempQuestionArray[4]);

            return tempQuestion;
        }
        static void Main(string[] args)
        {
            // Defining Initiate Values
            try
            {
                var CurrentQues = getQuestionById(Convert.ToInt32(args[0]));
                var CurrentAns = getAnswerById(CurrentQues.AnswerId);

                    Console.WriteLine(CurrentQues.Text + $"'<br><br><button onclick=\"window.location.href=\'http://127.0.0.1/soa/chatbot.php?id={CurrentAns.NextQuestionId[0]}\'\" class=\"button\">{CurrentAns.AnswerSeries[0]}</button>"
    + "<br>" + $"'<br><br><button onclick=\"window.location.href=\'http://127.0.0.1/soa/chatbot.php?id={CurrentAns.NextQuestionId[1]}\'\" class=\"button\">{CurrentAns.AnswerSeries[1]}</button>"
    + CurrentAns.Text[1]);

            }
            catch
            {
                Console.WriteLine("SOA need argument");
            }

        }
    }
}
