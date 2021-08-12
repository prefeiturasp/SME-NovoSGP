using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoAnoEscolarCommandHandler : IRequestHandler<InserirComunicadoAnoEscolarCommand, bool>
    {
        private readonly IRepositorioComunicadoAnoEscolar repositorioComunicadoAnoEscolar;

        public InserirComunicadoAnoEscolarCommandHandler(IRepositorioComunicadoAnoEscolar repositorioComunicadoTipoEscola)
        {
            this.repositorioComunicadoAnoEscolar = repositorioComunicadoAnoEscolar ?? throw new ArgumentNullException(nameof(repositorioComunicadoAnoEscolar));
        }

        public async Task<bool> Handle(InserirComunicadoAnoEscolarCommand request, CancellationToken cancellationToken)
        {
            foreach (var anoEscolar in request.Comunicado.AnosEscolares)
                await repositorioComunicadoAnoEscolar.SalvarAsync(new Dominio.ComunicadoAnoEscolar { ComunicadoId = request.Comunicado.Id, AnoEscolar = anoEscolar });

            return true;
        }
    }
}
