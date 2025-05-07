namespace DBGen.Data {
	class Skill {
		private string name;
		private int cost;
		private int? damage;
		private string? effect;

		public Skill(string name, int cost, int? damage, string? effect) {
			this.name = name;
			this.cost = cost;
			this.damage = damage;
			this.effect = effect;
		}

		public string Name => name;
		public int Cost => cost;
		public int? Damage => damage;
		public string? Effect => effect;
	}
}
