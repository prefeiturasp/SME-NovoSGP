using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamentoAluno
    {
        Task<FechamentoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo);
        Task<AnotacaoFechamentoAluno> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}