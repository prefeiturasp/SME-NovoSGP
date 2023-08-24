using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosPorPeriodoPAPUseCase
    {
        Task<IEnumerable<AlunoDadosBasicosDto>> Executar(string codigoTurma, long periodoRelatorioPAPId);
    }
}
