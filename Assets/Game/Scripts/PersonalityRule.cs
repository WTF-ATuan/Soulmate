using System.Collections.Generic;

namespace Game.Scripts{
	[System.Serializable]
	public class PersonalityRule{
		public Personality binding;
		public List<Personality> disallow;
		public List<Personality> attractive;
		public List<Personality> repulsive;
	}
}