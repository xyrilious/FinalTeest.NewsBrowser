using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;
using Newtonsoft.Json;

namespace XyrilleMamalateo.NewsBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Source
        {
            public string id { get; set; }
            public string name { get; set; }
        }
        public class Article
        {
            public Source source { get; set; }
            public object author { get; set; }
            public string title { get; set; }
            public object description { get; set; }
            public string url { get; set; }
            public object urlToImage { get; set; }
            public DateTime publishedAt { get; set; }
            public object content { get; set; }
        }

        public class RootObject
        {
            public string status { get; set; }
            public int totalResults { get; set; }
            public Article[] articles { get; set; }

        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey=571d47b81b2a45169ccfa97e9efe8ab7");
            var request = new RestRequest("", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var source = JsonConvert.DeserializeObject<RootObject>(content);
            int counter = 0;


            foreach (var items in source.articles)
            {
                int N = counter + 1;
                TreeViewItem ParentItem = new TreeViewItem();
                ParentItem.Header = N.ToString() + ")" + source.articles[counter].title;
                TreeView1.Items.Add(ParentItem);
                counter++;
                if (counter == 9) break;
            }
        }

        private void TreeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            lblAuthor.Content = "";
            lblTitle.Content = "";
            lblSourceName.Content = "";
            lblDesc.Content = "";
            txtContent.Text = "";
            lblDate.Content = "";
            var client = new RestClient("https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey=571d47b81b2a45169ccfa97e9efe8ab7");
            var request = new RestRequest("", Method.GET);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var source = JsonConvert.DeserializeObject<RootObject>(content);
            int X = TreeView1.Items.IndexOf(TreeView1.SelectedItem);
            lblAuthor.Content = source.articles[X].author;
            lblTitle.Content = source.articles[X].title;
            lblSourceName.Content = source.articles[X].source.name;
            lblDesc.Content = source.articles[X].description;

            if (source.articles[X].content != null)
            {
                txtContent.Text = source.articles[X].content.ToString();
            }
            lblDate.Content = source.articles[X].publishedAt;


        }
    }
}