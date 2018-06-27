using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Encryption;
using Amazon.S3.IO;
using Amazon.S3.Model;

namespace VirtualSales.Web.Services
{
    public class S3Storage : IRemoteBlobStorage
    {
        private const string PdfDocumentsBucketName = "WhiteBrand_ClientIllustrationDocuments";
        private RSA _algorithm;
        private AmazonS3Client _client;

        public S3Storage()
        {
            const string filename = "keyxml.pk";
            var path = WebServerPathUtils.GetPathTo(Path.Combine("bin", filename));
            var f = new FileInfo(path);

            if (f.Exists)
            {
                using (var file = f.OpenRead())
                {
                    var keyString = new StreamReader(file).ReadToEnd();
                    _algorithm = RSA.Create();
                    _algorithm.FromXmlString(keyString);

                    var encryptionMaterials = new EncryptionMaterials(_algorithm);
                    try
                    {
                        _client = new AmazonS3EncryptionClient(encryptionMaterials);

                        var bucket = new S3DirectoryInfo(_client, PdfDocumentsBucketName);
                        if (!bucket.Exists)
                        {
                            bucket.Create();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to initialize S3 client\n" + ex);
                    }
                }
            }
        }

        public Stream RetrieveValue(string key)
        {
            try
            {
                var response = _client.GetObject(new GetObjectRequest()
                {
                    BucketName = PdfDocumentsBucketName,
                    Key = key
                });
                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retrieve value from S3 client\n" + ex);
                return null;
            }
        }

        public void StoreValue(string key, Stream dataInputStream)
        {
            try
            {
                _client.PutObject(new PutObjectRequest()
                {
                    InputStream = dataInputStream,
                    Key = key,
                    BucketName = PdfDocumentsBucketName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to store value in S3\n" + ex);
            }
        }

        public void RemoveValue(string key)
        {
            try
            {
                _client.DeleteObject(new DeleteObjectRequest
                {
                    BucketName = PdfDocumentsBucketName,
                    Key = key
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove value from S3 client\n" + ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~S3Storage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _algorithm.Dispose();
                _client.Dispose();

                _client = null;
                _algorithm = null;
            }
        }
    }
}