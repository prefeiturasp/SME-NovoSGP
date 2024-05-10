using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoTipoEscolaCommandHandler : IRequestHandler<InserirComunicadoTipoEscolaCommand, bool>
    {
        private readonly IRepositorioComunicadoTipoEscola repositorioComunicadoTipoEscola;

        public InserirComunicadoTipoEscolaCommandHandler(IRepositorioComunicadoTipoEscola repositorioComunicadoTipoEscola)
        {
            this.repositorioComunicadoTipoEscola = repositorioComunicadoTipoEscola ?? throw new ArgumentNullException(nameof(repositorioComunicadoTipoEscola));
        }

        public async Task<bool> Handle(InserirComunicadoTipoEscolaCommand request, CancellationToken cancellationToken)
        {
            foreach (var tipoEscola in request.Comunicado.TiposEscolas)
                await repositorioComunicadoTipoEscola.SalvarAsync(new Dominio.ComunicadoTipoEscola { ComunicadoId = request.Comunicado.Id, TipoEscola = tipoEscola });

            return true;
        }
    }
}
