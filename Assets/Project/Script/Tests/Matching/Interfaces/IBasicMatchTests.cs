namespace Project.Script.Tests.Matching.Interfaces
{
    public interface IBasicMatchTests
    {
        public void ShouldDetectHorizontalMatch();
        
        public void ShouldDetectHorizontalMatchOnBeginLine();
        
        public void ShouldDetectHorizontalMatchOnMiddleLine();
        
        public void ShouldDetectHorizontalMatchOnFinalLine();
        
        public void ShouldDetectVerticalMatch();
        
        public void ShouldDetectVerticalMatchOnBeginLine();
        
        public void ShouldDetectVerticalMatchOnMiddleLine();
        
        public void ShouldDetectVerticalMatchOnFinalLine();
    }
}