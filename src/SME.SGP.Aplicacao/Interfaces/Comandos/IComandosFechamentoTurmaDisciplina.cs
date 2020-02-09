using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoTurmaDisciplina
    {
        Task<IEnumerable<AuditoriaFechamentoTurmaDto>> Inserir(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma);
        Task<IEnumerable<AuditoriaFechamentoTurmaDto>> Alterar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma);
    }
}
