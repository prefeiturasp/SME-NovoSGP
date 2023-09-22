using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;
        public ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler(IRepositorioCache repositorioCache, IMediator mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            string nomechavecache = string.Format(NomeChaveCache.COMPONENTES_PROFESSOR_TURMA,
                                                  request.Login,
                                                  request.CodigoTurma,
                                                  request.ChecaMotivoDisponibilizacao,
                                                  request.RealizarAgrupamentoComponente);

            var resultado = await repositorioCache.ObterAsync(nomechavecache, 
                async () => await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(request.CodigoTurma, request.Login,
                                                               request.PerfilUsuario,
                                                               request.RealizarAgrupamentoComponente,
                                                               request.ChecaMotivoDisponibilizacao))
                , minutosParaExpirar: 240);

            if (resultado == null)
                return Enumerable.Empty<ComponenteCurricularEol>();

            return resultado;
        }

    }
}
