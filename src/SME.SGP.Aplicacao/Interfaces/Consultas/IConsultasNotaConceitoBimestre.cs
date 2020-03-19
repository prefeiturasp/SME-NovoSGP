using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotaConceitoBimestre
    {
        Task<string> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}
