using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterFotosSemestreAlunoUseCase : IUseCase<long, IEnumerable<(byte[], string, string)>>
    {
    }
}
