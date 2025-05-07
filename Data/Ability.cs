namespace DBGen.Data {
	class Ability {
		private string name;
		private string effect;

		public Ability(string name, string effect) {
			this.name = name;
			this.effect = effect;
		}

		public string Name { get => name; }
		public string Effect { get => effect; }
	}
}
