using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaReduzidaPorTurmaComponenteEBimestreQueryHandler : IRequestHandler<ObterAulaReduzidaPorTurmaComponenteEBimestreQuery, IEnumerable<AulaReduzidaDto>>
    {
        private readonly IRepositorioAula repositorioAula;
        public ObterAulaReduzidaPorTurmaComponenteEBimestreQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaReduzidaDto>> Handle(ObterAulaReduzidaPorTurmaComponenteEBimestreQuery request, CancellationToken cancellationToken)
                        => await repositorioAula.ObterQuantidadeAulasReduzido(request.TurmaId,
                                                                              request.ComponenteCurricularId.ToString(),
                                                                              request.TipoCalendarioId,
                                                                              request.Bimestre,
                                                                              request.ProfessorCJ);
    }
}
