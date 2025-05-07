namespace DBGen.Data {
	class Action : Card {
		private int cost;
		private string effect;
		private ActionType actionType;

		public Action(string number, string name, int limit, string rarity, Group? group, string imagePath, int cost, string effect, ActionType actionType)
		:base(number, name, limit, rarity, group, imagePath) {
			this.cost = cost;
			this.effect = effect;
			this.actionType = actionType;
		}

		public int Cost { get => cost; }
		public string Effect { get => effect; }
		internal ActionType ActionType { get => actionType; }

		public override string? ToString() {
			return (base.ToString() + ", Cost: " + cost.ToString() + ", Effect: \"" + effect + "\", Type: " + ActionType.ToString());
		}
	}
}
