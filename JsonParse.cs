using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;

namespace myth.Classes
{
    public class JsonParseBase
    {
        public string result { get; set; }
        public string message { get; set; }
        public string messageTitle { get; set; }
        public string error { get; set; }
        public string errcode { get; set; }
        public string exists { get; set; }
    }

    public class JsonParse : JsonParseBase
    {
        [JsonProperty(PropertyName = "data")]
        public Data1 data { get; set; }

        public string getDataFromJSON(string fileName)
        {
            // take data from fileName
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            String jsonString = "";
            try
            {
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream("AppData\\" + fileName, FileMode.Open, isf))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        jsonString = isoFileReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                // Handle the case when the user attempts to click the Read button first.
                MessageBox.Show("Need to create directory and the file \"" + fileName + "\" first.");
            }
            return jsonString;
        }
        public void addToLocalStorage(string fileName)
        {
            //take file from json folder
            Uri uri = new Uri("json/" + fileName, UriKind.Relative);

            StreamResourceInfo sri = App.GetResourceStream(uri);
            StreamReader sr = new StreamReader(sri.Stream);
            string text = sr.ReadToEnd();
            sr.Close();

            // Obtain the virtual store for the application.
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();

            // Specify the file path and options.
            isf.CreateDirectory("AppData");
            fileName = "AppData\\" + fileName;
            using (var isoFileStream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, isf))
            {
                //Write the data
                using (var isoFileWriter = new StreamWriter(isoFileStream))
                {
                    isoFileWriter.Write(text.ToString());
                }
            }
        }
    }

    public class JsonParse1 : JsonParseBase
    {
        [JsonProperty(PropertyName = "data")]
        public List<Payments> data { get; set; }
    }

    public class JsonParse2 : JsonParseBase
    {
        [JsonProperty(PropertyName = "data")]
        public List<Company> data { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string logo { get; set; }
        public string pageName { get; set; }
        public int likes { get; set; }
        public bool liked { get; set; }
        public Uri ImagePath { get; set; }
    }

    public class Data1
    {
        public List<NewsJSON> news { get; set; }
        public List<Clients> clients { get; set; }
        public List<Contracts> contracts { get; set; }
        public CardBankData clcardbankdata { get; set; }   //Mobile.xaml
        public List<Country> countries { get; set; }   //Mobile.xaml
        public List<CardType> cardtypes { get; set; }   //Mobile.xaml
        public List<BankType> banks { get; set; }   //Mobile.xaml
        public List<CategoriesProviders> categoriesProviders { get; set; }
        public NewsJSON post { get; set; } //like/comment on news.xaml

        public string session { get; set; }
        public string exists { get; set; }
        public string providerCode { get; set; }

        //Mobile.xaml
        public string clcardid { get; set; }
        public string clbankid { get; set; }
        public int selectedCardId { get; set; }
        public int selectedBankId { get; set; }

        //PaymentsPanoramaPage.xaml
        public List<Payments> paymentsList { get; set; }

        public User user { get; set; }
    }

    #region user description classes
    public class User
    {
        public int id { get; set; }
        public int anonym { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickname { get; set; }
        public Profile profile { get; set; }
        public string phone { get; set; }
        public int phoneCountry { get; set; }
        public string fullPhone { get; set; }
        public string email { get; set; }
        public bool emailChecked { get; set; }
        public bool phoneChecked { get; set; }
        public int segment { get; set; }
        public int disabled { get; set; }
    }
    public class Profile
    {
        public ProfileInfo sector { get; set; }
        public ProfileInfo position { get; set; }
        public ProfileInfo work { get; set; }
        public ProfileInfo birthday { get; set; }
        public ProfileInfo occupation { get; set; }
        public ProfileInfo registration_step { get; set; }
        public ProfileInfo education { get; set; }
        public ProfileInfo fio { get; set; }
        public ProfileInfo maritalstate { get; set; }
        public ProfileInfo income { get; set; }
        public ProfileInfo gender { get; set; }
        public ProfileInfo card { get; set; }
        public ProfileInfo bank { get; set; }
    }
    public class ProfileInfo
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
    #endregion

    public class NewsJSON : Comments
    {
        public List<Comments> comments { get; set; }
    }

    public class Comments
    {
        [JsonProperty(PropertyName = "_id")]
        public string id { get; set; }
        public List<string> likes { get; set; }
        public string src { get; set; }
        public string like { get; set; }
        public Info info { get; set; }

        public string sender { get; set; }
        public string date { get; set; }
        public string msg { get; set; }
        public int likesNumber { get; set; }
        public Uri ImagePath { get; set; }
        public System.Windows.Visibility visibility { get; set; }

        public string likeXamlText { get; set; }
        public string likeImagePath { get; set; }
    }

    public class Info
    {
        public string message { get; set; }
        public string date { get; set; }
    }

    public class Clients
    {
        public string id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string avatar { get; set; }
        public Uri ImagePath { get; set; }
    }

    public class Contracts
    {
        public string id { get; set; }
        public string contract { get; set; }
        public string alias { get; set; }
        public string providerLogo { get; set; }
        public int category_id { get; set; }
        public Providers provider { get; set; }
        public string contract_code { get; set; }

        public string contract_info { get; set; }
        public Uri ImagePath { get; set; }
        public string thereIsNoContract { get; set; }
    }

    public class Providers
    {
        public int id { get; set; }
        public string logo { get; set; }
        public int infoRequestType { get; set; }
        public string contractMask { get; set; }
        public string name { get; set; }
        public string contractFormat { get; set; }
        public string providerCode { get; set; }
        public string contractName { get; set; }
        public string contractRegExp { get; set; }
        public bool disabled { get; set; }
    }
    public class CategoriesProviders
    {
        public int id { get; set; }
        public string categoryName { get; set; }
        public List<PaymentProviders> paymentProviders { get; set; }
        ///////
        public int categoryCount { get; set; }
        public Thickness margin { get; set; }
    }
    public class PaymentProviders
    {
        public int id { get; set; }
        public string logo { get; set; }
        public int infoRequestType { get; set; }
        public string contractMask { get; set; }
        public string name { get; set; }
        public string contractFormat { get; set; }
        public string providerCode { get; set; }
        public string contractName { get; set; }
        public string contractRegExp { get; set; }
        public bool disabled { get; set; }
        //////////
        public int type { get; set; }
    }

    public class CardBankData //last user's choosen card and bank id
    {
        public string clcardid { get; set; }
        public string clbankid { get; set; }
    }

    public class BankType
    {
        public int id { get; set; }
        public int selectedId { get; set; }
        public string name { get; set; }
        public string descr { get; set; }
    }

    public class CardType
    {
        public int id { get; set; }
        public int selectedId { get; set; }
        public string name { get; set; }
        public string descr { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    #region PaymentPanoramaPage.xaml
    public class Payments
    {
        public string operationType { get; set; }
        public string operationStatus { get; set; }
        public string operationAmount { get; set; }
        public string operationAmountCommission { get; set; }
        public string operationDate { get; set; }
        public int operationId { get; set; }
        public string operationText { get; set; }

        //paymentsPanoramaPage.xaml
        public string sumTextToXaml { get; set; }
        public Uri ImagePath { get; set; }
    }
    #endregion
}
