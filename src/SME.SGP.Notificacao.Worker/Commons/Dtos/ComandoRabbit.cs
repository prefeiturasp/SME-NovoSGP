using System;

namespace SME.SGP.Notificacao.Worker
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
