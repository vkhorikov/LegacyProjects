namespace PackageDelivery.Delivery
{
    public class Cstm
    {
        public int NMB_CLM { get; set; }
        public string NM_CLM { get; set; }
        public string ADDR { get; set; }
        public int? ADDR1 { get; set; }
        public int? ADDR2 { get; set; }

        public string NM_ADDR => NM_CLM.Trim() + ", " + ADDR;
    }
}
