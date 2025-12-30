using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios
{
    public interface IObterUsuariosCoreSsoUseCase
    {
        Task<PaginacaoResultadoDto<UsuarioCoreSsoDto>> Executar(string rf, string nome, int pagina, int registrosPorPagina);
    }
}