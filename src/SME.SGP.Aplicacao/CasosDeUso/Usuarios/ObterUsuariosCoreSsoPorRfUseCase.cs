using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios;
using SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPorRf;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Usuarios
{
    public class ObterUsuariosCoreSsoPorRfUseCase : IObterUsuariosCoreSsoPorRfUseCase
    {
        private readonly IMediator _mediator;
        public ObterUsuariosCoreSsoPorRfUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<UsuarioCoreSsoDto> Executar(string codigoRf)
        {
            return await _mediator.Send(new ObterUsuariosCoreSsoPorRfQuery(codigoRf));
        }
    }
}