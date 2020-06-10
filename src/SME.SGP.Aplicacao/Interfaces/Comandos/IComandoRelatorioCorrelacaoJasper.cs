using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandoRelatorioCorrelacaoJasper
    {
        Task<long> Salvar(RelatorioCorrelacaoJasper relatorioCorrelacaoJasper);
    }
}