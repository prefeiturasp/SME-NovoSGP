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

        public async Task<bool> Executar(UsuarioPerfilDto param)
        {
            await mediator.Send(new CarregarAbrangenciaUsuarioCommand(param.Login, param.Perfil));
            return true;
        }
    }
}
