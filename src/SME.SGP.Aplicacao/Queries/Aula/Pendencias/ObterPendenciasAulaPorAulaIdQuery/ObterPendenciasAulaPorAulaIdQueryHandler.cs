using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdQuery, long[]>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;
        private readonly IMediator mediator;

        public ObterPendenciasAulaPorAulaIdQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula,
                                                        IMediator mediator)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<long[]> Handle(ObterPendenciasAulaPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var componentesCurricularesDoProfessorCJ = Enumerable.Empty<AtribuicaoCJ>();
            var componentesCurricularesEolProfessor = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(aula.TurmaId,
                                                                                  usuarioLogado.Login,
                                                                                  usuarioLogado.PerfilAtual,
                                                                                  usuarioLogado.EhProfessorInfantilOuCjInfantil()));

            if (usuarioLogado.EhSomenteProfessorCj())
                componentesCurricularesDoProfessorCJ = await mediator
                     .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));
            

            var componenteCorrespondente = componentesCurricularesEolProfessor.NaoEhNulo()
                ? componentesCurricularesEolProfessor.FirstOrDefault(cc => (!cc.TerritorioSaber && cc.Codigo.ToString() == aula.DisciplinaId) || 
                                                                           (cc.TerritorioSaber && cc.Codigo.ToString() == aula.DisciplinaId && cc.Professor == aula.ProfessorRf))
                : usuarioLogado.EhSomenteProfessorCj() && componentesCurricularesDoProfessorCJ.Any() ?
                                                        componentesCurricularesDoProfessorCJ.Select(c => new ComponenteCurricularEol()
                                                        {
                                                            Codigo = c.DisciplinaId,
                                                            TurmaCodigo = c.TurmaId
                                                        }).FirstOrDefault(c => c.TurmaCodigo == aula.TurmaId)
                                                        : new ComponenteCurricularEol();



            var pendencias = await repositorioPendenciaAula.PossuiPendenciasPorAulaId(request.AulaId, request.EhModalidadeInfantil, request.UsuarioLogado, (componenteCorrespondente?.CodigoComponenteTerritorioSaber ?? 0) > 0 ? componenteCorrespondente?.CodigoComponenteTerritorioSaber : null);

            if (pendencias.EhNulo())
                return null;

            pendencias = new PendenciaAulaDto
            {
                AulaId = request.AulaId,
                PossuiPendenciaFrequencia = pendencias.PossuiPendenciaFrequencia,
                PossuiPendenciaPlanoAula = pendencias.PossuiPendenciaPlanoAula,
                PossuiPendenciaAtividadeAvaliativa = false,
                PossuiPendenciaDiarioBordo = pendencias.PossuiPendenciaDiarioBordo
            };
            pendencias.PossuiPendenciaAtividadeAvaliativa = request.TemAtividadeAvaliativa && await repositorioPendenciaAula.PossuiPendenciasAtividadeAvaliativaPorAulaId(request.AulaId);

            return pendencias.RetornarTipoPendecias();
        }
    }
}
