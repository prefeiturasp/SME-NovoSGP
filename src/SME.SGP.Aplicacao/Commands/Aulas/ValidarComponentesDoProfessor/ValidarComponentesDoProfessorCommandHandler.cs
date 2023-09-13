using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarComponentesDoProfessorCommandHandler : IRequestHandler<ValidarComponentesDoProfessorCommand, (bool resultado, string mensagem)>
    {
        private readonly IMediator mediator;

        public ValidarComponentesDoProfessorCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<(bool resultado, string mensagem)> Handle(ValidarComponentesDoProfessorCommand request, CancellationToken cancellationToken)
        {
            var podeCriarAulasParaTurma = false;

            var componentesCurricularesDoProfessor = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo, request.Usuario.Login, request.Usuario.PerfilAtual, request.Usuario.EhProfessorInfantilOuCjInfantil()));

            if (request.Usuario.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(request.Usuario.Login));

                if (componentesCurricularesDoProfessorCJ.NaoEhNulo())
                {
                    podeCriarAulasParaTurma =
                        componentesCurricularesDoProfessorCJ.Any(c => c.TurmaId == request.TurmaCodigo && (c.DisciplinaId == request.ComponenteCurricularCodigo || (request.CodigoTerritorioSaber.HasValue && request.CodigoTerritorioSaber.Value > 0 && c.DisciplinaId.Equals(request.CodigoTerritorioSaber.Value)))) ||
                        componentesCurricularesDoProfessor.Any(c => c.Codigo.Equals(request.ComponenteCurricularCodigo) || c.CodigoComponenteTerritorioSaber.Equals(request.ComponenteCurricularCodigo));
                }                

                if (!podeCriarAulasParaTurma)
                {
                    var componenteTerritorioDefinidoParaAula = await mediator
                        .Send(new DefinirComponenteCurricularParaAulaQuery(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.Usuario));

                    podeCriarAulasParaTurma = componenteTerritorioDefinidoParaAula != default &&
                                              componenteTerritorioDefinidoParaAula.codigoTerritorio.HasValue &&
                                              componenteTerritorioDefinidoParaAula.codigoTerritorio.Value > 0 &&
                                              componentesCurricularesDoProfessorCJ.Select(cc => cc.DisciplinaId).Contains(componenteTerritorioDefinidoParaAula.codigoTerritorio.Value);
                }                

                if (!podeCriarAulasParaTurma)
                    return (false, MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);
            }
            else
            {
                if (componentesCurricularesDoProfessor.EhNulo())
                    componentesCurricularesDoProfessor = await VerificaPossibilidadeDeTurmaComMotivoErroDeCadastroNoUsuario(request.TurmaCodigo, request.Usuario.Login, request.Usuario.PerfilAtual, request.Usuario.EhProfessorInfantilOuCjInfantil());

                podeCriarAulasParaTurma = componentesCurricularesDoProfessor.NaoEhNulo() &&
                                          (componentesCurricularesDoProfessor.Any(c => !c.Regencia && !c.TerritorioSaber && c.Codigo == request.ComponenteCurricularCodigo) ||
                                           componentesCurricularesDoProfessor.Any(c => !c.Regencia && c.TerritorioSaber && (c.CodigoComponenteTerritorioSaber == request.ComponenteCurricularCodigo || c.Codigo == request.ComponenteCurricularCodigo)) ||
                                           componentesCurricularesDoProfessor.Any(r => r.Regencia && (r.CodigoComponenteCurricularPai == request.ComponenteCurricularCodigo || r.Codigo == request.ComponenteCurricularCodigo)));

                if (!podeCriarAulasParaTurma)
                    return (false, MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);

                if (!request.Usuario.EhGestorEscolar())
                {
                    var usuarioPodePersistirTurmaNaData = await mediator
                        .Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(request.ComponenteCurricularCodigo, request.TurmaCodigo, request.Data, request.Usuario));

                    if (!usuarioPodePersistirTurmaNaData)
                        return (false, MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
                }
            }

            return (true, string.Empty);
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> VerificaPossibilidadeDeTurmaComMotivoErroDeCadastroNoUsuario(string turmaCodigo, string login, Guid perfilAtual, bool realizaAgrupamento)
         => await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaCodigo, login, perfilAtual, realizaAgrupamento, false));
    }
}