namespace FlowHub_MAUI.Utilities.OtherUtils;

public static class APIKeys
{
    // Static fields to hold keys
    public static string ApplicationId = "o7fm7sKW647VLx9kg022CyeKeVwgBOlw977IhG7s";  // Replace with your actual App ID

    public static string ServerUri = "https://dimer.b4a.io/";  // Back4App server URL
    public static string DotNetKEY = "x0HLQSnvzfufkmBXQJwcHfVAlfW6FtyjS5l0p9b8";  

    public static string UserName = "YBTopaz8";
    public static string Password = "Yvan";
    public static string Email = "8brunel@gmail.com";

    //// Static fields to hold keys
    //public static string ApplicationId = "ZPcqJVoyIqDYODqknB8KR3cgffA4zx67LfXtB85v";  // Replace with your actual App ID

    //public static string ServerUri = "https://flowhub.b4a.io/";  // Back4App server URL
    //public static string DotNetKEY = "IuUtTIslbe94qp7bbPZ7DvTJC0MQMxHid3hJWeCb";  // Replace with your actual Master Key

    //public static string ServerUri = "https://flowhub.b4a.io/";  // Back4App server URL + live Query
    //public static string ServerUri = "https://parseapi.back4app.com/";  // Back4App server URL


    // Method to save the credentials to SecureStorage
    public static async Task SaveKeysToSecureStorage()
    {
        // Store keys securely in SecureStorage
        await SecureStorage.SetAsync("ApplicationId", ApplicationId);
        await SecureStorage.SetAsync("ServerUri", ServerUri);
        //await SecureStorage.SetAsync("ClientKey", ClientKey);
        //await SecureStorage.SetAsync("MasterKey", MasterKey);
        await SecureStorage.SetAsync("DotNetKEY", DotNetKEY);
    }

    

    // Method to retrieve keys from SecureStorage (to be used when initializing ParseClient)
    public static async Task RetrieveKeysFromSecureStorage()
    {
        // Retrieve credentials from SecureStorage
#pragma warning disable CS8601 // Possible null reference assignment.
        ApplicationId = await SecureStorage.GetAsync("ApplicationId");
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
        ServerUri = await SecureStorage.GetAsync("ServerUri");
#pragma warning restore CS8601 // Possible null reference assignment.
        //ClientKey = await SecureStorage.GetAsync("ClientKey");
        //MasterKey = await SecureStorage.GetAsync("MasterKey");
#pragma warning disable CS8601 // Possible null reference assignment.
        DotNetKEY = await SecureStorage.GetAsync("DotNetKEY");
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
