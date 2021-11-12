using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.AtividadeInfantil.ObterListaAtividadesMural
{
    public class ObterListaAtividadesMuralPorAulaIdQueryHandler : IRequestHandler<ObterListaAtividadesMuralPorAulaIdQuery, IEnumerable<AtividadeInfantilDto>>
    {
        private readonly IRepositorioAtividadeInfantil repositorioAtividadeInfantil;
        public ObterListaAtividadesMuralPorAulaIdQueryHandler(IRepositorioAtividadeInfantil repositorioAtividadeInfantil)
        {
            this.repositorioAtividadeInfantil = repositorioAtividadeInfantil ?? throw new ArgumentNullException(nameof(repositorioAtividadeInfantil));
        }
        public Task<IEnumerable<AtividadeInfantilDto>> Handle(ObterListaAtividadesMuralPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioAtividadeInfantil.ObterPorAulaId(request.AulaId);
        }
    }
}
