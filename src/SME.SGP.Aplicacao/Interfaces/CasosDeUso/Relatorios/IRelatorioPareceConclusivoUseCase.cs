using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioParecerConclusivoUseCase
    {
        Task<bool> Executar(FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto);
    }
}