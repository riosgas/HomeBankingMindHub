using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Models;
using System.Collections.Generic;

namespace HomeBankingMindHub.Services
{
    public interface IClientService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        ClientDTO CreateClient(CreateClientRequestDTO client);
        ClientDTO GetCurrentClient();
    }
}
