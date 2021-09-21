using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioAtaBimestralUseCase
    {
        Task<bool> Executar(FiltroRelatorioAtaBimestralDto filtro);
    }
}