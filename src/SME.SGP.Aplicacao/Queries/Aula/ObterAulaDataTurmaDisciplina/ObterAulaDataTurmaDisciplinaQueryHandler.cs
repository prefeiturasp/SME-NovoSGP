using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaDataTurmaDisciplinaQueryHandler : IRequestHandler<ObterAulaDataTurmaDisciplinaQuery, AulaConsultaDto>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulaDataTurmaDisciplinaQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<AulaConsultaDto> Handle(ObterAulaDataTurmaDisciplinaQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulaDataTurmaDisciplina(request.Data, request.TurmaId, request.DisciplinaId);
    }
}
