using System;
using DBGen.Data;
using System.Data.SQLite;

namespace DBGen {
	class DataBase {
		static readonly string[] tableCreators = {
			"CREATE TABLE IF NOT EXISTS main_cards (card_number TEXT NOT NULL PRIMARY KEY, name TEXT NOT NULL, card_limit INTEGER NOT NULL, rarity TEXT NOT NULL);",

			"CREATE TABLE IF NOT EXISTS card_types (card_type TEXT NOT NULL PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS card_is_type (card_number TEXT PRIMARY KEY, card_type TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES main_cards(card_number), FOREIGN KEY(card_type) REFERENCES card_types(card_type));",

			"CREATE TABLE IF NOT EXISTS card_groups (card_group TEXT PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS card_has_group (card_number TEXT PRIMARY KEY, card_group TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES main_cards(card_number), FOREIGN KEY(card_group) REFERENCES card_groups(card_group));",

			"CREATE TABLE IF NOT EXISTS vivosaur_charge_skills (id INTEGER PRIMARY KEY, effect TEXT NOT NULL);",
			"CREATE TABLE IF NOT EXISTS abilities (id INTEGER PRIMARY KEY, name TEXT NOT NULL, effect TEXT NOT NULL);",
			"CREATE TABLE IF NOT EXISTS skills (id INTEGER PRIMARY KEY, name TEXT NOT NULL, cost INTEGER NOT NULL, damage INTEGER, effect TEXT);",

			"CREATE TABLE IF NOT EXISTS vivosaur_cards (card_number TEXT PRIMARY KEY, lp INTEGER NOT NULL, fp INTEGER NOT NULL, charge_skill INTEGER, ability INTEGER, skill1 INTEGER NOT NULL, skill2 INTEGER, " +
				"FOREIGN KEY(card_number) REFERENCES main_cards(card_number), FOREIGN KEY(charge_skill) REFERENCES vivosaur_charge_skills(id), FOREIGN KEY(ability) REFERENCES abilities(id), FOREIGN KEY(skill1) REFERENCES skills(id), FOREIGN KEY(skill2) REFERENCES skills(id));",

			"CREATE TABLE IF NOT EXISTS vivosaur_ranges (range TEXT PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS vivosaur_elements (element TEXT PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS vivosaur_has_range (card_number TEXT, range TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES vivosaur_cards(card_number), FOREIGN KEY(range) REFERENCES vivosaur_ranges(range));",
			"CREATE TABLE IF NOT EXISTS vivosaur_has_element (card_number TEXT, element TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES vivosaur_cards(card_number), FOREIGN KEY(element) REFERENCES vivosaur_elements(element));",

			"CREATE TABLE IF NOT EXISTS action_cards (card_number TEXT NOT NULL PRIMARY KEY, cost INTEGER NOT NULL, effect TEXT NOT NULL, " +
				"FOREIGN KEY(card_number) REFERENCES main_cards(card_number));",

			"CREATE TABLE IF NOT EXISTS action_types (action_type TEXT NOT NULL PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS action_is_type (card_number TEXT PRIMARY KEY, action_type TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES action_cards(card_number), FOREIGN KEY(action_type) REFERENCES action_types(action_type));",

			"CREATE TABLE IF NOT EXISTS vivosaur_effects (effect TEXT NOT NULL PRIMARY KEY);",
			"CREATE TABLE IF NOT EXISTS vivosaur_has_effect (card_number TEXT, effect TEXT, " +
				"FOREIGN KEY(card_number) REFERENCES vivosaur_cards(card_number), FOREIGN KEY(effect) REFERENCES vivosaur_effects(effect));",

			"CREATE TABLE IF NOT EXISTS card_images (card_number TEXT PRIMARY KEY, image BLOB NOT NULL UNIQUE, " +
				"FOREIGN KEY(card_number) REFERENCES main_cards(card_number));"
		};

		public static void CreateTables(SQLiteConnection connection) {
			Console.WriteLine("Creating Tables");
			foreach(string sql in tableCreators) {
				using SQLiteCommand command = new SQLiteCommand(sql, connection);
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public static void InsertEnums(SQLiteConnection connection) {
			Console.WriteLine("Inserting Enums");
			using SQLiteCommand command = new SQLiteCommand("", connection);

			command.CommandText = "INSERT INTO card_types VALUES (@card_type)";
			foreach (Object cardType in typeof(CardType).GetEnumValues()) {
				command.Parameters.AddWithValue("@card_type", cardType.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}

			command.CommandText = "INSERT INTO card_groups VALUES (@group)";
			foreach(Object group in typeof(Group).GetEnumValues()) {
				command.Parameters.AddWithValue("@group", group.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}

			command.CommandText = "INSERT INTO vivosaur_ranges VALUES (@range)";
			foreach(Object range in typeof(Data.Range).GetEnumValues()) {
				command.Parameters.AddWithValue("@range", range.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}

			command.CommandText = "INSERT INTO vivosaur_elements VALUES (@element)";
			foreach (Object element in typeof(Element).GetEnumValues()) {
				command.Parameters.AddWithValue("@element", element.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}

			command.CommandText = "INSERT INTO vivosaur_effects VALUES (@effect)";
			foreach (Object effect in typeof(Effect).GetEnumValues()) {
				command.Parameters.AddWithValue("@effect", effect.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}

			command.CommandText = "INSERT INTO action_types VALUES (@action_type)";
			foreach (Object actionType in typeof(ActionType).GetEnumValues()) {
				command.Parameters.AddWithValue("@action_type", actionType.ToString());
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public static void InsertCard(SQLiteConnection connection, Card card) {
			string cardNumber = card.Number;

			Console.WriteLine("Inserting Card: " + cardNumber + " - '" + card.Name + "'");

			using SQLiteCommand command = new SQLiteCommand("INSERT INTO main_cards VALUES (@card_number, @name, @card_limit, @rarity);", connection);
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@name", card.Name);
			command.Parameters.AddWithValue("@card_limit", card.Limit);
			command.Parameters.AddWithValue("@rarity", card.Rarity);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			if (card.Group != null) {
				command.CommandText = "INSERT INTO card_has_group VALUES (@card_number, @card_group);";
				command.Parameters.AddWithValue("@card_number", cardNumber);
				command.Parameters.AddWithValue("@card_group", card.Group.ToString());
				command.ExecuteNonQuery();
			}

			byte[] imageData = File.ReadAllBytes(card.ImagePath);
			command.CommandText = "INSERT INTO card_images VALUES (@card_number, @image)";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@image", imageData);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();
		}

		public static void InsertVivosaurCard(SQLiteConnection connection, Vivosaur vivosaur) {
			string cardNumber = vivosaur.Number;

			InsertCard(connection, vivosaur);
			using SQLiteCommand command = new SQLiteCommand("INSERT INTO card_is_type VALUES (@card_number, @card_type);", connection);
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@card_type", CardType.Vivosaur.ToString());
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			long? chargeSkillId = null;
			if (vivosaur.ChargeSkill != null) {
				command.CommandText = "INSERT INTO vivosaur_charge_skills (effect) VALUES (@effect);";
				command.Parameters.AddWithValue("@effect", vivosaur.ChargeSkill.Effect);
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
				chargeSkillId = connection.LastInsertRowId;
			}

			command.CommandText = "INSERT INTO abilities (name, effect) VALUES (@name, @effect);";
			command.Parameters.AddWithValue("@name", vivosaur.Ability.Name);
			command.Parameters.AddWithValue("@effect", vivosaur.Ability.Effect);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();
			long abilityId = connection.LastInsertRowId;

			command.CommandText = "INSERT INTO skills (name, cost, damage, effect) VALUES (@name, @cost, @damage, @effect);";
			command.Parameters.AddWithValue("@name", vivosaur.Skill1.Name);
			command.Parameters.AddWithValue("@cost", vivosaur.Skill1.Cost);
			command.Parameters.AddWithValue("@damage", vivosaur.Skill1.Damage);
			command.Parameters.AddWithValue("@effect", vivosaur.Skill1.Effect);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();
			long skill1Id = connection.LastInsertRowId;

			long? skill2Id = null;
			if (vivosaur.Skill2 != null) {
				command.Parameters.AddWithValue("@name", vivosaur.Skill2.Name);
				command.Parameters.AddWithValue("@cost", vivosaur.Skill2.Cost);
				command.Parameters.AddWithValue("@damage", vivosaur.Skill2.Damage);
				command.Parameters.AddWithValue("@effect", vivosaur.Skill2.Effect);
				//Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
				skill2Id = connection.LastInsertRowId;
			}

			command.CommandText = "INSERT INTO vivosaur_cards VALUES (@card_number, @lp, @fp, @charge_skill, @ability, @skill1, @skill2);";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@lp", vivosaur.LP);
			command.Parameters.AddWithValue("@fp", vivosaur.FP);
			command.Parameters.AddWithValue("@charge_skill", chargeSkillId);
			command.Parameters.AddWithValue("@ability", abilityId);
			command.Parameters.AddWithValue("@skill1", skill1Id);
			command.Parameters.AddWithValue("@skill2", skill2Id);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO vivosaur_has_range VALUES (@card_number, @range);";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@range", vivosaur.Range.ToString());
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO vivosaur_has_element VALUES (@card_number, @element);";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@element", vivosaur.Element.ToString());
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO vivosaur_has_effect VALUES (@card_number, @effect);";
			if (vivosaur.Effects.Any()) {
				foreach(Effect effect in vivosaur.Effects) {
					command.Parameters.AddWithValue("@card_number", cardNumber);
					command.Parameters.AddWithValue("@effect", effect.ToString());
					//Console.WriteLine(command.CommandText);
					command.ExecuteNonQuery();
				}
			}
		}

		public static void InsertActionCard(SQLiteConnection connection, Data.Action action) {
			InsertCard(connection, action);

			string cardNumber = action.Number;

			using SQLiteCommand command = new SQLiteCommand("INSERT INTO card_is_type VALUES (@card_number, @card_type);", connection);
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@card_type", CardType.Action.ToString());
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO action_cards VALUES (@card_number, @cost, @effect);";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@cost", action.Cost);
			command.Parameters.AddWithValue("@effect", action.Effect);
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO action_is_type VALUES (@card_number, @action_type);";
			command.Parameters.AddWithValue("@card_number", cardNumber);
			command.Parameters.AddWithValue("@action_type", action.ActionType.ToString());
			//Console.WriteLine(command.CommandText);
			command.ExecuteNonQuery();
		}
	}
}
