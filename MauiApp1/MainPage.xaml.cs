
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Maui.Storage;
namespace MauiApp1
{
    public enum DataType
    {
        Images,
        Links,
        //Text
    }


    public partial class MainPage : ContentPage
    {
        List<string> links = new List<string>();
        private Crawling _webCrawler;
        private string _headers;
        private string _url;
        private string _body;
        string whatSelected;
        string urlPattern = @"^(https?:\/\/)?([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,}(\/\S*)?$";

        public MainPage()
        {
            InitializeComponent();
            _webCrawler = new Crawling();
        }

        // Event handler برای تغییر انتخاب Picker
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedIndex = picker.SelectedIndex;

            // بررسی اینکه آیا ایندکس انتخاب شده معتبر است
            if (selectedIndex != -1)
            {
                whatSelected = picker.SelectedItem.ToString();
                PickerSelectedWhatData.Text = whatSelected;
            }
        }

        // سایر متدها
        private void OnClearClicked(object sender, EventArgs e)
        {

            urlEntry.Text = string.Empty;
            resultEditor.Text = string.Empty;

        }

        private async void OnCopyClicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(resultEditor.Text))
            {
                await Clipboard.SetTextAsync(resultEditor.Text);
                await DisplayAlert("Copy", "Content copied to clipboard", "OK");
            }
            else
            {
                await DisplayAlert("Copy", "No content to copy", "OK");
            }
        }

        private async void OnScrapeClicked(object sender, EventArgs e)
        {
            crawlButton.IsEnabled = false;
            crawlButton.IsVisible = false;
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;
            string url = urlEntry.Text;
            if (!string.IsNullOrEmpty(url))
            {


                if (Regex.IsMatch(url, urlPattern))
                {
                    if (!string.IsNullOrEmpty(whatSelected))
                    {
                        if (Enum.TryParse<DataType>(whatSelected, out var SelectedDataType))
                        {

                            switch (SelectedDataType)
                            {
                                case DataType.Links:
                                    await Task.Run(() => links = _webCrawler.GetURLs(url));
                                    foreach (var item in links)
                                    {
                                        resultEditor.Text += $"\n{item}";
                                    }

                                    break;
                                    /*
                                case DataType.Text:
                                    await Task.Run(() => _webCrawler.DownloadContentAsync(url));
                                    break;
                                    */
                                case DataType.Images:
                                    await Task.Run(() => links = _webCrawler.GetImageURLs(url));
                                    ShowImage(links);
                                    foreach (var item in links)
                                    {
                                        resultEditor.Text += $"\n{item}";
                                    }
                                    break;
                            }

                        }



                        _headers = _webCrawler.Headers2;
                        _body = _webCrawler.Body;


                    }
                    else
                    {
                        await DisplayAlert("What search??", "No Selected item", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("What search??", "Invalid Url", "OK");
                }
            }
            else
            {
                await DisplayAlert("What search??", "Url is Empty", "OK");
            }
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            crawlButton.IsVisible = true;
            crawlButton.IsEnabled = true;


        }
        private void OnHeadersClicked(object sender, EventArgs e)
        {

            resultEditor.Text = $"Server Header : \n {_headers} \n ClientHeader :{_webCrawler.ClientHeader}";
        }

        private void OnBodyClicked(object sender, EventArgs e)
        {
            resultEditor.Text = _body;
        }
        private void OnDataClicked(object sender, EventArgs e)
        {
            foreach (var item in links)
            {
                resultEditor.Text += $"\n{item}";
            }
        }
        private void ShowImage(List<string> imageUrls)
        {
            imageContainer.Children.Clear();

            foreach (var url in imageUrls)
            {
                // ایجاد یک VerticalStackLayout برای هر تصویر و دکمه آن
                var stackLayout = new VerticalStackLayout
                {
                    Margin = new Thickness(0, 10)
                };

                // ایجاد تصویر
                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(url)),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit,
                    Margin = new Thickness(0, 1)
                };

                // ایجاد دکمه دانلود
                var downloadButton = new Button
                {
                    Text = "Download Image",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5)
                };

                // رویداد کلیک دکمه دانلود
                downloadButton.Clicked += (sender, e) =>
                {
                    DownloadImage(url);
                };

                // اضافه کردن تصویر و دکمه به stackLayout
                /*
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(downloadButton);
                */
                // اضافه کردن stackLayout به imageContainer
                imageContainer.Children.Add(image);
            }
        }

        private async void DownloadImage(string Imageurl)
        {
            try
            {
                using var httpClient = new HttpClient();
                var ImageBytes = await httpClient.GetByteArrayAsync(Imageurl);
                var filename = Path.Combine(FileSystem.CacheDirectory, Path.GetFileName(Imageurl));
                File.WriteAllBytes(filename, ImageBytes);

                await DisplayAlert("Download Complete", $"Image saved to {filename}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to download image: {ex.Message}", "OK");
            }

        }
    }
}
