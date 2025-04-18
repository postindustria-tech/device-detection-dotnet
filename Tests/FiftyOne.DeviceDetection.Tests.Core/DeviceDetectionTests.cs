/* *********************************************************************
 * This Original Work is copyright of 51 Degrees Mobile Experts Limited.
 * Copyright 2025 51 Degrees Mobile Experts Limited, Davidson House,
 * Forbury Square, Reading, Berkshire, United Kingdom RG1 3EU.
 *
 * This Original Work is licensed under the European Union Public Licence
 * (EUPL) v.1.2 and is subject to its terms as set out below.
 *
 * If a copy of the EUPL was not distributed with this file, You can obtain
 * one at https://opensource.org/licenses/EUPL-1.2.
 *
 * The 'Compatible Licences' set out in the Appendix to the EUPL (as may be
 * amended by the European Commission) shall be deemed incompatible for
 * the purposes of the Work and the provisions of the compatibility
 * clause in Article 5 of the EUPL shall not apply.
 *
 * If using the Work as, or as part of, a network application, by
 * including the attribution notice(s) required under Article 5 of the EUPL
 * in the end user terms of the application under an appropriate heading,
 * such notice(s) shall fulfill the requirements of that article.
 * ********************************************************************* */

using FiftyOne.DeviceDetection.TestHelpers;
using FiftyOne.Pipeline.Core.Data;
using FiftyOne.Pipeline.Engines;
using FiftyOne.Pipeline.Engines.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Constants = FiftyOne.DeviceDetection.TestHelpers.Constants;

namespace FiftyOne.DeviceDetection.Tests.Core
{
    [TestClass]
    public class DeviceDetectionTests
    {
        private static UserAgentGenerator USER_AGENTS = new UserAgentGenerator(
            TestHelpers.Utils.GetFilePath(Constants.UA_FILE_NAME));

        private static readonly List<Dictionary<string, string>> UachTestValues = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>()
            {
                { Shared.Constants.EVIDENCE_SECCHUA_PLATFORM_HEADER_KEY, @"""Windows""" },
                { Shared.Constants.EVIDENCE_SECCHUA_PLATFORM_VERSION_HEADER_KEY, @"""14.0.0""" }
            }
        };

        [DataTestMethod]
        // ******** Hash with a single thread *********
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.MaxPerformance, false, false, DisplayName = "Hash-MaxPerformance-NoLazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.HighPerformance, false, false, DisplayName = "Hash-HighPerformance-NoLazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.LowMemory, false, false, DisplayName = "Hash-LowMemory-NoLazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.Balanced, false, false, DisplayName = "Hash-Balanced-NoLazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.BalancedTemp, false, false, DisplayName = "Hash-BalancedTemp-NoLazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.MaxPerformance, true, false, DisplayName = "Hash-MaxPerformance-LazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.HighPerformance, true, false, DisplayName = "Hash-HighPerformance-LazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.LowMemory, true, false, DisplayName = "Hash-LowMemory-LazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.Balanced, true, false, DisplayName = "Hash-Balanced-LazyLoad-SingleThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.BalancedTemp, true, false, DisplayName = "Hash-BalancedTemp-LazyLoad-SingleThread")]
        // ******** Hash with multiple threads *********
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.MaxPerformance, false, true, DisplayName = "Hash-MaxPerformance-NoLazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.HighPerformance, false, true, DisplayName = "Hash-HighPerformance-NoLazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.LowMemory, false, true, DisplayName = "Hash-LowMemory-NoLazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.Balanced, false, true, DisplayName = "Hash-Balanced-NoLazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.BalancedTemp, false, true, DisplayName = "Hash-BalancedTemp-NoLazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.MaxPerformance, true, true, DisplayName = "Hash-MaxPerformance-LazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.HighPerformance, true, true, DisplayName = "Hash-HighPerformance-LazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.LowMemory, true, true, DisplayName = "Hash-LowMemory-LazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.Balanced, true, true, DisplayName = "Hash-Balanced-LazyLoad-MultiThread")]
        [DataRow(Constants.LITE_HASH_DATA_FILE_NAME, PerformanceProfiles.BalancedTemp, true, true, DisplayName = "Hash-BalancedTemp-LazyLoad-MultiThread")]
        public void Hash_AllConfigurations_100_UserAgents(
            string datafileName,
            PerformanceProfiles performanceProfile,
            bool useLazyLoading,
            bool multiThreaded)
        {
            TestWithEvidence(datafileName,
                performanceProfile,
                useLazyLoading,
                multiThreaded,
                GetEvidenceFromUas(),
                // Just access the IsMobile property to ensure we can get data out.
                // This will throw an exception if the value was not populated
                (dd) => { var result = dd.IsMobile; });
        }

        public void Hash_Uach(
            string datafileName,
            PerformanceProfiles performanceProfile,
            bool useLazyLoading,
            bool multiThreaded)
        {
            TestWithEvidence(datafileName,
                performanceProfile,
                useLazyLoading,
                multiThreaded,
                UachTestValues,
                (dd) => { Assert.AreEqual("11", dd.PlatformVersion.Value); });
        }

        /// <summary>
        /// Use the 'USER_AGENTS' field to construct a collection of evidence values.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IEnumerable<KeyValuePair<string,string>>> GetEvidenceFromUas()
        {
            foreach (var ua in USER_AGENTS.GetRandomUserAgents(100))
            {
                var key = Pipeline.Core.Constants.EVIDENCE_HTTPHEADER_PREFIX +
                                Pipeline.Core.Constants.EVIDENCE_SEPERATOR + "User-Agent";
                yield return new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>(key, ua) };
            }
        }

        /// <summary>
        /// Test the supplied evidence using the specified device detection settings.
        /// </summary>
        /// <param name="datafileName">
        /// The filename of the data file to use for device detection.
        /// </param>
        /// <param name="performanceProfile">
        /// The performance profile to use.
        /// </param>
        /// <param name="useLazyLoading">
        /// Whether or not to use the lazy loading feature.
        /// </param>
        /// <param name="multiThreaded">
        /// Whether to use a single thread or multiple threads when 
        /// passing user agents to the pipeline for processing.
        /// </param>
        /// <param name="evidence">
        /// The evidence values to test with.
        /// </param>
        /// <param name="verifyAction">
        /// An action to execute to verify that the result includes the expected values.
        /// The action should throw an exception describing the problem if verification fails.
        /// </param>
        public void TestWithEvidence(
            string datafileName,
            PerformanceProfiles performanceProfile,
            bool useLazyLoading,
            bool multiThreaded,
            IEnumerable<IEnumerable<KeyValuePair<string, string>>> evidence,
            Action<IDeviceData> verifyAction)
        {
            var datafile = TestHelpers.Utils.GetFilePath(datafileName);
            var updateService = new Mock<IDataUpdateService>();

            // Configure the pipeline builder based on the 
            // parameters passed to this method.
            var builder = new DeviceDetectionPipelineBuilder(
                new NullLoggerFactory(), null, updateService.Object)
                .UseOnPremise(datafile.FullName, null, false)
                .SetPerformanceProfile(performanceProfile)
                .SetShareUsage(false)
                .SetDataFileSystemWatcher(false);
            if (useLazyLoading)
            {
                builder.UseLazyLoading();
            }

            CancellationTokenSource cancellationSource = new CancellationTokenSource();

            using (var pipeline = builder.Build())
            {
                var options = new ParallelOptions()
                {
                    CancellationToken = cancellationSource.Token,
                    // The max parallelism is limited to 8 when the
                    // multiThreaded flag is enabled.
                    // This is because, if it is not limited, the lazy 
                    // loading tests will start all requests almost
                    // immediately and then some will take so long to
                    // complete that they exceed the configured timeout.
                    MaxDegreeOfParallelism = (multiThreaded ? 8 : 1)
                };


                // Create a parallel loop to actually process the user agents.
                Parallel.ForEach(evidence, options,
                    (evidenceEntry) =>
                    {
                        // Create the flow data instance
                        using (var flowData = pipeline.CreateFlowData())
                        {

                            // Add the evidence to the flow data and process it
                            foreach (var subEntry in evidenceEntry)
                            {
                                flowData.AddEvidence(subEntry.Key, subEntry.Value);
                            }
                            flowData.Process();

                            // Make sure no errors occurred. If any did then
                            // cancel the parallel process.
                            if (flowData.Errors != null)
                            {
                                Assert.AreEqual(0, flowData.Errors.Count,
                                    $"Expected no errors but got {flowData.Errors.Count}" +
                                    $":{Environment.NewLine}{ReportErrors(flowData.Errors)}");
                                cancellationSource.Cancel();
                            }

                            // Get the device data instance and pass it to the verifyAction.
                            var deviceData = flowData.Get<IDeviceData>();
                            verifyAction(deviceData);
                        }
                    });
            }
        }


        /// <summary>
        /// Private method to present the given list of FlowError 
        /// instances as a single string.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static string ReportErrors(IList<IFlowError> errors)
        {
            StringBuilder result = new StringBuilder();
            foreach(var error in errors)
            {
                result.AppendLine($"Error in element '{error.FlowElement.GetType().Name}'");
                AddExceptionToMessage(result, error.ExceptionData);
            }
            return result.ToString();
        }

        private static void AddExceptionToMessage(StringBuilder message, Exception ex, int depth = 0)
        {
            AddToMessage(message, $"{ex.GetType().Name} - {ex.Message}", depth);
            AddToMessage(message, $"{ex.StackTrace}", depth);
            if (ex.InnerException != null)
            {
                AddExceptionToMessage(message, ex.InnerException, depth++);
            }
        }

        private static void AddToMessage(StringBuilder message, string textToAdd, int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                message.Append("   ");
            }
            message.AppendLine(textToAdd);
        }
    }
}
