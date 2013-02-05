using System;

namespace myth.Classes
{
    public class ExceptionsHandler
    {
        public static ErrorDescription getErrDescriptionByCode(string errorCode)
        {
            switch (errorCode)
            {
                case "902": //login or password is incorrect
                    return setErrDescriptionDetails("/Views/Internal/LoginPage.xaml");
                case "916": //email or phone number is not confirmed
                    return setErrDescriptionDetails("/Views/Internal/EmailPhoneConfirmationPage.xaml");
            }
            return null;
        }

        private static ErrorDescription setErrDescriptionDetails(string pageUrl)
        {
            ErrorDescription errorDescription = new ErrorDescription();

            errorDescription.message = GlobalVariables.jp.message;
            errorDescription.messageTitle = GlobalVariables.jp.messageTitle;
            errorDescription.url = new Uri(pageUrl, UriKind.Relative);

            return errorDescription;
        }
    }

    public class ErrorDescription
    {
        public Uri url;
        public string message = "";
        public string messageTitle = "";
    }
}
