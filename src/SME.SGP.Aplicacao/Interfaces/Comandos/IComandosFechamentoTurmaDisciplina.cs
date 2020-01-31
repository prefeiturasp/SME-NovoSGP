using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoTurmaDisciplina
    {
        Task<AuditoriaDto> Inserir(FechamentoTurmaDisciplinaDto fechamentoTurma);
        Task<AuditoriaDto> Alterar(long id, FechamentoTurmaDisciplinaDto fechamentoTurma);
    }
}
