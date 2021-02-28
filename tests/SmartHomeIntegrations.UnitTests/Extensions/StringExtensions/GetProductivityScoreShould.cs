using FluentAssertions;
using SmartHomeIntegrations.Core.Extensions;
using Xunit;

namespace SmartHomeIntegrations.UnitTests.Extensions.StringExtensions
{
    public class GetProductivityScoreShould
    {
        [Fact]
        public void FindNumberInHtml()
        {
            var html =
                "less than a minute ago\",\"efficiency_percent\\\":50};\\n//]]>\\n</template>  </body>\\n</html>\\n\"";
            html.GetProductivityScore().Should().Be(50);
        }
    }
}
