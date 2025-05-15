namespace FimsCPK.Data
{
    public class UserConfiguration
    {
        public string UserId { get; set; } = "2101521";
        public bool bDuration{ get; set; } = false;
        public DateTime dStartDate { get; set; } = DateTime.Now;
        public DateTime dEndDate { get; set; } = DateTime.Now;
        public string ModelName { get; set; } = "TestModel";
        public string TestItem { get; set; } = "3000";

        public string ConfigName { get; set; } = "cpk";   // 설정종류 구분자  (Cpk: Cpk검색 정보, CpkSettings: CpkSettings 정보, ...)
        public string Etc1 { get; set; } = "etc1";
        public string Etc2 { get; set; } = "etc2";
    }
}
