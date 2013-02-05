using System;
using System.IO;
using System.Net;

namespace myth.Classes
{
    public class Networking : PropertyChangedBase
    {
        private WebRequest request;
        private string parameters;
        private string feedback;

        private System.Windows.Media.Imaging.BitmapImage imgSource;
        public System.Windows.Media.Imaging.BitmapImage ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                NotifyPropertyChanged("ImgSource");
            }
        }

        public string Feedback
        {
            get { return feedback; }
            set
            {
                feedback = value;
                GlobalVariables.json = Feedback;

                NotifyPropertyChanged("Feedback"); // Call NotifyPropertyChanged when the property is updated
            }
        }

        public void SendRequest(string postRequest, Microsoft.Phone.Controls.PerformanceProgressBar performanceProgressBar)
        {
            var bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += (s, args) => // This runs on a background thread.
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    performanceProgressBar.IsIndeterminate = true;
                    performanceProgressBar.Visibility = System.Windows.Visibility.Visible;
                });

                this.parameters = postRequest;
                this.request = WebRequest.Create(new Uri("http://192.168.1.71:8080/MobileApplication/mythapi")) as WebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream(ReadCallback, request);
            };
            bw.RunWorkerCompleted += (s, args) =>
            {
                // Do your UI work here this will run on the UI thread.
                // Clear progress bar.
                performanceProgressBar.IsIndeterminate = false;
                performanceProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            };
            bw.RunWorkerAsync();
        }
        public void SendRequest(string postRequest)
        {
            this.parameters = postRequest;
            this.request = WebRequest.Create(new Uri("http://192.168.1.71:8080/MobileApplication/mythapi")) as WebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(ReadCallback, request);
        }
        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            using (var postStream = request.EndGetRequestStream(asynchronousResult))
            {
                using (var memStream = new MemoryStream())
                {
                    var content = string.Format(parameters);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Position = 0;
                    var tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    postStream.Write(tempBuffer, 0, tempBuffer.Length);
                }
            }
            request.BeginGetResponse(ResponseCallback, request);
        }
        private void ResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            using (StreamReader streamReader1 = new StreamReader(response.GetResponseStream()))
            {
                Feedback = streamReader1.ReadToEnd();
            }
        }
    }
}