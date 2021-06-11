using MediatR;
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
    public class SalvarPlanoAulaCommandHandler : AbstractUseCase, IRequestHandler<SalvarPlanoAulaCommand, AuditoriaDto>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        public SalvarPlanoAulaCommandHandler(IMediator mediator,
            IRepositorioAula repositorioAula,
            IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
            IRepositorioPlanoAula repositorioPlanoAula,
            IConsultasAbrangencia consultasAbrangencia,
            IUnitOfWork unitOfWork,
            IServicoUsuario servicoUsuario) : base(mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioObjetivosAula = repositorioObjetivosAula ?? throw new ArgumentNullException(nameof(repositorioObjetivosAula));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

        public async Task<AuditoriaDto> Handle(SalvarPlanoAulaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var planoAulaDto = request.PlanoAula;
                var aula = await mediator.Send(new ObterAulaPorIdQuery(planoAulaDto.AulaId));
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));
                DisciplinaDto disciplinaDto = null;

                if (request.PlanoAula.ComponenteCurricularId.HasValue)
                {
                    var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.PlanoAula.ComponenteCurricularId.Value }));
                    disciplinaDto = componentesCurriculares.SingleOrDefault();
                }

                var abrangenciaTurma = await consultasAbrangencia.ObterAbrangenciaTurma(aula.TurmaId, planoAulaDto.ConsideraHistorico);
                if (abrangenciaTurma == null)
                    throw new NegocioException("Usuario sem acesso a turma da respectiva aula");

                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                if(!usuario.EhGestorEscolar())
                    await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

                PlanoAula planoAula = await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(planoAulaDto.AulaId));
                planoAula = MapearParaDominio(planoAulaDto, planoAula);

                var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula.Date));
                if (periodoEscolar == null)
                    throw new NegocioException("Não foi possível concluir o cadastro, pois não foi localizado o bimestre da aula.");

                var planejamentoAnual = await mediator.Send(
                    new ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery(turma.Id, periodoEscolar.Id, long.Parse(aula.DisciplinaId))
                    );

                if ((planejamentoAnual?.Id <= 0 || planejamentoAnual == null) && periodoEscolar.TipoCalendario.AnoLetivo.Equals(DateTime.Now.Year) && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ) && !(disciplinaDto != null && disciplinaDto.TerritorioSaber))
                    throw new NegocioException("Não foi possível concluir o cadastro, pois não existe plano anual cadastrado");

                if (planoAulaDto.ObjetivosAprendizagemComponente == null || !planoAulaDto.ObjetivosAprendizagemComponente.Any() && !planoAula.Migrado)
                {
                    var permitePlanoSemObjetivos = false;

                    var possuiObjetivos = await mediator.Send(new VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery(long.Parse(aula.DisciplinaId)));                    

                    // Os seguintes componentes curriculares (disciplinas) não tem seleção de objetivos de aprendizagem
                    // Libras, Sala de Leitura
                    permitePlanoSemObjetivos = new string[] { "218", "1061" }.Contains(aula.DisciplinaId) ||
                                                   new[] { Modalidade.EJA, Modalidade.Medio }.Contains(abrangenciaTurma.Modalidade) ||  // EJA e Médio não obrigam seleção
                                                   usuario.EhProfessorCj() ||  // Para professores substitutos (CJ) a seleção dos objetivos deve ser opcional
                                                   periodoEscolar.TipoCalendario.AnoLetivo < DateTime.Now.Year || // Para anos anteriores não obrigatória seleção de objetivos
                                                   !possuiObjetivos || // Caso a disciplina não possui vinculo com Jurema, os objetivos não devem ser exigidos
                                                   abrangenciaTurma.Ano.Equals("0"); // Caso a turma for de  educação física multisseriadas, os objetivos não devem ser exigidos;

                    if (!permitePlanoSemObjetivos)
                        throw new NegocioException("A seleção de objetivos de aprendizagem é obrigatória para criação do plano de aula");
                }                            

                await repositorioPlanoAula.SalvarAsync(planoAula);

                await mediator.Send(new ExcluirPendenciaAulaCommand(planoAula.AulaId, Dominio.TipoPendencia.PlanoAula));

                // Salvar Objetivos
                await repositorioObjetivosAula.LimparObjetivosAula(planoAula.Id);
                if (planoAulaDto.ObjetivosAprendizagemComponente != null)
                    foreach (var objetivoAprendizagem in planoAulaDto.ObjetivosAprendizagemComponente)
                    {
                        await repositorioObjetivosAula.SalvarAsync(new ObjetivoAprendizagemAula(planoAula.Id, objetivoAprendizagem.Id, objetivoAprendizagem.ComponenteCurricularId));
                    }

                return (AuditoriaDto)planoAula;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private PlanoAula MapearParaDominio(PlanoAulaDto planoDto, PlanoAula planoAula = null)
        {
            if (planoAula == null)
                planoAula = new PlanoAula();

            planoAula.AulaId = planoDto.AulaId;
            planoAula.Descricao = planoDto.Descricao;
            planoAula.DesenvolvimentoAula = planoDto.DesenvolvimentoAula;
            planoAula.RecuperacaoAula = planoDto.RecuperacaoAula;
            planoAula.LicaoCasa = planoDto.LicaoCasa;

            return planoAula;
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario = null)
        {
            if (!usuario.EhProfessorCj() && !await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
