using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios
{
    public interface IObterUsuariosCoreSsoUseCase
    {
        Task<PaginacaoResultadoDto<UsuarioCoreSsoDto>> Executar(int pagina, int registrosPorPagina);
    }
}
