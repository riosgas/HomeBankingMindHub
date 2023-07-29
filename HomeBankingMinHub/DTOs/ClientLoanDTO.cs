using HomeBankingMindHub.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.dtos
{
    public class ClientLoanDTO
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
    }
}
