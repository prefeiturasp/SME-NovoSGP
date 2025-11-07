using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPorRf
{
    public class ObterUsuariosCoreSsoPorRfQuery : IRequest<UsuarioCoreSsoDto>
    {
        public ObterUsuariosCoreSsoPorRfQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
        }
        public string CodigoRf { get; }
    }
}