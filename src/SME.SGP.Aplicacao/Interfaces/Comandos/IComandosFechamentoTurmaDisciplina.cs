using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoTurmaDisciplina
    {
        Task<IEnumerable<AuditoriaFechamentoTurmaDto>> Salvar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma);
    }
}
