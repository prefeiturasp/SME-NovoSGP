using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase
    {
        Task<IEnumerable<PeriodoEscolarDto>> Executar(Modalidade modalidade, int anoLetivo, int? semestre);
    }
}