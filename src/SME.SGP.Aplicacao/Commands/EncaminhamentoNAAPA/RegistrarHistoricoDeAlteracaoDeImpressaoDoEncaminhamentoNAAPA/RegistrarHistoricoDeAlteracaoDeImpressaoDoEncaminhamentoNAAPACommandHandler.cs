using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator;
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes;
        }

        public async Task<bool> Handle(RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {

            foreach(var encaminhamentoId in request.EncaminhamentoNaapaIds)
            {
                var historico = new EncaminhamentoNAAPAHistoricoAlteracoes()
                {
                    EncaminhamentoNAAPAId = encaminhamentoId,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao,
                    UsuarioId = request.UsuarioLogadoId
                };

                await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historico);
            }

            return true;
        }
    }
}
