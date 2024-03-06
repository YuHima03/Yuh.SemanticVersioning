using Xunit.Abstractions;

namespace Yuh.SemanticVersioning.Tests
{
    public class SemanticVersionTest(ITestOutputHelper @out)
    {
        private ITestOutputHelper _out = @out;

        private static readonly SemanticVersion v0 = new(0, 1, 2);
        private static readonly SemanticVersion v1 = new(1, 2, 3);
        private static readonly SemanticVersion v2 = new(1, 2, 3, "alpha.1");
        private static readonly SemanticVersion v3 = new(1, 2, 3, "beta");
        private static readonly SemanticVersion v4 = new(1, 2, 3, "alpha.beta");
        private static readonly SemanticVersion v5 = new(1, 2, 3, "alpha.gamma");

        [Fact]
        public void ToStringTest()
        {
            Span<SemanticVersion> semVerSpan = [v0, v1, v2, v3, v4, v5];
            foreach(var v in semVerSpan)
            {
                _out.WriteLine(v.ToString());
            }
        }

        [Fact]
        public void ParseTest()
        {
            _out.WriteLine(SemanticVersion.Parse("1.2.3-alpha.1+build.0123456789abcdef-01").ToString());
        }

        [Fact]
        public void SortTest()
        {
            Span<SemanticVersion> semVerSpan = [v0, v1, v2, v3, v4, v5];
            semVerSpan.Sort();
            foreach(var v in semVerSpan)
            {
                _out.WriteLine(v.ToString());
            }
        }
    }
}