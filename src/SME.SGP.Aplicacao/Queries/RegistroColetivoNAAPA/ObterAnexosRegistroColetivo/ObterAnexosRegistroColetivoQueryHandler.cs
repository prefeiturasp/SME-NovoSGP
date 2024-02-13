using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnexosRegistroColetivoQueryHandler : IRequestHandler<ObterAnexosRegistroColetivoQuery, IEnumerable<AnexoDto>>
    {
        private readonly IRepositorioRegistroColetivoAnexo repositorio;

        public ObterAnexosRegistroColetivoQueryHandler(IRepositorioRegistroColetivoAnexo repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<AnexoDto>> Handle(ObterAnexosRegistroColetivoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnexoPorRegistroColetivoId(request.RegistroColetivoId);
        }
    }
}
