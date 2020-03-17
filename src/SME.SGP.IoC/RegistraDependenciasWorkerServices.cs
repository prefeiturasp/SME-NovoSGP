using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Servicos;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.IoC.Extensions;

namespace SME.SGP.IoC
{
    public static class RegistraDependenciasWorkerServices
    {
        public static void Registrar(IServiceCollection services)
        {
            ResgistraDependenciaHttp(services);
            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
            RegistrarServicos(services);
        }

        private static void RegistrarComandos(IServiceCollection services)
        {
            services.TryAddScopedWorkerService<IComandosPlanoCiclo, ComandosPlanoCiclo>();
            services.TryAddScopedWorkerService<IComandosPlanoAnual, ComandosPlanoAnual>();
            services.TryAddScopedWorkerService<IComandosSupervisor, ComandosSupervisor>();
            services.TryAddScopedWorkerService<IComandosNotificacao, ComandosNotificacao>();
            services.TryAddScopedWorkerService<IComandosWorkflowAprovacao, ComandosWorkflowAprovacao>();
            services.TryAddScopedWorkerService<IComandosUsuario, ComandosUsuario>();
            services.TryAddScopedWorkerService<IComandosTipoCalendario, ComandosTipoCalendario>();
            services.TryAddScopedWorkerService<IComandosFeriadoCalendario, ComandosFeriadoCalendario>();
            services.TryAddScopedWorkerService<IComandosPeriodoEscolar, ComandosPeriodoEscolar>();
            services.TryAddScopedWorkerService<IComandosEventoTipo, ComandosEventoTipo>();
            services.TryAddScopedWorkerService<IComandosEvento, ComandosEvento>();
            services.TryAddScopedWorkerService<IComandosDiasLetivos, ComandosDiasLetivos>();
            services.TryAddScopedWorkerService<IComandosAula, ComandosAula>();
            services.TryAddScopedWorkerService<IComandosGrade, ComandosGrade>();
            services.TryAddScopedWorkerService<IComandoFrequencia, ComandoFrequencia>();
            services.TryAddScopedWorkerService<IComandosAtribuicaoEsporadica, ComandosAtribuicaoEsporadica>();
            services.TryAddScopedWorkerService<IComandosAtividadeAvaliativa, ComandosAtividadeAvaliativa>();
            services.TryAddScopedWorkerService<IComandosTipoAvaliacao, ComandosTipoAavaliacao>();
            services.TryAddScopedWorkerService<IComandosPlanoAula, ComandosPlanoAula>();
            services.TryAddScopedWorkerService<IComandosAtribuicaoCJ, ComandosAtribuicaoCJ>();
            services.TryAddScopedWorkerService<IComandosEventoMatricula, ComandosEventoMatricula>();
            services.TryAddScopedWorkerService<IComandosNotasConceitos, ComandosNotasConceitos>();
            services.TryAddScopedWorkerService<IComandosAulaPrevista, ComandosAulaPrevista>();
            services.TryAddScopedWorkerService<IComandosRegistroPoa, ComandosRegistroPoa>();
            services.TryAddScopedWorkerService<IComandosFechamentoReabertura, ComandosFechamentoReabertura>();
            services.TryAddScopedWorkerService<IComandosCompensacaoAusencia, ComandosCompensacaoAusencia>();
            services.TryAddScopedWorkerService<IComandosCompensacaoAusenciaAluno, ComandosCompensacaoAusenciaAluno>();
            services.TryAddScopedWorkerService<IComandosCompensacaoAusenciaDisciplinaRegencia, ComandosCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScopedWorkerService<IComandosProcessoExecutando, ComandosProcessoExecutando>();
            services.TryAddScopedWorkerService<IComandosFechamento, ComandosFechamento>();
            services.TryAddScopedWorkerService<IComandosFechamentoFinal, ComandosFechamentoFinal>();
            services.TryAddScopedWorkerService<IComandosFechamentoTurmaDisciplina, ComandosFechamentoTurmaDisciplina>();
            services.TryAddScopedWorkerService<IComandosNotaConceitoBimestre, ComandosNotaConceitoBimestre>();
            services.TryAddScopedWorkerService<IComandosNotificacaoAula, ComandosNotificacaoAula>();
        }

        private static void RegistrarConsultas(IServiceCollection services)
        {
            services.TryAddScopedWorkerService<IConsultasPlanoCiclo, ConsultasPlanoCiclo>();
            services.TryAddScopedWorkerService<IConsultasMatrizSaber, ConsultasMatrizSaber>();
            services.TryAddScopedWorkerService<IConsultasObjetivoDesenvolvimento, ConsultasObjetivoDesenvolvimento>();
            services.TryAddScopedWorkerService<IConsultasCiclo, ConsultasCiclo>();
            services.TryAddScopedWorkerService<IConsultasObjetivoAprendizagem, ConsultasObjetivoAprendizagem>();
            services.TryAddScopedWorkerService<IConsultasPlanoAnual, ConsultasPlanoAnual>();
            services.TryAddScopedWorkerService<IConsultasDisciplina, ConsultasDisciplina>();
            services.TryAddScopedWorkerService<IConsultasProfessor, ConsultasProfessor>();
            services.TryAddScopedWorkerService<IConsultasSupervisor, ConsultasSupervisor>();
            services.TryAddScopedWorkerService<IConsultaDres, ConsultaDres>();
            services.TryAddScopedWorkerService<IConsultasNotificacao, ConsultasNotificacao>();
            services.TryAddScopedWorkerService<IConsultasWorkflowAprovacao, ConsultasWorkflowAprovacao>();
            services.TryAddScopedWorkerService<IConsultasUnidadesEscolares, ConsultasUnidadesEscolares>();
            services.TryAddScopedWorkerService<IConsultasTipoCalendario, ConsultasTipoCalendario>();
            services.TryAddScopedWorkerService<IConsultasFeriadoCalendario, ConsultasFeriadoCalendario>();
            services.TryAddScopedWorkerService<IConsultasPeriodoEscolar, ConsultasPeriodoEscolar>();
            services.TryAddScopedWorkerService<IConsultasUsuario, ConsultasUsuario>();
            services.TryAddScopedWorkerService<IConsultasAbrangencia, ConsultasAbrangencia>();
            services.TryAddScopedWorkerService<IConsultasEventoTipo, ConsultasEventoTipo>();
            services.TryAddScopedWorkerService<IConsultasEvento, ConsultasEvento>();
            services.TryAddScopedWorkerService<IConsultasAula, ConsultasAula>();
            services.TryAddScopedWorkerService<IConsultasGrade, ConsultasGrade>();
            services.TryAddScopedWorkerService<IConsultasFrequencia, ConsultasFrequencia>();
            services.TryAddScopedWorkerService<IConsultasAtribuicaoEsporadica, ConsultasAtribuicaoEsporadica>();
            services.TryAddScopedWorkerService<IConsultasEventoMatricula, ConsultasEventoMatricula>();
            services.TryAddScopedWorkerService<IConsultasRegistroPoa, ConsultasRegistroPoa>();
            services.TryAddScopedWorkerService<IConsultasPlanoAula, ConsultasPlanoAula>();
            services.TryAddScopedWorkerService<IConsultasObjetivoAprendizagemAula, ConsultasObjetivoAprendizagemAula>();
            services.TryAddScopedWorkerService<IConsultasCompensacaoAusencia, ConsultasCompensacaoAusencia>();
            services.TryAddScopedWorkerService<IConsultasCompensacaoAusenciaAluno, ConsultasCompensacaoAusenciaAluno>();
            services.TryAddScopedWorkerService<IConsultasCompensacaoAusenciaDisciplinaRegencia, ConsultasCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScopedWorkerService<IConsultasAulaPrevista, ConsultasAulaPrevista>();
            services.TryAddScopedWorkerService<IConsultasNotasConceitos, ConsultasNotasConceitos>();
            services.TryAddScopedWorkerService<IConsultasAtribuicoes, ConsultasAtribuicoes>();
            services.TryAddScopedWorkerService<IConsultaAtividadeAvaliativa, ConsultaAtividadeAvaliativa>();
            services.TryAddScopedWorkerService<IConsultaTipoAvaliacao, ConsultaTipoAvaliacao>();
            services.TryAddScopedWorkerService<IConsultasAtribuicaoCJ, ConsultasAtribuicaoCJ>();
            services.TryAddScopedWorkerService<IConsultasUe, ConsultasUe>();
            services.TryAddScopedWorkerService<IConsultasEventosAulasCalendario, ConsultasEventosAulasCalendario>();
            services.TryAddScopedWorkerService<IConsultasProcessoExecutando, ConsultasProcessoExecutando>();
            services.TryAddScopedWorkerService<IConsultasFechamento, ConsultasFechamento>();
            services.TryAddScopedWorkerService<IConsultasFechamentoTurmaDisciplina, ConsultasFechamentoTurmaDisciplina>();
            services.TryAddScopedWorkerService<IConsultasNotaConceitoBimestre, ConsultasNotaConceitoBimestre>();
            services.TryAddScopedWorkerService<IConsultasFechamentoReabertura, ConsultasFechamentoReabertura>();
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScopedWorkerService<IContextoAplicacao, WorkerContext>();
            services.TryAddScopedWorkerService<ISgpContext, SgpContext>();
            services.TryAddScopedWorkerService<IUnitOfWork, UnitOfWork>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScopedWorkerService<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.TryAddScopedWorkerService<IRepositorioAtividadeAvaliativa, RepositorioAtividadeAvaliativa>();
            services.TryAddScopedWorkerService<IRepositorioAtividadeAvaliativaDisciplina, RepositorioAtividadeAvaliativaDisciplina>();
            services.TryAddScopedWorkerService<IRepositorioAtividadeAvaliativaRegencia, RepositorioAtividadeAvaliativaRegencia>();
            services.TryAddScopedWorkerService<IRepositorioAtribuicaoCJ, RepositorioAtribuicaoCJ>();
            services.TryAddScopedWorkerService<IRepositorioAtribuicaoEsporadica, RepositorioAtribuicaoEsporadica>();
            services.TryAddScopedWorkerService<IRepositorioAula, RepositorioAula>();
            services.TryAddScopedWorkerService<IRepositorioAulaPrevista, RepositorioAulaPrevista>();
            services.TryAddScopedWorkerService<IRepositorioAulaPrevistaBimestre, RepositorioAulaPrevistaBimestre>();
            services.TryAddScopedWorkerService<IRepositorioCache, RepositorioCache>();
            services.TryAddScopedWorkerService<IRepositorioCiclo, RepositorioCiclo>();
            services.TryAddScopedWorkerService<IRepositorioComponenteCurricular, RepositorioComponenteCurricular>();
            services.TryAddScopedWorkerService<IRepositorioConceito, RepositorioConceito>();
            services.TryAddScopedWorkerService<IRepositorioConfiguracaoEmail, RepositorioConfiguracaoEmail>();
            services.TryAddScopedWorkerService<IRepositorioDre, RepositorioDre>();
            services.TryAddScopedWorkerService<IRepositorioEvento, RepositorioEvento>();
            services.TryAddScopedWorkerService<IRepositorioEventoMatricula, RepositorioEventoMatricula>();
            services.TryAddScopedWorkerService<IRepositorioEventoTipo, RepositorioEventoTipo>();
            services.TryAddScopedWorkerService<IRepositorioFeriadoCalendario, RepositorioFeriadoCalendario>();
            services.TryAddScopedWorkerService<IRepositorioFrequencia, RepositorioFrequencia>();
            services.TryAddScopedWorkerService<IRepositorioFrequenciaAlunoDisciplinaPeriodo, RepositorioFrequenciaAlunoDisciplinaPeriodo>();
            services.TryAddScopedWorkerService<IRepositorioGrade, RepositorioGrade>();
            services.TryAddScopedWorkerService<IRepositorioGradeDisciplina, RepositorioGradeDisciplina>();
            services.TryAddScopedWorkerService<IRepositorioGradeFiltro, RepositorioGradeFiltro>();
            services.TryAddScopedWorkerService<IRepositorioMatrizSaber, RepositorioMatrizSaber>();
            services.TryAddScopedWorkerService<IRepositorioMatrizSaberPlano, RepositorioMatrizSaberPlano>();
            services.TryAddScopedWorkerService<IRepositorioNotaParametro, RepositorioNotaParametro>();
            services.TryAddScopedWorkerService<IRepositorioNotaTipoValor, RepositorioNotaTipoValor>();
            services.TryAddScopedWorkerService<IRepositorioNotasConceitos, RepositorioNotasConceitos>();
            services.TryAddScopedWorkerService<IRepositorioNotificacao, RepositorioNotificacao>();
            services.TryAddScopedWorkerService<IRepositorioNotificacaoAulaPrevista, RepositorioNotificacaoAulaPrevista>();
            services.TryAddScopedWorkerService<IRepositorioNotificacaoFrequencia, RepositorioNotificacaoFrequencia>();
            services.TryAddScopedWorkerService<IRepositorioObjetivoAprendizagemAula, RepositorioObjetivoAprendizagemAula>();
            services.TryAddScopedWorkerService<IRepositorioObjetivoAprendizagemPlano, RepositorioObjetivoAprendizagemPlano>();
            services.TryAddScopedWorkerService<IRepositorioObjetivoDesenvolvimento, RepositorioObjetivoDesenvolvimento>();
            services.TryAddScopedWorkerService<IRepositorioObjetivoDesenvolvimentoPlano, RepositorioObjetivoDesenvolvimentoPlano>();
            services.TryAddScopedWorkerService<IRepositorioParametrosSistema, RepositorioParametrosSistema>();
            services.TryAddScopedWorkerService<IRepositorioPeriodoEscolar, RepositorioPeriodoEscolar>();
            services.TryAddScopedWorkerService<IRepositorioPlanoAnual, RepositorioPlanoAnual>();
            services.TryAddScopedWorkerService<IRepositorioPlanoAula, RepositorioPlanoAula>();
            services.TryAddScopedWorkerService<IRepositorioPlanoCiclo, RepositorioPlanoCiclo>();
            services.TryAddScopedWorkerService<IRepositorioPrioridadePerfil, RepositorioPrioridadePerfil>();
            services.TryAddScopedWorkerService<IRepositorioRegistroAusenciaAluno, RepositorioRegistroAusenciaAluno>();
            services.TryAddScopedWorkerService<IRepositorioRegistroPoa, RepositorioRegistroPoa>();
            services.TryAddScopedWorkerService<IRepositorioCompensacaoAusencia, RepositorioCompensacaoAusencia>();
            services.TryAddScopedWorkerService<IRepositorioCompensacaoAusenciaAluno, RepositorioCompensacaoAusenciaAluno>();
            services.TryAddScopedWorkerService<IRepositorioCompensacaoAusenciaDisciplinaRegencia, RepositorioCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScopedWorkerService<IRepositorioSupervisorEscolaDre, RepositorioSupervisorEscolaDre>();
            services.TryAddScopedWorkerService<IRepositorioTipoAvaliacao, RepositorioTipoAvaliacao>();
            services.TryAddScopedWorkerService<IRepositorioTipoCalendario, RepositorioTipoCalendario>();
            services.TryAddScopedWorkerService<IRepositorioTurma, RepositorioTurma>();
            services.TryAddScopedWorkerService<IRepositorioUe, RepositorioUe>();
            services.TryAddScopedWorkerService<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddScopedWorkerService<IRepositorioWorkflowAprovacao, RepositorioWorkflowAprovacao>();
            services.TryAddScopedWorkerService<IRepositorioWorkflowAprovacaoNivel, RepositorioWorkflowAprovacaoNivel>();
            services.TryAddScopedWorkerService<IRepositorioWorkflowAprovacaoNivelNotificacao, RepositorioWorkflowAprovaNivelNotificacao>();
            services.TryAddScopedWorkerService<IRepositorioWorkflowAprovacaoNivelUsuario, RepositorioWorkflowAprovacaoNivelUsuario>();
            services.TryAddScopedWorkerService<IRepositorioProcessoExecutando, RepositorioProcessoExecutando>();
            services.TryAddScopedWorkerService<IRepositorioPeriodoFechamento, RepositorioPeriodoFechamento>();
            services.TryAddScopedWorkerService<IRepositorioNotificacaoCompensacaoAusencia, RepositorioNotificacaoCompensacaoAusencia>();
            services.TryAddScopedWorkerService<IRepositorioEventoFechamento, RepositorioEventoFechamento>();
            services.TryAddScopedWorkerService<IRepositorioFechamentoTurmaDisciplina, RepositorioFechamentoTurmaDisciplina>();
            services.TryAddScopedWorkerService<IRepositorioNotaConceitoBimestre, RepositorioNotaConceitoBimestre>();
            services.TryAddScopedWorkerService<IRepositorioFechamentoReabertura, RepositorioFechamentoReabertura>();
            services.TryAddScopedWorkerService<IRepositorioNotificacaoAula, RepositorioNotificacaoAula>();
            services.TryAddScopedWorkerService<IRepositorioFechamentoFinal, RepositorioFechamentoFinal>();
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScopedWorkerService<IServicoWorkflowAprovacao, ServicoWorkflowAprovacao>();
            services.TryAddScopedWorkerService<IServicoNotificacao, ServicoNotificacao>();
            services.TryAddScopedWorkerService<IServicoUsuario, ServicoUsuario>();
            services.TryAddScopedWorkerService<IServicoAutenticacao, ServicoAutenticacao>();
            services.TryAddScopedWorkerService<IServicoPerfil, ServicoPerfil>();
            services.TryAddScopedWorkerService<IServicoEmail, ServicoEmail>();
            services.TryAddScopedWorkerService<IServicoTokenJwt, ServicoTokenJwt>();
            services.TryAddScopedWorkerService<IServicoMenu, ServicoMenu>();
            services.TryAddScopedWorkerService<IServicoPeriodoEscolar, ServicoPeriodoEscolar>();
            services.TryAddScopedWorkerService<IServicoFeriadoCalendario, ServicoFeriadoCalendario>();
            services.TryAddScopedWorkerService<IServicoAbrangencia, ServicoAbrangencia>();
            services.TryAddScopedWorkerService<IServicoEvento, ServicoEvento>();
            services.TryAddScopedWorkerService<IServicoDiaLetivo, ServicoDiaLetivo>();
            services.TryAddScopedWorkerService<IServicoLog, ServicoLog>();
            services.TryAddScopedWorkerService<IServicoFrequencia, ServicoFrequencia>();
            services.TryAddScopedWorkerService<IServicoAula, ServicoAula>();
            services.TryAddScopedWorkerService<IServicoAtribuicaoEsporadica, ServicoAtribuicaoEsporadica>();
            services.TryAddScopedWorkerService<IServicoCalculoFrequencia, ServicoCalculoFrequencia>();
            services.TryAddScopedWorkerService<IServicoNotificacaoFrequencia, ServicoNotificacaoFrequencia>();
            services.TryAddScopedWorkerService<IServicoNotificacaoAulaPrevista, ServicoNotificacaoAulaPrevista>();
            services.TryAddScopedWorkerService<IServicoEventoMatricula, ServicoEventoMatricula>();
            services.TryAddScopedWorkerService<IServicoAluno, ServicoAluno>();
            services.TryAddScopedWorkerService<IServicoCompensacaoAusencia, ServicoCompensacaoAusencia>();
            services.TryAddScopedWorkerService<IServicoAtribuicaoCJ, ServicoAtribuicaoCJ>();
            services.TryAddScopedWorkerService<IServicoDeNotasConceitos, ServicoDeNotasConceitos>();
            services.TryAddScopedWorkerService<IServicoFechamento, ServicoFechamento>();
            services.TryAddScopedWorkerService<IServicoFechamentoFinal, ServicoFechamentoFinal>();
            services.TryAddScopedWorkerService<IServicoFechamentoTurmaDisciplina, ServicoFechamentoTurmaDisciplina>();
        }

        private static void ResgistraDependenciaHttp(IServiceCollection services)
        {
            /// Este método não deveria existir, as dependencias dos objetos abaixo deveriam ser encapsuladas em um contexto da aplicação para serem utilizadas pela WebApi e WorkserService independentemente
            //services.TryAddScopedWorkerService<System.Net.Http.HttpClient>();
            services.TryAddScopedWorkerService<Microsoft.AspNetCore.Http.IHttpContextAccessor, NoHttpContext>();
        }
    }
}