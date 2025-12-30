using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPaginado
{
    public class ObterUsuariosCoreSsoPaginadoQuery : IRequest<PaginacaoResultadoDto<UsuarioCoreSsoDto>>
    {
        public ObterUsuariosCoreSsoPaginadoQuery(int pagina = 1, int registrosPorPagina = 10, string rf = null, string nome = null)
        {
            Pagina = pagina;
            RegistrosPorPagina = registrosPorPagina;
            Rf = rf;
            Nome = nome;
        }
        public int Pagina { get; }
        public int RegistrosPorPagina { get; }
        public string Rf { get; }
        public string Nome { get; }
    }
}