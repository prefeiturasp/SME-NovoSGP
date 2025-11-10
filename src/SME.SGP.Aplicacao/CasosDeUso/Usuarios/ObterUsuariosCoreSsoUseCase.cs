using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios;
using SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPaginado;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Usuarios
{
    public class ObterUsuariosCoreSsoUseCase : IObterUsuariosCoreSsoUseCase
    {
        private readonly IMediator _mediator;

        public ObterUsuariosCoreSsoUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<PaginacaoResultadoDto<UsuarioCoreSsoDto>> Executar(string rf, string nome, int pagina, int registrosPorPagina)
        {
            return await _mediator.Send(new ObterUsuariosCoreSsoPaginadoQuery(pagina, registrosPorPagina, rf, nome));
        }
    }
}