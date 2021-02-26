using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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

            string destPath =  path + "/" + sourceFile.FileName; // <-- low-level s3 path uses /
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = sourceFile.InputStream,
                BucketName = bucket,
                Key = Path.Combine(destPath, sourceFile.FileName),
                CannedACL = S3CannedACL.PublicRead
            };  
            PutObjectResponse response = await  client.PutObjectAsync(request);
            return response;
        }       

        private static string GeneratePreSignedURL(string bucketName , string objectKey, double duration, IAmazonS3 s3Client)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(12)                
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }

        public static async Task<String> AwsS3FileUpdloadWithURL(string bucket, string path, HttpPostedFileBase sourceFile)
        {
            IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUCentral1);
            string destPath = path + "/" + sourceFile.FileName;
            string url = GeneratePreSignedURL(bucket, destPath, 1000, client );
            await UploadObject(bucket, destPath, sourceFile, url);
            return url;
        }

        private static async Task<HttpWebResponse> UploadObject(string bucket, string path, HttpPostedFileBase sourceFile, string preasignedURL)
        {
            System.Net.HttpWebRequest httpRequest = WebRequest.Create(preasignedURL) as HttpWebRequest;
            httpRequest.Method = "PUT";
            using (Stream dataStream =await httpRequest.GetRequestStreamAsync())
            {
                var buffer = new byte[8000];
               
                    int bytesRead = 0;
                    while ((bytesRead = await sourceFile.InputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await dataStream.WriteAsync(buffer, 0, bytesRead);
                    }                
            }
            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
            return response;
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
