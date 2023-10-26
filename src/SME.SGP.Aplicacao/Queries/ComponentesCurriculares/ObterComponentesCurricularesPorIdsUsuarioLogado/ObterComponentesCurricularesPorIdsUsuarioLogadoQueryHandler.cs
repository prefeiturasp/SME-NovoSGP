using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var disciplinasRetorno = new List<DisciplinaDto>();
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.CodigoTurma));
            var componentesCurricularesUsuarioTurma = await mediator
                                                            .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, 
                                                                                                                          usuarioLogado.Login, 
                                                                                                                          usuarioLogado.PerfilAtual,
                                                                                                                          turma.EhTurmaInfantil));
            var disciplinasPorIds = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(request.Ids));

            IEnumerable<AtribuicaoCJ> componentesCurricularesDoProfessorCJInfantil = Enumerable.Empty<AtribuicaoCJ>();
            if (usuarioLogado.EhProfessorCjInfantil())
                componentesCurricularesDoProfessorCJInfantil = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

            foreach (var id in request.Ids)
            {
                var componenteUsuarioTurma = componentesCurricularesUsuarioTurma.FirstOrDefault(d => d.Codigo.Equals(id) || 
                                                                                 d.CodigoComponenteTerritorioSaber.Equals(id));

                if (componenteUsuarioTurma.EhNulo())
                {
                    if (usuarioLogado.EhProfessorCjInfantil())
                        if (!componentesCurricularesDoProfessorCJInfantil.Any(c => c.DisciplinaId == id))
                            continue;

                    disciplinasRetorno.Add(disciplinasPorIds.FirstOrDefault(d => d.CodigoComponenteCurricular.Equals(id)));
                }
                else
                    disciplinasRetorno.Add(new DisciplinaDto()
                    {
                        Id = componenteUsuarioTurma.Codigo,
                        CodigoComponenteCurricular = componenteUsuarioTurma.Codigo,
                        CdComponenteCurricularPai = componenteUsuarioTurma.CodigoComponenteCurricularPai,
                        CodigoComponenteCurricularTerritorioSaber = componenteUsuarioTurma.CodigoComponenteTerritorioSaber,
                        Compartilhada = componenteUsuarioTurma.Compartilhada,
                        Nome = componenteUsuarioTurma.Descricao,
                        NomeComponenteInfantil = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? componenteUsuarioTurma.DescricaoComponenteInfantil : componenteUsuarioTurma.Descricao,
                        PossuiObjetivos = componenteUsuarioTurma.PossuiObjetivos,
                        Regencia = componenteUsuarioTurma.Regencia,
                        RegistraFrequencia = componenteUsuarioTurma.RegistraFrequencia,
                        TerritorioSaber = componenteUsuarioTurma.TerritorioSaber,
                        LancaNota = componenteUsuarioTurma.LancaNota,
                        TurmaCodigo = componenteUsuarioTurma.TurmaCodigo,
                        GrupoMatrizId = componenteUsuarioTurma.GrupoMatriz?.Id ?? 0,
                        GrupoMatrizNome = componenteUsuarioTurma.GrupoMatriz?.Nome ?? ""
                    });
            }
            return disciplinasRetorno;
        }
    }
}
