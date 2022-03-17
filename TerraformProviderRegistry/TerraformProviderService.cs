using Amazon.S3;
using Amazon.S3.Model;
using System.Text.Json;
using TerraformProviderRegistry.Model;
using TerraformProviderRegistry.Model.Response;

namespace TerraformProviderRegistry
{
    public class TerraformProviderService
    {

        private string? _bucketName = String.Empty;

        public TerraformProviderService(string? bucketName)
        {
            _bucketName = bucketName;
        }

        public static string serviceDiscovery()
        {
            return "{\"providers.v1\": \"/terraform/providers/v1/\"}";
        }

        public string? versions(string? name_space, string? name)
        {
            string returnData = String.Empty;
            var task = Task.Run(() => Content(_bucketName, $"{name_space}/{name}.json"));
            task.Wait();

            if (task.Result != null)
            {
                string data = task.Result;
                TerraformProvider? tp = JsonSerializer.Deserialize<TerraformProvider>(data);

                TerraformAvailableProvider availableResponse = new TerraformAvailableProvider();

                tp?.versions?.ForEach(tpv =>
                {
                    TerraformAvailableVersion tav = new TerraformAvailableVersion
                    {
                        version = tpv.version,
                        protocols = tpv.protocols
                    };

                    List<TerraformAvailablePlatform> platformList = new List<TerraformAvailablePlatform>();

                    tpv?.platforms?.ForEach(tpp =>
                    {
                        platformList.Add(new TerraformAvailablePlatform
                        {
                            arch = tpp.arch,
                            os = tpp.os
                        });
                    });

                    tav.platforms = platformList;
                    availableResponse?.versions?.Add(tav);
                });

                returnData = JsonSerializer.Serialize(availableResponse);
            }

            return returnData;
        }

        public string? providerPackage(string? name_space, string? name, string? version, string? os, string? arch)
        {
            string? responseData = null;

            var task = Task.Run(() => Content(_bucketName, $"{name_space}/{name}.json"));
            task.Wait();

            if (task.Result != null)
            {
                string data = task.Result;
                TerraformProvider? tp = JsonSerializer.Deserialize<TerraformProvider>(data);

                tp?.versions?.ForEach(tpv =>
                {
                    if (string.Equals(tpv.version, version, StringComparison.OrdinalIgnoreCase))
                    {
                        tpv?.platforms?.ForEach(p =>
                        {
                            if (string.Equals(p.os, os, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.arch, arch, StringComparison.OrdinalIgnoreCase))
                            {
                                TerraformProviderPackage tpp = new TerraformProviderPackage
                                {
                                    protocols = tpv.protocols,
                                    filename = p.filename,
                                    arch = p.arch,
                                    download_url = p.download_url,
                                    os = p.os,
                                    shasum = p.shasum,
                                    shasums_signature_url = p.shasums_signature_url,
                                    shasums_url = p.shasums_url,
                                    signing_keys = p.signing_keys
                                };

                                responseData = JsonSerializer.Serialize(tpp);
                            }
                        });
                    }
                });
            }

            return responseData;
        }

        private async Task<string?> Content(string? bucketName, string key)
        {
            string? content = null;
            AmazonS3Client client = new AmazonS3Client();

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    using (Stream responseStream = response.ResponseStream)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            { }

            return content;
        }

    }
}
