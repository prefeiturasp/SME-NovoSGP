using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseRecomendacao
    {
        Task<ConsultasConselhoClasseRecomendacaoConsultaDto> ObterRecomendacoesAlunoFamilia(string codigoTurma, string codigoAluno, int bimestre);
    }
}