using System.Collections.Generic;

namespace Gazeus.DesafioMatch3.Core.Matching
{
    public class ComplexMatch
    {
        private ComposedMatch[] _composedMatches;

        public ComplexMatch(List<ComposedMatch> composedMatches)
        {
            _composedMatches = composedMatches.ToArray();
        }
        
        
    }
}