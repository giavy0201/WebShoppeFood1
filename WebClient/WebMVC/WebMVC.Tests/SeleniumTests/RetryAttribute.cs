using Xunit;
using Xunit.Sdk;

namespace WebMVC.Tests.SeleniumTests
{
    [XunitTestCaseDiscoverer("WebMVC.Tests.SeleniumTests.RetryFactDiscoverer", "WebMVC.Tests")]
    [AttributeUsage(AttributeTargets.Method)]
    public class RetryFactAttribute : FactAttribute
    {
        public RetryFactAttribute(int maxRetries = 3)
        {
            MaxRetries = maxRetries;
        }

        public int MaxRetries { get; }
    }

    public class RetryFactDiscoverer : IXunitTestCaseDiscoverer
    {
        public IEnumerable<IXunitTestCase> GetCases(IXunitDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            var maxRetries = factAttribute.GetNamedArgument<int>("MaxRetries");
            yield return new RetryTestCase(testMethod, maxRetries);
        }
    }

    public class RetryTestCase : XunitTestCase
    {
        private readonly int _maxRetries;

        public RetryTestCase(IMethodInfo method, int maxRetries) : base(method)
        {
            _maxRetries = maxRetries;
        }

        public override async Task<RunSummary> RunAsync(IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runSummary = new RunSummary();
            for (int i = 0; i < _maxRetries; i++)
            {
                runSummary = await base.RunAsync(messageBus, constructorArguments, aggregator, cancellationTokenSource);
                if (runSummary.Failed == 0)
                    return runSummary;
            }
            return runSummary;
        }
    }
} 