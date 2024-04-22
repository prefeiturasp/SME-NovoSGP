using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo), cancellationToken);
            var componentesCurricularesDoProfessor = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo, request.Usuario.Login, request.Usuario.PerfilAtual, turma.EhTurmaInfantil), cancellationToken);

            if (request.Usuario.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(request.Usuario.Login), cancellationToken);

                podeCriarAulasParaTurma = await ProfessorCJPodeCriarAulasTurma(componentesCurricularesDoProfessorCJ, componentesCurricularesDoProfessor,
                                                                               request.ComponenteCurricularCodigo, request.TurmaCodigo, request.CodigoTerritorioSaber);
            }
            else
            {
                componentesCurricularesDoProfessor = componentesCurricularesDoProfessor ??
                                                     await VerificaPossibilidadeDeTurmaComMotivoErroDeCadastroNoUsuario(request.TurmaCodigo,
                                                                                                                        request.Usuario.Login,
                                                                                                                        request.Usuario.PerfilAtual,
                                                                                                                        request.Usuario.EhProfessorInfantilOuCjInfantil());

                componentesCurricularesDoProfessor.LancarExcecaoNegocioSeEhNulo(MensagemNegocioComponentesCurriculares.NAO_FORAM_ENCONTRADOS_COMPONENTES_CURRICULARES_PARA_O_PROFESSOR);
                
                if(componentesCurricularesDoProfessor.NaoEhNulo() && componentesCurricularesDoProfessor.Any())
                {
                    var componenteCurricularFiltrado = componentesCurricularesDoProfessor.FirstOrDefault(x => x.Codigo == request.ComponenteCurricularCodigo);
                    if (componenteCurricularFiltrado.NaoEhNulo())
                    {
                        var componenteEhVigente = await ValidaVigenciaComponenteTerritorioSaberDoProfessor(request.Usuario, request.TurmaCodigo, request.Data, componenteCurricularFiltrado);
                        if (!componenteEhVigente)
                        {
                            return (false, MensagemNegocioComuns.VOCE_NAO_PODE_CRIAR_AULAS_PARA_COMPONENTES_SEM_ATRIBUICAO_NA_DATA_SELECIONADA);
                        }
                    }
                }

                podeCriarAulasParaTurma = await ProfessorPodeCriarAulasTurma(componentesCurricularesDoProfessor,
                                                                             request.ComponenteCurricularCodigo,
                                                                             request.CodigoTerritorioSaber);

                if (podeCriarAulasParaTurma)
                {
                    var usuarioPodePersistirTurmaNaData = await ObterUsuarioPossuiPermissaoNaTurmaEDisciplina(request.ComponenteCurricularCodigo,
                                                                                                          request.TurmaCodigo,
                                                                                                          request.Data,
                                                                                                          request.Usuario,
                                                                                                          cancellationToken);
                    if (!usuarioPodePersistirTurmaNaData)
                        return (false, MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
                }
            }

            if (!podeCriarAulasParaTurma)
                return (false, MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);

            return (true, string.Empty);
        }

        private async Task<bool> ValidaVigenciaComponenteTerritorioSaberDoProfessor(Usuario usuario, string turmaCodigo, DateTime data, ComponenteCurricularEol componenteCurricularDoProfessor)
        {
            var componenteEhVigente = true;
            if (componenteCurricularDoProfessor.TerritorioSaber)
                componenteEhVigente = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaEOLQuery(usuario, turmaCodigo, componenteCurricularDoProfessor.Codigo.ToString(), data, componenteCurricularDoProfessor.TerritorioSaber));
            return componenteEhVigente;
        }

        private async Task<bool> ObterUsuarioPossuiPermissaoNaTurmaEDisciplina(long componenteCurricularId, string codigoTurma, DateTime data, Dominio.Usuario usuario, CancellationToken cancellationToken)
        => usuario.EhGestorEscolar() ||
           await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(componenteCurricularId, codigoTurma, data, usuario), cancellationToken);

        private async Task<bool> ProfessorPodeCriarAulasTurma(IEnumerable<ComponenteCurricularEol> componentesCurricularesDoProfessor,
                                                              long componenteCurricularCodigo,
                                                              long? componenteCurricularTerritorioSaberCodigo)
        => componentesCurricularesDoProfessor.NaoEhNulo() &&
           (componentesCurricularesDoProfessor.Any(c => !c.Regencia && !c.TerritorioSaber && c.Codigo == componenteCurricularCodigo) ||
            componentesCurricularesDoProfessor.Any(c => !c.Regencia && c.TerritorioSaber && (c.CodigoComponenteTerritorioSaber == componenteCurricularTerritorioSaberCodigo || c.Codigo == componenteCurricularCodigo)) ||
            componentesCurricularesDoProfessor.Any(r => r.Regencia && (r.CodigoComponenteCurricularPai == componenteCurricularCodigo || r.Codigo == componenteCurricularCodigo)));

        private async Task<bool> ProfessorCJPodeCriarAulasTurma(IEnumerable<AtribuicaoCJ> componentesCurricularesDoProfessorCJ,
                                                                IEnumerable<ComponenteCurricularEol> componentesCurricularesDoProfessor,
                                                                long componenteCurricularCodigo, string turmaCodigo,
                                                                long? componenteCurricularTerritorioSaberCodigo)
        => componentesCurricularesDoProfessorCJ.Any(c => c.TurmaId == turmaCodigo && (c.DisciplinaId == componenteCurricularCodigo || (componenteCurricularTerritorioSaberCodigo.HasValue && componenteCurricularTerritorioSaberCodigo.Value > 0 && c.DisciplinaId.Equals(componenteCurricularTerritorioSaberCodigo)))) ||
           componentesCurricularesDoProfessor.Any(c => c.Codigo.Equals(componenteCurricularCodigo) || c.CodigoComponenteTerritorioSaber.Equals(componenteCurricularCodigo));

        public async Task<IEnumerable<ComponenteCurricularEol>> VerificaPossibilidadeDeTurmaComMotivoErroDeCadastroNoUsuario(string turmaCodigo, string login, Guid perfilAtual, bool realizaAgrupamento)
         => await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaCodigo, login, perfilAtual, realizaAgrupamento, false));
    }
}