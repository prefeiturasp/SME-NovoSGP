using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoAusenciaPorIdQueryHandler : IRequestHandler<ObterMotivoAusenciaPorIdQuery, MotivoAusencia>
    {
        private readonly IRepositorioMotivoAusencia repositorioMotivoAusencia;

        public ObterMotivoAusenciaPorIdQueryHandler(IRepositorioMotivoAusencia repositorioMotivoAusencia)
        {
            this.repositorioMotivoAusencia = repositorioMotivoAusencia ?? throw new ArgumentNullException(nameof(repositorioMotivoAusencia));
        }
        public async Task<MotivoAusencia> Handle(ObterMotivoAusenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            var motivoAusencia =  await repositorioMotivoAusencia.ObterPorIdAsync(request.Id);
         
            return motivoAusencia;
        }
    }
}
