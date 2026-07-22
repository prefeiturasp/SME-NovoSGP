using MediatR;
using SME.SGP.Dominio;
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
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandler(IMediator mediator)
        {
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
                var disciplinaPorId = disciplinasPorIds.FirstOrDefault(d => d.CodigoComponenteCurricular.Equals(id));
                var componenteUsuarioTurma = ObterComponenteUsuarioTurma(componentesCurricularesUsuarioTurma, id, disciplinaPorId);

                if (componenteUsuarioTurma.EhNulo())
                { 
                    if (usuarioLogado.EhProfessorCjInfantil() &&
                        !componentesCurricularesDoProfessorCJInfantil.Any(c => c.DisciplinaId == id))
                        continue;

                    if (disciplinaPorId.NaoEhNulo())
                        disciplinasRetorno.Add(disciplinaPorId);
                }
                else
                    disciplinasRetorno.Add(MapearParaDto(componenteUsuarioTurma, turma, id));
            }
            return disciplinasRetorno;
        }

        private ComponenteCurricularEol ObterComponenteUsuarioTurma(IEnumerable<ComponenteCurricularEol> componentesCurricularesUsuarioTurma, long id, DisciplinaDto disciplinaPorId)
        {
            var componenteUsuarioTurma = componentesCurricularesUsuarioTurma
                .FirstOrDefault(d => d.PossuiCodigoEquivalente(id));

            if (componenteUsuarioTurma.NaoEhNulo())
                return componenteUsuarioTurma;

            if (disciplinaPorId.NaoEhNulo() && disciplinaPorId.CodigoComponenteCurricularTerritorioSaber > 0)
            {
                componenteUsuarioTurma = componentesCurricularesUsuarioTurma
                    .FirstOrDefault(d => d.PossuiCodigoEquivalente(disciplinaPorId.CodigoComponenteCurricularTerritorioSaber));

                if (componenteUsuarioTurma.NaoEhNulo())
                    return componenteUsuarioTurma;
            }

            var componentesTerritorioSaber = componentesCurricularesUsuarioTurma
                .Where(d => d.TerritorioSaber)
                .ToList();

            if (disciplinaPorId.NaoEhNulo() && disciplinaPorId.TerritorioSaber && componentesTerritorioSaber.Count == 1)
                return componentesTerritorioSaber.First();

            return null;
        }

        private static DisciplinaDto MapearParaDto(ComponenteCurricularEol componenteUsuarioTurma, Turma turma, long idAula)
            => new DisciplinaDto()
            {
                Id = idAula,
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
            };
    }
}
