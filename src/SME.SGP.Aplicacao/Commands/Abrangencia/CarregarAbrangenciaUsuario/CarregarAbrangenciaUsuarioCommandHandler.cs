using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarAbrangenciaUsuarioCommandHandler : AsyncRequestHandler<CarregarAbrangenciaUsuarioCommand>
    {
        private readonly IServicoAbrangencia servicoAbrangencia;

        public CarregarAbrangenciaUsuarioCommandHandler(IServicoAbrangencia servicoAbrangencia)
        {
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
        }

        protected override Task Handle(CarregarAbrangenciaUsuarioCommand request, CancellationToken cancellationToken)
        {
            return servicoAbrangencia.Salvar(request.Login, request.Perfil, true);
        }
    }
}
