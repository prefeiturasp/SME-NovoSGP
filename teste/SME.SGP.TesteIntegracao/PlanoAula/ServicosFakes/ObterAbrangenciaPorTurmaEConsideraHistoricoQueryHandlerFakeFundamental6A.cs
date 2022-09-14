using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes
{
    public class ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A: IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>
    {
        private readonly string DRE_CODIGO_1 = "1";
        private readonly string DRE_NOME_1 = "NOME DRE 1";
        private readonly string TURMA_CODIGO_1 = "1";
        private readonly string TURMA_ANO_6 = "6";
        private readonly string TURMA_6A = "6A";
        private readonly string UE_CODIGO_1 = "1";
        private readonly string UE_NOME_1 = "NOME UE 1";
        
        public async Task<AbrangenciaFiltroRetorno> Handle(ObterAbrangenciaPorTurmaEConsideraHistoricoQuery request,CancellationToken cancellationToken)
        {
            return new AbrangenciaFiltroRetorno()
            {
                Modalidade = Modalidade.Fundamental,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Ano = TURMA_ANO_6,
                CodigoDre = DRE_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                TipoEscola = TipoEscola.CEUEMEF,
                NomeDre = DRE_NOME_1,
                NomeTurma = TURMA_6A,
                NomeUe = UE_NOME_1,
                Semestre = 0,
                QtDuracaoAula = 7,
                TipoTurno = 6
            };
        }
    }
}