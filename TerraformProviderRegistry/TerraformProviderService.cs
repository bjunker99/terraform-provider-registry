using Amazon.S3;
using Amazon.S3.Model;
using System.Text.Json;
using TerraformProviderRegistry.Model;
using TerraformProviderRegistry.Model.Response;

namespace TerraformProviderRegistry
{
    public class TerraformProviderService
    {

        private string? _bucketName = string.Empty;
        private Amazon.RegionEndpoint _region = Amazon.RegionEndpoint.USEast1;

        public TerraformProviderService(string? bucketName, string? region)
        {
            _bucketName = bucketName;
            _region = Amazon.RegionEndpoint.GetBySystemName(region);
        }

        public async Task<string?> Versions(string? name_space, string? name)
        {
            string returnData = String.Empty;
            string? data = await Content(_bucketName, $"{name_space}/{name}.json");

            if (data != null)
            {
                TerraformProvider? tp = JsonSerializer.Deserialize<TerraformProvider>(data);

                TerraformAvailableProvider availableResponse = new TerraformAvailableProvider();

                tp.Versions.ForEach(tpv =>
                {
                    TerraformAvailableVersion tav = new TerraformAvailableVersion
                    {
                        Version = tpv.Version,
                        Protocols = tpv.Protocols
                    };

                    List<TerraformAvailablePlatform> platformList = new List<TerraformAvailablePlatform>();

                    tpv.Platforms.ForEach(tpp =>
                    {
                        platformList.Add(new TerraformAvailablePlatform
                        {
                            Arch = tpp.Arch,
                            OS = tpp.OS
                        });
                    });

                    tav.Platforms = platformList;
                    availableResponse.versions.Add(tav);
                });

                returnData = JsonSerializer.Serialize(availableResponse);
            }

            return returnData;
        }

        public async Task<string?> ProviderPackage(string? name_space, string? name, string? version, string? os, string? arch)
        {
            string? responseData = null;

            string? data = await Content(_bucketName, $"{name_space}/{name}.json");
            
            if (data != null)
            { 
                TerraformProvider? tp = JsonSerializer.Deserialize<TerraformProvider>(data);

                tp.Versions.ForEach(tpv =>
                {
                    if (string.Equals(tpv.Version, version, StringComparison.OrdinalIgnoreCase))
                    {
                        tpv.Platforms.ForEach(p =>
                        {
                            if (string.Equals(p.OS, os, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.Arch, arch, StringComparison.OrdinalIgnoreCase))
                            {
                                TerraformProviderPackage tpp = new TerraformProviderPackage
                                {
                                    protocols = tpv.Protocols,
                                    Filename = p.Filename,
                                    Arch = p.Arch,
                                    DownloadUrl = p.DownloadUrl,
                                    OS = p.OS,
                                    Shasum = p.Shasum,
                                    ShasumsSignatureUrl = p.ShasumsSignatureUrl,
                                    ShasumsUrl = p.ShasumsUrl,
                                    SigningKeys = p.SigningKeys
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
            AmazonS3Client client = new AmazonS3Client(_region);

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
