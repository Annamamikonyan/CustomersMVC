using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using static System.Net.Mime.MediaTypeNames;
//using Amazon.SimpleSystemsManagement;
//using Amazon.SimpleSystemsManagement.Model;
//using IqSoft.CP.Common.Models.AWS;

namespace Common.Helpers
{
    public class AwsAdapter
    {
        //public static async Task<AwsParameterStoreValue> GetValueByPathAsync(string path, string parameter)
        //{
        //    var result = new AwsParameterStoreValue();
        //    var ssmClient = new AmazonSimpleSystemsManagementClient(RegionEndpoint.EUCentral1);

        //    var response = await ssmClient.GetParametersByPathAsync(new GetParametersByPathRequest
        //    {
        //        Path = path,
        //        WithDecryption = true
        //    });
        //    if (response.Parameters != null && response.Parameters.Any())
        //    {
        //        var responseResult = response.Parameters.Where(item => string.Equals(item.Name.ToLower(), parameter.ToLower())).FirstOrDefault();
        //        if (responseResult != null)
        //        {
        //            result = new AwsParameterStoreValue()
        //            {
        //                Name = responseResult.Name,
        //                LastModifiedDate = responseResult.LastModifiedDate,
        //                Value = responseResult.Value,
        //                Version = responseResult.Version
        //            };
        //        }
        //        if (response.Parameters != null && response.Parameters.Any())
        //        {
        //            var res = response.Parameters.FirstOrDefault(x => x.Name.ToLower() == $"{path.ToLower()}{parameter.ToLower()}");
        //            if (res != null)
        //            {
        //                result = new AwsParameterStoreValue()
        //                {
        //                    LastModifiedDate = res.LastModifiedDate,
        //                    Name = res.Name,
        //                    Value = res.Value,
        //                    Version = res.Version
        //                };
        //            }
        //        }
        //    }
        //    return result;
        //}

        //public static async Task<List<AwsParameterStoreValue>> GetPathValuesAsync(string path)
        //{
        //    var result = new List<AwsParameterStoreValue>();
        //    var ssmClient = new AmazonSimpleSystemsManagementClient(RegionEndpoint.EUCentral1);

        //    var response = await ssmClient.GetParametersByPathAsync(new GetParametersByPathRequest
        //    {
        //        Path = path,
        //        WithDecryption = true
        //    });
        //    if (response.Parameters != null && response.Parameters.Any())
        //    {
        //        response.Parameters.ForEach(x =>
        //        {
        //            result.Add(new AwsParameterStoreValue()
        //            {
        //                Name = x.Name,
        //                LastModifiedDate = x.LastModifiedDate,
        //                Value = x.Value,
        //                Version = x.Version
        //            });
        //        });
        //    }
            
        //    return result;
        //}

        public static async Task<PutObjectResponse> AwsS3FileUpdload(string bucket, string path, HttpPostedFileBase sourceFile)
        {
            IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUCentral1);

            string destPath = "/" + path; // <-- low-level s3 path uses /
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = sourceFile.InputStream,
                BucketName = bucket,
                Key = Path.Combine (destPath, sourceFile.FileName)
            };

            PutObjectResponse response =await  client.PutObjectAsync(request);
            return response;
        }

        public static async  Task<GetObjectResponse> AwsS3FileDownload(string bucket, string path, string fileName )
        {
            IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUCentral1);

            string destPath = "/" + path; // <-- low-level s3 path uses /
            GetObjectRequest request = new GetObjectRequest()
            {
                // = sourceFile.InputStream,
                BucketName = bucket,
                Key = Path.Combine(destPath, "avatar_person1.jpg")
            };
           // var file = File.Create();
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    string contentType = response.Headers["Content-Type"];
                    string responseBody = reader.ReadToEnd();
                    // Now you process the response body.
                    //if (File.Exists(SelectedToDownload.FileName))
                    //    File.Delete(SelectedToDownload.FileName);

                    //File.WriteAllText(SelectedToDownload.FileName, responseBody);
                    //string readText = File.ReadAllText(SelectedToDownload.FileName);


                }
            }
            
            return new GetObjectResponse();
        }

        public static PutObjectResponse AwsS3FileUpdload(string bucket, string partnerName, string path, string content)
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUCentral1);

                string destPath = "/folder/sub-folder/testUpload.txt"; // <-- low-level s3 path uses /
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = new MemoryStream(Encoding.ASCII.GetBytes("aaaaaaa")),
                    BucketName = "qwaszx111",
                    Key = destPath
                };

                PutObjectResponse response = client.PutObject(request);
                return response;
            }
            catch ( Exception e)
            {

                throw e;
            }
            

        }
    }
}
