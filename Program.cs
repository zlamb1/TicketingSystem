using System;
using System.IO;
using System.Collections.Generic;

namespace TicketingSystem
{
    class Ticket
    {
        public Ticket(int id, string summary, string status, string priority, string submitter, string assigned, List<string> watchers)
        {
            this.id = id;
            this.summary = summary;
            this.status = status;
            this.priority = priority;
            this.submitter = submitter;
            this.assigned = assigned;
            this.watchers = watchers;
        }
        private int id;
        private string summary, status, priority, submitter, assigned;
        private List<string> watchers;
        public override string ToString()
        {
            string line = id + "," + summary + ","
            + status + "," + priority + ","
            + submitter + "," + assigned + ",";
            for (int wIndex = 0; wIndex < watchers.Count; wIndex++)
            {
                line += watchers[wIndex];
                if (wIndex != watchers.Count - 1) line += "|";
            }
            return line;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int option = MenuOptions();
                if (option == 0) continue;
                if (option == 1) ReadFromFile();
                if (option == 2) CreateData();
                break;
            }

        }
        static int MenuOptions()
        {
            Console.WriteLine("Choose Option:");
            Console.WriteLine("1. Read tickets from a file.");
            Console.WriteLine("2. Create file from ticket data.");
            string option = GetInput("Enter Choice: ");
            switch (option)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                default:
                    return 0;
            }
        }
        static void ReadFromFile()
        {
            while (true)
            {
                string file = GetInput("Enter a file name: ");
                if (File.Exists(file))
                {
                    StreamReader reader = new StreamReader(file);
                    int i = 0;
                    Console.WriteLine("Tickets: ");
                    while (!reader.EndOfStream)
                    {
                        if (i != 0) Console.WriteLine(reader.ReadLine());
                        i++;
                    }
                    reader.Close();
                }
                else
                {
                    Console.WriteLine("Could not find that file!");
                    continue;
                }
                break;
            }
        }
        static void CreateData()
        {
            List<Ticket> tickets = new List<Ticket>();
            while (true)
            {
                int id;
                while (true)
                {
                    if (int.TryParse(GetInput("Enter Ticket ID: "), out id)) break;
                    else
                    {
                        Console.WriteLine("That is not a valid number.");
                        continue;
                    }
                }

                List<string> watchers = new List<string>();
                while (true)
                {
                    watchers.Add(GetInput("Enter Watcher: "));
                    string resp1 = GetInput("Would you like to enter another watcher (Y/N)? ").ToUpper();
                    if (resp1 == "Y") continue;
                    else break;
                }
                tickets.Add(new Ticket(id, GetInput("Enter Summary: "),
                    GetInput("Enter Status: "), GetInput("Enter Priority: "),
                    GetInput("Enter Submitter: "), GetInput("Enter Assigned: "),
                    watchers));
                string resp = GetInput("Would you like to enter another ticket (Y/N)? ").ToUpper();
                if (resp == "Y") continue;
                else break;
            }
            string path = GetInput("Enter a file name: ");

            FileStream fs;
            if (!File.Exists(path))
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            else fs = File.Create(path);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine("TicketID,Summary,Status,Priority,Submitter,Assigned,Watching");
            for (int index = 0; index < tickets.Count; index++)
            {
                writer.WriteLine(tickets[index]);
            }
            writer.Flush();
            writer.Close();
            fs.Close();
        }
        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}