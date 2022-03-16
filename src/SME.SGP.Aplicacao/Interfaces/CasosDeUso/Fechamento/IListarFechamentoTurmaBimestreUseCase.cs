using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IListarFechamentoTurmaBimestreUseCase 
    {
        Task<FechamentoNotaConceitoTurmaDto> Executar(string turmaCodigo, long componenteCurricularCodigo, int bimestre, int? semestre);
    }
}
