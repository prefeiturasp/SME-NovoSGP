using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoRelatorioPeriodicoPAP : IRepositorioBase<SecaoRelatorioPeriodicoPAP>
    {
        Task<SecaoTurmaAlunoPAPDto> ObterSecoesPorAluno(string codigoTurma, string codigoAluno, long pAPPeriodoId);
    }
}
