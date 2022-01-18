using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaPlanoAulaQueryHandler : IRequestHandler<ExistePendenciaPlanoAulaQuery, bool>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;

        public ExistePendenciaPlanoAulaQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<bool> Handle(ExistePendenciaPlanoAulaQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAula.ValidarPendenciaPlanoPorTurmaDataEPeriodo(request.Data, request.TurmaId, request.DisciplinaId, request.AnoLetivo, request.Bimestre);
    }
}
