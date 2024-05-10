using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterInformacoesEscolaresDoAlunoUseCase
    {
        Task<InformacoesEscolaresAlunoDto> Executar(string codigoAluno, string turmaId);
    }
}
