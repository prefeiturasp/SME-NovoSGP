using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommadHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAAPA;

        public RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommadHandler(
                                                    IMediator mediator, 
                                                    IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes,
                                                    IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator;
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes;
            this.repositorioSecaoEncaminhamentoNAAPA = repositorioSecaoEncaminhamentoNAAPA;
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var atendimento = await repositorioSecaoEncaminhamentoNAAPA.ObterAtendimentoSecaoItinerancia(request.EncaminhamentoNAAPASecaoId);
            var historico = new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                EncaminhamentoNAAPAId = atendimento.EncaminhamentoId,
                SecaoEncaminhamentoNAAPAId = atendimento.SecaoEncaminhamentoNAAPAId,
                DataAtendimento = atendimento.DataDoAtendimento.ToString("dd/MM/yyyy"),
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Exclusao,
                CamposAlterados = "Situação",
                UsuarioId = usuarioLogado.Id
            };

            return await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historico);
        }
    }
}
