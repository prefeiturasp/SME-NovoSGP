using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IListarFechamentoTurmaBimestreUseCase
    {
        Task<FechamentoNotaConceitoTurmaDto> Executar(string turmaCodigo, long componenteCurricularCodigo, int bimestre, int? semestre);
    }
}
