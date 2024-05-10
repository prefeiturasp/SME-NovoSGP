using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterModalidadeAnoItineranciaProgramaUseCase
    {
        Task<IEnumerable<RetornoModalidadesPorAnoItineranciaProgramaDto>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre);
    }
}
