using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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
        public Task<NotaTipoValor> Handle(ObterTipoNotaPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(request.TurmaId, request.TipoTurma);
            return Task.FromResult(tipoNota);
        }
    }
}
