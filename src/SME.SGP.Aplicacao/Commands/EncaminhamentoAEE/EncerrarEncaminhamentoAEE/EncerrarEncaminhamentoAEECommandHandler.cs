using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEECommandHandler : IRequestHandler<EncerrarEncaminhamentoAEECommand, bool>
    {
        public EncerrarEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public async Task<bool> Handle(EncerrarEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");
            
            encaminhamentoAEE.MotivoEncerramento = request.MotivoEncerramento;
            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Indeferido;

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await mediator.Send(new ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand(encaminhamentoAEE.Id, usuarioLogado.CodigoRf, usuarioLogado.Nome));
            
            return idEntidadeEncaminhamento != 0;

        }        
    }
}
