using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommadHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommad, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAtendimentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAAPA;

        public RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommadHandler(
                                                    IMediator mediator, 
                                                    IRepositorioAtendimentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes,
                                                    IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); 
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPAHistoricoAlteracoes));
            this.repositorioSecaoEncaminhamentoNAAPA = repositorioSecaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAAPA));
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommad request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var atendimento = await repositorioSecaoEncaminhamentoNAAPA.ObterAtendimentoSecaoItinerancia(request.EncaminhamentoNAAPASecaoId);
            var historico = new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                EncaminhamentoNAAPAId = atendimento.EncaminhamentoId,
                SecaoEncaminhamentoNAAPAId = atendimento.SecaoEncaminhamentoNAAPAId,
                DataAtendimento = atendimento.DataAtendimento.ToString("dd/MM/yyyy"),
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Exclusao,
                UsuarioId = usuarioLogado.Id
            };

            return await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historico);
        }
    }
}
