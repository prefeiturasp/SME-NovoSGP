using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarRegistroAcaoUseCase
    {
        Task<ResultadoRegistroAcaoBuscaAtivaDto> Executar(RegistroAcaoBuscaAtivaDto registroAcaoDto);
    }
}
