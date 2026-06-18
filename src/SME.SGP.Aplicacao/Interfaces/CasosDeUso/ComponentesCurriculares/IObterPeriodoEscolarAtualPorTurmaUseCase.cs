using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodoEscolarAtualPorTurmaUseCase
    {
        Task<PeriodoDto> Executar(long turmaId, DateTime dataReferencia);
    }
}
