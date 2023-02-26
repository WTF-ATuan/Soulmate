using System;
using System.Collections.Generic;

namespace Game.Scripts{
	[Serializable]
	public class Personality{
		public string id;
		public int loveValue;
		public List<Personality> disallow;
		public List<Personality> attractive;
		public List<Personality> repulsive;
	}
}