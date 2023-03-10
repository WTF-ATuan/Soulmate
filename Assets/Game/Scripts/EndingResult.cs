using System;
using System.Collections.Generic;

namespace Game.Scripts{
	[Serializable]
	public class EndingResult{
		public List<string> Person1;
		public List<string> Person2;
		public MatchingRules ResultMatching;
		public int LovePoint;

		public EndingResult(List<string> person1, List<string> person2, MatchingRules resultMatching, int lovePoint){
			Person1 = person1;
			Person2 = person2;
			ResultMatching = resultMatching;
			LovePoint = lovePoint;
		}
	}
}