using System;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;





namespace MauiApp1
{
    internal class Crawling
    {
        private readonly HttpClient _httpClient;
        public string Body;
        public string Headers2;
        public string Error;
        public string ClientHeader;
        public Crawling ()
        {
            _httpClient = new HttpClient();

        } 
        public void  DownloadContentAsync(string url) 
        {
            try
            {
                var response = _httpClient.GetAsync(url).Result;
                var headers = new StringBuilder();

                var body = response.Content.ReadAsStringAsync();
                Body = response.Content.ReadAsStringAsync().Result;
                var requestheader = new StringBuilder();

                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    requestheader.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                foreach (var header in response.Headers)
                {
                    headers.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }   
                Headers2 = headers.ToString();
                ClientHeader = requestheader.ToString();


            }
            catch (Exception ex) {
                Error = ex.Message;

            }
        }

        public List<string> GetURLs( string url) 
        {
            var urls = new List<string>();
            try
            {
                DownloadContentAsync(url);

                var document = new HtmlDocument();
                document.LoadHtml(Body);

                var baseuri = new Uri(url);
                var host = baseuri.Host;

                var links = document.DocumentNode.SelectNodes("//a[@href]");

                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var hrefvalue = link.GetAttributeValue("href", string.Empty);
                        var absoluteuri = new Uri(baseuri, hrefvalue);
                        if (absoluteuri.Host == host)
                        {
                            urls.Add(absoluteuri.ToString());
                        }
                    }


                }
            }
            catch (Exception ex) { Error = ex.Message; }
            return urls;

        }
        public List<string> GetImageURLs(string url)
        {
            var ImgUrls = new List<string>();
            try
            {
                DownloadContentAsync(url);
                var document = new HtmlDocument();

                document.LoadHtml(Body);
                var baseuri = new Uri(url);
                var imagenodes = document.DocumentNode.SelectNodes("//img[@src]");
                if(imagenodes != null)
                {
                    foreach (var imagen in imagenodes)
                    {
                        var srcvalue = imagen.GetAttributeValue("src", string.Empty);
                        var absoluteUri = new Uri(baseuri, srcvalue);
                        ImgUrls.Add(absoluteUri.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return ImgUrls;
        }
        public string ProcessDataAsync(string url, DataType dataType)
        {
            return "null";
        }
    }

}
