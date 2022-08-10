using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSerapEstudantesCommandHandler : IRequestHandler<PublicarFilaSerapEstudantesCommand, bool>
    {
        private readonly IServicoMensageriaSGP servicoMensageria;

        public PublicarFilaSerapEstudantesCommandHandler(IServicoMensageriaSGP servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new System.ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Handle(PublicarFilaSerapEstudantesCommand request, CancellationToken cancellationToken)
            => servicoMensageria.Publicar(new MensagemRabbit(request.Mensagem),
                                              request.Fila,
                                              RotasRabbitSerapEstudantes.ExchangeSerapEstudantes,
                                              "PublicarFilaSerap");
    }
}
