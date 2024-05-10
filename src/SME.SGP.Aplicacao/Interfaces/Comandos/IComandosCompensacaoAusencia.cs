using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosCompensacaoAusencia
    {
        Task Inserir(CompensacaoAusenciaDto compensacao);
        Task Alterar(long id, CompensacaoAusenciaDto compensacao);
    }
}
