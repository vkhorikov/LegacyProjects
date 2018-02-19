using System;

namespace PackageDelivery.Delivery
{
    public class Dlvr
    {
        public int NMB_CLM { get; set; }
        public int? CSTM { get; set; }
        public string CSTM_NAME { get; set; }
        public string STS { get; set; }

        public string STATUS
        {
            get
            {
                switch (STS)
                {
                    case "N":
                        return "New";
                    case "R":
                        return "Ready";
                    case "P":
                        return "In Progress";
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public double ESTM_CLM { get; set; }
        public int? PRD_LN_1 { get; set; }
        public string PRD_LN_1_AMN { get; set; }
        public int? PRD_LN_2 { get; set; }
        public string PRD_LN_2_AMN { get; set; }
        public int? PRD_LN_3 { get; set; }
        public string PRD_LN_3_AMN { get; set; }
        public int? PRD_LN_4 { get; set; }
        public string PRD_LN_4_AMN { get; set; }
        public string ADDRESS { get; set; }
    }
}
