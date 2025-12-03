using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarAtendimentoNAAPACommandHandler : IRequestHandler<EncerrarAtendimentoNAAPACommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public EncerrarAtendimentoNAAPACommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IMediator mediator { get; }
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public async Task<bool> Handle(EncerrarAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterCabecalhoEncaminhamentoNAAPAQuery(request.EncaminhamentoId));
            
            if (encaminhamentoNAAPA.EhNulo() || encaminhamentoNAAPA.Id == 0)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var encaminhamentoNAAPAPersistido = encaminhamentoNAAPA.Clone();
            encaminhamentoNAAPA.MotivoEncerramento = request.MotivoEncerramento;
            encaminhamentoNAAPA.Situacao = Dominio.Enumerados.SituacaoNAAPA.Encerrado;

            long idEntidadeEncaminhamento = 0;
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    idEntidadeEncaminhamento = await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamentoNAAPA);
                    await mediator.Send(new RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand(encaminhamentoNAAPAPersistido, encaminhamentoNAAPA.Situacao));
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return idEntidadeEncaminhamento != 0;
        }        
    }
}
