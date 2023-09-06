using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using SME.SGP.Dominio.Constantes;
using System;

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
                                                               request.ChecaMotivoDisponibilizacao)));

            if (resultado == null)
                return Enumerable.Empty<ComponenteCurricularEol>();

            return resultado;
        }

    }
}
