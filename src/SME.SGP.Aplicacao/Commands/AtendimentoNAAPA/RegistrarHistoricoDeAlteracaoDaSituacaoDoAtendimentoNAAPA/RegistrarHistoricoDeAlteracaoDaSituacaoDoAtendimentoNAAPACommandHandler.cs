using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPAHistoricoAlteracoes));
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var historicoAlteracao = await ObterHistoricoAlteracaoDaSituacao(request.EncaminhamentoNAAPA, request.SituacaoAlterada);

            if (historicoAlteracao.NaoEhNulo())
                return await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historicoAlteracao);

            return 0;
        }

        private async Task<EncaminhamentoNAAPAHistoricoAlteracoes> ObterHistoricoAlteracaoDaSituacao(EncaminhamentoNAAPA encaminhamento, SituacaoNAAPA situacao)
        {
            if (encaminhamento.Situacao != situacao)
            {
                var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

                return new EncaminhamentoNAAPAHistoricoAlteracoes()
                {
                    EncaminhamentoNAAPAId = encaminhamento.Id,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao,
                    CamposAlterados = "Situação",
                    UsuarioId = usuarioLogado.Id
                };
            }

            return null;
        }
    }
}
