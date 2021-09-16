using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAtividadeAvaliativasRegenciaQueryHandler : IRequestHandler<ObterTotalAtividadeAvaliativasRegenciaQuery, IEnumerable<ComponentesRegenciaComAtividadeAvaliativaDto>>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;

        public ObterTotalAtividadeAvaliativasRegenciaQueryHandler(IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)
        {
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
        }

        public async Task<IEnumerable<ComponentesRegenciaComAtividadeAvaliativaDto>> Handle(ObterTotalAtividadeAvaliativasRegenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativaDisciplina.TotalAtividadesAvaliativasRegenciaPorAtividadesAvaliativas(request.AtividadesAvaliativasId);
        }
    }
}
