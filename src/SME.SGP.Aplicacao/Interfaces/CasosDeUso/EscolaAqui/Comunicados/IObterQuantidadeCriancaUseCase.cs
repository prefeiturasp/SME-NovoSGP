using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterQuantidadeCriancaUseCase
    {
        Task<QuantidadeCriancaDto> Executar(string anoTurma, string turma, string dreId, string ueId, int modalidade, int anoLetivo);

    }
}
