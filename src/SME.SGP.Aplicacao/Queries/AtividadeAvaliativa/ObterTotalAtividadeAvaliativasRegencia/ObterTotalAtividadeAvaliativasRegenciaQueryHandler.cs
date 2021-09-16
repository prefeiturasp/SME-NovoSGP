using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAtividadeAvaliativasRegenciaQueryHandler : IRequestHandler<ObterTotalAtividadeAvaliativasRegenciaQuery, TotalizadorAtividadesAvaliativasRegenciaDto>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;

        public ObterTotalAtividadeAvaliativasRegenciaQueryHandler(IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)
        {
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
        }

        public async Task<TotalizadorAtividadesAvaliativasRegenciaDto> Handle(ObterTotalAtividadeAvaliativasRegenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativaDisciplina.TotalAtividadesAvaliativasRegenciaPorAtividadesAvaliativas(request.AtividadesAvaliativasId);
        }
    }
}
