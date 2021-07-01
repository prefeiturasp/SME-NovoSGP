using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoModalidadesCommandHandler : IRequestHandler<ExcluirComunicadoModalidadesCommand, bool>
    {
        private readonly IRepositorioComunicadoModalidade repositorioComunicadoModalidade;

        public ExcluirComunicadoModalidadesCommandHandler(IRepositorioComunicadoModalidade repositorioComunicadoModalidade)
        {
            this.repositorioComunicadoModalidade = repositorioComunicadoModalidade ?? throw new ArgumentNullException(nameof(repositorioComunicadoModalidade));
        }

        public async Task<bool> Handle(ExcluirComunicadoModalidadesCommand request, CancellationToken cancellationToken)
        {
            await repositorioComunicadoModalidade.ExcluirPorIdComunicado(request.Id);

            return true;
        }
    }
}
