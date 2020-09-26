using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase
    {
        Task<IEnumerable<AlunoPorTurmaResposta>> Executar(string codigoTurma, int anoLetivo);
    }

    
}
