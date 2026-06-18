using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma
{
    public interface IObterTurmasPapPorAnoLetivoUseCase
    {
        Task<IEnumerable<TurmasPapDto>> Executar(long anoLetivo, string codigoUe);
    }
}