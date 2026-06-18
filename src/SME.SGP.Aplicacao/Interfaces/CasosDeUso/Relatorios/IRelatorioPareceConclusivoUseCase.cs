using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioParecerConclusivoUseCase
    {
        Task<bool> Executar(FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto);
    }
}