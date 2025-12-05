using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterHistoricosDeAlteracoesAtendimentoNAAPAQueryHandlerFake : IRequestHandler<ObterHistoricosDeAlteracoesAtendimentoNAAPAQuery, EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public async Task<EncaminhamentoNAAPAHistoricoAlteracoes> Handle(ObterHistoricosDeAlteracoesAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new EncaminhamentoNAAPAHistoricoAlteracoes
            {
                Id = 1,
                EncaminhamentoNAAPAId = 1,
                UsuarioId = 1,
                CamposInseridos = string.Empty,
                CamposAlterados = string.Empty,
                DataAtendimento = DateTime.Now.ToString(),
                DataHistorico = DateTime.Now,
                TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Impressao
            });
        }
    }
}