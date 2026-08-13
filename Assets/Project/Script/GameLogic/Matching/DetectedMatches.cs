using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Matching
{
    public class DetectedMatches
    {
        public IEnumerable<ComplexMatch> ComplexMatches { get; private set; }
        public IEnumerable<ComposedMatch> ComposedMatches { get; private set; }
        public IEnumerable<BasicMatch> BasicMatches { get; private set; }

        public DetectedMatches(IEnumerable<ComplexMatch> complexMatches, IEnumerable<ComposedMatch> composedMatches,
            IEnumerable<BasicMatch> basicMatches)
        {
            ComplexMatches = complexMatches;
            ComposedMatches = composedMatches;
            BasicMatches = basicMatches;
        }

        public bool HasComplexMatches => ComplexMatches.Any();
        public bool HasComposedMatches => ComposedMatches.Any();
        public bool HasBasicMatches => BasicMatches.Any();

        public bool HasHorizontalMatched => BasicMatches.Any(x => x.IsHorizontal());
        public bool HasVerticalMatched => BasicMatches.Any(x => x.IsVertical());
        public int HorizontalMatchesCount => BasicMatches.Count(x => x.IsHorizontal());
        public int VerticalMatchesCount => BasicMatches.Count(x => x.IsVertical());
        public bool HasComposedMatchL => ComposedMatches.Any(x => x.IsMatchL());
        public bool HasComposedMatchT => ComposedMatches.Any(x => x.IsMatchT());
    }
}