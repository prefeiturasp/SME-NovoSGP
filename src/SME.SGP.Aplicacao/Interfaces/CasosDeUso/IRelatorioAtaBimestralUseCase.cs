using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioAtaBimestralUseCase
    {
        Task<bool> Executar(FiltroRelatorioAtaBimestralDto filtro);
    }
}