using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseRecomendacao
    {
        Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int? bimestre);
    }
}