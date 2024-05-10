using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoModalidadeCommandHandler : IRequestHandler<InserirComunicadoModalidadeCommand, bool>
    {
        private readonly IRepositorioComunicadoModalidade repositorioComunicadoModalidade;

        public InserirComunicadoModalidadeCommandHandler(IRepositorioComunicadoModalidade repositorioComunicadoModalidade)
        {
            this.repositorioComunicadoModalidade = repositorioComunicadoModalidade ?? throw new ArgumentNullException(nameof(repositorioComunicadoModalidade));
        }

        public async Task<bool> Handle(InserirComunicadoModalidadeCommand request, CancellationToken cancellationToken)
        {
            foreach(var modalidade in request.Comunicado.Modalidades)            
                await repositorioComunicadoModalidade.SalvarAsync(new Dominio.ComunicadoModalidade { ComunicadoId = request.Comunicado.Id, Modalidade = modalidade });

            return true;            
        }
    }
}
