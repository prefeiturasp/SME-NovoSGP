using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IServicoMensageria servicoMensageria;

        public PublicarFilaSgpCommandHandler(IServicoMensageria servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             command.Usuario?.Nome,
                                             command.Usuario?.CodigoRf,
                                             command.Usuario?.PerfilAtual,
                                             command.NotificarErroUsuario);

            await servicoMensageria.Publicar(mensagem, command.Rota, command.Exchange ?? ExchangeSgpRabbit.Sgp, "PublicarFilaSgp");

            return true;
        }
    }
}
