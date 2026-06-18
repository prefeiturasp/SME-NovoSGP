using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterInformacoesAlunoPorCodigoUseCase
    {
        Task<AlunoEnderecoRespostaDto> Executar(string codigoAluno);
    }
}
