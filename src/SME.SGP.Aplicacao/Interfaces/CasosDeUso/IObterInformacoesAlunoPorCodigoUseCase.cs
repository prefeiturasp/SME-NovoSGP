using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterInformacoesAlunoPorCodigoUseCase
    {
        Task<AlunoEnderecoRespostaDto> Executar(string codigoAluno);
    }
}
