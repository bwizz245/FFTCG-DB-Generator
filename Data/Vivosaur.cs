namespace DBGen.Data {
	class Vivosaur : Card {
		private int lp;
		private int fp;
		private ChargeSkill? chargeSkill;
		private Ability ability;
		private Skill skill1;
		private Skill? skill2;

		private Range range;
		private Element element;

		private List<Effect> effects;

		public Vivosaur(string number, string name, int limit, string rarity, Group group, string imagePath, int lp, int fp, ChargeSkill chargeSkill, Ability ability, Skill skill1, Skill skill2, Range range, Element element, List<Effect> effects)
		:base(number, name, limit, rarity, group, imagePath) {
			this.lp = lp;
			this.fp = fp;
			this.chargeSkill = chargeSkill;
			this.ability = ability;
			this.skill1 = skill1;
			this.skill2 = skill2;
			this.range = range;
			this.element = element;
			this.effects = effects;
		}

		public int LP { get => lp; }
		public int FP { get => fp; }
		public ChargeSkill? ChargeSkill => chargeSkill;
		internal Ability Ability => ability;
		internal Skill Skill1 => skill1;
		internal Skill? Skill2 => skill2;

		internal Range Range => range;
		internal Element Element => element;

		internal List<Effect> Effects => effects;
	}
}
