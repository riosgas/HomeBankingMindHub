﻿namespace HomeBankingMindHub.dtos
{
    public class LoanApplicationDTO
    {
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
        public string ToAccountNumber { get; set; }
    }
}
