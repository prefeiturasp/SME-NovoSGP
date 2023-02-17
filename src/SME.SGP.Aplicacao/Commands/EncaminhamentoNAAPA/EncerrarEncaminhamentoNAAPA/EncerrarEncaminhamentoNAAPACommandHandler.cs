using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoNAAPACommandHandler : IRequestHandler<EncerrarEncaminhamentoNAAPACommand, bool>
    {
        public EncerrarEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public async Task<bool> Handle(EncerrarEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(request.EncaminhamentoId));
            
            if (encaminhamentoNAAPA == null || encaminhamentoNAAPA.Id == 0)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);
            
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            
            encaminhamentoNAAPA.MotivoEncerramento = request.MotivoEncerramento;
            encaminhamentoNAAPA.Situacao = Dominio.Enumerados.SituacaoNAAPA.Encerrado;
            
            var idEntidadeEncaminhamento = await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamentoNAAPA);
            
            return idEntidadeEncaminhamento != 0;
        }        
    }
}
