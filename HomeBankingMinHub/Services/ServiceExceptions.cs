using System;

namespace HomeBankingMindHub.Services
{
    public class ServiceExceptions
    {
        public class ClientNotFoundException : Exception
        {
            public ClientNotFoundException() : base("No se encontró el cliente") { }
        }
        public class MaxAccountsException : Exception
        {
            public MaxAccountsException() : base("El cliente tiene 3 cuentas, no es posible crear otra") { }
        }
        public class InvalidRequest : Exception
        {
            public InvalidRequest() : base("Datos no válidos") { }
        }
        public class EmailAlreadyUsed : Exception
        {
            public EmailAlreadyUsed() : base("Email está en uso") { }
        }
    }
}
