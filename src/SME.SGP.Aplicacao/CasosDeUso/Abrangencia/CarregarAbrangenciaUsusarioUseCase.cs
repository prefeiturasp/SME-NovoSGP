using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarAbrangenciaUsusarioUseCase : AbstractUseCase, ICarregarAbrangenciaUsusarioUseCase
    {
        public CarregarAbrangenciaUsusarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(UsuarioPerfilDto usuarioPerfil)
        {
            await mediator.Send(new CarregarAbrangenciaUsuarioCommand(usuarioPerfil.Login, usuarioPerfil.Perfil));
            return true;
        }
    }
}
