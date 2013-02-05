using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace myth.Classes
{
    public class CorrectionCheck
    {
        private string text;
        public string type;
        private string pattern;
        private string errorMessageLocal;
        private string errorMessageServer;
        public bool resultLocal = false;
        public bool resultServer = false;

        public CorrectionCheck()
        {
            this.text = "";
            this.type = "";
            this.pattern = "";
            this.errorMessageLocal = "";
            this.errorMessageServer = "";
        }

        public CorrectionCheck(string textToCheck, string type, string pattern, string errorMessageLocal, string errorMessageServer)
        {
            this.text = textToCheck;
            this.type = type;
            this.pattern = pattern;
            this.errorMessageLocal = errorMessageLocal;
            this.errorMessageServer = errorMessageServer;
        }

        public void GetTelephoneOperator(Image img, string errorMessage)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImg.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.BackgroundCreation;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("command", "getMobilePhoneProviderCode");
            parameters.Add("session_key", GlobalVariables.sessionId);
            parameters.Add("phone", text);

            var proxy = new WindowsPhonePostClient.PostClient(parameters);
            proxy.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    JsonParse deserializedProduct = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonParse>(e.Result);
                    if (deserializedProduct.result == "success")
                    {
                        bitmapImg.UriSource = new Uri("https://www.myth.kz/VAADIN/themes/mytheme/images/" + deserializedProduct.data.providerCode + ".png", UriKind.Absolute);
                    }
                    else
                    {
                        bitmapImg.UriSource = new Uri("/img/registration/False.png", UriKind.Relative);
                        MessageBox.Show(errorMessage);
                    }
                }
                img.Source = bitmapImg;
            };
            proxy.DownloadStringAsync(new Uri("http://192.168.1.71:8080/MobileApplication/mythapi", UriKind.Absolute));
        }

        public void SetPhoneNumberName(string number, TextBlock outputTextBlock)
        {
            string phoneNumberName = "";
            if (GlobalVariables._contractsList.contracts.Count > 0)
            {
                foreach (Contracts item in GlobalVariables._contractsList.contracts)
                {
                    if (item.contract == number)
                    {
                        phoneNumberName = item.alias;
                        break;
                    }
                }
            }
            outputTextBlock.Text = phoneNumberName;
        }

        public void LocalCheck()
        {
            resultServer = true;
            if (text.Length > 0)
            {
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
                if (!r.IsMatch(text))
                {
                    resultLocal = false;
                }
                else
                {
                    resultLocal = true;
                }
            }
            else
            {
                resultLocal = false;
            }
        }

        public void Check(Image img)
        {
            if (text.Length > 0)
            {
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
                if (!r.IsMatch(text))
                {
                    MessageBox.Show(errorMessageLocal);
                    resultLocal = false;
                    img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/img/registration/" + resultServer.ToString() + ".png", UriKind.Relative));
                }
                else
                {
                    resultLocal = true;
                    if (type != "password")
                    {
                        ServerCheck(img);
                    }
                    else
                    {
                        resultServer = true;
                        img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/img/registration/" + resultServer.ToString() + ".png", UriKind.Relative));
                    }
                }
            }
            else
            {
                resultLocal = false;
                resultServer = false;
                img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/img/registration/" + resultServer.ToString() + ".png", UriKind.Relative));
            }
        }

        private void ServerCheck(Image img)
        {
            System.Collections.Generic.Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("command", "check" + type + "exist");
            parameters.Add(type, text);

            WindowsPhonePostClient.PostClient proxy = new WindowsPhonePostClient.PostClient(parameters);
            proxy.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    JsonParse deserializedProduct = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonParse>(e.Result);

                    if (deserializedProduct.result == "success")
                    {
                        if (deserializedProduct.exists == "true")
                        {
                            resultServer = false;
                            MessageBox.Show(errorMessageServer);
                        }
                        else
                        {
                            resultServer = true;
                        }
                    }
                    else
                        MessageBox.Show(deserializedProduct.message);

                    img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/img/registration/" + resultServer.ToString() + ".png", UriKind.Relative));
                }
            };
            proxy.DownloadStringAsync(new Uri("http://192.168.1.71:8080/MobileApplication/mythapi", UriKind.Absolute));
        }

        public bool isOk()
        {
            if (text.Length > 0)
            {
                if ((resultLocal == true) && (resultServer == true))
                {
                    return true;
                }
                else
                {
                    if (resultLocal == false)
                    {
                        MessageBox.Show(errorMessageLocal);
                    }
                    if (resultServer == false)
                    {
                        MessageBox.Show(errorMessageServer);
                    }
                }
                return false;
            }
            else
            {
                MessageBox.Show(errorMessageLocal);
                return false;
            }
        }
    }
}
