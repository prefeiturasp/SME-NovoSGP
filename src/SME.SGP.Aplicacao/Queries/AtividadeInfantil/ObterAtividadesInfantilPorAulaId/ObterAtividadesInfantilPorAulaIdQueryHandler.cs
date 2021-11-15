using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesInfantilPorAulaIdQueryHandler : IRequestHandler<ObterAtividadesInfantilPorAulaIdQuery, IEnumerable<AtividadeInfantilDto>>
    {
        private readonly IRepositorioAtividadeInfantil repositorioAtividadeInfantil;
        public ObterAtividadesInfantilPorAulaIdQueryHandler(IRepositorioAtividadeInfantil repositorioAtividadeInfantil)
        {
            this.repositorioAtividadeInfantil = repositorioAtividadeInfantil ?? throw new ArgumentNullException(nameof(repositorioAtividadeInfantil));
        }
        public Task<IEnumerable<AtividadeInfantilDto>> Handle(ObterAtividadesInfantilPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioAtividadeInfantil.ObterPorAulaId(request.AulaId);
        }
    }
}
