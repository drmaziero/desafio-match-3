using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Matching
{
    public class MatchConsumptionTracker
    {
        private HashSet<BasicMatch> _consumedMatches = new();

        public void Clear()
        {
            _consumedMatches.Clear();
        }
        
        public void Consume(IEnumerable<BasicMatch> matches)
        {
            foreach (var match in matches)
                _consumedMatches.Add(match);
        }
        
        public void Consume(BasicMatch match)
        {
            _consumedMatches.Add(match);
        }

        public bool IsConsumed(BasicMatch match)
        {
            return _consumedMatches.Contains(match);
        }

        public bool HasConsumedMatches(IEnumerable<BasicMatch> matches)
        {
            return matches.Any(IsConsumed);
        }
        
       
    }
}