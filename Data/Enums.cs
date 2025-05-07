namespace DBGen.Data {

	class Enums {
		public static Group GetGroup(string str) {
			switch (str) {
				case "L-Theropod":
					return Group.LTheropod;
				case "S-Theropod":
					return Group.STheropod;
				case "BB Bandit":
					return Group.BBBandit;
				case "BareBones Brigade":
					return Group.BareBonesBrigade;
				default:
					return (Group)Enum.Parse(typeof(Group), str);
			}
		}

		public static Effect GetEffect(string str) {
			switch (str) {
				case "FP Steal":
					return Effect.FPSteal;
				case "LP Recovery":
					return Effect.LPRecovery;
				default:
					return (Effect)Enum.Parse(typeof(Effect), str);
			}
		}
	}

	enum CardType {
		Vivosaur,
		Action
	}

	enum Group {
		Tyranno,
		LTheropod,
		STheropod,
		Raptor,
		Spinosaur,
		Sauropod,
		Ornithopod,
		Pachycephalo,
		Ceratopsian,
		Pterosaur,
		Aquatic,
		Fish,
		BBBandit,
		BareBonesBrigade,
		Dinaurian
	}

	enum Range {
		Close,
		Mid,
		Long
	}

	enum Element {
		Fire,
		Earth,
		Air,
		Water,
		Neutral
	}

	enum Effect {
		Discard,
		Draw,
		Sleep,
		Poison,
		Enrage,
		Excite,
		Scare,
		Confuse,
		Enflame,
		Harden,
		Quicken,
		Counter,
		FPSteal,
		Rest,
		Swap,
		Mill,
		Charge,
		Unrest,
		LPRecovery
	}

	enum ActionType {
		Support,
		Assist,
		Attack,
		Arena,
		Team
	}
}
