using System.IO.IsolatedStorage;

namespace myth.Classes
{
    public class UserInfo
    {
        private string phoneNumber;
        private string email;
        public string password { get; set; }

        public string lastname { get; set; }
        public string nickname { get; set; }
        public string name { get; set; }
        public string midname { get; set; }
        public string birthDate { get; set; }
        public string gender { get; set; }
        public string secretQuestion { get; set; }
        public string secretAnswer { get; set; }

        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set
            {
                if (value != "")
                {
                    string thePhonePattern = "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]";
                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(thePhonePattern);
                    if (r.IsMatch(value))
                        this.phoneNumber = value;
                }
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (value != "")
                {
                    string theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                       + "@"
                                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(theEmailPattern);
                    if (r.IsMatch(value))
                        this.email = value;
                }
            }
        }

        public void getData()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            AppSettings appSettings = new AppSettings();

            this.Email = appSettings.GetValueOrDefault<string>("userSettingEmail", "");
            this.PhoneNumber = appSettings.GetValueOrDefault<string>("userSettingPhoneNumber", "");
            this.password = appSettings.GetValueOrDefault<string>("userSettingPassword", "");

            this.name = appSettings.GetValueOrDefault<string>("userSettingName", "");
            this.nickname = appSettings.GetValueOrDefault<string>("userSettingNickname", "");
            this.lastname = appSettings.GetValueOrDefault<string>("userSettingLastname", "");
            this.midname = appSettings.GetValueOrDefault<string>("userSettingMidname", "");
            this.gender = appSettings.GetValueOrDefault<string>("userSettingGender", "");
            this.birthDate = appSettings.GetValueOrDefault<string>("userSettingBirthDate", "");

            this.secretQuestion = appSettings.GetValueOrDefault<string>("userSettingSecretQuestion", "");
            this.secretAnswer = appSettings.GetValueOrDefault<string>("userSettingSecretAnswer", "");
        }

        public void saveData()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            AppSettings appSettings = new AppSettings();

            appSettings.AddOrUpdateValue("userSettingEmail", this.Email);
            appSettings.AddOrUpdateValue("userSettingPhoneNumber", this.PhoneNumber);
            appSettings.AddOrUpdateValue("userSettingPassword", this.password);


            appSettings.AddOrUpdateValue("userSettingName", this.name);
            appSettings.AddOrUpdateValue("userSettingNickname", this.nickname);
            appSettings.AddOrUpdateValue("userSettingLastname", this.lastname);
            appSettings.AddOrUpdateValue("userSettingMidname", this.midname);
            appSettings.AddOrUpdateValue("userSettingGender", this.gender);
            appSettings.AddOrUpdateValue("userSettingBirthDate", this.birthDate);

            appSettings.AddOrUpdateValue("userSettingSecretQuestion", this.secretQuestion);
            appSettings.AddOrUpdateValue("userSettingSecretAnswer", this.secretAnswer);
        }
    }
}
