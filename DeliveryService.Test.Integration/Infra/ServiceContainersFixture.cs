using FluentAssertions;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryService.Test.Integration.Infra
{
    public class ServiceContainersFixture : IDisposable
    {
        private const string ServicePath = "../../../../";

        private const string TestUrl = "http://localhost:56020";

        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        private static HttpClient _client;


        public ServiceContainersFixture()
        {
            StartContainers();

            var started = WaitForService().Result;

            if (!started)
            {
                throw new Exception($"Startup failed, could not get '{TestUrl}' after trying for '{TestTimeout}'");
            }
        }

        private void StartContainers()
        {
            RunProcess("docker-compose", $"-f {ServicePath}/docker-compose.yml -f {ServicePath}/docker-compose-testing.override.yml build");
            RunProcess("docker-compose", $"-f {ServicePath}/docker-compose.yml -f {ServicePath}/docker-compose-testing.override.yml up -d");
        }

        private void StopContainers()
            => RunProcess("docker-compose", $"-f {ServicePath}/docker-compose.yml -f {ServicePath}/docker-compose-testing.override.yml down -v");

        private void RunProcess(string fileName, string arguments)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments
            });

            process.WaitForExit();
            process.ExitCode.Should().Be(0);
        }

        public HttpClient GetClient()
        {
            if (_client != null) return _client;

            _client = new HttpClient
            {
                BaseAddress = new Uri(TestUrl),
            };

            return _client;
        }

        public void Dispose()
            => StopContainers();

        private async Task<bool> WaitForService()
        {
            var client = GetClient();
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < TestTimeout)
            {
                try
                {
                    using (var response = await client.GetAsync("/health").ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    // Ignore exceptions, just retry
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }
            return false;
        }

    }
}
