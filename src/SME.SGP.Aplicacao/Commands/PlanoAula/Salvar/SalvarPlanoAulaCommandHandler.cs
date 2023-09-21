using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAulaCommandHandler : AbstractUseCase, IRequestHandler<SalvarPlanoAulaCommand, PlanoAulaDto>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;
        
        public SalvarPlanoAulaCommandHandler(IMediator mediator,
            IRepositorioAula repositorioAula,
            IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
            IRepositorioPlanoAula repositorioPlanoAula,
            IConsultasAbrangencia consultasAbrangencia,
            IUnitOfWork unitOfWork,
            IServicoUsuario servicoUsuario,
            IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions) : base(mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioObjetivosAula = repositorioObjetivosAula ?? throw new ArgumentNullException(nameof(repositorioObjetivosAula));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<PlanoAulaDto> Handle(SalvarPlanoAulaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var planoAulaDto = request.PlanoAula;
                var aula = await mediator.Send(new ObterAulaPorIdQuery(planoAulaDto.AulaId));
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));
                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

                var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula.Date));
                if (periodoEscolar.EhNulo())
                    throw new NegocioException(MensagemNegocioPlanoAula.NAO_FOI_LOCALIZADO_BIMESTRE_DA_AULA);
                
                var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, periodoEscolar.Bimestre,
                    turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year));

                if (!periodoAberto)
                    throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);
                
                DisciplinaDto disciplinaDto = null;

                if (request.PlanoAula.ComponenteCurricularId.HasValue)
                {
                    long componenteCurricularId = request.PlanoAula.ComponenteCurricularId.Value;

                    var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { componenteCurricularId }, codigoTurma: turma.CodigoTurma));

                    if(!componentesCurriculares.Any())
                    {
                        var componentesEol = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turma.CodigoTurma, usuario.Login, usuario.PerfilAtual, true, false));
                        if (componentesEol.Any())
                            componentesCurriculares = componentesEol.Where(c=> c.Codigo == componenteCurricularId).Select(c => new DisciplinaDto()
                            {
                                CdComponenteCurricularPai = c.CodigoComponenteCurricularPai,
                                CodigoComponenteCurricular = c.Codigo,
                                Nome = c.Descricao,
                                TerritorioSaber = c.TerritorioSaber,
                                LancaNota = c.LancaNota,
                                Compartilhada = c.Compartilhada,
                                Regencia = c.Regencia,
                                RegistraFrequencia = c.RegistraFrequencia
                            });
                    }
                        
                    disciplinaDto = componentesCurriculares.SingleOrDefault();
                }

                

                if (usuario.EhGestorEscolar())
                    await ValidarAbrangenciaGestorEscolar(usuario, turma.CodigoTurma, turma.EhTurmaHistorica);
                else
                    await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario.EhProfessorCj());

                PlanoAula planoAula = await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(planoAulaDto.AulaId));
                var planoAulaResumidoDto = new PlanoAulaResumidoDto()
                {
                    DescricaoNovo = request.PlanoAula.Descricao,
                    RecuperacaoAulaNovo = request.PlanoAula.RecuperacaoAula,
                    LicaoCasaNovo = request.PlanoAula.LicaoCasa,

                    DescricaoAtual = planoAula?.Descricao ?? string.Empty,
                    LicaoCasaAtual = planoAula?.LicaoCasa ?? string.Empty,
                    RecuperacaoAulaAtual = planoAula?.RecuperacaoAula ?? string.Empty
                };
                planoAula = MapearParaDominio(planoAulaDto, planoAula);
                
                var planejamentoAnual = await mediator.Send(
                    new ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery(turma.Id, periodoEscolar.Id, long.Parse(aula.DisciplinaId))
                    );

                if (ValidarSeNaoExistePlanoAnualCadastrado(planejamentoAnual, periodoEscolar, usuario, disciplinaDto))
                    throw new NegocioException(MensagemNegocioPlanoAula.NAO_EXISTE_PLANO_ANUAL_CADASTRADO);

                if (planoAulaDto.ObjetivosAprendizagemComponente.EhNulo() || !planoAulaDto.ObjetivosAprendizagemComponente.Any() && !planoAula.Migrado)
                {
                    var permitePlanoSemObjetivos = false;

                    var possuiObjetivos = await mediator.Send(new VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery(long.Parse(aula.DisciplinaId)));                    

                    // Os seguintes componentes curriculares (disciplinas) não tem seleção de objetivos de aprendizagem
                    // Libras, Sala de Leitura
                    permitePlanoSemObjetivos = new [] { "218", "1061" }.Contains(aula.DisciplinaId) ||
                                                   new [] { Modalidade.EJA, Modalidade.Medio }.Contains(turma.ModalidadeCodigo) ||  // EJA e Médio não obrigam seleção
                                                   usuario.EhProfessorCj() ||  // Para professores substitutos (CJ) a seleção dos objetivos deve ser opcional
                                                   periodoEscolar.TipoCalendario.AnoLetivo < DateTime.Now.Year || // Para anos anteriores não obrigatória seleção de objetivos
                                                   !possuiObjetivos || // Caso a disciplina não possui vinculo com Jurema, os objetivos não devem ser exigidos
                                                   turma.Ano.Equals("0"); // Caso a turma for de  educação física multisseriadas, os objetivos não devem ser exigidos;

                    if (!permitePlanoSemObjetivos)
                        throw new NegocioException(MensagemNegocioPlanoAula.OBRIGATORIO_SELECIONAR_OBJETIVOS_APRENDIZAGEM);
                }                            
                unitOfWork.IniciarTransacao();
                
                await repositorioPlanoAula.SalvarAsync(planoAula);

                await mediator.Send(new ExcluirPendenciaAulaCommand(planoAula.AulaId, Dominio.TipoPendencia.PlanoAula));

                // Salvar Objetivos aprendizagem
                var objetivosAtuais =  (await mediator.Send(new ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery(planoAula.Id))).ToList();
                var objetivosPropostos = planoAulaDto.ObjetivosAprendizagemComponente;
                
                if (objetivosPropostos.NaoEhNulo() && objetivosPropostos.Any())
                {
                    var objetivosIdParaIncluir = objetivosPropostos.Select(s => s.Id).Except(objetivosAtuais.Select(s => s.ObjetivoAprendizagemId));
                    
                    var objetivosIdParaExcluir = objetivosAtuais.Select(s => s.ObjetivoAprendizagemId).Except(objetivosPropostos.Select(s => s.Id));

                    foreach (var objetivoAtual in objetivosAtuais.Where(w => objetivosIdParaExcluir.Contains(w.ObjetivoAprendizagemId)))
                        await ExcluirObjetivoAprendizagemAulaLogicamente(objetivoAtual);
                        
                    foreach (var objetivoAprendizagem in objetivosPropostos.Where(w=> objetivosIdParaIncluir.Contains(w.Id)))
                        await repositorioObjetivosAula.SalvarAsync(new ObjetivoAprendizagemAula(planoAula.Id, objetivoAprendizagem.Id, objetivoAprendizagem.ComponenteCurricularId));
                }
                else
                {
                    foreach (var objetivoAtual in objetivosAtuais)
                        await ExcluirObjetivoAprendizagemAulaLogicamente(objetivoAtual);
                }
                
                unitOfWork.PersistirTransacao();

                 var planoAulaDescricao = await MoverRemoverExcluidos(planoAulaResumidoDto.DescricaoNovo, planoAulaResumidoDto.DescricaoAtual,TipoArquivo.PlanoAula);
                 var recuperacaoAula = await MoverRemoverExcluidos(planoAulaResumidoDto.RecuperacaoAulaNovo, planoAulaResumidoDto.RecuperacaoAulaAtual,TipoArquivo.PlanoAulaRecuperacao);
                 var licaoCasa = await MoverRemoverExcluidos(planoAulaResumidoDto.LicaoCasaNovo, planoAulaResumidoDto.LicaoCasaAtual, TipoArquivo.PlanoAulaLicaoCasa);

                planoAulaDto.Id = planoAula.Id;
                planoAulaDto.Descricao = planoAulaDescricao;
                planoAulaDto.RecuperacaoAula = recuperacaoAula;
                planoAulaDto.LicaoCasa = licaoCasa;

                //Se houver plano para copiar
                if (planoAulaDto.CopiarConteudo.NaoEhNulo())
                {
                    var migrarPlanoAula = planoAulaDto.CopiarConteudo;

                    migrarPlanoAula.PlanoAulaId = planoAula.Id;

                    await mediator.Send(new MigrarPlanoAulaCommand(migrarPlanoAula, usuario));
                }

                planoAulaDto.Id = planoAula.Id;
                planoAulaDto.AlteradoEm = planoAula.AlteradoEm;
                planoAulaDto.AlteradoPor = planoAula.AlteradoPor;
                planoAulaDto.AlteradoRf = planoAula.AlteradoRF;
                planoAulaDto.CriadoEm = planoAula.CriadoEm;
                planoAulaDto.CriadoPor = planoAula.CriadoPor;
                planoAulaDto.CriadoRf = planoAula.CriadoRF;

                return planoAulaDto;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi registrar o plano de aula.", LogNivel.Negocio, LogContexto.PlanoAula,ex.Message,"SGP",string.Empty,ex.StackTrace));
                throw;
            }
        }

        private static bool ValidarSeNaoExistePlanoAnualCadastrado(PlanejamentoAnual planejamentoAnual, PeriodoEscolar periodoEscolar, Usuario usuario, DisciplinaDto disciplinaDto)
        {
            return (planejamentoAnual?.Id <= 0 || planejamentoAnual.EhNulo()) && periodoEscolar.TipoCalendario.AnoLetivo.Equals(DateTime.Now.Year) && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ) && !(disciplinaDto.NaoEhNulo() && disciplinaDto.TerritorioSaber);
        }


        private async Task ExcluirObjetivoAprendizagemAulaLogicamente(ObjetivoAprendizagemAula objetivoAtual)
        {
            objetivoAtual.Excluido = true;
            await repositorioObjetivosAula.SalvarAsync(objetivoAtual);
        }
        private async Task ValidarAbrangenciaGestorEscolar(Usuario usuario, string turmaCodigo, bool ehTurmaHistorica)
        {
            var ehAbrangenciaUeOuDreOuSme = usuario.EhPerfilUE() || usuario.EhPerfilDRE() || usuario.EhPerfilSME();
            
            var abrangenciaTurmas = await mediator.Send(new ObterAbrangenciaTurmaQuery(turmaCodigo, usuario.Login,
                usuario.PerfilAtual, ehTurmaHistorica, ehAbrangenciaUeOuDreOuSme));

            if (abrangenciaTurmas.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.USUARIO_SEM_ACESSO_TURMA_RESPECTIVA_AULA);
        }

        private PlanoAula MapearParaDominio(PlanoAulaDto planoDto, PlanoAula planoAula = null)
        {
            if (planoAula.EhNulo())
                planoAula = new PlanoAula();

            planoAula.AulaId = planoDto.AulaId;
            planoAula.Descricao = planoDto.Descricao?.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            planoAula.RecuperacaoAula = planoDto.RecuperacaoAula?.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            planoAula.LicaoCasa = planoDto.LicaoCasa?.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);

            return planoAula;
        }
        private async Task<string> MoverRemoverExcluidos(string novo, string atual, TipoArquivo tipo)
        {
            string novaDescricao = string.Empty;
            if (!string.IsNullOrEmpty(novo))
            {
                 novaDescricao = await mediator.Send(new MoverArquivosTemporariosCommand(tipo, atual, novo));
            }
            if (!string.IsNullOrEmpty(atual))
            {
                 await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, tipo.Name()));
            }
            return novaDescricao;
        }
        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, bool ehProfessorCj)
        {
            if (!ehProfessorCj && !await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula))
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
