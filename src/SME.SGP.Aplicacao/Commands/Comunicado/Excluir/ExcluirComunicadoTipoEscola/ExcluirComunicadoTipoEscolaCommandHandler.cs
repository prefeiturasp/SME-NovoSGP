using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoTipoEscolaCommandHandler : IRequestHandler<ExcluirComunicadoTipoEscolaCommand, bool>
    {
        private readonly IRepositorioComunicadoTipoEscola repositorioComunicadoTipoEscola;

        public ExcluirComunicadoTipoEscolaCommandHandler(IRepositorioComunicadoTipoEscola repositorioComunicadoTipoEscola)
        {
            this.repositorioComunicadoTipoEscola = repositorioComunicadoTipoEscola ?? throw new ArgumentNullException(nameof(repositorioComunicadoTipoEscola));
        }

        public async Task<bool> Handle(ExcluirComunicadoTipoEscolaCommand request, CancellationToken cancellationToken)
            => await repositorioComunicadoTipoEscola.ExcluirPorIdComunicado(request.Id);        
    }
}
