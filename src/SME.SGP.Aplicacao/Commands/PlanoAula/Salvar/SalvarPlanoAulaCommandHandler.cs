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
using System.Text.RegularExpressions;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAulaCommandHandler : AbstractUseCase, IRequestHandler<SalvarPlanoAulaCommand, PlanoAulaDto>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;
        
        public SalvarPlanoAulaCommandHandler(IMediator mediator,
            IRepositorioObjetivoAprendizagemAula repositorioObjetivosAula,
            IRepositorioPlanoAula repositorioPlanoAula,
            IUnitOfWork unitOfWork,
            IServicoUsuario servicoUsuario,
            IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions) : base(mediator)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioObjetivosAula = repositorioObjetivosAula ?? throw new ArgumentNullException(nameof(repositorioObjetivosAula));
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
                var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula.Date))
                                     ?? throw new NegocioException(MensagemNegocioPlanoAula.NAO_FOI_LOCALIZADO_BIMESTRE_DA_AULA);

                await ValidarPeriodoAberto(turma, periodoEscolar.Bimestre);
                await ValidarPermissoesAbrangencia(turma, usuario, aula.DisciplinaId, aula.DataAula);
                DisciplinaDto disciplinaDto = await ObterComponenteCurricularPlanoAula(turma, usuario, request.PlanoAula.ComponenteCurricularId);

                PlanoAula planoAula = await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(planoAulaDto.AulaId));
                var planoAulaResumidoDto = MapearParaDto(planoAulaDto, planoAula);
                planoAula = MapearParaDominio(planoAulaDto, planoAula);
                
                var planejamentoAnual = await mediator.Send(new ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery(turma.Id, 
                                                                                                                      periodoEscolar.Id, 
                                                                                                                      long.Parse(aula.DisciplinaId)));
                ValidarSeNaoExistePlanoAnualCadastrado(planejamentoAnual, periodoEscolar, usuario, disciplinaDto);
                    
                if (planoAulaDto.ObjetivosAprendizagemComponente.NaoPossuiRegistros() 
                    && !planoAula.Migrado)
                    await ValidarPlanoSemObjetivos(turma, usuario, aula.DisciplinaId, periodoEscolar.TipoCalendario.AnoLetivo);
                
                unitOfWork.IniciarTransacao();
                await repositorioPlanoAula.SalvarAsync(planoAula);
                await mediator.Send(new ExcluirPendenciaAulaCommand(planoAula.AulaId, Dominio.TipoPendencia.PlanoAula));

                // Salvar Objetivos aprendizagem
                var objetivosAtuais =  (await mediator.Send(new ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery(planoAula.Id))).ToList();
                var objetivosPropostos = planoAulaDto.ObjetivosAprendizagemComponente;
                
                if (objetivosPropostos.PossuiRegistros())
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

                await MigrarPlanoAula(planoAulaDto, usuario, planoAula.Id);

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

        private async Task MigrarPlanoAula(PlanoAulaDto planoAulaDto, Usuario usuario, long planoAulaId)
        {
            if (planoAulaDto.CopiarConteudo.EhNulo())
                return;
            
            var migrarPlanoAula = planoAulaDto.CopiarConteudo;
            migrarPlanoAula.PlanoAulaId = planoAulaId;
            await mediator.Send(new MigrarPlanoAulaCommand(migrarPlanoAula, usuario));
        }

        private async Task ValidarPlanoSemObjetivos(Turma turma, Usuario usuario, string componenteCurricularCodigo, int anoLetivo)
        {
            var possuiObjetivos = await mediator.Send(new VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery(long.Parse(componenteCurricularCodigo)));
            // Os seguintes componentes curriculares (disciplinas) não tem seleção de objetivos de aprendizagem
            // Libras, Sala de Leitura
            var permitePlanoSemObjetivos = new[] { "218", "1061" }.Contains(componenteCurricularCodigo) ||
                                           new[] { Modalidade.EJA, Modalidade.Medio }.Contains(turma.ModalidadeCodigo) ||  // EJA e Médio não obrigam seleção
                                           usuario.EhProfessorCj() ||  // Para professores substitutos (CJ) a seleção dos objetivos deve ser opcional
                                           anoLetivo < DateTime.Now.Year || // Para anos anteriores não obrigatória seleção de objetivos
                                           !possuiObjetivos || // Caso a disciplina não possui vinculo com Jurema, os objetivos não devem ser exigidos
                                           turma.Ano.Equals("0"); // Caso a turma for de  educação física multisseriadas, os objetivos não devem ser exigidos
            if (!permitePlanoSemObjetivos)
                throw new NegocioException(MensagemNegocioPlanoAula.OBRIGATORIO_SELECIONAR_OBJETIVOS_APRENDIZAGEM);
        }

        private async Task ValidarPermissoesAbrangencia(Turma turma, Usuario usuario, string componenteCurricularId, DateTime dataAula)
        {
            if (usuario.EhGestorEscolar())
                await ValidarAbrangenciaGestorEscolar(usuario, turma.CodigoTurma, turma.Historica);
            else
                await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, turma.CodigoTurma, componenteCurricularId, dataAula, usuario.EhProfessorCj());
        }

        private async Task<DisciplinaDto> ObterComponenteCurricularPlanoAula(Turma turma, Usuario usuario, long? componenteCurricularId)
        {
            if (!componenteCurricularId.HasValue)
                return null;
            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { componenteCurricularId.Value }, codigoTurma: turma.CodigoTurma));
            if (!componentesCurriculares.Any())
            {
                var componentesEol = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turma.CodigoTurma, usuario.Login, usuario.PerfilAtual, true, false));
                if (componentesEol.Any())
                    componentesCurriculares = componentesEol.Where(c => c.Codigo == componenteCurricularId).Select(c => new DisciplinaDto()
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
            return componentesCurriculares.SingleOrDefault();
        }

        private async Task ValidarPeriodoAberto(Turma turma, int bimestre)
        {
            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestre,
                    turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year));

            if (!periodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);
        }

        private static void ValidarSeNaoExistePlanoAnualCadastrado(PlanejamentoAnual planejamentoAnual, PeriodoEscolar periodoEscolar, Usuario usuario, DisciplinaDto disciplinaDto)
        {
            var naoExistePlanoAulaCadastrado = (planejamentoAnual?.Id <= 0 || planejamentoAnual.EhNulo()) 
                                                && periodoEscolar.TipoCalendario.AnoLetivo.Equals(DateTime.Now.Year) 
                                                && !usuario.PerfilAtual.Equals(Perfis.PERFIL_CJ) 
                                                && !(disciplinaDto.NaoEhNulo() && disciplinaDto.TerritorioSaber);
            if (naoExistePlanoAulaCadastrado)
                throw new NegocioException(MensagemNegocioPlanoAula.NAO_EXISTE_PLANO_ANUAL_CADASTRADO);
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

        private PlanoAulaResumidoDto MapearParaDto(PlanoAulaDto planoDto, PlanoAula planoAula)
        {
            return new PlanoAulaResumidoDto()
            {
                DescricaoNovo = planoDto.Descricao,
                RecuperacaoAulaNovo = planoDto.RecuperacaoAula,
                LicaoCasaNovo = planoDto.LicaoCasa,
                DescricaoAtual = planoAula?.Descricao ?? string.Empty,
                LicaoCasaAtual = planoAula?.LicaoCasa ?? string.Empty,
                RecuperacaoAulaAtual = planoAula?.RecuperacaoAula ?? string.Empty
            };
        }

        private PlanoAula MapearParaDominio(PlanoAulaDto planoDto, PlanoAula planoAula = null)
        {
            if (planoAula.EhNulo())
                planoAula = new PlanoAula();

            planoAula.AulaId = planoDto.AulaId;
            planoAula.Descricao = TratativaReplaceTempParaArquivoPorRegex(planoDto.Descricao);
            planoAula.RecuperacaoAula = planoDto.RecuperacaoAula?.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            planoAula.LicaoCasa = TratativaReplaceTempParaArquivoPorRegex(planoDto.LicaoCasa);

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
        private string TratativaReplaceTempParaArquivoPorRegex(string descricao)
        {
            string pattern = $@"\b{configuracaoArmazenamentoOptions.Value.BucketTemp}\b";
            return Regex.Replace(descricao, pattern, configuracaoArmazenamentoOptions.Value.BucketArquivos);
        }
    }
}
