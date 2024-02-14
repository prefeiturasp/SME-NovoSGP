using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposReuniaoNAAPAQueryHandler : IRequestHandler<ObterTiposReuniaoNAAPAQuery, IEnumerable<TipoReuniaoDto>>
    {
        private readonly IRepositorioTipoReuniaoNAAPA repositorio;

        public ObterTiposReuniaoNAAPAQueryHandler(IRepositorioTipoReuniaoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<TipoReuniaoDto>> Handle(ObterTiposReuniaoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterTiposDeReuniao();
        }
    }
}
