using System;

namespace SME.SGP.Notificacoes.Worker
{
    public class ComandoRabbit
    {
        public ComandoRabbit(string nomeProcesso, string endpoint)
        {
            NomeProcesso = nomeProcesso;
            Endpoint = endpoint;
        }

        public string NomeProcesso { get; }
        public string Endpoint { get; }
    }
}
