namespace FimsCPK.Common
{
    public static class ApiEndPoints
    {
        public const string BaseUrl = "https://localhost:7095";
        public static string SaveUserConfig => $"{BaseUrl}/api/userconfig/save";
        public static string LoadUserConfig => $"{BaseUrl}/api/userconfig/load";
    }
}
