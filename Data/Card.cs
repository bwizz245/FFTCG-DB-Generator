namespace DBGen.Data {
	class Card {
		private string number;
		private string name;
		private int limit;
		private string rarity;

		private Group? group;

		private string imagePath;

		public Card(string number, string name, int limit, string rarity, Group? group, string imagePath) {
			this.number = number;
			this.name = name;
			this.limit = limit;
			this.rarity = rarity;
			this.group = group;
			this.imagePath = imagePath;
		}

		public string Number => number;
		public string Name => name;
		public int Limit => limit;
		public string Rarity => rarity;
		internal Group? Group => group;
		public string ImagePath => imagePath;

		public override string? ToString() {
			return (number + " | Name: \"" + name + "\", Limit: " + limit.ToString() + ", Rarity: " + rarity);
		}
	}
}