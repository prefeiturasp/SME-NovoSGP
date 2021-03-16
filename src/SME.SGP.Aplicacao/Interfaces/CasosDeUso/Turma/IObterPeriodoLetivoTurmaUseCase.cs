using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma
{
    public interface IObterPeriodoLetivoTurmaUseCase
    {
        Task<PeriodoEscolarLetivoTurmaDto> Executar(string codigoTurma);
    }
}
