using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaGoogleClassroomCommandHandler : IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>
    {
        private readonly IServicoMensageriaSGP servicoMensageria;

        public PublicarFilaGoogleClassroomCommandHandler(IServicoMensageriaSGP servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new System.ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Handle(PublicarFilaGoogleClassroomCommand request, CancellationToken cancellationToken)
            => servicoMensageria.Publicar(new MensagemRabbit(request.Mensagem),
                                              request.Fila,
                                              RotasRabbitSgpGoogleClassroomApi.ExchangeGoogleSync,
                                              "PublicarFilaGCA");
    }
}
