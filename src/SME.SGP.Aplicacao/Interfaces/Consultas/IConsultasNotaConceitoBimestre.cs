using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotaConceitoBimestre
    {
        Task<AnotacaoAlunoCompletoDto> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}
