using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCorrelacaoRelatorioJasper
    {
        long Salvar(RelatorioCorrelacaoJasper relatorioCorrelacaoJasper);
    }
}