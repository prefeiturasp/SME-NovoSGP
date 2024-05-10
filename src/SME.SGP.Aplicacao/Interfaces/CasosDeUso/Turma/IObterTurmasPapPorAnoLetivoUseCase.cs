using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma
{
    public interface IObterTurmasPapPorAnoLetivoUseCase
    {
        Task<IEnumerable<TurmasPapDto>> Executar(long anoLetivo);
    }
}