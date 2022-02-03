using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoNotaPorTurmaIdQueryHandler : IRequestHandler<ObterTipoNotaPorTurmaIdQuery, NotaTipoValor>
    {
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;

        public ObterTipoNotaPorTurmaIdQueryHandler(IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor)
        {
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
        }

        public async Task<NotaTipoValor> Handle(ObterTipoNotaPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotaTipoValor.ObterPorTurmaIdAsync(request.TurmaId, request.TipoTurma);
        }
    }
}
