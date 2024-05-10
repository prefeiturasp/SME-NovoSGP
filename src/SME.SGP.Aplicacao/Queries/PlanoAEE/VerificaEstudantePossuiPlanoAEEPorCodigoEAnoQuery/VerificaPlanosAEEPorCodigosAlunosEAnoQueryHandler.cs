using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPlanosAEEPorCodigosAlunosEAnoQueryHandler : IRequestHandler<VerificaPlanosAEEPorCodigosAlunosEAnoQuery, IEnumerable<PlanoAEEResumoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta;
        public VerificaPlanosAEEPorCodigosAlunosEAnoQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta)
        {
            this.repositorioPlanoAEEConsulta = repositorioPlanoAEEConsulta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEConsulta));
        }
        public async Task<IEnumerable<PlanoAEEResumoDto>> Handle(VerificaPlanosAEEPorCodigosAlunosEAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAEEConsulta.ObterPlanosPorAlunosEAno(request.CodigoEstudante, request.AnoLetivo);
        }
    }
}
