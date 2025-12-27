namespace FimsCPK.Data
{
    public class CoolantInfo
    {
        public string serial { get; set; }
        public string fsc { get; set; }
        public string model { get; set; }
        public DateTime dateStarted { get; set; }
        public DateTime dateEnded { get; set; }
        public int NumCh { get; set; }

        public string coolantname1 { get; set; }
        public double coolantcharge1 { get; set; }
        public double coolantdrain11 { get; set; }
        public double coolantdrain12 { get; set; }

        public string coolantname2 { get; set; }
        public double coolantcharge2 { get; set; }
        public double coolantdrain21 { get; set; }
        public double coolantdrain22 { get; set; }

        public string coolantname3 { get; set; }
        public double coolantcharge3 { get; set; }
        public double coolantdrain31 { get; set; }
        public double coolantdrain32 { get; set; }

        public string coolantname4 { get; set; }
        public double coolantcharge4 { get; set; }
        public double coolantdrain41 { get; set; }
        public double coolantdrain42 { get; set; }
    }

    public class CoolantAmount
    {
        public string coolantname { get; set; }
        public double amount_purchase { get; set; }     // 구입량
        public double amount_usage { get; set; }        // 투입량
        public double amount_drain { get; set; }        // 회수량
        public double amount_notdrain { get; set; }        // 미회수량
        public double amount_remain { get; set; }       // 재고량 = 구입량 - 투입량 + 회수량
    }

    public class CoolantPurchaseInfo
    {
        public int Id { get; set; }
        public string CoolantName { get; set; }
        public double amount_purchase { get; set; }
        public string ? Etc { get; set; }
        public DateTime dtPurchased { get; set; }
        public DateTime? dtUpdated { get; set; }
    }
}
