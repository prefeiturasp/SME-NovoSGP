using MediatR;

namespace SME.SGP.Metrica.Worker.Commands
{
    public class PublicarFilaCommand : IRequest
    {
        public PublicarFilaCommand(string rota, object mensagem)
        {
            Rota = rota;
            Mensagem = mensagem;
        }

        public string Rota { get; }
        public object Mensagem { get; }
    }
}
