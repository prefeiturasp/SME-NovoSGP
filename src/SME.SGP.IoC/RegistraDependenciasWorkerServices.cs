using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services.TryAddTransient<IComandosPlanoCiclo, ComandosPlanoCiclo>();
            services.TryAddTransient<IComandosPlanoAnual, ComandosPlanoAnual>();
            services.TryAddTransient<IComandosSupervisor, ComandosSupervisor>();
            services.TryAddTransient<IComandosNotificacao, ComandosNotificacao>();
            services.TryAddTransient<IComandosWorkflowAprovacao, ComandosWorkflowAprovacao>();
            services.TryAddTransient<IComandosUsuario, ComandosUsuario>();
            services.TryAddTransient<IComandosTipoCalendario, ComandosTipoCalendario>();
            services.TryAddTransient<IComandosFeriadoCalendario, ComandosFeriadoCalendario>();
            services.TryAddTransient<IComandosPeriodoEscolar, ComandosPeriodoEscolar>();
            services.TryAddTransient<IComandosEventoTipo, ComandosEventoTipo>();
            services.TryAddTransient<IComandosEvento, ComandosEvento>();
            services.TryAddTransient<IComandosDiasLetivos, ComandosDiasLetivos>();
            services.TryAddTransient<IComandosAula, ComandosAula>();
            services.TryAddTransient<IComandosGrade, ComandosGrade>();
            services.TryAddTransient<IComandoFrequencia, ComandoFrequencia>();
            services.TryAddTransient<IComandosAtribuicaoEsporadica, ComandosAtribuicaoEsporadica>();
        }

        private static void RegistrarConsultas(IServiceCollection services)
        {
            services.TryAddTransient<IConsultasPlanoCiclo, ConsultasPlanoCiclo>();
            services.TryAddTransient<IConsultasMatrizSaber, ConsultasMatrizSaber>();
            services.TryAddTransient<IConsultasObjetivoDesenvolvimento, ConsultasObjetivoDesenvolvimento>();
            services.TryAddTransient<IConsultasCiclo, ConsultasCiclo>();
            services.TryAddTransient<IConsultasObjetivoAprendizagem, ConsultasObjetivoAprendizagem>();
            services.TryAddTransient<IConsultasPlanoAnual, ConsultasPlanoAnual>();
            services.TryAddTransient<IConsultasDisciplina, ConsultasDisciplina>();
            services.TryAddTransient<IConsultasProfessor, ConsultasProfessor>();
            services.TryAddTransient<IConsultasSupervisor, ConsultasSupervisor>();
            services.TryAddTransient<IConsultaDres, ConsultaDres>();
            services.TryAddTransient<IConsultasNotificacao, ConsultasNotificacao>();
            services.TryAddTransient<IConsultasWorkflowAprovacao, ConsultasWorkflowAprovacao>();
            services.TryAddTransient<IConsultasUnidadesEscolares, ConsultasUnidadesEscolares>();
            services.TryAddTransient<IConsultasTipoCalendario, ConsultasTipoCalendario>();
            services.TryAddTransient<IConsultasFeriadoCalendario, ConsultasFeriadoCalendario>();
            services.TryAddTransient<IConsultasPeriodoEscolar, ConsultasPeriodoEscolar>();
            services.TryAddTransient<IConsultasUsuario, ConsultasUsuario>();
            services.TryAddTransient<IConsultasAbrangencia, ConsultasAbrangencia>();
            services.TryAddTransient<IConsultasEventoTipo, ConsultasEventoTipo>();
            services.TryAddTransient<IConsultasEvento, ConsultasEvento>();
            services.TryAddTransient<IConsultasAula, ConsultasAula>();
            services.TryAddTransient<IConsultasGrade, ConsultasGrade>();
            services.TryAddTransient<IConsultasFrequencia, ConsultasFrequencia>();
            services.TryAddTransient<IConsultasAtribuicaoEsporadica, ConsultasAtribuicaoEsporadica>();            
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddTransient<IContextoAplicacao, WorkerContext>();
            services.TryAddTransient<ISgpContext, SgpContext>();
            services.TryAddTransient<IUnitOfWork, UnitOfWork>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddTransient<IRepositorioPlanoCiclo, RepositorioPlanoCiclo>();
            services.TryAddTransient<IRepositorioMatrizSaberPlano, RepositorioMatrizSaberPlano>();
            services.TryAddTransient<IRepositorioObjetivoDesenvolvimentoPlano, RepositorioObjetivoDesenvolvimentoPlano>();
            services.TryAddTransient<IRepositorioMatrizSaber, RepositorioMatrizSaber>();
            services.TryAddTransient<IRepositorioObjetivoDesenvolvimento, RepositorioObjetivoDesenvolvimento>();
            services.TryAddTransient<IRepositorioCiclo, RepositorioCiclo>();
            services.TryAddTransient<IRepositorioPlanoAnual, RepositorioPlanoAnual>();
            services.TryAddTransient<IRepositorioObjetivoAprendizagemPlano, RepositorioObjetivoAprendizagemPlano>();
            services.TryAddTransient<IRepositorioCache, RepositorioCache>();
            services.TryAddTransient<IRepositorioComponenteCurricular, RepositorioComponenteCurricular>();
            services.TryAddTransient<IRepositorioSupervisorEscolaDre, RepositorioSupervisorEscolaDre>();
            services.TryAddTransient<IRepositorioNotificacao, RepositorioNotificacao>();
            services.TryAddTransient<IRepositorioWorkflowAprovacao, RepositorioWorkflowAprovacao>();
            services.TryAddTransient<IRepositorioWorkflowAprovacaoNivelNotificacao, RepositorioWorkflowAprovaNivelNotificacao>();
            services.TryAddTransient<IRepositorioWorkflowAprovacaoNivel, RepositorioWorkflowAprovacaoNivel>();
            services.TryAddTransient<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddTransient<IRepositorioWorkflowAprovacaoNivelUsuario, RepositorioWorkflowAprovacaoNivelUsuario>();
            services.TryAddTransient<IRepositorioPrioridadePerfil, RepositorioPrioridadePerfil>();
            services.TryAddTransient<IRepositorioConfiguracaoEmail, RepositorioConfiguracaoEmail>();
            services.TryAddTransient<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.TryAddTransient<IRepositorioTipoCalendario, RepositorioTipoCalendario>();
            services.TryAddTransient<IRepositorioFeriadoCalendario, RepositorioFeriadoCalendario>();
            services.TryAddTransient<IRepositorioPeriodoEscolar, RepositorioPeriodoEscolar>();
            services.TryAddTransient<IRepositorioEvento, RepositorioEvento>();
            services.TryAddTransient<IRepositorioEventoTipo, RepositorioEventoTipo>();
            services.TryAddTransient<IRepositorioParametrosSistema, RepositorioParametrosSistema>();
            services.TryAddTransient<IRepositorioAula, RepositorioAula>();
            services.TryAddTransient<IRepositorioGrade, RepositorioGrade>();
            services.TryAddTransient<IRepositorioGradeFiltro, RepositorioGradeFiltro>();
            services.TryAddTransient<IRepositorioGradeDisciplina, RepositorioGradeDisciplina>();
            services.TryAddTransient<IRepositorioFrequencia, RepositorioFrequencia>();
            services.TryAddTransient<IRepositorioRegistroAusenciaAluno, RepositorioRegistroAusenciaAluno>();
            services.TryAddTransient<IRepositorioAtribuicaoEsporadica, RepositorioAtribuicaoEsporadica>();
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddTransient<IServicoWorkflowAprovacao, ServicoWorkflowAprovacao>();
            services.TryAddTransient<IServicoNotificacao, ServicoNotificacao>();
            services.TryAddTransient<IServicoUsuario, ServicoUsuario>();
            services.TryAddTransient<IServicoAutenticacao, ServicoAutenticacao>();
            services.TryAddTransient<IServicoPerfil, ServicoPerfil>();
            services.TryAddTransient<IServicoEmail, ServicoEmail>();
            services.TryAddTransient<IServicoTokenJwt, ServicoTokenJwt>();
            services.TryAddTransient<IServicoMenu, ServicoMenu>();
            services.TryAddTransient<IServicoPeriodoEscolar, ServicoPeriodoEscolar>();
            services.TryAddTransient<IServicoFeriadoCalendario, ServicoFeriadoCalendario>();
            services.TryAddTransient<IServicoAbrangencia, ServicoAbrangencia>();
            services.TryAddTransient<IServicoEvento, ServicoEvento>();
            services.TryAddTransient<IServicoDiaLetivo, ServicoDiaLetivo>();
            services.TryAddTransient<IServicoLog, ServicoLog>();
            services.TryAddTransient<IServicoFrequencia, ServicoFrequencia>();
            services.TryAddTransient<IServicoAula, ServicoAula>();
            services.TryAddTransient<IServicoAtribuicaoEsporadica, ServicoAtribuicaoEsporadica>();
        }

        private static void ResgistraDependenciaHttp(IServiceCollection services)
        {
            /// Este método não deveria existir, as dependencias dos objetos abaixo deveriam ser encapsuladas em um contexto da aplicação para serem utilizadas pela WebApi e WorkserService independentemente
            //services.TryAddTransient<System.Net.Http.HttpClient>();
            services.TryAddTransient<Microsoft.AspNetCore.Http.IHttpContextAccessor, NoHttpContext>();
        }
    }
}