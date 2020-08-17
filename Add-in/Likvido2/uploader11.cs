using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace likvido.v1
{
    public class Uploader
    {
        /// <summary>
        /// Uploads content from the sourceStream to the provided uri
        /// </summary>
        /// <param name="uri">provided by Likvido</param>
        /// <param name="fileName">
        /// target file name for example
        /// 2020-05-25/06:44/2020-05-25/06:44:30-invoices.json
        /// 2020-05-25/06:44/2020-05-25/07:04:30-customers.json
        /// </param>
        /// <param name="format">
        /// "application/navision.v1.customers.dump"
        /// "application/navision.v1.invoices.dump"
        /// </param>
        /// <param name="contentEncoding">
        /// can be empty/null or the following
        /// "deflate"
        /// "gzip"
        /// </param>
        /// <param name="sourceStream">the data to be uploaded</param>
        public string Upload(
            string uri,
            string fileName,
            string format,
            Stream sourceStream,
            string contentEncoding,
            bool isLast)
        {
            var container = new CloudBlobContainer(new Uri(uri));
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = "application/json; charset=utf-8";
            blob.Properties.ContentEncoding = contentEncoding;
            blob.Metadata.Add("format", format);
            blob.Metadata.Add("last", isLast ? "1" : "0");
            try
            {
                blob.UploadFromStream(sourceStream);
                return fileName+ "successfully uploaded.";
            }
            catch (Exception e) {
                return e.ToString();
            }
            
        }
    }
}