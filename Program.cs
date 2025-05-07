using DBGen.Data;
using System;
using System.Data.SQLite;

namespace DBGen {
	internal class Program {
		static void Main(string[] args) {
			if (args.Length == 2) {
				try {
					using SQLiteConnection connection = new SQLiteConnection("Data Source=" + args[1]);
					connection.Open();
					Console.WriteLine("SQLite connection opened.");

					DataBase.CreateTables(connection);
					Console.WriteLine("Database tables created successfully.");
					DataBase.InsertEnums(connection);
					Console.WriteLine("Enumeration tables filled successfully.");

					string jsonString = File.ReadAllText(args[0]);
					Json.ReadData(args[0], jsonString, connection);
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			} else {
				Console.WriteLine("Incorrect number of arguments. Try again.");
			}
		}
	}
}