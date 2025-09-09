using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse;
using SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Aplicacao.CasosDeUso.HistoricoEscolar;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.CasosDeUso.Informes;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.CasosDeUso.Turma;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dominio.Servicos;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC.Extensions.RegistrarCasoDeUsoRabbitSgp;

namespace SME.SGP.IoC
{
    public class RegistrarDependencias //Raphael. Tirei o static para facilitar o teste
    {
        public virtual void Registrar(IServiceCollection services, IConfiguration configuration)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            
            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
            RegistrarServicos(services);
            RegistrarCasosDeUso(services);
            RegistrarRabbit(services, configuration);
            RegistrarTelemetria(services, configuration);
            RegistrarMetricas(services);
            RegistrarCache(services, configuration);
            RegistrarAuditoria(services);
            RegistrarServicoArmazenamento(services, configuration);


            RegistrarMapeamentos.Registrar();
        }

        protected virtual void RegistrarServicoArmazenamento(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurarArmazenamento(configuration);
        }

        private void RegistrarAuditoria(IServiceCollection services)
        {
            services.ConfigurarAuditoria();
        }

        private void RegistrarCache(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurarCache(configuration);
        }

        public virtual void RegistrarParaWorkers(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();

            RegistrarHttpClients(services, configuration);
            RegistrarPolicies(services);
            RegistrarGoogleClassroomSync(services, configuration);
            RegistrarConsumoFilas(services, configuration);

            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
            RegistrarServicos(services);
            RegistrarRabbit(services, configuration);
            RegistrarTelemetria(services, configuration);
            RegistrarMetricas(services);
            RegistrarCache(services, configuration);
            RegistrarAuditoria(services);
            RegistrarServicoArmazenamento(services,configuration);

            RegistrarMapeamentos.Registrar();
        }

        private void RegistrarMetricas(IServiceCollection services)
        {
            services.ConfigurarMetricasCache();
        }

        public virtual void RegistrarConsumoFilas(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurarConsumoFilas(configuration);
        }

        public virtual void RegistrarHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            services.AdicionarHttpClients(configuration);
        }

        public virtual void RegistrarGoogleClassroomSync(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurarGoogleClassroomSync(configuration);
        }

        protected virtual void RegistrarComandos(IServiceCollection services)
        {
            services.TryAddScoped<IComandosPlanoCiclo, ComandosPlanoCiclo>();
            services.TryAddScoped<IComandosPlanoAnual, ComandosPlanoAnual>();
            services.TryAddScoped<IComandosNotificacao, ComandosNotificacao>();
            services.TryAddScoped<IComandosWorkflowAprovacao, ComandosWorkflowAprovacao>();
            services.TryAddScoped<IComandosUsuario, ComandosUsuario>();
            services.TryAddScoped<IComandosTipoCalendario, ComandosTipoCalendario>();
            services.TryAddScoped<IComandosFeriadoCalendario, ComandosFeriadoCalendario>();
            services.TryAddScoped<IComandosPeriodoEscolar, ComandosPeriodoEscolar>();
            services.TryAddScoped<IComandosEventoTipo, ComandosEventoTipo>();
            services.TryAddScoped<IComandosEvento, ComandosEvento>();
            services.TryAddScoped<IComandosDiasLetivos, ComandosDiasLetivos>();
            services.TryAddScoped<IComandosAtribuicaoEsporadica, ComandosAtribuicaoEsporadica>();
            services.TryAddScoped<IComandosAtividadeAvaliativa, ComandosAtividadeAvaliativa>();
            services.TryAddScoped<IComandosTipoAvaliacao, ComandosTipoAavaliacao>();
            services.TryAddScoped<IComandosAtribuicaoCJ, ComandosAtribuicaoCJ>();
            services.TryAddScoped<IComandosEventoMatricula, ComandosEventoMatricula>();
            services.TryAddScoped<IComandosNotasConceitos, ComandosNotasConceitos>();
            services.TryAddScoped<IComandosAulaPrevista, ComandosAulaPrevista>();
            services.TryAddScoped<IComandosRegistroPoa, ComandosRegistroPoa>();
            services.TryAddScoped<IComandosFechamentoReabertura, ComandosFechamentoReabertura>();
            services.TryAddScoped<IComandosCompensacaoAusencia, ComandosCompensacaoAusencia>();
            services.TryAddScoped<IComandosProcessoExecutando, ComandosProcessoExecutando>();
            services.TryAddScoped<IComandosPeriodoFechamento, ComandosPeriodoFechamento>();
            services.TryAddScoped<IComandosFechamentoTurmaDisciplina, ComandosFechamentoTurmaDisciplina>();
            services.TryAddScoped<IComandosFechamentoFinal, ComandosFechamentoFinal>();
            services.TryAddScoped<IComandosRecuperacaoParalela, ComandosRecuperacaoParalela>();
            services.TryAddScoped<IComandosPendenciaFechamento, ComandosPendenciaFechamento>();
            services.TryAddScoped<IComandosFechamentoTurma, ComandosFechamentoTurma>();
            services.TryAddScoped<IComandoComunicado, ComandoComunicado>();
            services.TryAddScoped<IComandosPlanoAnualTerritorioSaber, ComandosPlanoAnualTerritorioSaber>();
        }

        protected virtual void RegistrarConsultas(IServiceCollection services)
        {
            services.TryAddScoped<IConsultasPlanoCiclo, ConsultasPlanoCiclo>();
            services.TryAddScoped<IConsultasMatrizSaber, ConsultasMatrizSaber>();
            services.TryAddScoped<IConsultasObjetivoDesenvolvimento, ConsultasObjetivoDesenvolvimento>();
            services.TryAddScoped<IConsultasCiclo, ConsultasCiclo>();
            services.TryAddScoped<IConsultasObjetivoAprendizagem, ConsultasObjetivoAprendizagem>();
            services.TryAddScoped<IConsultasPlanoAnual, ConsultasPlanoAnual>();
            services.TryAddScoped<IConsultasDisciplina, ConsultasDisciplina>();
            services.TryAddScoped<IConsultasProfessor, ConsultasProfessor>();
            services.TryAddScoped<IConsultasSupervisor, ConsultasSupervisor>();
            services.TryAddScoped<IConsultaDres, ConsultaDres>();
            services.TryAddScoped<IConsultasNotificacao, ConsultasNotificacao>();
            services.TryAddScoped<IConsultasWorkflowAprovacao, ConsultasWorkflowAprovacao>();
            services.TryAddScoped<IConsultasTipoCalendario, ConsultasTipoCalendario>();
            services.TryAddScoped<IConsultasFeriadoCalendario, ConsultasFeriadoCalendario>();
            services.TryAddScoped<IConsultasPeriodoEscolar, ConsultasPeriodoEscolar>();
            services.TryAddScoped<IConsultasUsuario, ConsultasUsuario>();
            services.TryAddScoped<IConsultasAbrangencia, ConsultasAbrangencia>();
            services.TryAddScoped<IConsultasEventoTipo, ConsultasEventoTipo>();
            services.TryAddScoped<IConsultasEvento, ConsultasEvento>();
            services.TryAddScoped<IConsultasAula, ConsultasAula>();
            services.TryAddScoped<IConsultasEventosAulasCalendario, ConsultasEventosAulasCalendario>();
            services.TryAddScoped<IConsultasGrade, ConsultasGrade>();
            services.TryAddScoped<IConsultasPlanoAula, ConsultasPlanoAula>();
            services.TryAddScoped<IConsultasObjetivoAprendizagemAula, ConsultasObjetivoAprendizagemAula>();
            services.TryAddScoped<IConsultasAtribuicaoEsporadica, ConsultasAtribuicaoEsporadica>();
            services.TryAddScoped<IConsultaAtividadeAvaliativa, ConsultaAtividadeAvaliativa>();
            services.TryAddScoped<IConsultaTipoAvaliacao, ConsultaTipoAvaliacao>();
            services.TryAddScoped<IConsultasAtribuicaoCJ, ConsultasAtribuicaoCJ>();
            services.TryAddScoped<IConsultasUe, ConsultasUe>();
            services.TryAddScoped<IConsultasEventoMatricula, ConsultasEventoMatricula>();
            services.TryAddScoped<IConsultasAulaPrevista, ConsultasAulaPrevista>();
            services.TryAddScoped<IConsultasNotasConceitos, ConsultasNotasConceitos>();
            services.TryAddScoped<IConsultasAtribuicoes, ConsultasAtribuicoes>();
            services.TryAddScoped<IConsultasRegistroPoa, ConsultasRegistroPoa>();
            services.TryAddScoped<IConsultasFechamentoReabertura, ConsultasFechamentoReabertura>();
            services.TryAddScoped<IConsultasCompensacaoAusencia, ConsultasCompensacaoAusencia>();
            services.TryAddScoped<IConsultasCompensacaoAusenciaAluno, ConsultasCompensacaoAusenciaAluno>();
            services.TryAddScoped<IConsultasCompensacaoAusenciaDisciplinaRegencia, ConsultasCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScoped<IConsultasProcessoExecutando, ConsultasProcessoExecutando>();
            services.TryAddScoped<IConsultasPeriodoFechamento, ConsultasPeriodoFechamento>();
            services.TryAddScoped<IConsultasFechamentoTurmaDisciplina, ConsultasFechamentoTurmaDisciplina>();
            services.TryAddScoped<IConsultasFechamentoNota, ConsultasFechamentoNota>();
            services.TryAddScoped<IConsultaRecuperacaoParalela, ConsultasRecuperacaoParalela>();
            services.TryAddScoped<IConsultasFechamentoFinal, ConsultasFechamentoFinal>();
            services.TryAddScoped<IConsultasTurma, ConsultasTurma>();
            services.TryAddScoped<IConsultasPendenciaFechamento, ConsultasPendenciaFechamento>();
            services.TryAddScoped<IConsultasFechamentoAluno, ConsultasFechamentoAluno>();
            services.TryAddScoped<IConsultasFechamentoTurma, ConsultasFechamentoTurma>();
            services.TryAddScoped<IConsultasConselhoClasse, ConsultasConselhoClasse>();
            services.TryAddScoped<IConsultasConselhoClasseNota, ConsultasConselhoClasseNota>();
            services.TryAddScoped<IConsultaComunicado, ConsultaComunicado>();
            services.TryAddScoped<IConsultasRelatorioSemestralPAPAlunoSecao, ConsultasRelatorioSemestralPAPAlunoSecao>();
            services.TryAddScoped<IConsultasSecaoRelatorioSemestralPAP, ConsultasSecaoRelatorioSemestralPAP>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultasPlanoAnualTerritorioSaber, ConsultasPlanoAnualTerritorioSaber>();
            services.TryAddScoped<IRepositorioConceitoConsulta, RepositorioConceitoConsulta>();
            services.TryAddScoped<IRepositorioAulaPrevistaConsulta, RepositorioAulaPrevistaConsulta>();
            services.TryAddScoped<IRepositorioAulaPrevistaBimestreConsulta, RepositorioAulaPrevistaBimestreConsulta>();
            services.TryAddScoped<IRepositorioConsolidacaoMatriculaTurma, RepositorioConsolidacaoMatriculaTurma>();
            services.TryAddScoped<IRepositorioImportacaoLog, RepositorioImportacaoLog>();
            services.TryAddScoped<IConsultasVisaoGeralPainelEducacionalUseCase, ConsultasPainelEducacionalVisaoGeralUseCase>();
        }

        protected virtual void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();
            services.TryAddScoped<ISgpContext, SgpContext>();
            services.TryAddScoped<ISgpContextConsultas, SgpContextConsultas>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        }

        protected virtual void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScoped<IRepositorioPlanoCiclo, RepositorioPlanoCiclo>();
            services.TryAddScoped<IRepositorioMatrizSaberPlano, RepositorioMatrizSaberPlano>();
            services.TryAddScoped<IRepositorioObjetivoDesenvolvimentoPlano, RepositorioObjetivoDesenvolvimentoPlano>();
            services.TryAddScoped<IRepositorioMatrizSaber, RepositorioMatrizSaber>();
            services.TryAddScoped<IRepositorioObjetivoDesenvolvimento, RepositorioObjetivoDesenvolvimento>();
            services.TryAddScoped<IRepositorioCiclo, RepositorioCiclo>();
            services.TryAddScoped<IRepositorioPlanoAnual, RepositorioPlanoAnual>();
            services.TryAddScoped<IRepositorioObjetivoAprendizagemPlano, RepositorioObjetivoAprendizagemPlano>();
            services.TryAddScoped<IRepositorioCache, RepositorioCache>();
            services.TryAddScoped<IRepositorioComponenteCurricularJurema, RepositorioComponenteCurricularJurema>();
            services.TryAddScoped<IRepositorioSupervisorEscolaDre, RepositorioSupervisorEscolaDre>();
            services.TryAddScoped<IRepositorioNotificacao, RepositorioNotificacao>();
            services.TryAddScoped<IRepositorioNotificacaoConsulta, RepositorioNotificacaoConsulta>();
            services.TryAddScoped<IRepositorioWorkflowAprovacao, RepositorioWorkflowAprovacao>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivelNotificacao, RepositorioWorkflowAprovaNivelNotificacao>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivel, RepositorioWorkflowAprovacaoNivel>();
            services.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddScoped<IRepositorioUsuarioConsulta, RepositorioUsuarioConsulta>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivelUsuario, RepositorioWorkflowAprovacaoNivelUsuario>();
            services.TryAddScoped<IRepositorioPrioridadePerfil, RepositorioPrioridadePerfil>();
            services.TryAddScoped<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.TryAddScoped<IRepositorioTipoCalendario, RepositorioTipoCalendario>();
            services.TryAddScoped<IRepositorioTipoCalendarioConsulta, RepositorioTipoCalendarioConsulta>();
            services.TryAddScoped<IRepositorioFeriadoCalendario, RepositorioFeriadoCalendario>();
            services.TryAddScoped<IRepositorioPeriodoEscolar, RepositorioPeriodoEscolar>();
            services.TryAddScoped<IRepositorioPeriodoEscolarConsulta, RepositorioPeriodoEscolarConsulta>();
            services.TryAddScoped<IRepositorioEvento, RepositorioEvento>();
            services.TryAddScoped<IRepositorioParametrosSistema, RepositorioParametrosSistema>();
            services.TryAddScoped<IRepositorioParametrosSistemaConsulta, RepositorioParametrosSistemaConsulta>();
            services.TryAddScoped<IRepositorioAula, RepositorioAula>();
            services.TryAddScoped<IRepositorioAulaConsulta, RepositorioAulaConsulta>();
            services.TryAddScoped<IRepositorioGrade, RepositorioGrade>();
            services.TryAddScoped<IRepositorioGradeFiltro, RepositorioGradeFiltro>();
            services.TryAddScoped<IRepositorioGradeDisciplina, RepositorioGradeDisciplina>();
            services.TryAddScoped<IRepositorioFrequencia, RepositorioFrequencia>();
            services.TryAddScoped<IRepositorioFrequenciaConsulta, RepositorioFrequenciaConsulta>();
            services.TryAddScoped<IRepositorioAtribuicaoEsporadica, RepositorioAtribuicaoEsporadica>();
            services.TryAddScoped<IRepositorioFrequenciaAlunoDisciplinaPeriodo, RepositorioFrequenciaAlunoDisciplinaPeriodo>();
            services.TryAddScoped<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta, RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativa, RepositorioAtividadeAvaliativa>();
            services.TryAddScoped<IRepositorioTipoAvaliacao, RepositorioTipoAvaliacao>();
            services.TryAddScoped<IRepositorioPlanoAula, RepositorioPlanoAula>();
            services.TryAddScoped<IRepositorioObjetivoAprendizagemAula, RepositorioObjetivoAprendizagemAula>();
            services.TryAddScoped<IRepositorioAtribuicaoCJ, RepositorioAtribuicaoCJ>();
            services.TryAddScoped<IRepositorioDre, RepositorioDre>();
            services.TryAddScoped<IRepositorioDreConsulta, RepositorioDreConsulta>();
            services.TryAddScoped<IRepositorioUe, RepositorioUe>();
            services.TryAddScoped<IRepositorioUeConsulta, RepositorioUeConsulta>();
            services.TryAddScoped<IRepositorioTurma, RepositorioTurma>();
            services.TryAddScoped<IRepositorioTurmaConsulta, RepositorioTurmaConsulta>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativaRegencia, RepositorioAtividadeAvaliativaRegencia>();
            services.TryAddScoped<IRepositorioNotasConceitos, RepositorioNotasConceitos>();
            services.TryAddScoped<IRepositorioNotasConceitosConsulta, RepositorioNotasConceitosConsulta>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativa, RepositorioAtividadeAvaliativa>();
            services.TryAddScoped<IRepositorioConceito, RepositorioConceito>();
            services.TryAddScoped<IRepositorioNotaParametro, RepositorioNotaParametro>();
            services.TryAddScoped<IRepositorioNotaTipoValor, RepositorioNotaTipoValor>();
            services.TryAddScoped<IRepositorioNotaTipoValorConsulta, RepositorioNotaTipoValorConsulta>();
            services.TryAddScoped<IRepositorioNotificacaoFrequencia, RepositorioNotificacaoFrequencia>();
            services.TryAddScoped<IRepositorioNotificacaoFrequenciaConsulta, RepositorioNotificacaoFrequenciaConsulta>();
            services.TryAddScoped<IRepositorioEventoMatricula, RepositorioEventoMatricula>();
            services.TryAddScoped<IRepositorioAulaPrevista, RepositorioAulaPrevista>();
            services.TryAddScoped<IRepositorioNotificacaoAulaPrevista, RepositorioNotificacaoAulaPrevista>();
            services.TryAddScoped<IRepositorioAulaPrevistaBimestre, RepositorioAulaPrevistaBimestre>();
            services.TryAddScoped<IRepositorioRegistroPoa, RepositorioRegistroPoa>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativaDisciplina, RepositorioAtividadeAvaliativaDisciplina>();
            services.TryAddScoped<IRepositorioFechamentoReabertura, RepositorioFechamentoReabertura>();
            services.TryAddScoped<IRepositorioFechamentoConsolidado, RepositorioFechamentoConsolidado>();
            services.TryAddScoped<IRepositorioConselhoClasseConsolidado, RepositorioConselhoClasseConsolidado>();
            services.TryAddScoped<IRepositorioConselhoClasseConsolidadoConsulta, RepositorioConselhoClasseConsolidadoConsulta>();
            services.TryAddScoped<IRepositorioConselhoClasseConsolidadoNota, RepositorioConselhoClasseConsolidadoNota>();
            services.TryAddScoped<IRepositorioCompensacaoAusencia, RepositorioCompensacaoAusencia>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaConsulta, RepositorioCompensacaoAusenciaConsulta>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAluno, RepositorioCompensacaoAusenciaAluno>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAlunoConsulta, RepositorioCompensacaoAusenciaAlunoConsulta>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaDisciplinaRegencia, RepositorioCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAlunoAula, RepositorioCompensacaoAusenciaAlunoAula>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAlunoAulaConsulta, RepositorioCompensacaoAusenciaAlunoAulaConsulta>();
            services.TryAddScoped<IRepositorioProcessoExecutando, RepositorioProcessoExecutando>();
            services.TryAddScoped<IRepositorioPeriodoFechamento, RepositorioPeriodoFechamento>();
            services.TryAddScoped<IRepositorioPeriodoFechamentoBimestre, RepositorioPeriodoFechamentoBimestre>();
            services.TryAddScoped<IRepositorioNotificacaoCompensacaoAusencia, RepositorioNotificacaoCompensacaoAusencia>();
            services.TryAddScoped<IRepositorioEventoFechamento, RepositorioEventoFechamento>();
            services.TryAddScoped<IRepositorioEventoFechamentoConsulta, RepositorioEventoFechamentoConsulta>();
            services.TryAddScoped<IRepositorioFechamentoTurmaDisciplina, RepositorioFechamentoTurmaDisciplina>();
            services.TryAddScoped<IRepositorioFechamentoTurmaDisciplinaConsulta, RepositorioFechamentoTurmaDisciplinaConsulta>();
            services.TryAddScoped<IRepositorioFechamentoNota, RepositorioFechamentoNota>();
            services.TryAddScoped<IRepositorioFechamentoNotaConsulta, RepositorioFechamentoNotaConsulta>();
            services.TryAddScoped<IRepositorioRecuperacaoParalela, RepositorioRecuperacaoParalela>();
            services.TryAddScoped<IRepositorioRecuperacaoParalelaPeriodo, RepositorioRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta, RepositorioRecuperacaoParalelaPeriodoObjetivoResposta>();
            services.TryAddScoped<IRepositorioEixo, RepositorioEixo>();
            services.TryAddScoped<IRepositorioResposta, RepositorioResposta>();
            services.TryAddScoped<IRepositorioObjetivo, RepositorioObjetivo>();
            services.TryAddScoped<IRepositorioNotificacaoAula, RepositorioNotificacaoAula>();
            services.TryAddScoped<IRepositorioHistoricoEmailUsuario, RepositorioHistoricoEmailUsuario>();
            services.TryAddScoped<IRepositorioSintese, RepositorioSintese>();
            services.TryAddScoped<IRepositorioFechamentoAluno, RepositorioFechamentoAluno>();
            services.TryAddScoped<IRepositorioFechamentoAlunoConsulta, RepositorioFechamentoAlunoConsulta>();
            services.TryAddScoped<IRepositorioFechamentoTurma, RepositorioFechamentoTurma>();
            services.TryAddScoped<IRepositorioFechamentoTurmaConsulta, RepositorioFechamentoTurmaConsulta>();
            services.TryAddScoped<IRepositorioConselhoClasse, RepositorioConselhoClasse>();
            services.TryAddScoped<IRepositorioConselhoClasseConsulta, RepositorioConselhoClasseConsulta>();
            services.TryAddScoped<IRepositorioConselhoClasseAluno, RepositorioConselhoClasseAluno>();
            services.TryAddScoped<IRepositorioConselhoClasseAlunoConsulta, RepositorioConselhoClasseAlunoConsulta>();
            services.TryAddScoped<IRepositorioConselhoClasseAlunoTurmaComplementar, RepositorioConselhoClasseAlunoTurmaComplementar>();
            services.TryAddScoped<IRepositorioConselhoClasseNotaConsulta, RepositorioConselhoClasseNotaConsulta>();
            services.TryAddScoped<IRepositorioConselhoClasseNota, RepositorioConselhoClasseNota>();
            services.TryAddScoped<IRepositorioComunicado, RepositorioComunicado>();
            services.TryAddScoped<IRepositorioWfAprovacaoNotaFechamento, RepositorioWfAprovacaoNotaFechamento>();
            services.TryAddScoped<IRepositorioWFAprovacaoNotaConselho, RepositorioWFAprovacaoNotaConselho>();
            services.TryAddScoped<IRepositorioWFAprovacaoParecerConclusivo, RepositorioWFAprovacaoParecerConclusivo>();
            services.TryAddScoped<IRepositorioConselhoClasseRecomendacao, RepositorioConselhoClasseRecomendacao>();
            services.TryAddScoped<IRepositorioCicloEnsino, RepositorioCicloEnsino>();
            services.TryAddScoped<IRepositorioTipoEscola, RepositorioTipoEscola>();
            services.TryAddScoped<IRepositorioRelatorioSemestralTurmaPAP, RepositorioRelatorioSemestralTurmaPAP>();
            services.TryAddScoped<IRepositorioRelatorioSemestralPAPAluno, RepositorioRelatorioSemestralPAPAluno>();
            services.TryAddScoped<IRepositorioSecaoRelatorioSemestralPAP, RepositorioSecaoRelatorioSemestralPAP>();
            services.TryAddScoped<IRepositorioObjetivoAprendizagem, RepositorioObjetivoAprendizagem>();
            services.TryAddScoped<IRepositorioConselhoClasseParecerConclusivo, RepositorioConselhoClasseParecerConclusivoConsulta>();
            services.TryAddScoped<IRepositorioPlanoAnualTerritorioSaber, RepositorioPlanoAnualTerritorioSaber>();
            services.TryAddScoped<IRepositorioCorrelacaoRelatorio, RepositorioCorrelacaoRelatorio>();
            services.TryAddScoped<IRepositorioCorrelacaoRelatorioJasper, RepositorioRelatorioCorrelacaoJasper>();
            services.TryAddScoped<IRepositorioFechamentoReaberturaBimestre, RepositorioFechamentoReaberturaBimestre>();
            services.TryAddScoped<IRepositorioHistoricoReinicioSenha, RepositorioHistoricoReinicioSenha>();
            services.TryAddScoped<IRepositorioComunicadoAluno, RepositorioComunicadoAluno>();
            services.TryAddScoped<IRepositorioComunicadoTurma, RepositorioComunicadoTurma>();
            services.TryAddScoped<IRepositorioComunicadoModalidade, RepositorioComunicadoModalidade>();
            services.TryAddScoped<IRepositorioComunicadoTipoEscola, RepositorioComunicadoTipoEscola>();
            services.TryAddScoped<IRepositorioComunicadoAnoEscolar, RepositorioComunicadoAnoEscolar>();
            services.TryAddScoped<IRepositorioDiarioBordo, RepositorioDiarioBordo>();
            services.TryAddScoped<IRepositorioDiarioBordoConsulta, RepositorioDiarioBordoConsulta>();
            services.TryAddScoped<IRepositorioDevolutiva, RepositorioDevolutiva>();
            services.TryAddScoped<IRepositorioAnoEscolar, RepositorioAnoEscolar>();
            services.TryAddScoped<IRepositorioCartaIntencoes, RepositorioCartaIntencoes>();
            services.TryAddScoped<IRepositorioDiarioBordoObservacao, RepositorioDiarioBordoObservacao>();
            services.TryAddScoped<IRepositorioAnotacaoFrequenciaAluno, RepositorioAnotacaoFrequenciaAluno>();
            services.TryAddScoped<IRepositorioAnotacaoFrequenciaAlunoConsulta, RepositorioAnotacaoFrequenciaAlunoConsulta>();
            services.TryAddScoped<IRepositorioMotivoAusencia, RepositorioMotivoAusencia>();
            services.TryAddScoped<IRepositorioCartaIntencoesObservacao, RepositorioCartaIntencoesObservacao>();
            services.TryAddScoped<IRepositorioPlanejamentoAnual, RepositorioPlanejamentoAnual>();
            services.TryAddScoped<IRepositorioPlanejamentoAnualPeriodoEscolar, RepositorioPlanejamentoAnualPeriodoEscolar>();
            services.TryAddScoped<IRepositorioPlanejamentoAnualComponente, RepositorioPlanejamentoAnualComponente>();
            services.TryAddScoped<IRepositorioPlanejamentoAnualObjetivosAprendizagem, RepositorioPlanejamentoAnualObjetivosAprendizagem>();
            services.TryAddScoped<IRepositorioNotificacaoCartaIntencoesObservacao, RepositorioNotificacaoCartaIntencoesObservacao>();
            services.TryAddScoped<IRepositorioDiarioBordoObservacaoNotificacao, RepositorioDiarioBordoObservacaoNotificacao>();
            services.TryAddScoped<IRepositorioNotificacaoDevolutiva, RepositorioNotificacaoDevolutiva>();
            services.TryAddScoped<IRepositorioComponenteCurricular, RepositorioComponenteCurricular>();
            services.TryAddScoped<IRepositorioComponenteCurricularConsulta, RepositorioComponenteCurricularConsulta>();
            services.TryAddScoped<IRepositorioArquivo, RepositorioArquivo>();
            services.TryAddScoped<IRepositorioHistoricoNota, RepositorioHistoricoNota>();
            services.TryAddScoped<IRepositorioHistoricoNotaConselhoClasse, RepositorioHistoricoNotaConselhoClasse>();
            services.TryAddScoped<IRepositorioHistoricoNotaFechamento, RepositorioHistoricoNotaFechamento>();
            services.TryAddScoped<IRepositorioDocumento, RepositorioDocumento>();
            services.TryAddScoped<IRepositorioDocumentoArquivo, RepositorioDocumentoArquivo>();
            services.TryAddScoped<IRepositorioClassificacaoDocumento, RepositorioClassificacaoDocumento>();
            services.TryAddScoped<IRepositorioTipoDocumento, RepositorioTipoDocumento>();
            services.TryAddScoped<IRepositorioRemoveConexaoIdle, RepositorioRemoveConexaoIdle>();
            services.TryAddScoped<IRepositorioRegistroIndividual, RepositorioRegistroIndividual>();
            services.TryAddScoped<IRepositorioOcorrencia, RepositorioOcorrencia>();
            services.TryAddScoped<IRepositorioOcorrenciaAluno, RepositorioOcorrenciaAluno>();
            services.TryAddScoped<IRepositorioOcorrenciaTipo, RepositorioOcorrenciaTipo>();
            services.TryAddScoped<IRepositorioOcorrenciaServidor, RepositorioOcorrenciaServidor>();
            services.TryAddScoped<IRepositorioAlunoFoto, RepositorioAlunoFoto>();
            services.TryAddScoped<IRepositorioAreaDoConhecimento, RepositorioAreaDoConhecimento>();
            services.TryAddScoped<IRepositorioComponenteCurricularGrupoAreaOrdenacao, RepositorioComponenteCurricularGrupoAreaOrdenacao>();
            services.TryAddScoped<IRepositorioPendenciaDevolutiva, RepositorioPendenciaDevolutiva>();
            services.TryAddScoped<IRepositorioFrequenciaDiariaAluno, RepositorioFrequenciaDiariaAluno>();
            services.TryAddScoped<IRepositorioConselhoClasseAlunoRecomendacaoConsulta, RepositorioConselhoClasseAlunoRecomendacaoConsulta>();
            services.TryAddScoped<IRepositorioEventoConsulta, RepositorioEventoConsulta>();
            services.TryAddScoped<IRepositorioComunicadoConsulta, RepositorioComunicadoConsulta>();

            // Acompanhamento Aluno
            services.TryAddScoped<IRepositorioAcompanhamentoAluno, RepositorioAcompanhamentoAluno>();
            services.TryAddScoped<IRepositorioAcompanhamentoAlunoConsulta, RepositorioAcompanhamentoAlunoConsulta>();
            services.TryAddScoped<IRepositorioAcompanhamentoAlunoConsulta, RepositorioAcompanhamentoAlunoConsulta>();
            services.TryAddScoped<IRepositorioAcompanhamentoTurma, RepositorioAcompanhamentoTurma>();
            services.TryAddScoped<IRepositorioAcompanhamentoAlunoSemestre, RepositorioAcompanhamentoAlunoSemestre>();
            services.TryAddScoped<IRepositorioAcompanhamentoAlunoFoto, RepositorioAcompanhamentoAlunoFoto>();

            // Mural de Avisos
            services.TryAddScoped<IRepositorioAviso, RepositorioAviso>();
            services.TryAddScoped<IRepositorioAtividadeInfantil, RepositorioAtividadeInfantil>();

            // Encaminhamento AEE
            services.TryAddScoped<IRepositorioSecaoEncaminhamentoAEE, RepositorioSecaoEncaminhamentoAEE>();
            services.TryAddScoped<IRepositorioEncaminhamentoAEE, RepositorioEncaminhamentoAEE>();
            services.TryAddScoped<IRepositorioEncaminhamentoAEESecao, RepositorioEncaminhamentoAEESecao>();
            services.TryAddScoped<IRepositorioQuestaoEncaminhamentoAEE, RepositorioQuestaoEncaminhamentoAEE>();
            services.TryAddScoped<IRepositorioRespostaEncaminhamentoAEE, RepositorioRespostaEncaminhamentoAEE>();
            services.TryAddScoped<IRepositorioEncaminhamentoAEETurmaAluno, RepositorioEncaminhamentoAEETurmaAluno>();
            services.TryAddScoped<IRepositorioEncaminhamentoAEETurmaAlunoConsulta, RepositorioEncaminhamentoAEETurmaAlunoConsulta>();

            // EventoTipo
            services.TryAddScoped<IRepositorioEventoTipo, RepositorioEventoTipo>();
            services.TryAddScoped<IRepositorioPerfilEventoTipo, RepositorioPerfilEventoTipo>();

            // Fechamento
            services.TryAddScoped<IRepositorioAnotacaoFechamentoAluno, RepositorioAnotacaoFechamentoAluno>();
            services.TryAddScoped<IRepositorioAnotacaoFechamentoAlunoConsulta, RepositorioAnotacaoFechamentoAlunoConsulta>();

            // Pendencias do EncaminhamentoAEE
            services.TryAddScoped<IRepositorioPendenciaEncaminhamentoAEE, RepositorioPendenciaEncaminhamentoAEE>();

            // Questionario
            services.TryAddScoped<IRepositorioQuestionario, RepositorioQuestionario>();
            services.TryAddScoped<IRepositorioQuestao, RepositorioQuestao>();
            services.TryAddScoped<IRepositorioOpcaoResposta, RepositorioOpcaoResposta>();
            services.TryAddScoped<IRepositorioOpcaoQuestaoComplementar, RepositorioOpcaoQuestaoComplementar>();

            // Pendencias
            services.TryAddScoped<IRepositorioPendencia, RepositorioPendencia>();
            services.TryAddScoped<IRepositorioPendenciaConsulta, RepositorioPendenciaConsulta>();
            services.TryAddScoped<IRepositorioPendenciaFechamento, RepositorioPendenciaFechamento>();
            services.TryAddScoped<IRepositorioPendenciaAula, RepositorioPendenciaAula>();
            services.TryAddScoped<IRepositorioPendenciaAulaConsulta, RepositorioPendenciaAulaConsulta>();
            services.TryAddScoped<IRepositorioPendenciaUsuario, RepositorioPendenciaUsuario>();
            services.TryAddScoped<IRepositorioPendenciaUsuarioConsulta, RepositorioPendenciaUsuarioConsulta>();
            services.TryAddScoped<IRepositorioPendenciaCalendarioUe, RepositorioPendenciaCalendarioUe>();
            services.TryAddScoped<IRepositorioPendenciaParametroEvento, RepositorioPendenciaParametroEvento>();
            services.TryAddScoped<IRepositorioPendenciaProfessor, RepositorioPendenciaProfessor>();
            services.TryAddScoped<IRepositorioPendenciaRegistroIndividual, RepositorioPendenciaRegistroIndividual>();
            services.TryAddScoped<IRepositorioPendenciaRegistroIndividualAluno, RepositorioPendenciaRegistroIndividualAluno>();
            services.TryAddScoped<IRepositorioPendenciaPerfil, RepositorioPendenciaPerfil>();
            services.TryAddScoped<IRepositorioPendenciaPerfilUsuario, RepositorioPendenciaPerfilUsuario>();
            services.TryAddScoped<IRepositorioPendenciaDiarioBordo, RepositorioPendenciaDiarioBordo>();
            services.TryAddScoped<IRepositorioPendenciaDiarioBordoConsulta, RepositorioPendenciaDiarioBordoConsulta>();
            services.TryAddScoped<IRepositorioPendenciaFechamentoAula, RepositorioPendenciaFechamentoAula>();
            services.TryAddScoped<IRepositorioPendenciaFechamentoAulaConsulta, RepositorioPendenciaFechamentoAulaConsulta>();
            services.TryAddScoped<IRepositorioPendenciaFechamentoAtividadeAvaliativa, RepositorioPendenciaFechamentoAtividadeAvaliativa>();
            services.TryAddScoped<IRepositorioPendenciaFechamentoAtividadeAvaliativaConsulta, RepositorioPendenciaFechamentoAtividadeAvaliativaConsulta>();
            services.TryAddScoped<IRepositorioPendenciaAulaReposicaoConsulta, RepositorioPendenciaAulaReposicaoConsulta>();

            // Itinerancia
            services.TryAddScoped<IRepositorioItinerancia, RepositorioItinerancia>();
            services.TryAddScoped<IRepositorioItineranciaAluno, RepositorioItineranciaAluno>();
            services.TryAddScoped<IRepositorioItineranciaAlunoQuestao, RepositorioItineranciaAlunoQuestao>();
            services.TryAddScoped<IRepositorioItineranciaQuestao, RepositorioItineranciaQuestao>();
            services.TryAddScoped<IRepositorioItineranciaObjetivo, RepositorioItineranciaObjetivo>();
            services.TryAddScoped<IRepositorioWfAprovacaoItinerancia, RepositorioWfAprovacaoItinerancia>();

            services.TryAddScoped<IRepositorioItineranciaEvento, RepositorioItineranciaEvento>();

            // PlanoAEE
            services.TryAddScoped<IRepositorioPlanoAEE, RepositorioPlanoAEE>();
            services.TryAddScoped<IRepositorioPlanoAEEConsulta, RepositorioPlanoAEEConsulta>();
            services.TryAddScoped<IRepositorioPlanoAEEVersao, RepositorioPlanoAEEVersao>();
            services.TryAddScoped<IRepositorioPlanoAEEQuestao, RepositorioPlanoAEEQuestao>();
            services.TryAddScoped<IRepositorioPlanoAEEResposta, RepositorioPlanoAEEResposta>();
            services.TryAddScoped<IRepositorioPlanoAEEReestruturacao, RepositorioPlanoAEEReestruturacao>();
            services.TryAddScoped<IRepositorioPendenciaPlanoAEE, RepositorioPendenciaPlanoAEE>();

            services.TryAddScoped<IRepositorioPlanoAEEObservacao, RepositorioPlanoAEEObservacao>();
            services.TryAddScoped<IRepositorioNotificacaoPlanoAEEObservacao, RepositorioNotificacaoPlanoAEEObservacao>();
            services.TryAddScoped<IRepositorioPlanoAEETurmaAluno, RepositorioPlanoAEETurmaAluno>();
            services.TryAddScoped<IRepositorioPlanoAEETurmaAlunoConsulta, RepositorioPlanoAEETurmaAlunoConsulta>();

            // Notificações Plano AEE
            services.TryAddScoped<IRepositorioNotificacaoPlanoAEE, RepositorioNotificacaoPlanoAEE>();

            // Consolidação Frequeência Turma
            services.TryAddScoped<IRepositorioConsolidacaoFrequenciaTurma, RepositorioConsolidacaoFrequenciaTurma>();
            services.TryAddScoped<IRepositorioConsolidacaoFrequenciaTurmaConsulta, RepositorioConsolidacaoFrequenciaTurmaConsulta>();

            // Consolidação Frequência Aluno Mensal
            services.TryAddScoped<IRepositorioConsolidacaoFrequenciaAlunoMensal, RepositorioConsolidacaoFrequenciaAlunoMensal>();

            // Frequência turma evasão
            services.TryAddScoped<IRepositorioFrequenciaTurmaEvasao, RepositorioFrequenciaTurmaEvasao>();
            services.TryAddScoped<IRepositorioFrequenciaTurmaEvasaoAluno, RepositorioFrequenciaTurmaEvasaoAluno>();

            // Consolidação Devolutivas
            services.TryAddScoped<IRepositorioConsolidacaoDevolutivas, RepositorioConsolidacaoDevolutivas>();
            services.TryAddScoped<IRepositorioConsolidacaoDevolutivasConsulta, RepositorioConsolidacaoDevolutivasConsulta>();

            // Frequência
            services.TryAddScoped<IRepositorioFrequenciaPreDefinida, RepositorioFrequenciaPreDefinida>();
            services.TryAddScoped<IRepositorioFrequenciaPreDefinidaConsulta, RepositorioFrequenciaPreDefinidaConsulta>();
            services.TryAddScoped<IRepositorioRegistroFrequenciaAluno, RepositorioRegistroFrequenciaAluno>();
            services.TryAddScoped<IRepositorioRegistroFrequenciaAlunoConsulta, RepositorioRegistroFrequenciaAlunoConsulta>();
            services.TryAddScoped<IRepositorioDashBoardFrequencia, RepositorioDashBoardFrequencia>();

            //Evento Bimestre
            services.TryAddScoped<IRepositorioEventoBimestre, RepositorioEventoBimestre>();
            //Consolidacao registro individual média
            services.TryAddScoped<IRepositorioConsolidacaoRegistroIndividualMedia, RepositorioConsolidacaoRegistroIndividualMedia>();

            // Consolidacao de Acompanhamento Aprendizagem Aluno
            services.TryAddScoped<IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno, RepositorioConsolidacaoAcompanhamentoAprendizagemAluno>();

            // Consolidacao de Diarios de Bordo
            services.TryAddScoped<IRepositorioConsolidacaoDiariosBordo, RepositorioConsolidacaoDiariosBordo>();

            // Consolidacao de Registros Pedagogicos
            services.TryAddScoped<IRepositorioConsolidacaoRegistrosPedagogicos, RepositorioConsolidacaoRegistrosPedagogicos>();

            services.TryAddScoped<IRepositorioConselhoClasseAlunoRecomendacao, RepositorioConselhoClasseAlunoRecomendacao>();

            services.TryAddScoped<IRepositorioSuporteUsuario, RepositorioSuporteUsuario>();

            services.TryAddScoped<IRepositorioTipoRelatorio, RepositorioTipoRelatorio>();

            // Encaminhamento NAAPA
            services.TryAddScoped<IRepositorioSecaoEncaminhamentoNAAPA, RepositorioSecaoEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioEncaminhamentoNAAPA, RepositorioEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioEncaminhamentoNAAPASecao, RepositorioEncaminhamentoNAAPASecao>();
            services.TryAddScoped<IRepositorioQuestaoEncaminhamentoNAAPA, RepositorioQuestaoEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioRespostaEncaminhamentoNAAPA, RepositorioRespostaEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioObservacaoEncaminhamentoNAAPA, RepositorioObservacaoEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes, RepositorioEncaminhamentoNAAPAHistoricoAlteracoes>();
            services.TryAddScoped<IRepositorioConsolidadoEncaminhamentoNAAPA, RepositorioConsolidadoEncaminhamentoNAAPA>();
            services.TryAddScoped<IRepositorioConsolidadoAtendimentoNAAPA, RepositorioConsolidadoAtendimentoNAAPA>();

            services.TryAddScoped<IRepositorioHistoricoEscolarObservacao, RepositorioHistoricoEscolarObservacao>();

            //Relatório PAP
            services.TryAddScoped<IRepositorioConfiguracaoRelatorioPAP, RepositorioConfiguracaoRelatorioPAP>();
            services.TryAddScoped<IRepositorioPeriodoRelatorioPAP, RepositorioPeriodoRelatorioPAP>();
            services.TryAddScoped<IRepositorioSecaoRelatorioPeriodicoPAP, RepositorioSecaoRelatorioPeriodicoPAP>();
            services.TryAddScoped<IRepositorioRelatorioPeriodicoPAPTurma, RepositorioRelatorioPeriodicoPAPTurma>();
            services.TryAddScoped<IRepositorioRelatorioPeriodicoPAPAluno, RepositorioRelatorioPeriodicoPAPAluno>();
            services.TryAddScoped<IRepositorioRelatorioPeriodicoPAPSecao, RepositorioRelatorioPeriodicoPAPSecao>();
            services.TryAddScoped<IRepositorioRelatorioPeriodicoPAPQuestao, RepositorioRelatorioPeriodicoPAPQuestao>();
            services.TryAddScoped<IRepositorioRelatorioPeriodicoPAPResposta, RepositorioRelatorioPeriodicoPAPResposta>();

            //Relatório dinâmico NAAPA
            services.TryAddScoped<IRepositorioRelatorioDinamicoNAAPA, RepositorioRelatorioDinamicoNAAPA>();

            //Informativo
            services.TryAddScoped<IRepositorioInformativo, RepositorioInformativo>();
            services.TryAddScoped<IRepositorioInformativoPerfil, RepositorioInformativoPerfil>();
            services.TryAddScoped<IRepositorioInformativoNotificacao, RepositorioInformativoNotificacao>();
            services.TryAddScoped<IRepositorioInformativoAnexo, RepositorioInformativoAnexo>();
            services.TryAddScoped<IRepositorioInformativoModalidade, RepositorioInformativoModalidade>();

            //CadastroAcessoABAE
            services.TryAddScoped<IRepositorioCadastroAcessoABAE, RepositorioCadastroAcessoABAE>();
            services.TryAddScoped<IRepositorioCadastroAcessoABAEConsulta, RepositorioCadastroAcessoABAEConsulta>();

            //Consulta Crianças Estudantes Ausentes
            services.TryAddScoped<IRepositorioConsultaCriancasEstudantesAusentes, RepositorioConsultaCriancasEstudantesAusentes>();

            // Registro Ação Busca Ativa 
            services.TryAddScoped<IRepositorioSecaoRegistroAcaoBuscaAtiva, RepositorioSecaoRegistroAcaoBuscaAtiva>();
            services.TryAddScoped<IRepositorioRegistroAcaoBuscaAtiva, RepositorioRegistroAcaoBuscaAtiva>();
            services.TryAddScoped<IRepositorioRegistroAcaoBuscaAtivaSecao, RepositorioRegistroAcaoBuscaAtivaSecao>();
            services.TryAddScoped<IRepositorioQuestaoRegistroAcaoBuscaAtiva, RepositorioQuestaoRegistroAcaoBuscaAtiva>();
            services.TryAddScoped<IRepositorioRespostaRegistroAcaoBuscaAtiva, RepositorioRespostaRegistroAcaoBuscaAtiva>();
            services.TryAddScoped<IRepositorioDashBoardBuscaAtiva, RepositorioDashBoardBuscaAtiva>();
            services.TryAddScoped<IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva, RepositorioConsolidacaoReflexoFrequenciaBuscaAtiva>();

            // Registro Coletivo NAAPA
            services.TryAddScoped<IRepositorioTipoReuniaoNAAPA, RepositorioTipoReuniaoNAAPA>();
            services.TryAddScoped<IRepositorioRegistroColetivo, RepositorioRegistroColetivo>();
            services.TryAddScoped<IRepositorioRegistroColetivoUe, RepositorioRegistroColetivoUe>();
            services.TryAddScoped<IRepositorioRegistroColetivoAnexo, RepositorioRegistroColetivoAnexo>();

            services.TryAddScoped<IRepositorioObjetivoAprendizagemConsulta, RepositorioObjetivoAprendizagemConsulta>();

            // Mapeamento Estudante
            services.TryAddScoped<IRepositorioSecaoMapeamentoEstudante, RepositorioSecaoMapeamentoEstudante>();
            services.TryAddScoped<IRepositorioMapeamentoEstudante, RepositorioMapeamentoEstudante>();
            services.TryAddScoped<IRepositorioMapeamentoEstudanteSecao, RepositorioMapeamentoEstudanteSecao>();
            services.TryAddScoped<IRepositorioQuestaoMapeamentoEstudante, RepositorioQuestaoMapeamentoEstudante>();
            services.TryAddScoped<IRepositorioRespostaMapeamentoEstudante, RepositorioRespostaMapeamentoEstudante>();

            services.TryAddScoped<IRepositorioConsolidacaoProdutividadeFrequencia, RepositorioConsolidacaoProdutividadeFrequencia>();
            services.TryAddScoped<IRepositorioInatividadeAtendimentoNAAPANotificacao, RepositorioInatividadeAtendimentoNAAPANotificacao>();

            //ImportacaoLog - Arquivos
            services.TryAddScoped<IRepositorioImportacaoLog, RepositorioImportacaoLog>();
            services.TryAddScoped<IRepositorioImportacaoLogErro, RepositorioImportacaoLogErro>();
            services.TryAddScoped<IRepositorioIdeb, RepositorioIdeb>();
            services.TryAddScoped<IRepositorioIdep, RepositorioIdep>();
            services.TryAddScoped<IRepositorioFluenciaLeitora, RepositorioFluenciaLeitora>();
            
            services.TryAddScoped<IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal, RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobal>();
            services.TryAddScoped<IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola, RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola>();
            services.TryAddScoped<IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal, RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal>();

            //Painel Educacional
            services.AddTransient<IRepositorioIdepPainelEducacionalConsulta, RepositorioIdepPainelEducacionalConsulta>();
            services.AddTransient<IRepositorioIdepPainelEducacionalConsolidacao, RepositorioIdepPainelEducacionalConsolidacao>();
            services.TryAddScoped<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita, RepositorioConsolidacaoAlfabetizacaoNivelEscrita>();
            services.TryAddScoped<IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita, RepositorioConsolidacaoAlfabetizacaoCriticaEscrita>();
            services.TryAddScoped<IRepositorioPainelEducacionalVisaoGeral, RepositorioPainelEducacionalVisaoGeral>();
        }

        protected virtual void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScoped<IServicoWorkflowAprovacao, ServicoWorkflowAprovacao>();
            services.TryAddScoped<IServicoNotificacao, ServicoNotificacao>();
            services.TryAddScoped<IServicoUsuario, ServicoUsuario>();
            services.TryAddScoped<IServicoAutenticacao, ServicoAutenticacao>();
            services.TryAddScoped<IServicoPerfil, ServicoPerfil>();
            services.TryAddScoped<IServicoMenu, ServicoMenu>();
            services.TryAddScoped<IServicoPeriodoEscolar, ServicoPeriodoEscolar>();
            services.TryAddScoped<IServicoFeriadoCalendario, ServicoFeriadoCalendario>();
            services.TryAddScoped<IServicoAbrangencia, ServicoAbrangencia>();
            services.TryAddScoped<IServicoEvento, ServicoEvento>();
            services.TryAddScoped<IServicoAtribuicaoEsporadica, ServicoAtribuicaoEsporadica>();
            services.TryAddScoped<IServicoCalculoFrequencia, ServicoCalculoFrequencia>();
            services.TryAddScoped<IServicoDeNotasConceitos, ServicoDeNotasConceitos>();
            services.TryAddScoped<IServicoNotificacaoFrequencia, ServicoNotificacaoFrequencia>();
            services.TryAddScoped<IServicoAtribuicaoCJ, ServicoAtribuicaoCJ>();
            services.TryAddScoped<IServicoAluno, ServicoAluno>();
            services.TryAddScoped<IServicoFechamentoReabertura, ServicoFechamentoReabertura>();
            services.TryAddScoped<IServicoCompensacaoAusencia, ServicoCompensacaoAusencia>();
            services.TryAddScoped<IServicoPeriodoFechamento, ServicoPeriodoFechamento>();
            services.TryAddScoped<IServicoFechamentoTurmaDisciplina, ServicoFechamentoTurmaDisciplina>();
            services.TryAddScoped<IServicoRecuperacaoParalela, ServicoRecuperacaoParalela>();
            services.TryAddScoped<IServicoPendenciaFechamento, ServicoPendenciaFechamento>();
            services.TryAddScoped<IServicoFechamentoFinal, ServicoFechamentoFinal>();
            services.TryAddScoped<IServicoObjetivosAprendizagem, ServicoObjetivosAprendizagem>();
        }

        public virtual void RegistrarCasoDeUsoAEERabbitSgp(IServiceCollection services)
        {
            services.RegistrarAEEUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoAulaRabbitSgp(IServiceCollection services)
        {
            services.RegistrarAulaUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoFechamentoRabbitSgp(IServiceCollection services)
        {
            services.RegistrarFechamentoUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoFrequenciaRabbitSgp(IServiceCollection services)
        {
            services.RegistrarFrequenciaUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoInstitucionalRabbitSgp(IServiceCollection services)
        {
            services.RegistrarInstitucionalUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoPendenciasRabbitSgp(IServiceCollection services)
        {
            services.RegistrarPendenciasUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoNAAPARabbitSgp(IServiceCollection services)
        {
            services.RegistrarNAAPAUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoRabbitSgp(IServiceCollection services)
        {
            services.RegistrarUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoAvaliacaoRabbitSgp(IServiceCollection services)
        {
            services.RegistrarAvaliacaoUseCaseRabbitSgp();
        }

        public virtual void RegistrarCasoDeUsoPainelEducacionalRabbitSgp(IServiceCollection services)
        {
            services.RegistrarPainelEducacionalUseCaseRabbitSgp();
        }

        protected virtual void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.TryAddScoped<IObterPrefixosCacheUseCase, ObterPrefixosCacheUseCase>();
            services.TryAddScoped<IObterUltimaVersaoUseCase, ObterUltimaVersaoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseAlunoUseCase, ImpressaoConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseTurmaUseCase, ImpressaoConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IBoletimUseCase, BoletimUseCase>();
            services.TryAddScoped<IBoletimEscolaAquiUseCase, BoletimEscolaAquiUseCase>();
            services.TryAddScoped<IObterListaAlunosDaTurmaUseCase, ObterListaAlunosDaTurmaUseCase>();
            services.TryAddScoped<IReceberDadosDownloadRelatorioUseCase, ReceberDadosDownloadRelatorioUseCase>();
            services.TryAddScoped<IRelatorioConselhoClasseAtaFinalUseCase, RelatorioConselhoClasseAtaFinalUseCase>();
            services.TryAddScoped<IGamesUseCase, GamesUseCase>();
            services.TryAddScoped<IReiniciarSenhaUseCase, ReiniciarSenhaUseCase>();
            services.TryAddScoped<IInserirAulaUseCase, InserirAulaUseCase>();
            services.TryAddScoped<IAlterarAulaUseCase, AlterarAulaUseCase>();
            services.TryAddScoped<IObterAulaPorIdUseCase, ObterAulaPorIdUseCase>();
            services.TryAddScoped<IExcluirAulaUseCase, ExcluirAulaUseCase>();
            services.TryAddScoped<IPodeCadastrarAulaUseCase, PodeCadastrarAulaUseCase>();
            services.TryAddScoped<IObterAulasPorTurmaComponenteDataUseCase, ObterAulasPorTurmaComponenteDataUseCase>();
            services.TryAddScoped<IObterFuncionariosUseCase, ObterFuncionariosUseCase>();            
            services.TryAddScoped<IObterBimestresLiberacaoBoletimUseCase, ObterBimestresLiberacaoBoletimUseCase>();
            services.TryAddScoped<IReceberRelatorioComErroUseCase, ReceberRelatorioComErroUseCase>();
            services.TryAddScoped<IHistoricoEscolarUseCase, HistoricoEscolarUseCase>();
            services.TryAddScoped<IObterAlunosPorCodigoEolNomeUseCase, ObterAlunosPorCodigoEolNomeUseCase>();
            services.TryAddScoped<IGerarRelatorioFrequenciaUseCase, GerarRelatorioFrequenciaUseCase>();
            services.TryAddScoped<IGerarRelatorioFrequenciaMensalUseCase, GerarRelatorioFrequenciaMensalUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosDresPorAbrangenciaUseCase, ObterFiltroRelatoriosDresPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosUesPorAbrangenciaUseCase, ObterFiltroRelatoriosUesPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosModalidadesPorUeUseCase, ObterFiltroRelatoriosModalidadesPorUeUseCase>();
            services.TryAddScoped<IRelatorioPendenciasUseCase, RelatorioPendenciasFechamentoUseCase>();
            services.TryAddScoped<IObterAnotacaoFrequenciaAlunoUseCase, ObterAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase, ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase>();            
            services.TryAddScoped<IObterDiarioBordoUseCase, ObterDiarioBordoUseCase>();
            services.TryAddScoped<IObterDiarioBordoPorIdUseCase, ObterDiarioBordoPorIdUseCase>();
            services.TryAddScoped<IInserirDiarioBordoUseCase, InserirDiarioBordoUseCase>();
            services.TryAddScoped<IAlterarDiarioBordoUseCase, AlterarDiarioBordoUseCase>();
            services.TryAddScoped<IObterFrequenciaOuPlanoNaRecorrenciaUseCase, ObterFrequenciaOuPlanoNaRecorrenciaUseCase>();
            services.TryAddScoped<IObterDatasAulasPorTurmaEComponenteUseCase, ObterDatasAulasPorTurmaEComponenteUseCase>();
            services.TryAddScoped<IRelatorioRecuperacaoParalelaUseCase, RelatorioRecuperacaoParalelaUseCase>();
            services.TryAddScoped<IRelatorioParecerConclusivoUseCase, RelatorioParecerConclusivoUseCase>();
            services.TryAddScoped<IObterCiclosPorModalidadeECodigoUeUseCase, ObterCiclosPorModalidadeECodigoUeUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase, ObterFiltroRelatoriosModalidadesPorUeAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosAnosPorCicloModalidadeUseCase, ObterFiltroRelatoriosAnosPorCicloModalidadeUseCase>();
            services.TryAddScoped<IRelatorioNotasEConceitosFinaisUseCase, RelatorioNotasEConceitosFinaisUseCase>();
            services.TryAddScoped<IRelatorioAtribuicaoCJUseCase, RelatorioAtribuicaoCJUseCase>();
            services.TryAddScoped<IObterJustificativasAlunoPorComponenteCurricularUseCase, ObterJustificativasAlunoPorComponenteCurricularUseCase>();
            services.TryAddScoped<IObterFrequenciaDiariaAlunoUseCase, ObterFrequenciaDiariaAlunoUseCase>();
            services.TryAddScoped<IObterUsuarioFuncionarioUseCase, ObterUsuarioFuncionarioUseCase>();
            services.TryAddScoped<IObterObservacoesDosAlunosNoHistoricoEscolarUseCase, ObterObservacoesDosAlunosNoHistoricoEscolarUseCase>();

            services.TryAddScoped<IExcluirDevolutivaUseCase, ExcluirDevolutivaUseCase>();
            services.TryAddScoped<IObterListaDevolutivasPorTurmaComponenteUseCase, ObterListaDevolutivasPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterDiariosBordoPorDevolutiva, ObterDiariosBordoPorDevolutiva>();
            services.TryAddScoped<IObterDevolutivaPorIdUseCase, ObterDevolutivaPorIdUseCase>();
            services.TryAddScoped<IObterDiariosDeBordoPorPeriodoUseCase, ObterDiariosDeBordoPorPeriodoUseCase>();
            services.TryAddScoped<IObterListagemDiariosDeBordoPorPeriodoUseCase, ObterListagemDiariosDeBordoPorPeriodoUseCase>();
            services.TryAddScoped<IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase, ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterUsuarioNotificarDiarioBordoObservacaoUseCase, ObterUsuarioNotificarDiarioBordoObservacaoUseCase>();
            services.TryAddScoped<IRelatorioCompensacaoAusenciaUseCase, RelatorioCompensacaoAusenciaUseCase>();
            services.TryAddScoped<IRelatorioCalendarioUseCase, RelatorioCalendarioUseCase>();
            services.TryAddScoped<IRelatorioAnaliticoSondagemUseCase, RelatorioAnaliticoSondagemUseCase>();
            services.TryAddScoped<IRelatorioListagemItineranciasUseCase, RelatorioListagemItineranciasUseCase>();

            services.TryAddScoped<ICartaIntencoesPersistenciaUseCase, CartaIntencoesPersistenciaUseCase>();
            services.TryAddScoped<IObterCartasDeIntencoesPorTurmaEComponenteUseCase, ObterCartasDeIntencoesPorTurmaEComponenteUseCase>();
            services.TryAddScoped<IObterUsuarioNotificarCartaIntencoesObservacaoUseCase, ObterUsuarioNotificarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IAdicionarObservacaoDiarioBordoUseCase, AdicionarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IAlterarObservacaoDiarioBordoUseCase, AlterarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IExcluirObservacaoDiarioBordoUseCase, ExcluirObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IExcluirDiarioBordoUseCase, ExcluirDiarioBordoUseCase>();
            services.TryAddScoped<IListarObservacaoDiarioBordoUseCase, ListarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterSintesePorAnoLetivoUseCase, ObterSintesePorAnoLetivoUseCase>();                                    
            services.TryAddScoped<ISalvarAnotacaoFrequenciaAlunoUseCase, SalvarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IAlterarAnotacaoFrequenciaAlunoUseCase, AlterarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IExcluirAnotacaoFrequenciaAlunoUseCase, ExcluirAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterAnotacaoFrequenciaAlunoPorIdUseCase, ObterAnotacaoFrequenciaAlunoPorIdUseCase>();
            services.TryAddScoped<IObterMotivosAusenciaUseCase, ObterMotivosAusenciaUseCase>();
            services.TryAddScoped<IObterFechamentoConsolidadoPorTurmaBimestreUseCase, ObterFechamentoConsolidadoPorTurmaBimestreUseCase>();
            services.TryAddScoped<IObterFechamentoConselhoClasseAlunosPorTurmaUseCase, ObterFechamentoConselhoClasseAlunosPorTurmaUseCase>();
            services.TryAddScoped<IObterDetalhamentoFechamentoConselhoClasseAlunoUseCase, ObterDetalhamentoFechamentoConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase, ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase>();
            services.TryAddScoped<IIniciaConsolidacaoTurmaGeralUseCase, IniciaConsolidacaoTurmaGeralUseCase>();
            services.TryAddScoped<IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase, ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase>();
            services.TryAddScoped<IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase, ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase>();
            services.TryAddScoped<IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase, ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase>();            
            services.TryAddScoped<IConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase, ConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase>();
            services.AddScoped<IConsultasIdepPainelEducacionalUseCase, ConsultasIdepPainelEducacionalUseCase>();

            services.TryAddScoped<IObterDashBoardUseCase, ObterDashBoardUseCase>();
            services.TryAddScoped<IInserirDevolutivaUseCase, InserirDevolutivaUseCase>();
            services.TryAddScoped<IAlterarDevolutivaUseCase, AlterarDevolutivaUseCase>();
            services.TryAddScoped<IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase, ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase>();
            services.TryAddScoped<IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase, ObterUltimasNotificacoesNaoLidasPorUsuarioUseCase>();

            // Abrangencia
            services.TryAddScoped<IObterUEsPorDreUseCase, ObterUEsPorDreUseCase>();
            services.TryAddScoped<IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase, ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase>();
            services.TryAddScoped<IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase, ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase>();
            services.TryAddScoped<IUsuarioPossuiAbrangenciaAcessoSondagemUseCase, UsuarioPossuiAbrangenciaAcessoSondagemUseCase>();
            services.TryAddScoped<IUsuarioPossuiAbrangenciaAdmUseCase, UsuarioPossuiAbrangenciaAdmUseCase>();
            services.TryAddScoped<IObterModalidadesPorAnoUseCase, ObterModalidadesPorAnoUseCase>();
            services.TryAddScoped<IObterAbrangenciaDresUseCase, ObterAbrangenciaDresUseCase>();
            services.TryAddScoped<IObterTurmasNaoHistoricasUseCase, ObterTurmasNaoHistoricasUseCase>();
            services.TryAddScoped<ICarregarAbrangenciaUsusarioUseCase, CarregarAbrangenciaUsusarioUseCase>();
            
            services.TryAddScoped<IRabbitDeadletterSerapSyncUseCase, RabbitDeadletterSerapSyncUseCase>();

            // Acompanhamento Aluno
            services.TryAddScoped<ISalvarAcompanhamentoAlunoUseCase, SalvarAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<ISalvarFotoAcompanhamentoAlunoUseCase, SalvarFotoAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterFotosSemestreAlunoUseCase, ObterFotosSemestreAlunoUseCase>();
            services.TryAddScoped<IExcluirFotoAlunoUseCase, ExcluirFotoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoAlunoUseCase, ObterAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoAlunoUseCase, ObterAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoTurmaApanhadoGeralUseCase, ObterAcompanhamentoTurmaApanhadoGeralUseCase>();
            services.TryAddScoped<IObterValidacaoPercusoRAAUseCase, ObterValidacaoPercusoRAAUseCase>();

            // Acompanhamento Turma
            services.TryAddScoped<ISalvarAcompanhamentoTurmaUseCase, SalvarAcompanhamentoTurmaUseCase>();
            services.TryAddScoped<IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase, ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase>();

            // Area do Conhecimento
            services.TryAddScoped<IObterAreasConhecimentoUseCase, ObterAreasConhecimentoUseCase>();
            services.TryAddScoped<IObterOrdenacaoAreasConhecimentoUseCase, ObterOrdenacaoAreasConhecimentoUseCase>();
            services.TryAddScoped<IMapearAreasDoConhecimentoUseCase, MapearAreasDoConhecimentoUseCase>();
            services.TryAddScoped<IObterComponentesDasAreasDeConhecimentoUseCase, ObterComponentesDasAreasDeConhecimentoUseCase>();

            // Armazenamento de arquivos
            services.TryAddScoped<IUploadDeArquivoUseCase, UploadDeArquivoUseCase>();
            services.TryAddScoped<IDownloadDeArquivoUseCase, DownloadDeArquivoUseCase>();
            services.TryAddScoped<IExcluirArquivoUseCase, ExcluirArquivoUseCase>();
            services.TryAddScoped<IUploadDeArquivoItineranciaUseCase, UploadDeArquivoItineranciaUseCase>();
            services.TryAddScoped<IExcluirArquivoItineranciaUseCase, ExcluirArquivoItineranciaUseCase>();
            services.TryAddScoped<IExcluirArquivosUseCase, ExcluirArquivosUseCase>();

            // Atividades
            services.TryAddScoped<IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase, ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase>();

            // Atividade Infantil
            services.TryAddScoped<IObterAtividadesInfantilUseCase, ObterAtividadesInfantilUseCase>();

            // Avisos do Mural Gsa
            services.TryAddScoped<IObterMuralAvisosUseCase, ObterMuralAvisosUseCase>();
            services.TryAddScoped<IAlterarAvisoMuralUseCase, AlterarAvisoMuralUseCase>();

            //Carta Intenções Observacao
            services.TryAddScoped<IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase, ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase>();
            services.TryAddScoped<ISalvarCartaIntencoesObservacaoUseCase, SalvarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IAlterarCartaIntencoesObservacaoUseCase, AlterarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirCartaIntencoesObservacaoUseCase, ExcluirCartaIntencoesObservacaoUseCase>();

            // Compensacao Ausencia
            services.TryAddScoped<IPeriodoDeCompensacaoAbertoUseCase, PeriodoDeCompensacaoAbertoUseCase>();
            services.TryAddScoped<ICopiarCompensacaoAusenciaUseCase, CopiarCompensacaoAusenciaUseCase>();
            services.TryAddScoped<IExcluirCompensacaoAusenciaUseCase, ExcluirCompensacaoAusenciaUseCase>();

            // Componentes Curriculares
            services.TryAddScoped<IObterComponentesCurricularesPorTurmaECodigoUeUseCase, ObterComponentesCurricularesPorTurmaECodigoUeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorUeAnosModalidadeUseCase, ObterComponentesCurricularesPorUeAnosModalidadeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorAnoEscolarUseCase, ObterComponentesCurricularesPorAnoEscolarUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase, ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase>();

            // Conselho de Classe
            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterPareceresConclusivosUseCase, ObterPareceresConclusivosUseCase>();
            services.TryAddScoped<IObterTotalAulasPorAlunoTurmaUseCase, ObterTotalAulasPorAlunoTurmaUseCase>();
            services.TryAddScoped<IObterTotalAulasSemFrequenciaPorTurmaUseCase, ObterTotalAulasSemFrequenciaPorTurmaUseCase>();
            services.TryAddScoped<IObterTotalAulasNaoLancamNotaUseCase, ObterTotalAulasNaoLancamNotaUseCase>();
            services.TryAddScoped<IObterTotalCompensacoesComponenteNaoLancaNotaUseCase, ObterTotalCompensacoesComponenteNaoLancaNotaUseCase>();
            services.TryAddScoped<IObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase, ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase>();
            services.TryAddScoped<IObterRecomendacoesAlunoFamiliaUseCase, ObterRecomendacoesAlunoFamiliaUseCase>();
            services.TryAddScoped<IConsolidarConselhoClasseUseCase, ConsolidarConselhoClasseUseCase>();
            services.TryAddScoped<IGerarParecerConclusivoUseCase, GerarParecerConclusivoUseCase>();
            services.TryAddScoped<ISalvarConselhoClasseAlunoNotaUseCase, SalvarConselhoClasseAlunoNotaUseCase>();
            services.TryAddScoped<IConsultaConselhoClasseRecomendacaoUseCase, ConsultaConselhoClasseRecomendacaoUseCase>();
            services.TryAddScoped<IObterAlunosSemNotasRecomendacoesUseCase, ObterAlunosSemNotasRecomendacoesUseCase>();
            services.TryAddScoped<IObterSinteseConselhoDeClasseUseCase, ObterSinteseConselhoDeClasseUseCase>();
            services.TryAddScoped<IObterNotasFrequenciaUseCase, ObterNotasFrequenciaUseCase>();
            services.TryAddScoped<IObterParecerConclusivoUseCase, ObterParecerConclusivoUseCase>();
            services.TryAddScoped<IObterParecerConclusivoAlunoTurmaUseCase, ObterParecerConclusivoAlunoTurmaUseCase>();
            services.TryAddScoped<ISalvarConselhoClasseAlunoRecomendacaoUseCase, SalvarConselhoClasseAlunoRecomendacaoUseCase>();
            services.TryAddScoped<IObterPareceresConclusivosTurmaUseCase, ObterPareceresConclusivosTurmaUseCase>();
            services.TryAddScoped<IAlterarParecerConclusivoUseCase, AlterarParecerConclusivoUseCase>();
            services.TryAddScoped<IObterPareceresConclusivosAnoLetivoModalidadeUseCase, ObterPareceresConclusivosAnoLetivoModalidadeUseCase>();

            // Fechamento
            services.TryAddScoped<IExecutarVarreduraFechamentosEmProcessamentoPendentes, ExecutarVarreduraFechamentosEmProcessamentoPendentes>();
            services.TryAddScoped<IInserirFechamentoTurmaDisciplinaUseCase, InserirFechamentoTurmaDisciplinaUseCase>();
            services.TryAddScoped<IObterFechamentoIdPorTurmaBimestreUseCase, ObterFechamentoIdPorTurmaBimestreUseCase>();
            services.TryAddScoped<IGerarFechamentoTurmaEdFisica2020UseCase, GerarFechamentoTurmaEdFisica2020UseCase>();
            services.TryAddScoped<IGerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase, GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase>();

            // Fechamento Aluno
            services.TryAddScoped<ISalvarAnotacaoFechamentoAlunoUseCase, SalvarAnotacaoFechamentoAlunoUseCase>();

            // Comunicados EA
            services.TryAddScoped<ISolicitarInclusaoComunicadoEscolaAquiUseCase, SolicitarInclusaoComunicadoEscolaAquiUseCase>();
            services.TryAddScoped<ISolicitarAlteracaoComunicadoEscolaAquiUseCase, SolicitarAlteracaoComunicadoEscolaAquiUseCase>();
            services.TryAddScoped<ISolicitarExclusaoComunicadosEscolaAquiUseCase, SolicitarExclusaoComunicadosEscolaAquiUseCase>();
            services.TryAddScoped<IObterComunicadoEscolaAquiUseCase, ObterComunicadoEscolaAquiUseCase>();
            services.TryAddScoped<IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase, ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase>();
            services.TryAddScoped<IObterComunicadosPaginadosEscolaAquiUseCase, ObterComunicadosPaginadosEscolaAquiUseCase>();
            services.TryAddScoped<IMigrarPlanejamentoAnualUseCase, MigrarPlanejamentoAnualUseCase>();
            services.TryAddScoped<IObterTurmasParaCopiaUseCase, ObterTurmasParaCopiaUseCase>();
            services.TryAddScoped<IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase, ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase>();
            services.TryAddScoped<IListarEventosPorCalendarioUseCase, ListarEventosPorCalendarioUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosUseCase, ObterDadosDeLeituraDeComunicadosUseCase>();
            services.TryAddScoped<IObterComunicadosPaginadosAlunoUseCase, ObterComunicadosPaginadosAlunoUseCase>();
            services.TryAddScoped<IObterAnosLetivosComunicadoUseCase, ObterAnosLetivosComunicadoUseCase>();
            services.TryAddScoped<IObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase, ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresUseCase>();
            services.TryAddScoped<IObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase, ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase>();

            // Dashboard - Acompanhamento de Aprendizagem
            services.TryAddScoped<IObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase, ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase>();
            services.TryAddScoped<IObterDashboardAcompanhamentoAprendizagemUseCase, ObterDashboardAcompanhamentoAprendizagemUseCase>();
            services.TryAddScoped<IObterDashboardAcompanhamentoAprendizagemPorDreUseCase, ObterDashboardAcompanhamentoAprendizagemPorDreUseCase>();

            // Dashboard Diário de bordo
            services.TryAddScoped<IObterQuantidadeTotalDeDevolutivasPorDREUseCase, ObterQuantidadeTotalDeDevolutivasPorDREUseCase>();
            services.TryAddScoped<IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase, ObterQuantidadeTotalDeDiariosPendentesPorDREUseCase>();
            services.TryAddScoped<IObterUltimaConsolidacaoDiarioBordoUseCase, ObterUltimaConsolidacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase, ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase>();

            // Dashboard devolutivas
            services.TryAddScoped<IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase, ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase>();
            services.TryAddScoped<IObterDevolutivasEstimadasEConfirmadasUseCase, ObterDevolutivasEstimadasEConfirmadasUseCase>();
            services.TryAddScoped<IObterPeriodoDeDiasDevolutivaUseCase, ObterPeriodoDeDiasDevolutivaUseCase>();
            services.TryAddScoped<IObterUltimaConsolidacaoDevolutivaUseCase, ObterUltimaConsolidacaoDevolutivaUseCase>();
            services.TryAddScoped<IObterGraficoTotalDevolutivasPorDreUseCase, ObterGraficoTotalDevolutivasPorDreUseCase>();

            // Dashboard EA
            services.TryAddScoped<IObterTotalUsuariosComAcessoIncompletoUseCase, ObterTotalUsuariosComAcessoIncompletoUseCase>();
            services.TryAddScoped<IObterTotalUsuariosValidosUseCase, ObterTotalUsuariosValidosUseCase>();
            services.TryAddScoped<IObterTotaisAdesaoUseCase, ObterTotaisAdesaoUseCase>();
            services.TryAddScoped<IObterTotaisAdesaoAgrupadosPorDreUseCase, ObterTotaisAdesaoAgrupadosPorDreUseCase>();
            services.TryAddScoped<IObterUltimaAtualizacaoPorProcessoUseCase, ObterUltimaAtualizacaoPorProcessoUseCase>();
            services.TryAddScoped<IObterComunicadosTotaisUseCase, ObterComunicadosTotaisUseCase>();
            services.TryAddScoped<IObterComunicadosTotaisAgrupadosPorDreUseCase, ObterComunicadosTotaisAgrupadosPorDreUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase, ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase>();
            services.TryAddScoped<IObterComunicadosParaFiltroDaDashboardUseCase, ObterComunicadosParaFiltroDaDashboardUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosPorModalidadeUseCase, ObterDadosDeLeituraDeComunicadosPorModalidadeUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase, ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase, ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase>();

            // Listão
            services.TryAddScoped<IListarTurmasComComponentesUseCase, ListarTurmasComComponentesUseCase>();

            // Dias Letivos
            services.TryAddScoped<IObterDiasLetivosPorCalendarioUseCase, ObterDiasLetivosPorCalendarioUseCase>();

            //Editor
            services.TryAddScoped<IUploadArquivoEditorUseCase, UploadArquivoEditorUseCase>();
            services.TryAddScoped<ICopiarServicoArmazenamentoUseCase, CopiarServicoArmazenamentoUseCase>();
            services.TryAddScoped<IExcluirTemporarioServicoArmazenamentoUseCase, ExcluirTemporarioServicoArmazenamentoUseCase>();
            services.TryAddScoped<IObterServicoArmazenamentoUseCase, ObterServicoArmazenamentoUseCase>();

            // EncaminhamentoAEE
            services.TryAddScoped<IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase, ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterQuestionarioEncaminhamentoAeeUseCase, ObterQuestionarioEncaminhamentoAeeUseCase>();
            services.TryAddScoped<IExcluirArquivoAeeUseCase, ExcluirArquivoAeeUseCase>();
            services.TryAddScoped<IExcluirEncaminhamentoAEEUseCase, ExcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterInstrucoesModalUseCase, ObterInstrucoesModalUseCase>();
            services.TryAddScoped<IObterEncaminhamentosAEEUseCase, ObterEncaminhamentosAEEUseCase>();
            services.TryAddScoped<IPesquisaResponsavelEncaminhamentoPorDreUEUseCase, PesquisaResponsavelEncaminhamentoPorDreUEUseCase>();
            services.TryAddScoped<IPesquisaResponsavelPlanoPorDreUEUseCase, PesquisaResponsavelPlanoPorDreUEUseCase>();
            services.TryAddScoped<IRelatorioEncaminhamentoAEEUseCase, RelatorioEncaminhamentoAeeUseCase>();
            
            //Encaminhamento NAAPA
            services.TryAddScoped<IRelatorioEncaminhamentoNaapaDetalhadoUseCase, RelatorioEncaminhamentoNaapaDetalhadoUseCase>();
            
            // Funcionario
            services.TryAddScoped<IPesquisaFuncionariosPorDreUeUseCase, PesquisaFuncionariosPorDreUeUseCase>();
            services.TryAddScoped<IObterFuncionariosPAAIPorDreUseCase, ObterFuncionariosPAAIPorDreUseCase>();
            services.TryAddScoped<IObterFuncionariosPorUeUseCase, ObterFuncionariosPorUeUseCase>();

            // Grade Curricular
            services.TryAddScoped<IRelatorioControleGradeUseCase, RelatorioControleGradeUseCase>();

            //Notificacao Devolutiva
            services.TryAddScoped<ISalvarNotificacaoDevolutivaUseCase, SalvarNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDevolutivaUseCase, ExcluirNotificacaoDevolutivaUseCase>();

            services.TryAddScoped<IObterUsuarioPorCpfUseCase, ObterUsuarioPorCpfUseCase>();
            services.TryAddScoped<ISolicitarReiniciarSenhaEscolaAquiUseCase, SolicitarReiniciarSenhaEscolaAquiUseCase>();
            services.TryAddScoped<IObterAnosLetivosPAPUseCase, ObterAnosLetivosPAPUseCase>();

            services.TryAddScoped<IBuscarTiposCalendarioPorDescricaoUseCase, BuscarTiposCalendarioPorDescricaoUseCase>();
            
            services.TryAddScoped<ISalvarPlanejamentoAnualUseCase, SalvarPlanejamentoAnualUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase, ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase, ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponenteUseCase, ObterPlanejamentoAnualPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase, ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase>();
            services.TryAddScoped<IObterPeriodoLetivoTurmaUseCase, ObterPeriodoLetivoTurmaUseCase>();

            // Frequência
            services.TryAddScoped<IExecutaConsolidacaoFrequenciaPorAnoUseCase, ExecutaConsolidacaoFrequenciaPorAnoUseCase>();
            services.TryAddScoped<IInserirFrequenciaListaoUseCase, InserirFrequenciaListaoUseCase>();
            services.TryAddScoped<IVerificaFrequenciaRegistradaAlunosInativosUseCase, VerificaFrequenciaRegistradaAlunosInativosUseCase>();

            //Objetivo Curricular
            services.TryAddScoped<IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase, ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase>();

            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterObjetivosPorDisciplinaUseCase, ObterObjetivosPorDisciplinaUseCase>();

            // Parecer Conclusivo
            services.TryAddScoped<IReprocessarParecerConclusivoPorAnoUseCase, ReprocessarParecerConclusivoPorAnoUseCase>();

            //Pendencias Devolutiva

            // Pendencias
            services.TryAddScoped<IObterPendenciasUseCase, ObterPendenciasUseCase>();

            //Notificação Resultado Insatisfatorio
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();

            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();

            // Periodo Fechamento
            services.TryAddScoped<IObterPeriodoFechamentoVigenteUseCase, ObterPeriodoFechamentoVigenteUseCase>();
            services.TryAddScoped<IPeriodoFechamentoUseCase, PeriodoFechamentoUseCase>();

            //PeriodoEscolar
            services.TryAddScoped<IObterPeriodosPorComponenteUseCase, ObterPeriodosPorComponenteUseCase>();

            // Plano AEE
            services.TryAddScoped<IObterPlanoAEEPorIdUseCase, ObterPlanoAEEPorIdUseCase>();
            services.TryAddScoped<IObterQuestoesPlanoAEEPorVersaoUseCase, ObterQuestoesPlanoAEEPorVersaoUseCase>();
            services.TryAddScoped<IObterPlanoAEEPorCodigoEstudanteUseCase, ObterPlanoAEEPorCodigoEstudanteUseCase>();
            services.TryAddScoped<IVerificarExistenciaPlanoAEEPorEstudanteUseCase, VerificarExistenciaPlanoAEEPorEstudanteUseCase>();
            services.TryAddScoped<IImpressaoPlanoAeeUseCase, ImpressaoPlanoAeeUseCase>();
            services.TryAddScoped<IObterSrmPaeeColaborativoUseCase, ObterSrmPaeeColaborativoUseCase>();
            services.TryAddScoped<IObterResponsaveisPlanosAEEUseCase, ObterResponsaveisPlanosAEEUseCase>();

            // Plano Aula
            services.TryAddScoped<IObterPlanoAulaUseCase, ObterPlanoAulaUseCase>();
            services.TryAddScoped<IExcluirPlanoAulaUseCase, ExcluirPlanoAulaUseCase>();
            services.TryAddScoped<IMigrarPlanoAulaUseCase, MigrarPlanoAulaUseCase>();
            services.TryAddScoped<ISalvarPlanoAulaUseCase, SalvarPlanoAulaUseCase>();
            services.TryAddScoped<IObterPlanoAulasPorTurmaEComponentePeriodoUseCase, ObterPlanoAulasPorTurmaEComponentePeriodoUseCase>();

            // Relatórios
            services.TryAddScoped<IRelatorioPlanoAulaUseCase, RelatorioPlanoAulaUseCase>();
            services.TryAddScoped<IRelatorioPlanoAnualUseCase, RelatorioPlanoAnualUseCase>();
            services.TryAddScoped<IRelatorioUsuariosUseCase, RelatorioUsuariosUseCase>();
            services.TryAddScoped<IRelatorioControleFrequenciaMensalUseCase, RelatorioControleFrequenciaMensalUseCase>();
            services.TryAddScoped<IGerarRelatorioProdutividadeFrequenciaUseCase, GerarRelatorioProdutividadeFrequenciaUseCase>();

            //Sincronismo CC Eol
            services.TryAddScoped<IListarComponentesCurricularesEolUseCase, ListarComponentesCurricularesEolUseCase>();

            services.TryAddScoped<IObterComponentesCurricularesRegenciaPorTurmaUseCase, ObterComponentesCurricularesRegenciaPorTurmaUseCase>();
            services.TryAddScoped<IObterPeriodoEscolarPorTurmaUseCase, ObterPeriodoEscolarPorTurmaUseCase>();

            services.TryAddScoped<IRelatorioResumoPAPUseCase, RelatorioResumoPAPUseCase>();
            services.TryAddScoped<IRelatorioGraficoPAPUseCase, RelatorioGraficoPAPUseCase>();

            services.TryAddScoped<IVerificarUsuarioDocumentoUseCase, VerificarUsuarioDocumentoUseCase>();
            services.TryAddScoped<IListarTiposDeDocumentosUseCase, ListarTiposDeDocumentosUseCase>();
            services.TryAddScoped<IListarDocumentosUseCase, ListarDocumentosUseCase>();
            services.TryAddScoped<ISalvarDocumentoUseCase, SalvarDocumentoUseCase>();
            services.TryAddScoped<IUploadDocumentoUseCase, UploadDocumentoUseCase>();
            services.TryAddScoped<IExcluirDocumentoArquivoUseCase, ExcluirDocumentoArquivoUseCase>();
            services.TryAddScoped<IExcluirDocumentoUseCase, ExcluirDocumentoUseCase>();
            services.TryAddScoped<IObterDocumentoUseCase, ObterDocumentoUseCase>();
            services.TryAddScoped<IAlterarDocumentoUseCase, AlterarDocumentoUseCase>();
            services.TryAddScoped<IRelatorioNotificacaoUseCase, RelatorioNotificacaoUseCase>();
            services.TryAddScoped<IRelatorioAlteracaoNotasUseCase, RelatorioAlteracaoNotasUseCase>();

            // Usuários
            services.TryAddScoped<IObterListaSituacoesUsuarioUseCase, ObterListaSituacoesUsuarioUseCase>();
            services.TryAddScoped<IObterHierarquiaPerfisUsuarioUseCase, ObterHierarquiaPerfisUsuarioUseCase>();

            services.TryAddScoped<IObterTiposCalendarioPorAnoLetivoModalidadeUseCase, ObterTiposCalendarioPorAnoLetivoModalidadeUseCase>();

            services.TryAddScoped<IRelatorioAEAdesaoUseCase, RelatorioAEAdesaoUseCase>();

            services.TryAddScoped<IRelatorioLeituraComunicadosUseCase, RelatorioLeituraComunicadosUseCase>();
            services.TryAddScoped<IRelatorioPlanejamentoDiarioUseCase, RelatorioPlanejamentoDiarioUseCase>();

            services.TryAddScoped<IRemoveConexaoIdleUseCase, RemoveConexaoIdleUseCase>();
            services.TryAddScoped<IDeslogarSuporteUsuarioUseCase, DeslogarSuporteUsuarioUseCase>();
            services.TryAddScoped<IObterGuidAutenticacaoFrequencia, ObterGuidAutenticacaoFrequencia>();
            services.TryAddScoped<IObterAutenticacaoFrequencia, ObterAutenticacaoFrequencia>();            

            // EncaminhamentoAEE
            services.TryAddScoped<IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase, ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterQuestionarioEncaminhamentoAeeUseCase, ObterQuestionarioEncaminhamentoAeeUseCase>();
            services.TryAddScoped<IExcluirArquivoAeeUseCase, ExcluirArquivoAeeUseCase>();
            services.TryAddScoped<IExcluirEncaminhamentoAEEUseCase, ExcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterInstrucoesModalUseCase, ObterInstrucoesModalUseCase>();
            services.TryAddScoped<IObterEncaminhamentosAEEUseCase, ObterEncaminhamentosAEEUseCase>();
            services.TryAddScoped<IEncerrarEncaminhamentoAEEUseCase, EncerrarEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IEnviarParaAnaliseEncaminhamentoAEEUseCase, EnviarParaAnaliseEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase, VerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase>();
            services.TryAddScoped<IConcluirEncaminhamentoAEEUseCase, ConcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterResponsaveisEncaminhamentosAEE, ObterResponsaveisEncaminhamentosAEE>();
            services.TryAddScoped<IDevolverEncaminhamentoUseCase, DevolverEncaminhamentoUseCase>();
            services.TryAddScoped<IRelatorioEncaminhamentoAeeDetalhadoUseCase, RelatorioEncaminhamentoAeeDetalhadoUseCase>();

            // Plano AEE
            services.TryAddScoped<IObterPlanosAEEUseCase, ObterPlanosAEEUseCase>();
            services.TryAddScoped<IObterSituacaoEncaminhamentoPorEstudanteUseCase, ObterSituacaoEncaminhamentoPorEstudanteUseCase>();
            services.TryAddScoped<ISalvarPlanoAEEUseCase, SalvarPlanoAEEUseCase>();
            services.TryAddScoped<IObterVersoesPlanoAEEUseCase, ObterVersoesPlanoAEEUseCase>();
            services.TryAddScoped<IObterRestruturacoesPlanoAEEPorIdUseCase, ObterRestruturacoesPlanoAEEPorIdUseCase>();

            services.TryAddScoped<IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase, ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase>();
            services.TryAddScoped<IObterParecerPlanoAEEPorIdUseCase, ObterParecerPlanoAEEPorIdUseCase>();
            services.TryAddScoped<IEncerrarPlanoAEEUseCase, EncerrarPlanoAEEUseCase>();

            services.TryAddScoped<IObterAlunoPorCodigoEolEAnoLetivoUseCase, ObterAlunoPorCodigoEolEAnoLetivoUseCase>();
            services.TryAddScoped<IRegistrarEncaminhamentoAEEUseCase, RegistrarEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterEncaminhamentoPorIdUseCase, ObterEncaminhamentoPorIdUseCase>();
            services.TryAddScoped<IObterInformacoesEscolaresDoAlunoUseCase, ObterInformacoesEscolaresDoAlunoUseCase>();
            services.TryAddScoped<IEncerrarEncaminhamentoAEEUseCase, EncerrarEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IEnviarParaAnaliseEncaminhamentoAEEUseCase, EnviarParaAnaliseEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IAtribuirResponsavelEncaminhamentoAEEUseCase, AtribuirResponsavelEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IRemoverResponsavelEncaminhamentoAEEUseCase, RemoverResponsavelEncaminhamentoAEEUseCase>();

            services.TryAddScoped<IAlterarRegistroIndividualUseCase, AlterarRegistroIndividualUseCase>();
            services.TryAddScoped<IInserirRegistroIndividualUseCase, InserirRegistroIndividualUseCase>();
            services.TryAddScoped<IExcluirRegistroIndividualUseCase, ExcluirRegistroIndividualUseCase>();
            services.TryAddScoped<IListarAlunosDaTurmaRegistroIndividualUseCase, ListarAlunosDaTurmaRegistroIndividualUseCase>();
            services.TryAddScoped<IObterRegistroIndividualPorAlunoDataUseCase, ObterRegistroIndividualPorAlunoDataUseCase>();
            services.TryAddScoped<IObterRegistroIndividualUseCase, ObterRegistroIndividualUseCase>();
            services.TryAddScoped<IObterRegistrosIndividuaisPorAlunoPeriodoUseCase, ObterRegistrosIndividuaisPorAlunoPeriodoUseCase>();
            services.TryAddScoped<IObterSugestaoTopicoRegistroIndividualPorMesUseCase, ObterSugestaoTopicoRegistroIndividualPorMesUseCase>();

            services.TryAddScoped<IListarOcorrenciasUseCase, ListarOcorrenciasUseCase>();
            services.TryAddScoped<IListarTiposOcorrenciaUseCase, ListarTiposOcorrenciaUseCase>();
            services.TryAddScoped<IObterOcorrenciaUseCase, ObterOcorrenciaUseCase>();
            services.TryAddScoped<IAlterarOcorrenciaUseCase, AlterarOcorrenciaUseCase>();
            services.TryAddScoped<IExcluirOcorrenciaUseCase, ExcluirOcorrenciaUseCase>();
            services.TryAddScoped<IInserirOcorrenciaUseCase, InserirOcorrenciaUseCase>();
            services.TryAddScoped<IObterOcorrenciasPorAlunoUseCase, ObterOcorrenciasPorAlunoUseCase>();
            services.TryAddScoped<IRelatorioOcorrenciasUseCase, RelatorioOcorrenciasUseCase>();
            services.TryAddScoped<IRelatorioListagemOcorrenciasUseCase, RelatorioListagemOcorrenciasUseCase>();

            // Itinerancia
            services.TryAddScoped<IObterObjetivosBaseUseCase, ObterObjetivosBaseUseCase>();
            services.TryAddScoped<IObterQuestoesItineranciaAlunoUseCase, ObterQuestoesItineranciaAlunoUseCase>();
            services.TryAddScoped<IObterQuestoesBaseUseCase, ObterQuestoesBaseUseCase>();
            services.TryAddScoped<IObterItineranciaPorIdUseCase, ObterItineranciaPorIdUseCase>();
            services.TryAddScoped<ISalvarItineranciaUseCase, SalvarItineranciaUseCase>();
            services.TryAddScoped<IExcluirItineranciaUseCase, ExcluirItineranciaUseCase>();
            services.TryAddScoped<IAlterarItineranciaUseCase, AlterarItineranciaUseCase>();
            services.TryAddScoped<IObterItineranciasUseCase, ObterItineranciasUseCase>();
            services.TryAddScoped<IObterAnosLetivosItineranciaUseCase, ObterAnosLetivosItineranciaUseCase>();
            services.TryAddScoped<IObterRfsPorNomesItineranciaUseCase, ObterRfsPorNomesItineranciaUseCase>();
            services.TryAddScoped<IRelatorioItineranciasUseCase, RelatorioItineranciasUseCase>();

            // Plano AEE
            services.TryAddScoped<ICadastrarParecerCPPlanoAEEUseCase, CadastrarParecerCPPlanoAEEUseCase>();
            services.TryAddScoped<ICadastrarParecerPAAIPlanoAEEUseCase, CadastrarParecerPAAIPlanoAEEUseCase>();
            services.TryAddScoped<IAtribuirResponsavelPlanoAEEUseCase, AtribuirResponsavelPlanoAEEUseCase>();

            services.TryAddScoped<IObterInformacoesDeFrequenciaAlunosPorBimestreUseCase, ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase>();
            services.TryAddScoped<IObterAlunosAtivosPorUeENomeUseCase, ObterAlunosAtivosPorUeENomeUseCase>();

            services.TryAddScoped<ICriarPlanoAEEObservacaoUseCase, CriarPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IAlterarPlanoAEEObservacaoUseCase, AlterarPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IExcluirPlanoAEEObservacaoUseCase, ExcluirPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IDevolverPlanoAEEUseCase, DevolverPlanoAEEUseCase>();
            services.TryAddScoped<IAtribuirResponsavelGeralDoPlanoUseCase, AtribuirResponsavelGeralDoPlanoUseCase>();
            services.TryAddScoped<IExcluirPlanoAEEUseCase, ExcluirPlanoAEEUseCase>();
            services.TryAddScoped<IRemoverResponsavelPlanoAEEUseCase, RemoverResponsavelPlanoAEEUseCase>();
            services.TryAddScoped<IRelatorioPlanosAEEUseCase, RelatorioPlanosAeeUseCase>();

            //Notificacoes EncaminhamentoAEE
            services.TryAddScoped<IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase, ObterInformacoesDeFrequenciaAlunoPorSemestreUseCase>();

            services.TryAddScoped<IRelatorioDevolutivasUseCase, RelatorioDevolutivasUseCase>();
            services.TryAddScoped<IObterBimestreAtualPorTurmaIdUseCase, ObterBimestreAtualPorTurmaIdUseCase>();
            services.TryAddScoped<IObterPeriodoLetivoTurmaUseCase, ObterPeriodoLetivoTurmaUseCase>();

            services.TryAddScoped<IObterEstudanteFotoUseCase, ObterEstudanteFotoUseCase>();
            services.TryAddScoped<ISalvarFotoEstudanteUseCase, SalvarFotoEstudanteUseCase>();
            services.TryAddScoped<IExcluirEstudanteFotoUseCase, ExcluirEstudanteFotoUseCase>();
            services.TryAddScoped<IObterEstudanteTurmasProgramaUseCase, ObterEstudanteTurmasProgramaUseCase>();

            services.TryAddScoped<IObterEncaminhamentoAEESituacoesUseCase, ObterEncaminhamentoAEESituacoesUseCase>();
            services.TryAddScoped<IObterPlanoAEESituacoesUseCase, ObterPlanoAEESituacoesUseCase>();
            services.TryAddScoped<IObterEncaminhamentosAEEDeferidosUseCase, ObterEncaminhamentosAEEDeferidosUseCase>();
            services.TryAddScoped<IObterPlanosAEEVigentesUseCase, ObterPlanosAEEVigentesUseCase>();
            services.TryAddScoped<IObterPlanosAEEAcessibilidadesUseCase, ObterPlanosAEEAcessibilidadesUseCase>();
            services.TryAddScoped<IObterAlunosMatriculadosSRMPAEEUseCase, ObterAlunosMatriculadosSRMPAEEUseCase>();
            services.TryAddScoped<IObterPlanoAEEObservacaoUseCase, ObterPlanoAEEObservacaoUseCase>();

            services.TryAddScoped<IObterDashboardItineranciaVisitasPAAIsUseCase, ObterDashboardItineranciaVisitasPAAIsUseCase>();
            services.TryAddScoped<IObterDashboardItineranciaObjetivosUseCase, ObterDashboardItineranciaObjetivosUseCase>();
            services.TryAddScoped<IObterEventosItinerânciaPorTipoCalendarioUseCase, ObterEventosItinerânciaPorTipoCalendarioUseCase>();
            services.TryAddScoped<IObterDadosDashboardAusenciasComJustificativaUseCase, ObterDadosDashboardAusenciasComJustificativaUseCase>();
            services.TryAddScoped<IObterModalidadesAnoUseCase, ObterModalidadesAnoUseCase>();
            services.TryAddScoped<IObterQuantidadeCriancaUseCase, ObterQuantidadeCriancaUseCase>();

            services.TryAddScoped<IObterDataConsolidacaoFrequenciaUseCase, ObterDataConsolidacaoFrequenciaUseCase>();
            services.TryAddScoped<IObterModalidadesPorUeUseCase, ObterModalidadesPorUeUseCase>();
            services.TryAddScoped<IRelatorioAcompanhamentoAprendizagemUseCase, RelatorioAcompanhamentoAprendizagemUseCase>();
            services.TryAddScoped<IRelatorioRaaEscolaAquiUseCase, RelatorioRaaEscolaAquiUseCase>();
            services.TryAddScoped<IRelatorioRegistroIndividualUseCase, RelatorioRegistroIndividualUseCase>();
            services.TryAddScoped<IObterTurmasFechamentoAcompanhamentoUseCase, ObterTurmasFechamentoAcompanhamentoUseCase>();

            services.TryAddScoped<IObterDashboardFrequenciaPorAnoUseCase, ObterDashboardFrequenciaPorAnoUseCase>();
            services.TryAddScoped<IObterDadosDashboardFrequenciaPorDreUseCase, ObterDadosDashboardFrequenciaPorDreUseCase>();
            services.TryAddScoped<IObterDashboardFrequenciaAusenciasPorMotivoUseCase, ObterDashboardFrequenciaAusenciasPorMotivoUseCase>();

            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase>();

            services.TryAddScoped<IObterDashboardInformacoesEscolaresPorMatriculaUseCase, ObterDashboardInformacoesEscolaresPorMatriculaUseCase>();
            services.TryAddScoped<IObterDashboardInformacoesEscolaresPorTurmaUseCase, ObterDashboardInformacoesEscolaresPorTurmaUseCase>();
            services.TryAddScoped<IObterDataConsolidacaoInformacoesEscolaresUseCase, ObterDataConsolidacaoInformacoesEscolaresUseCase>();
            services.TryAddScoped<IObterModalidadeAnoItineranciaProgramaUseCase, ObterModalidadeAnoItineranciaProgramaUseCase>();

            services.TryAddScoped<IObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase, ObterComponentesFechamentoConsolidadoPorTurmaBimestreUseCase>();
            services.TryAddScoped<IObterPendenciasParaFechamentoConsolidadoUseCase, ObterPendenciasParaFechamentoConsolidadoUseCase>();
            services.TryAddScoped<IObterDetalhamentoPendenciaFechamentoConsolidadoUseCase, ObterDetalhamentoPendenciaFechamentoConsolidadoUseCase>();
            services.TryAddScoped<IObterDetalhamentoPendenciaAulaUseCase, ObterDetalhamentoPendenciaAulaUseCase>();
            services.TryAddScoped<IObterAulasEventosProfessorCalendarioPorMesDiaUseCase, ObterAulasEventosProfessorCalendarioPorMesDiaUseCase>();
            services.TryAddScoped<IObterAulasEventosProfessorCalendarioPorMesUseCase, ObterAulasEventosProfessorCalendarioPorMesUseCase>();

            services.TryAddScoped<IObterFrequenciasPreDefinidasUseCase, ObterFrequenciasPreDefinidasUseCase>();
            services.TryAddScoped<IObterTiposFrequenciasUseCase, ObterTiposFrequenciasUseCase>();
            services.TryAddScoped<IInserirFrequenciaUseCase, InserirFrequenciaUseCase>();
            services.TryAddScoped<IObterFrequenciaPorAulaUseCase, ObterFrequenciaPorAulaUseCase>();

            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosCronUseCase, ConciliacaoFrequenciaTurmasAlunosCronUseCase>();

            services.TryAddScoped<IObterNotasParaAvaliacoesUseCase, ObterNotasParaAvaliacoesUseCase>();
            services.TryAddScoped<IObterNotasParaAvaliacoesListaoUseCase, ObterNotasParaAvaliacoesListaoUseCase>();
            services.TryAddScoped<IObterPeriodosParaConsultaNotasUseCase, ObterPeriodosParaConsultaNotasUseCase>();

            // Consolidação de Registros Pedagógicos
            services.TryAddScoped<IRelatorioAcompanhamentoRegistrosPedagogicosUseCase, RelatorioAcompanhamentoRegistrosPedagogicosUseCase>();

            //Acompanhamento de Frequencia
            services.TryAddScoped<IRelatorioAcompanhamentoDeFrequênciaUseCase, RelatorioAcompanhamentoDeFrequênciaUseCase>();

            // Dashboard Registro Individual
            services.TryAddScoped<IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase, ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterDadosDashboardRegistrosIndividuaisUseCase, ObterDadosDashboardRegistrosIndividuaisUseCase>();
            services.TryAddScoped<IObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase, ObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase>();
            services.TryAddScoped<IObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase, ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase>();
            services.TryAddScoped<IObterParametroDiasSemRegistroIndividualUseCase, ObterParametroDiasSemRegistroIndividualUseCase>();
            services.TryAddScoped<IObterTotalRIsPorDreUseCase, ObterTotalRIsPorDreUseCase>();

            services.TryAddScoped<IObterNotasPorBimestresUeAlunoTurmaUseCase, ObterNotasPorBimestresUeAlunoTurmaUseCase>();
            services.TryAddScoped<IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase, ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase>();
            services.TryAddScoped<IObterTurmaModalidadesPorCodigosUseCase, ObterTurmaModalidadesPorCodigosUseCase>();

            services.TryAddScoped<IObterBimestrePorModalidadeUseCase, ObterBimestrePorModalidadeUseCase>();
            services.TryAddScoped<IObterSituacoesFechamentoUseCase, ObterSituacoesFechamentoUseCase>();
            services.TryAddScoped<IObterSituacoesConselhoClasseUseCase, ObterSituacoesConselhoClasseUseCase>();

            services.TryAddScoped<IRelatorioAcompanhamentoFechamentoUseCase, RelatorioAcompanhamentoFechamentoUseCase>();

            // Sincronização de Acompanhamento de Aprendizagem
            services.TryAddScoped<IListarAtribuicaoEsporadicaUseCase, ListarAtribuicaoEsporadicaUseCase>();
            services.TryAddScoped<IObterPeriodoAtribuicaoPorUeUseCase, ObterPeriodoAtribuicaoPorUeUseCase>();
            services.TryAddScoped<IExcluirAtribuicaoEsporadicaUseCase, ExcluirAtribuicaoEsporadicaUseCase>();

            services.TryAddScoped<IObterAnosLetivosAtribuicaoCJUseCase, ObterAnosLetivosAtribuicaoCJUseCase>();
            services.TryAddScoped<IObterProfessoresTitularesECjsUseCase, ObterProfessoresTitularesECjsUseCase>();
            services.TryAddScoped<ISalvarAtribuicaoCJUseCase, SalvarAtribuicaoCJUseCase>();
            services.TryAddScoped<IListarAtribuicoesCJPorFiltroUseCase, ListarAtribuicoesCJPorFiltroUseCase>();
            services.TryAddScoped<IObterFiltroSemanaUseCase, ObterFiltroSemanaUseCase>();

            // Dashboard Frequencia Aluno
            services.TryAddScoped<IObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase, ObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase, ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterFrequenciasPorPeriodoUseCase, ObterFrequenciasPorPeriodoUseCase>();
            services.TryAddScoped<IObterFaltasNaoCompensadaUseCase, ObterFaltasNaoCompensadaUseCase>();
            services.TryAddScoped<IObterDadosDashboardTotalAtividadesCompensacaoUseCase, ObterDadosDashboardTotalAtividadesCompensacaoUseCase>();
            services.TryAddScoped<IObterDadosDashboardTotalAusenciasCompensadasUseCase, ObterDadosDashboardTotalAusenciasCompensadasUseCase>();

            //  Dashboard Compensação ausência
            services.TryAddScoped<IObterDadosDashboardTotalAusenciasCompensadasUseCase, ObterDadosDashboardTotalAusenciasCompensadasUseCase>();
            
            // Consolidação Dashboard
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase>();

            // Rotas Agendamento Sync
            services.TryAddScoped<IObterTurmaModalidadesPorCodigosUseCase, ObterTurmaModalidadesPorCodigosUseCase>();

            services.TryAddScoped<IObterBimestrePorModalidadeUseCase, ObterBimestrePorModalidadeUseCase>();
            services.TryAddScoped<IObterSituacoesFechamentoUseCase, ObterSituacoesFechamentoUseCase>();
            services.TryAddScoped<IObterSituacoesConselhoClasseUseCase, ObterSituacoesConselhoClasseUseCase>();

            services.TryAddScoped<IRelatorioAcompanhamentoFechamentoUseCase, RelatorioAcompanhamentoFechamentoUseCase>();

            // Tipo Escola
            services.TryAddScoped<IObterTipoEscolaPorDreEUeUseCase, ObterTipoEscolaPorDreEUeUseCase>();
            services.TryAddScoped<IObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase, ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase>();

            //Dashbord Fechamento
            services.TryAddScoped<IObterFechamentoSituacaoUseCase, ObterFechamentoSituacaoUseCase>();
            services.TryAddScoped<IObterFechamentoPendenciasUseCase, ObterFechamentoPendenciasUseCase>();
            services.TryAddScoped<IObterFechamentoSituacaoPorEstudanteUseCase, ObterFechamentoSituacaoPorEstudanteUseCase>();
            services.TryAddScoped<IObterFechamentoConselhoClasseSituacaoUseCase, ObterFechamentoConselhoClasseSituacaoUseCase>();
            services.TryAddScoped<IObterPendenciaParecerConclusivoUseCases, ObterPendenciaParecerConclusivoUseCases>();
            services.TryAddScoped<IObterNotasFinaisUseCases, ObterNotasFinaisUseCases>();

            // Dias Letivos
            services.TryAddScoped<IObterDiasLetivosPorUeETurnoUseCase, ObterDiasLetivosPorUeETurnoUseCase>();
            services.TryAddScoped<IRelatorioAtaBimestralUseCase, RelatorioAtaBimestralUseCase>();

            //Componentes curriculares integração
            services.TryAddScoped<IObterComponenteCurricularLancaNotaUseCase, ObterComponenteCurricularLancaNotaUseCase>();

            //Período escolar integração
            services.TryAddScoped<IObterPeriodoEscolarAtualPorTurmaUseCase, ObterPeriodoEscolarAtualPorTurmaUseCase>();

            services.TryAddScoped<IPendenciasGeraisUseCase, PendenciasGeraisUseCase>();

            //Remoção da Atribuição de Pendencia Usuário
            services.TryAddScoped<IObterDatasDiarioBordoPorPeriodoUseCase, ObterDatasDiarioBordoPorPeriodoUseCase>();
            services.TryAddScoped<IInserirAlterarDiarioBordoUseCase, InserirAlterarDiarioBordoUseCase>();

            services.TryAddScoped<IListarFechamentoTurmaBimestreUseCase, ListarFechamentoTurmaBimestreUseCase>();

            // NAAPA - Frequência turma evasão
            services.TryAddScoped<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase, ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();
            services.TryAddScoped<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase, ObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();
            services.TryAddScoped<IObterEncaminhamentoNAAPAUseCase, ObterEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IRegistrarEncaminhamentoNAAPAUseCase, RegistrarEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IEncerrarEncaminhamentoNAAPAUseCase, EncerrarEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IReabrirEncaminhamentoNAAPAUseCase, ReabrirEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase, ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();
            services.TryAddScoped<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase, ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            services.TryAddScoped<IVerificarExistenciaRelatorioPorCodigoUseCase, VerificarExistenciaRelatorioPorCodigoUseCase>();
            services.TryAddScoped<IObterPAAIPorDreUseCase, ObterPAAIPorDreUseCase>();
            services.TryAddScoped<IObterResponsaveisPorDreUseCase, ObterResponsaveisPorDreUseCase>();
            services.TryAddScoped<IAtribuirUeResponsavelUseCase, AtribuirUeResponsavelUseCase>();
            services.TryAddScoped<IObterListaTipoReponsavelUseCase, ObterListaTipoReponsavelUseCase>();
            services.TryAddScoped<IListarAtribuicoesResponsaveisPorFiltroUseCase, ListarAtribuicoesResponsaveisPorFiltroUseCase>();
            services.TryAddScoped<IObterObservacoesDeEncaminhamentoNAAPAUseCase, ObterObservacoesDeEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<ISalvarObservacoesDeEncaminhamentoNAAPAUseCase, SalvarObservacoesDeEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExcluirObservacoesDeEncaminhamentoNAAPAUseCase, ExcluirObservacoesDeEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase, ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterQuantidadeEncaminhamentoPorSituacaoUseCase, ObterQuantidadeEncaminhamentoPorSituacaoUseCase>();

            // Encaminhamento NAAPA
            services.TryAddScoped<IObterSecoesEncaminhamentosSecaoNAAPAUseCase, ObterSecoesEncaminhamentosSecaoNAAPAUseCase>();
            services.TryAddScoped<IObterQuestionarioEncaminhamentoNAAPAUseCase, ObterQuestionarioEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase, ObterQuestoesRelatorioDinamicoEncaminhamentoNaapaPorModalidadesUseCase>();
            services.TryAddScoped<IObterPrioridadeEncaminhamentoNAAPAUseCase, ObterPrioridadeEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterInformacoesAlunoPorCodigoUseCase, ObterInformacoesAlunoPorCodigoUseCase>();
            services.TryAddScoped<IObterEncaminhamentoNAAPAPorIdUseCase, ObterEncaminhamentoNAAPAPorIdUseCase>();
            services.TryAddScoped<IExcluirArquivoNAAPAUseCase, ExcluirArquivoNAAPAUseCase>();
            services.TryAddScoped<IExcluirEncaminhamentoNAAPAUseCase, ExcluirEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase, ObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterQuestionarioItinerarioEncaminhamentoNAAPAUseCase, ObterQuestionarioItinerarioEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase, ExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IRegistrarEncaminhamentoItinerarioNAAPAUseCase, RegistrarEncaminhamentoItinerarioNAAPAUseCase>();
            services.TryAddScoped<IObterSituacaoEncaminhamentoNAAPAUseCase, ObterSituacaoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExcluirArquivoItineranciaNAAPAUseCase, ExcluirArquivoItineranciaNAAPAUseCase>();

            services.TryAddScoped<IObterOpcoesRespostaFluxoAlertaEncaminhamentosNAAPAUseCase, ObterOpcoesRespostaFluxoAlertaEncaminhamentosNAAPAUseCase>();
            services.TryAddScoped<IObterOpcoesRespostaPortaEntradaEncaminhamentosNAAPAUseCase, ObterOpcoesRespostaPortaEntradaEncaminhamentosNAAPAUseCase>();
            services.TryAddScoped<IRelatorioEncaminhamentoNAAPAUseCase, RelatorioEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase, ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase>();
            services.TryAddScoped<IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase, ObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase>();
            services.TryAddScoped<IExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase, ExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase>();
            services.TryAddScoped<IObterRegistrosDeAcaoParaNAAPAUseCase, ObterRegistrosDeAcaoParaNAAPAUseCase>();
            services.TryAddScoped<IObterTiposDeImprimirAnexosNAAPAUseCase, ObterTiposDeImprimirAnexosNAAPAUseCase>();
            services.TryAddScoped<IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase, ObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase>();

            //Relatório Dinâmico NAAPA
            services.TryAddScoped<IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase, RelatorioDinamicoObterEncaminhamentoNAAPAUseCase>();

            // Relatório PAP
            services.TryAddScoped<IObterPeriodosPAPUseCase, ObterPeriodosPAPUseCase>();
            services.TryAddScoped<IObterSecoesPAPUseCase, ObterSecoesPAPUseCase>();
            services.TryAddScoped<IObterQuestionarioPAPUseCase, ObterQuestionarioPAPUseCase>();
            services.TryAddScoped<IObterAlunosPorPeriodoPAPUseCase, ObterAlunosPorPeriodoPAPUseCase>();
            services.TryAddScoped<ISalvarRelatorioPAPUseCase, SalvarRelatorioPAPUseCase>();
            services.TryAddScoped<IExcluirArquivoPAPUseCase, ExcluirArquivoPAPUseCase>();
            services.TryAddScoped<IObterRelatorioPAPConselhoClasseUseCase, ObterRelatorioPAPConselhoClasseUseCase>();
            services.TryAddScoped<ICopiarRelatorioPAPUseCase, CopiarRelatorioPAPUseCase>();
            services.TryAddScoped<IObterTurmasPapPorAnoLetivoUseCase, ObterTurmasPapPorAnoLetivoUseCase>();

            // Historico Escolar Observação
            services.TryAddScoped<IObterHistoricoEscolarObservacaoUseCase, ObterHistoricoEscolarObservacaoUseCase>();

            services.TryAddScoped<ISalvarCompensacaoAusenciaUseCase, SalvarCompensacaoAusenciaUseCase>();

            // Notificação
            services.TryAddScoped<IObterNotificacaoPorIdUseCase, ObterNotificacaoPorIdUseCase>();

            // Log.
            services.TryAddScoped<ISalvarLogUseCase, SalvarLogUseCase>();

            //Eventos Escola Aqui/Conselho Classe Recomendação/Dre integração
            services.TryAddScoped<IObterRecomendacoesPorAlunoTurmaUseCase, ObterRecomendacoesPorAlunoTurmaUseCase>();
            services.TryAddScoped<IObterEventosEscolaAquiPorDreUeTurmaMesUseCase, ObterEventosEscolaAquiPorDreUeTurmaMesUseCase>();
            services.TryAddScoped<IObterDresUseCase, ObterDresUseCase>();
            services.TryAddScoped<IObterComunicadosAnoAtualUseCase, ObterComunicadosAnoAtualUseCase>();

            //Informes
            services.TryAddScoped<IObterGruposDeUsuariosUseCase, ObterGruposDeUsuariosUseCase>();
            services.TryAddScoped<ISalvarInformesUseCase, SalvarInformesUseCase>();
            services.TryAddScoped<IExcluirInformesUseCase, ExcluirInformesUseCase>();
            services.TryAddScoped<IObterInformeUseCase, ObterInformeUseCase>();
            services.TryAddScoped<IObterInformesPorFiltroUseCase, ObterInformesPorFiltroUseCase>();
            services.TryAddScoped<IExcluirArquivoInformeUseCase, ExcluirArquivoInformeUseCase>();
            services.TryAddScoped<IDownloadTodosAnexosInformativoUseCase, DownloadTodosAnexosInformativoUseCase>();
            services.TryAddScoped<IDownloadArquivoInformativoUseCase, DownloadArquivoInformativoUseCase>();

            //Consulta Crianças Estudantes Ausentes
            services.TryAddScoped<IObterTurmasAlunosAusentesUseCase, ObterTurmasAlunosAusentesUseCase>();

            services.TryAddScoped<ISalvarCadastroAcessoABAEUseCase, SalvarCadastroAcessoABAEUseCase>();
            services.TryAddScoped<IBuscaCepUseCase, BuscaCepUseCase>();
            services.TryAddScoped<IExcluirCadastroAcessoABAEUseCase, ExcluirCadastroAcessoABAEUseCase>();
            services.TryAddScoped<IObterCadastroAcessoABAEUseCase, ObterCadastroAcessoABAEUseCase>();
            services.TryAddScoped<IObterPaginadoCadastroAcessoABAEUseCase, ObterPaginadoCadastroAcessoABAEUseCase>();

            //Busca Ativa - Registro Ação
            services.TryAddScoped<IObterSecoesRegistroAcaoSecaoUseCase, ObterSecoesRegistroAcaoSecaoUseCase>();
            services.TryAddScoped<IObterQuestionarioRegistroAcaoUseCase, ObterQuestionarioRegistroAcaoUseCase>();
            services.TryAddScoped<IRegistrarRegistroAcaoUseCase, RegistrarRegistroAcaoUseCase>();
            services.TryAddScoped<IExcluirRegistroAcaoUseCase, ExcluirRegistroAcaoUseCase>();
            services.TryAddScoped<IObterRegistrosAcaoCriancaEstudanteAusenteUseCase, ObterRegistrosAcaoCriancaEstudanteAusenteUseCase>();
            services.TryAddScoped<IObterRegistroAcaoPorIdUseCase, ObterRegistroAcaoPorIdUseCase>();
            services.TryAddScoped<IAtualizarDadosResponsaveisUseCase, AtualizarDadosResponsaveisUseCase>();
            services.TryAddScoped<IObterRegistrosAcaoUseCase, ObterRegistrosAcaoUseCase>();
            services.TryAddScoped<IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase, ObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase>();
            services.TryAddScoped<IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase, ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase>();
            services.TryAddScoped<IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase, ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase>();
            services.TryAddScoped<IRelatorioBuscasAtivasUseCase, RelatorioBuscasAtivasUseCase>();
            services.TryAddScoped<IObterFuncionariosABAEUseCase, ObterFuncionariosABAEUseCase>();
            services.TryAddScoped<IObterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase, ObterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase>();

            //Registro Coletivo 
            services.TryAddScoped<IObterTiposDeReuniaoUseCase, ObterTiposDeReuniaoUseCase>();
            services.TryAddScoped<ISalvarRegistroColetivoUseCase, SalvarRegistroColetivoUseCase>();
            services.TryAddScoped<IExcluirRegistroColetivoUseCase, ExcluirRegistroColetivoUseCase>();
            services.TryAddScoped<IObterRegistrosColetivosNAAPAUseCase, ObterRegistrosColetivosNAAPAUseCase>();
            services.TryAddScoped<IObterRegistroColetivoNAAPAPorIdUseCase, ObterRegistroColetivoNAAPAPorIdUseCase>();

            //Turma
            services.TryAddScoped<IObterTurmaSondagemUseCase, ObterTurmaSondagemUseCase>();

            //Mapeamento de estudantes
            services.TryAddScoped<IObterSecoesMapeamentoSecaoUseCase, ObterSecoesMapeamentoSecaoUseCase>();
            services.TryAddScoped<IObterQuestionarioMapeamentoEstudanteUseCase, ObterQuestionarioMapeamentoEstudanteUseCase>();
            services.TryAddScoped<IObterIdentificadorMapeamentoEstudanteUseCase, ObterIdentificadorMapeamentoEstudanteUseCase>();
            services.TryAddScoped<IRegistrarMapeamentoEstudanteUseCase, RegistrarMapeamentoEstudanteUseCase>();
            services.TryAddScoped<IAtualizarMapeamentoDosEstudantesUseCase, AtualizarMapeamentoDosEstudantesUseCase>();
            services.TryAddScoped<IAtualizarMapeamentoDoEstudanteDoBimestreUseCase, AtualizarMapeamentoDoEstudanteDoBimestreUseCase>();
            services.TryAddScoped<IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase, ObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase>();
            services.TryAddScoped<IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase, ObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase>();
            services.TryAddScoped<IRelatorioMapeamentoEstudantesUseCase, RelatorioMapeamentoEstudantesUseCase>();
            services.TryAddScoped<IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase, ConsultasRegistroFrequenciaAgrupamentoGlobalUseCase>();
            services.TryAddScoped<IConsultasRegistroFrequenciaAgrupamentoMensalUseCase, ConsultasRegistroFrequenciaAgrupamentoMensalUseCase>();
            services.TryAddScoped<IConsultasRegistroFrequenciaAgrupamentoRankingUseCase, ConsultasRegistroFrequenciaAgrupamentoRankingUseCase>();
            services.TryAddScoped<IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase, ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase>();
            services.TryAddScoped<IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase, ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase>();

            //ImportacaoLog - Arquivos
            services.TryAddScoped<IImportacaoLogUseCase, ImportacaoLogUseCase>();
            services.TryAddScoped<IImportacaoLogErroUseCase, ImportacaoLogErroUseCase>();
            services.TryAddScoped<IImportacaoArquivoIdebUseCase, ImportacaoArquivoIdebUseCase>();
            services.TryAddScoped<IImportacaoArquivoIdepUseCase, ImportacaoArquivoIdepUseCase>();
            services.TryAddScoped<IImportacaoArquivoFluenciaLeitoraUseCase, ImportacaoArquivoFluenciaLeitoraUseCase>();

            RegistrarCasoDeUsoAEERabbitSgp(services);
            RegistrarCasoDeUsoAulaRabbitSgp(services);
            RegistrarCasoDeUsoFechamentoRabbitSgp(services);
            RegistrarCasoDeUsoFrequenciaRabbitSgp(services);
            RegistrarCasoDeUsoInstitucionalRabbitSgp(services);
            RegistrarCasoDeUsoPendenciasRabbitSgp(services);
            RegistrarCasoDeUsoNAAPARabbitSgp(services);
            RegistrarCasoDeUsoRabbitSgp(services);
            RegistrarCasoDeUsoPainelEducacionalRabbitSgp(services);
        }

        public virtual void RegistrarTelemetria(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurarTelemetria(configuration);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<TelemetriaOptions>>();
            var servicoTelemetria = new ServicoTelemetria(options);
            DapperExtensionMethods.Init(servicoTelemetria);
        }

        public virtual void RegistrarPolicies(IServiceCollection services)
        {
            services.AddPolicies();
        }

        public virtual void RegistrarRabbit(IServiceCollection services, IConfiguration configuration)
        {

            services.ConfigurarRabbit(configuration);
            services.ConfigurarRabbitParaLogs(configuration);
        }
    }
}
