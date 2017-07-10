using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Development.Core
{
    public class ApiWorker
    {
      
        public T CallApi<T>(ApiWorkerRequestDto request)
        {

            T obj = default(T);

            //To Make REST full call
            var req =
                WebRequest.Create(new Uri(request.Scheme + "://" + request.BaseUri))
                    as HttpWebRequest;
            if (req == null) return obj;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            // Build a string with all the params, properly encoded.
            var paramz = new StringBuilder();
            foreach (var param in request.PostParams)
            {
                paramz.Append(param.Name);
                paramz.Append("=");
                paramz.Append(param.Value);
                paramz.Append("&");
            }

            // Encode the parameters as form data:
            var formData =
                Encoding.UTF8.GetBytes(paramz.ToString());
            req.ContentLength = formData.Length;

            // Send the request:
            using (var post = req.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            // Pick up the response:
            string response = null;
            using (var resp = req.GetResponse()
                as HttpWebResponse)
            {
                if (resp != null)
                {
                    var reader =
                        new StreamReader(resp.GetResponseStream());
                    response = reader.ReadToEnd();
                }
            }

            //Deserialize the JOSN string
            if (response != null) obj = DeserialiseJsonString<T>(response);

            //Return the Object
            return obj;
        }

        //To Deserialise 
        private T DeserialiseJsonString<T>(string responseString)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = XDocument.Parse(responseString).Root?.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }

    public class ApiWorkerRequestDto
    {
        public string Scheme { get; set; }
        public bool HttpGet { get; set; }
        public string BaseUri { get; set; }
        public List<RequestParam> PostParams { get; set; }
    }

    public class RequestParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public static class SerializationUtil
    {

        // To Clean XML
        public static string SerializeToString(object value)
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(value.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, value, emptyNamepsaces);
                return stream.ToString();
            }
        }
    }

}
