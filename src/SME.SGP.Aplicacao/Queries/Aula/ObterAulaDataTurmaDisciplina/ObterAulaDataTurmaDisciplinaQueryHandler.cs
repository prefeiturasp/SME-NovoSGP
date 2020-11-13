using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaDataTurmaDisciplinaQueryHandler : IRequestHandler<ObterAulaDataTurmaDisciplinaQuery, AulaConsultaDto>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulaDataTurmaDisciplinaQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<AulaConsultaDto> Handle(ObterAulaDataTurmaDisciplinaQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulaDataTurmaDisciplina(request.Data, request.TurmaId, request.DisciplinaId);
    }
}
