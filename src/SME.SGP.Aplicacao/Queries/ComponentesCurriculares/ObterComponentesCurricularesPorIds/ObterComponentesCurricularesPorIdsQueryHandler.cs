using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IServicoEol servicoEol, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            if (request.PossuiTerritorio.HasValue && request.PossuiTerritorio.Value)
            {
                var listaDisciplinas = new List<DisciplinaDto>();
                var disciplinasAgrupadas = await servicoEol.ObterDisciplinasPorIdsAgrupadas(request.Ids, request.CodigoTurma);
                foreach (var disciplina in disciplinasAgrupadas)
                {
                    disciplina.RegistraFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(disciplina.CodigoComponenteCurricular));
                    listaDisciplinas.Add(disciplina);
                }

                return listaDisciplinas;

            }
            else
                return await repositorioComponenteCurricular.ObterDisciplinasPorIds(request.Ids);

        }


    }
}
