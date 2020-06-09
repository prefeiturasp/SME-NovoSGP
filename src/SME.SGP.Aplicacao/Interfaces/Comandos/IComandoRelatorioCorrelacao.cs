using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandoRelatorioCorrelacao
    {
        Task<long> Salvar(RelatorioCorrelacao relatorioCorrelacao);
    }
}