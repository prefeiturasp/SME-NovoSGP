using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandlerFake : IRequestHandler<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery, EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public async Task<EncaminhamentoNAAPAHistoricoAlteracoes> Handle(ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return new EncaminhamentoNAAPAHistoricoAlteracoes
            {
                Id = 1,
                EncaminhamentoNAAPAId = 1,
                UsuarioId = 1,
                CamposInseridos = string.Empty,
                CamposAlterados = string.Empty,
                DataAtendimento = DateTime.Now.ToString(),
                DataHistorico = DateTime.Now,
                TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao
            };
        }
    }
}