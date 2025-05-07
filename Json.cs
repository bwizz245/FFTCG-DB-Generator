using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DBGen.Data;
using System.Data.SQLite;

namespace DBGen {
	class Json {
		public static void ReadData(string jsonPath, string jsonString, SQLiteConnection connection) {
			using JsonDocument doc = JsonDocument.Parse(jsonString);
			JsonElement root = doc.RootElement;
			foreach(JsonProperty cardObject in root.EnumerateObject()) {
				JsonElement card = cardObject.Value;
				
				string cardNumber = cardObject.Name;
				string name;
				int limit;
				string rarity;
				Group? group = null;
				string imagePath;

				CardType type;

				try {
					name = card.GetProperty("name").GetString();
					rarity = card.GetProperty("rarity").GetString();
					imagePath = card.GetProperty("image").GetString();

					if (name == null) {
						throw new Exception("Property \"name\" is not defined.");
					}

					if (rarity == null) {
						throw new Exception("Property \"rarity\" is not defined.");
					}
					
					if (card.TryGetProperty("limit", out JsonElement limitElement)) {
						limit = limitElement.GetInt32();
					} else {
						limit = 3;
					}

					if (card.TryGetProperty("group", out JsonElement groupElement)) {
						string groupString = groupElement.GetString();
						group = Enums.GetGroup(groupString);
					}

					if (imagePath == null) {
						throw new Exception("Property \"image\" is not defined.");
					} else {
						imagePath = Path.Combine(Path.GetDirectoryName(jsonPath), imagePath);
					}

						type = (CardType)Enum.Parse(typeof(CardType), card.GetProperty("type").GetString());
				} catch (Exception e) {
					Console.WriteLine("Exception in object \"" + cardNumber.ToString() + "\": " + e.Message);
					continue;
				}

				try {
					switch (type) {
						case CardType.Vivosaur:
							if (group == null) {
								throw new Exception("Vivosaurs must belong to a group.");
							} else {
								Vivosaur vivosaur = ConvertVivosaur(card, cardNumber, name, limit, rarity, (Group)group, imagePath);
								DataBase.InsertVivosaurCard(connection, vivosaur);
							}
							break;
						case CardType.Action:
							Data.Action action = ConvertAction(card, cardNumber, name, limit, rarity, group, imagePath);
							DataBase.InsertActionCard(connection, action);
							break;
					}
				} catch (Exception e) {
					Console.WriteLine("Exception in object \"" + cardNumber.ToString() + "\": " + e.Message);
					Console.WriteLine(e.StackTrace);
					continue;
				}
			}
		}

		private static Vivosaur ConvertVivosaur(JsonElement card, string cardNumber, string name, int limit, string rarity, Group group, string imagePath) {
			int lp = card.GetProperty("lp").GetInt32();
			int fp = card.GetProperty("fp").GetInt32();
			ChargeSkill? chargeSkill = null;
			Ability ability;
			Skill skill1;
			Skill? skill2 = null;

			Data.Range range = (Data.Range)Enum.Parse(typeof(Data.Range), card.GetProperty("range").GetString());
			Element element = (Element)Enum.Parse(typeof(Element), card.GetProperty("element").GetString());

			List<Effect> effects = new List<Effect>();
			if (card.TryGetProperty("effects", out JsonElement effectElement)) {
				foreach (JsonElement effect in effectElement.EnumerateArray()) {
					Effect eff = Enums.GetEffect(effect.GetString());
					effects.Add(eff);
				}
			}

			if (card.TryGetProperty("charge-skill", out JsonElement chargeSkillElement)) {
				chargeSkill = new ChargeSkill(chargeSkillElement.GetString());
			}

			JsonElement abilityElement = card.GetProperty("ability");
			ability = new Ability(
				abilityElement.GetProperty("name").GetString(),
				abilityElement.GetProperty("effect").GetString()
				);

			JsonElement skill1Element = card.GetProperty("skill-1");
			skill1 = ConvertSkill(skill1Element);

			if (card.TryGetProperty("skill-2", out JsonElement skill2Element)) {
				skill2 = ConvertSkill(skill2Element);
			}

			return new Vivosaur(
				cardNumber,
				name,
				limit,
				rarity,
				group,
				imagePath,
				lp,
				fp,
				chargeSkill,
				ability,
				skill1,
				skill2,
				range,
				element,
				effects
				);
		}

		private static Data.Action ConvertAction(JsonElement card, string cardNumber, string name, int limit, string rarity, Group? group, string imagePath) {
			int cost = card.GetProperty("cost").GetInt32();
			string effect = card.GetProperty("effect").GetString();
			ActionType type = (ActionType)Enum.Parse(typeof(ActionType), card.GetProperty("action-type").GetString());

			return new Data.Action(
				cardNumber,
				name,
				limit,
				rarity,
				group,
				imagePath,
				cost,
				effect,
				type
				);
		}

		private static Skill ConvertSkill(JsonElement skill) {
			string name = skill.GetProperty("name").GetString();
			int cost = skill.GetProperty("cost").GetInt32();
			int? damage = null;
			string? effect = null;

			JsonElement damageElement;
			if (skill.TryGetProperty("damage", out damageElement)) {
				damage = damageElement.GetInt32();
			}

			JsonElement effectElement;
			if (skill.TryGetProperty("effect", out effectElement)) {
				effect = effectElement.GetString();
			}

			return new Skill(name, cost, damage, effect);
		}
	}
}
