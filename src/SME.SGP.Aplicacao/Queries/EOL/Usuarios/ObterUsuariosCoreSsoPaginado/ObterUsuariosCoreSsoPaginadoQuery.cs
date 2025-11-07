using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPaginado
{
    public class ObterUsuariosCoreSsoPaginadoQuery : IRequest<PaginacaoResultadoDto<UsuarioCoreSsoDto>>
    {
        public ObterUsuariosCoreSsoPaginadoQuery(int pagina = 1, int registrosPorPagina = 10)
        {
            Pagina = pagina;
            RegistrosPorPagina = registrosPorPagina;
        }
        public int Pagina { get; }
        public int RegistrosPorPagina { get; }
    }
}