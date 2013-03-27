using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace JaspersoftRESTClient
{
    class DataSource
    {
        private readonly string _organizationId;
        private string _tanent;

        public DataSource(string organizationId)
        {
            _organizationId = organizationId;
        }

        public bool Exists()
        {
            var cookieContainer = new CookieContainer();
            var request = WebRequest.Create("http://localhost:8080/jasperserver-pro/rest/login?j_username=superuser&j_password=superuser") as HttpWebRequest;
            request.CookieContainer = cookieContainer;
            request.Method = "GET";
            var response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                request = WebRequest.Create("http://localhost:8080/jasperserver-pro/rest/resources/organizations/organisation_142/Data_Sources/?q=ds_142") as HttpWebRequest;
                request.CookieContainer = cookieContainer;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                Char[] read = new Char[256];
                var count = reader.Read(read, 0, 256);
                var builder = new StringBuilder();
                builder.Append(new String(read, 0, count));
                while (count > 0)
                {
                    count = reader.Read(read, 0, 256);
                    builder.Append(new String(read, 0, count));
                }
                reader.Close();
                _tanent = builder.ToString();
                response.Close();
            }

            return _tanent != null;
        }

        public void Create(string organizationId)
        {
            if (_tanent != null)
            {
                var cookieContainer = new CookieContainer();
                var request = WebRequest.Create("http://localhost:8080/jasperserver-pro/rest/login?j_username=superuser&j_password=superuser") as HttpWebRequest;
                request.CookieContainer = cookieContainer;
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var dataToSend = _tanent.Replace(_organizationId, organizationId);
                    request = WebRequest.Create("http://localhost:8080/jasperserver-pro/rest/resources/organizations/organizarion_100/Data_Sources/ds_100/") as HttpWebRequest;
                    request.CookieContainer = cookieContainer;
                    request.Method = "PUT";
                    request.ContentType = "text/plain";
                    request.ContentLength = dataToSend.Length;
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(dataToSend);
                    }

                    response = request.GetResponse() as HttpWebResponse;
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        throw new ApplicationException("Could not create Organisation");
                    }
                }
            }
        }
    }
}
