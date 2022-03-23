using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
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
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Turma;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Servicos;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services, ConfiguracaoRabbitOptions configRabbit)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();

            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
            RegistrarServicos(services);
            RegistrarCasosDeUso(services);
            RegistrarRabbit(services, configRabbit);
            RegistrarMapeamentos.Registrar();
        }

        private static void RegistrarRabbit(IServiceCollection services, ConfiguracaoRabbitOptions configRabbit)
        {
            services.AddRabbit(configRabbit);
        }

        private static void RegistrarComandos(IServiceCollection services)
        {
            services.TryAddScoped<IComandosPlanoCiclo, ComandosPlanoCiclo>();
            services.TryAddScoped<IComandosPlanoAnual, ComandosPlanoAnual>();
            services.TryAddScoped<IComandosSupervisor, ComandosSupervisor>();
            services.TryAddScoped<IComandosNotificacao, ComandosNotificacao>();
            services.TryAddScoped<IComandosWorkflowAprovacao, ComandosWorkflowAprovacao>();
            services.TryAddScoped<IComandosUsuario, ComandosUsuario>();
            services.TryAddScoped<IComandosTipoCalendario, ComandosTipoCalendario>();
            services.TryAddScoped<IComandosFeriadoCalendario, ComandosFeriadoCalendario>();
            services.TryAddScoped<IComandosPeriodoEscolar, ComandosPeriodoEscolar>();
            services.TryAddScoped<IComandosEventoTipo, ComandosEventoTipo>();
            services.TryAddScoped<IComandosEvento, ComandosEvento>();
            services.TryAddScoped<IComandosDiasLetivos, ComandosDiasLetivos>();
            services.TryAddScoped<IComandosGrade, ComandosGrade>();
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
            services.TryAddScoped<IComandosCompensacaoAusenciaAluno, ComandosCompensacaoAusenciaAluno>();
            services.TryAddScoped<IComandosCompensacaoAusenciaDisciplinaRegencia, ComandosCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScoped<IComandosProcessoExecutando, ComandosProcessoExecutando>();
            services.TryAddScoped<IComandosEventoFechamento, ComandosEventoFechamento>();
            services.TryAddScoped<IComandosPeriodoFechamento, ComandosPeriodoFechamento>();
            services.TryAddScoped<IComandosFechamentoTurmaDisciplina, ComandosFechamentoTurmaDisciplina>();
            services.TryAddScoped<IComandosFechamentoNota, ComandosFechamentoNota>();
            services.TryAddScoped<IComandosNotificacaoAula, ComandosNotificacaoAula>();
            services.TryAddScoped<IComandosFechamentoFinal, ComandosFechamentoFinal>();
            services.TryAddScoped<IComandosRecuperacaoParalela, ComandosRecuperacaoParalela>();
            services.TryAddScoped<IComandosPendenciaFechamento, ComandosPendenciaFechamento>();
            services.TryAddScoped<IComandosFechamentoTurma, ComandosFechamentoTurma>();
            services.TryAddScoped<IComandosConselhoClasse, ComandosConselhoClasse>();
            services.TryAddScoped<IComandosConselhoClasseAluno, ComandosConselhoClasseAluno>();
            services.TryAddScoped<IComandosConselhoClasseNota, ComandosConselhoClasseNota>();
            services.TryAddScoped<IComandoComunicado, ComandoComunicado>();
            services.TryAddScoped<IComandosRelatorioSemestralTurmaPAP, ComandosRelatorioSemestralTurmaPAP>();
            services.TryAddScoped<IComandosRelatorioSemestralPAPAluno, ComandosRelatorioSemestralPAPAluno>();
            services.TryAddScoped<IComandosRelatorioSemestralPAPAlunoSecao, ComandosRelatorioSemestralPAPAlunoSecao>();
            services.TryAddScoped<IComandosPlanoAnualTerritorioSaber, ComandosPlanoAnualTerritorioSaber>();
        }

        private static void RegistrarConsultas(IServiceCollection services)
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
            services.TryAddScoped<IConsultasUnidadesEscolares, ConsultasUnidadesEscolares>();
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
            services.TryAddScoped<IConsultasFrequencia, ConsultasFrequencia>();
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
            services.TryAddScoped<IConsultasEventoFechamento, ConsultasEventoFechamento>();
            services.TryAddScoped<IConsultaRecuperacaoParalela, ConsultasRecuperacaoParalela>();
            services.TryAddScoped<IConsultasFechamentoFinal, ConsultasFechamentoFinal>();
            services.TryAddScoped<IConsultasTurma, ConsultasTurma>();
            services.TryAddScoped<IConsultasPendenciaFechamento, ConsultasPendenciaFechamento>();
            services.TryAddScoped<IConsultasFechamentoAluno, ConsultasFechamentoAluno>();
            services.TryAddScoped<IConsultasFechamentoTurma, ConsultasFechamentoTurma>();
            services.TryAddScoped<IConsultasConselhoClasse, ConsultasConselhoClasse>();
            services.TryAddScoped<IConsultasConselhoClasseAluno, ConsultasConselhoClasseAluno>();
            services.TryAddScoped<IConsultasConselhoClasseNota, ConsultasConselhoClasseNota>();
            services.TryAddScoped<IConsultaComunicado, ConsultaComunicado>();
            services.TryAddScoped<IConsultasConselhoClasseRecomendacao, ConsultasConselhoClasseRecomendacao>();
            services.TryAddScoped<IConsultasRelatorioSemestralTurmaPAP, ConsultasRelatorioSemestralTurmaPAP>();
            services.TryAddScoped<IConsultasRelatorioSemestralPAPAluno, ConsultasRelatorioSemestralPAPAluno>();
            services.TryAddScoped<IConsultasRelatorioSemestralPAPAlunoSecao, ConsultasRelatorioSemestralPAPAlunoSecao>();
            services.TryAddScoped<IConsultasSecaoRelatorioSemestralPAP, ConsultasSecaoRelatorioSemestralPAP>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultasPlanoAnualTerritorioSaber, ConsultasPlanoAnualTerritorioSaber>();
            services.TryAddScoped<IRepositorioConceitoConsulta, RepositorioConceitoConsulta>();
            services.TryAddScoped<IRepositorioAulaPrevistaConsulta, RepositorioAulaPrevistaConsulta>();
            services.TryAddScoped<IRepositorioAulaPrevistaBimestreConsulta, RepositorioAulaPrevistaBimestreConsulta>();
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();
            services.TryAddScoped<ISgpContext, SgpContext>();
            services.TryAddScoped<ISgpContextConsultas, SgpContextConsultas>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
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
            services.TryAddScoped<IRepositorioConfiguracaoEmail, RepositorioConfiguracaoEmail>();
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
            services.TryAddScoped<IRepositorioCompensacaoAusencia, RepositorioCompensacaoAusencia>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaConsulta, RepositorioCompensacaoAusenciaConsulta>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAluno, RepositorioCompensacaoAusenciaAluno>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAlunoConsulta, RepositorioCompensacaoAusenciaAlunoConsulta>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaDisciplinaRegencia, RepositorioCompensacaoAusenciaDisciplinaRegencia>();
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
            services.TryAddScoped<IRepositorioRelatorioSemestralPAPAlunoSecao, RepositorioRelatorioSemestralPAPAlunoSecao>();
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
            services.TryAddScoped<IRepositorioClassificacaoDocumento, RepositorioClassificacaoDocumento>();
            services.TryAddScoped<IRepositorioTipoDocumento, RepositorioTipoDocumento>();
            services.TryAddScoped<IRepositorioRemoveConexaoIdle, RepositorioRemoveConexaoIdle>();
            services.TryAddScoped<IRepositorioRegistroIndividual, RepositorioRegistroIndividual>();
            services.TryAddScoped<IRepositorioOcorrencia, RepositorioOcorrencia>();
            services.TryAddScoped<IRepositorioOcorrenciaAluno, RepositorioOcorrenciaAluno>();
            services.TryAddScoped<IRepositorioOcorrenciaTipo, RepositorioOcorrenciaTipo>();
            services.TryAddScoped<IRepositorioAlunoFoto, RepositorioAlunoFoto>();
            services.TryAddScoped<IRepositorioAreaDoConhecimento, RepositorioAreaDoConhecimento>();
            services.TryAddScoped<IRepositorioComponenteCurricularGrupoAreaOrdenacao, RepositorioComponenteCurricularGrupoAreaOrdenacao>();

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

            // EventoTipo
            services.TryAddScoped<IRepositorioEventoTipo, RepositorioEventoTipo>();
            services.TryAddScoped<IRepositorioPerfilEventoTipo, RepositorioPerfilEventoTipo>();

            // Fechamento
            services.TryAddScoped<IRepositorioAnotacaoFechamentoAluno, RepositorioAnotacaoFechamentoAluno>();

            // Pendencias do EncaminhamentoAEE
            services.TryAddScoped<IRepositorioPendenciaEncaminhamentoAEE, RepositorioPendenciaEncaminhamentoAEE>();



            // Questionario
            services.TryAddScoped<IRepositorioQuestionario, RepositorioQuestionario>();
            services.TryAddScoped<IRepositorioQuestao, RepositorioQuestao>();
            services.TryAddScoped<IRepositorioOpcaoResposta, RepositorioOpcaoResposta>();
            services.TryAddScoped<IRepositorioOpcaoQuestaoComplementar, RepositorioOpcaoQuestaoComplementar>();

            // Pendencias
            services.TryAddScoped<IRepositorioPendencia, RepositorioPendencia>();
            services.TryAddScoped<IRepositorioPendenciaFechamento, RepositorioPendenciaFechamento>();
            services.TryAddScoped<IRepositorioPendenciaAula, RepositorioPendenciaAula>();
            services.TryAddScoped<IRepositorioPendenciaAulaConsulta, RepositorioPendenciaAulaConsulta>();
            services.TryAddScoped<IRepositorioPendenciaUsuario, RepositorioPendenciaUsuario>();
            services.TryAddScoped<IRepositorioPendenciaCalendarioUe, RepositorioPendenciaCalendarioUe>();
            services.TryAddScoped<IRepositorioPendenciaParametroEvento, RepositorioPendenciaParametroEvento>();
            services.TryAddScoped<IRepositorioPendenciaProfessor, RepositorioPendenciaProfessor>();
            services.TryAddScoped<IRepositorioPendenciaRegistroIndividual, RepositorioPendenciaRegistroIndividual>();
            services.TryAddScoped<IRepositorioPendenciaRegistroIndividualAluno, RepositorioPendenciaRegistroIndividualAluno>();
            services.TryAddScoped<IRepositorioPendenciaPerfil, RepositorioPendenciaPerfil>();
            services.TryAddScoped<IRepositorioPendenciaPerfilUsuario, RepositorioPendenciaPerfilUsuario>();
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

            // Notificações Plano AEE
            services.TryAddScoped<IRepositorioNotificacaoPlanoAEE, RepositorioNotificacaoPlanoAEE>();

            // Consolidação Frequeência Turma
            services.TryAddScoped<IRepositorioConsolidacaoFrequenciaTurma, RepositorioConsolidacaoFrequenciaTurma>();
            services.TryAddScoped<IRepositorioConsolidacaoFrequenciaTurmaConsulta, RepositorioConsolidacaoFrequenciaTurmaConsulta>();

            // Consolidação Devolutivas
            services.TryAddScoped<IRepositorioConsolidacaoDevolutivas, RepositorioConsolidacaoDevolutivas>();

            // Frequência 
            services.TryAddScoped<IRepositorioFrequenciaPreDefinida, RepositorioFrequenciaPreDefinida>();
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

            // Area do Conhecimento
            services.TryAddScoped<IObterAreasConhecimentoUseCase, ObterAreasConhecimentoUseCase>();
            services.TryAddScoped<IObterOrdenacaoAreasConhecimentoUseCase, ObterOrdenacaoAreasConhecimentoUseCase>();
            services.TryAddScoped<IMapearAreasDoConhecimentoUseCase, MapearAreasDoConhecimentoUseCase>();
            services.TryAddScoped<IObterComponentesDasAreasDeConhecimentoUseCase, ObterComponentesDasAreasDeConhecimentoUseCase>();

            services.TryAddScoped<IRabbitDeadletterSerapSyncUseCase, RabbitDeadletterSerapSyncUseCase>();            

            // Consolidacao de Registros Pedagogicos
            services.TryAddScoped<IRepositorioConsolidacaoRegistrosPedagogicos, RepositorioConsolidacaoRegistrosPedagogicos>();
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScoped<IServicoWorkflowAprovacao, ServicoWorkflowAprovacao>();
            services.TryAddScoped<IServicoNotificacao, ServicoNotificacao>();
            services.TryAddScoped<IServicoUsuario, ServicoUsuario>();
            services.TryAddScoped<IServicoAutenticacao, ServicoAutenticacao>();
            services.TryAddScoped<IServicoPerfil, ServicoPerfil>();
            services.TryAddScoped<IServicoEmail, ServicoEmail>();
            services.TryAddScoped<IServicoTokenJwt, ServicoTokenJwt>();
            services.TryAddScoped<IServicoMenu, ServicoMenu>();
            services.TryAddScoped<IServicoPeriodoEscolar, ServicoPeriodoEscolar>();
            services.TryAddScoped<IServicoFeriadoCalendario, ServicoFeriadoCalendario>();
            services.TryAddScoped<IServicoAbrangencia, ServicoAbrangencia>();
            services.TryAddScoped<IServicoEvento, ServicoEvento>();
            services.TryAddScoped<IServicoLog, ServicoLog>();
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
            services.TryAddScoped<IServicoConselhoClasse, ServicoConselhoClasse>();
            services.TryAddScoped<IServicoObjetivosAprendizagem, ServicoObjetivosAprendizagem>();
            services.TryAddScoped<IServicoFila, FilaRabbit>();
            services.TryAddScoped<IServicoTelemetria, ServicoTelemetria>();          
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.TryAddScoped<IObterUltimaVersaoUseCase, ObterUltimaVersaoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseAlunoUseCase, ImpressaoConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseTurmaUseCase, ImpressaoConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IReceberRelatorioProntoUseCase, ReceberRelatorioProntoUseCase>();
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
            services.TryAddScoped<IObterFuncionariosUseCase, ObterFuncionariosUseCase>();
            services.TryAddScoped<IExcluirAulaRecorrenteUseCase, ExcluirAulaRecorrenteUseCase>();
            services.TryAddScoped<IInserirAulaRecorrenteUseCase, InserirAulaRecorrenteUseCase>();
            services.TryAddScoped<IAlterarAulaRecorrenteUseCase, AlterarAulaRecorrenteUseCase>();
            services.TryAddScoped<IObterBimestresLiberacaoBoletimUseCase, ObterBimestresLiberacaoBoletimUseCase>();
            services.TryAddScoped<INotificarUsuarioUseCase, NotificarUsuarioUseCase>();
            services.TryAddScoped<IReceberRelatorioComErroUseCase, ReceberRelatorioComErroUseCase>();
            services.TryAddScoped<IHistoricoEscolarUseCase, HistoricoEscolarUseCase>();
            services.TryAddScoped<IObterAlunosPorCodigoEolNomeUseCase, ObterAlunosPorCodigoEolNomeUseCase>();
            services.TryAddScoped<IGerarRelatorioFrequenciaUseCase, GerarRelatorioFrequenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosDresPorAbrangenciaUseCase, ObterFiltroRelatoriosDresPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosUesPorAbrangenciaUseCase, ObterFiltroRelatoriosUesPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosModalidadesPorUeUseCase, ObterFiltroRelatoriosModalidadesPorUeUseCase>();
            services.TryAddScoped<IRelatorioPendenciasUseCase, RelatorioPendenciasFechamentoUseCase>();
            services.TryAddScoped<IObterAnotacaoFrequenciaAlunoUseCase, ObterAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase, ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase>();
            services.TryAddScoped<ICopiarCodigoCorrelacaoUseCase, CopiarCodigoCorrelacaoUseCase>();
            services.TryAddScoped<IInserirCodigoCorrelacaoUseCase, InserirCodigoCorrelacaoUseCase>();
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

            services.TryAddScoped<IExcluirDevolutivaUseCase, ExcluirDevolutivaUseCase>();
            services.TryAddScoped<IObterListaDevolutivasPorTurmaComponenteUseCase, ObterListaDevolutivasPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterDiariosBordoPorDevolutiva, ObterDiariosBordoPorDevolutiva>();
            services.TryAddScoped<IObterDevolutivaPorIdUseCase, ObterDevolutivaPorIdUseCase>();
            services.TryAddScoped<IObterDiariosDeBordoPorPeriodoUseCase, ObterDiariosDeBordoPorPeriodoUseCase>();
            services.TryAddScoped<IObterListagemDiariosDeBordoPorPeriodoUseCase, ObterListagemDiariosDeBordoPorPeriodoUseCase>();
            services.TryAddScoped<IAlterarNotificacaoObservacaoDiarioBordoUseCase, AlterarNotificacaoObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase, ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterUsuarioNotificarDiarioBordoObservacaoUseCase, ObterUsuarioNotificarDiarioBordoObservacaoUseCase>();
            services.TryAddScoped<IRelatorioCompensacaoAusenciaUseCase, RelatorioCompensacaoAusenciaUseCase>();
            services.TryAddScoped<IRelatorioCalendarioUseCase, RelatorioCalendarioUseCase>();

            services.TryAddScoped<ICartaIntencoesPersistenciaUseCase, CartaIntencoesPersistenciaUseCase>();
            services.TryAddScoped<IObterCartasDeIntencoesPorTurmaEComponenteUseCase, ObterCartasDeIntencoesPorTurmaEComponenteUseCase>();
            services.TryAddScoped<IAdicionarObservacaoDiarioBordoUseCase, AdicionarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IAlterarObservacaoDiarioBordoUseCase, AlterarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IExcluirObservacaoDiarioBordoUseCase, ExcluirObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IListarObservacaoDiarioBordoUseCase, ListarObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterSintesePorAnoLetivoUseCase, ObterSintesePorAnoLetivoUseCase>();
            services.TryAddScoped<ICriarAulasInfantilAutomaticamenteUseCase, CriarAulasInfantilAutomaticamenteUseCase>();
            services.TryAddScoped<ICriarAulasInfantilUseCase, CriarAulasInfantilUseCase>();
            services.TryAddScoped<INotificarExclusaoAulaComFrequenciaUseCase, NotificarExclusaoAulaComFrequenciaUseCase>();
            services.TryAddScoped<ISalvarAnotacaoFrequenciaAlunoUseCase, SalvarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IAlterarAnotacaoFrequenciaAlunoUseCase, AlterarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IExcluirAnotacaoFrequenciaAlunoUseCase, ExcluirAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterAnotacaoFrequenciaAlunoPorIdUseCase, ObterAnotacaoFrequenciaAlunoPorIdUseCase>();
            services.TryAddScoped<IObterMotivosAusenciaUseCase, ObterMotivosAusenciaUseCase>();
            services.TryAddScoped<IObterFechamentoConsolidadoPorTurmaBimestreUseCase, ObterFechamentoConsolidadoPorTurmaBimestreUseCase>();
            services.TryAddScoped<IObterFechamentoConselhoClasseAlunosPorTurmaUseCase, ObterFechamentoConselhoClasseAlunosPorTurmaUseCase>();
            services.TryAddScoped<IObterDetalhamentoFechamentoConselhoClasseAlunoUseCase, ObterDetalhamentoFechamentoConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase, ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaFechamentoComponenteUseCase, ExecutarConsolidacaoTurmaFechamentoComponenteUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaGeralUseCase, ExecutarConsolidacaoTurmaGeralUseCase>();
            services.TryAddScoped<IIniciaConsolidacaoTurmaGeralUseCase, IniciaConsolidacaoTurmaGeralUseCase>();

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



            // Acompanhamento Aluno
            services.TryAddScoped<ISalvarAcompanhamentoAlunoUseCase, SalvarAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<ISalvarFotoAcompanhamentoAlunoUseCase, SalvarFotoAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterFotosSemestreAlunoUseCase, ObterFotosSemestreAlunoUseCase>();
            services.TryAddScoped<IExcluirFotoAlunoUseCase, ExcluirFotoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoAlunoUseCase, ObterAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoAlunoUseCase, ObterAcompanhamentoAlunoUseCase>();
            services.TryAddScoped<IObterAcompanhamentoTurmaApanhadoGeralUseCase, ObterAcompanhamentoTurmaApanhadoGeralUseCase>();
            services.TryAddScoped<ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, CarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();
            services.TryAddScoped<ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, SyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();

            // Aulas Previstas
            services.TryAddScoped<INotificacaoAulasPrevistrasSyncUseCase, NotificacaoAulasPrevistrasSyncUseCase>();
            services.TryAddScoped<INotificacaoAulasPrevistrasUseCase, NotificacaoAulasPrevistrasUseCase>();

            // Acompanhamento Turma
            services.TryAddScoped<ISalvarAcompanhamentoTurmaUseCase, SalvarAcompanhamentoTurmaUseCase>();
            services.TryAddScoped<IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase, ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase>();

            // Armazenamento de arquivos
            services.TryAddScoped<IUploadDeArquivoUseCase, UploadDeArquivoUseCase>();
            services.TryAddScoped<IDownloadDeArquivoUseCase, DownloadDeArquivoUseCase>();
            services.TryAddScoped<IExcluirArquivoUseCase, ExcluirArquivoUseCase>();

            // Atividades
            services.TryAddScoped<IImportarAtividadesGsaUseCase, ImportarAtividadesGsaUseCase>();
            services.TryAddScoped<IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase, ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase>();

            // Atividade Infantil
            services.TryAddScoped<IObterAtividadesInfantilUseCase, ObterAtividadesInfantilUseCase>();

            // Avisos do Mural Gsa
            services.TryAddScoped<IImportarAvisoDoMuralGsaUseCase, ImportarAvisoDoMuralGsaUseCase>();
            services.TryAddScoped<IObterMuralAvisosUseCase, ObterMuralAvisosUseCase>();
            services.TryAddScoped<IAlterarAvisoMuralUseCase, AlterarAvisoMuralUseCase>();

            //Carta Intenções Observacao
            services.TryAddScoped<IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase, ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase>();
            services.TryAddScoped<ISalvarCartaIntencoesObservacaoUseCase, SalvarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IAlterarCartaIntencoesObservacaoUseCase, AlterarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirCartaIntencoesObservacaoUseCase, ExcluirCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<ISalvarNotificacaoCartaIntencoesObservacaoUseCase, SalvarNotificacaoCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirNotificacaoCartaIntencoesObservacaoUseCase, ExcluirNotificacaoCartaIntencoesObservacaoUseCase>();

            // Compensacao Ausencia
            services.TryAddScoped<IPeriodoDeCompensacaoAbertoUseCase, PeriodoDeCompensacaoAbertoUseCase>();
            services.TryAddScoped<INotificarCompensacaoAusenciaUseCase, NotificarCompensacaoAusenciaUseCase>();

            // Componentes Curriculares
            services.TryAddScoped<IObterComponentesCurricularesPorTurmaECodigoUeUseCase, ObterComponentesCurricularesPorTurmaECodigoUeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorUeAnosModalidadeUseCase, ObterComponentesCurricularesPorUeAnosModalidadeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorAnoEscolarUseCase, ObterComponentesCurricularesPorAnoEscolarUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase, ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase>();

            // Conselho de Classe
            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterPareceresConclusivosUseCase, ObterPareceresConclusivosUseCase>();
            services.TryAddScoped<IAtualizarSituacaoConselhoClasseUseCase, AtualizarSituacaoConselhoClasseUseCase>();

            // Consolidacao Frequencia Turma
            services.TryAddScoped<IConsolidarFrequenciaTurmasUseCase, ConsolidarFrequenciaTurmasUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorAnoUseCase, ConsolidarFrequenciaTurmasPorAnoUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaPorTurmaUseCase, ConsolidarFrequenciaPorTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase, ConsolidacaoDashBoardFrequenciaPorDataETipoUseCase>();

            //Calcula Frequencia Geral
            services.TryAddScoped<ICalcularFrequenciaGeralUseCase, CalcularFrequenciaGeralUseCase>();

            // Fechamento
            services.TryAddScoped<IGerarPendenciasFechamentoUseCase, GerarPendenciasFechamentoUseCase>();
            services.TryAddScoped<IExecutarVarreduraFechamentosEmProcessamentoPendentes, ExecutarVarreduraFechamentosEmProcessamentoPendentes>();
            services.TryAddScoped<IVarreduraFechamentosEmProcessamentoPendentesUseCase, VarreduraFechamentosEmProcessamentoPendentesUseCase>();
            services.TryAddScoped<IInserirFechamentoTurmaDisciplinaUseCase, InserirFechamentoTurmaDisciplinaUseCase>();
            services.TryAddScoped<IObterFechamentoIdPorTurmaBimestreUseCase, ObterFechamentoIdPorTurmaBimestreUseCase>();

            // Fechamento Aluno
            services.TryAddScoped<ISalvarAnotacaoFechamentoAlunoUseCase, SalvarAnotacaoFechamentoAlunoUseCase>();

            //Fechamento Reabertura
            services.TryAddScoped<INotificacaoPeriodoFechamentoReaberturaIniciando, NotificacaoPeriodoFechamentoReaberturaIniciandoUseCase>();
            services.TryAddScoped<INotificacaoPeriodoFechamentoReaberturaEncerrando, NotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase>();


            //Notificacao Devolutivoa
            services.TryAddScoped<ISalvarNotificacaoDevolutivaUseCase, SalvarNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDevolutivaUseCase, ExcluirNotificacaoDevolutivaUseCase>();

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
            services.TryAddScoped<IObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase, ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaUseCase, ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase, ObterQuantidadeTotalDeDiariosPendentesPorDREUseCase>();
            services.TryAddScoped<IObterUltimaConsolidacaoDiarioBordoUseCase, ObterUltimaConsolidacaoDiarioBordoUseCase>();

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

            // EncaminhamentoAEE
            services.TryAddScoped<IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase, ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterQuestionarioEncaminhamentoAeeUseCase, ObterQuestionarioEncaminhamentoAeeUseCase>();
            services.TryAddScoped<IExcluirArquivoAeeUseCase, ExcluirArquivoAeeUseCase>();
            services.TryAddScoped<IExcluirEncaminhamentoAEEUseCase, ExcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterInstrucoesModalUseCase, ObterInstrucoesModalUseCase>();
            services.TryAddScoped<IObterEncaminhamentosAEEUseCase, ObterEncaminhamentosAEEUseCase>();
            services.TryAddScoped<IPesquisaResponsavelEncaminhamentoPorDreUEUseCase, PesquisaResponsavelEncaminhamentoPorDreUEUseCase>();

            //FechamentoReabertura
            services.TryAddScoped<INotificarFechamentoReaberturaUseCase, NotificarFechamentoReaberturaUseCase>();
            services.TryAddScoped<INotificarFechamentoReaberturaDREUseCase, NotificarFechamentoReaberturaDREUseCase>();
            services.TryAddScoped<INotificarFechamentoReaberturaUEUseCase, NotificarFechamentoReaberturaUEUseCase>();

            // Funcionario
            services.TryAddScoped<IPesquisaFuncionariosPorDreUeUseCase, PesquisaFuncionariosPorDreUeUseCase>();
            services.TryAddScoped<IObterFuncionariosPAAIPorDreUseCase, ObterFuncionariosPAAIPorDreUseCase>();


            // Grade Curricular
            services.TryAddScoped<IRelatorioControleGradeUseCase, RelatorioControleGradeUseCase>();


            //Notificacao Devolutiva
            services.TryAddScoped<ISalvarNotificacaoDevolutivaUseCase, SalvarNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDevolutivaUseCase, ExcluirNotificacaoDevolutivaUseCase>();


            services.TryAddScoped<IObterUsuarioPorCpfUseCase, ObterUsuarioPorCpfUseCase>();
            services.TryAddScoped<ISolicitarReiniciarSenhaEscolaAquiUseCase, SolicitarReiniciarSenhaEscolaAquiUseCase>();
            services.TryAddScoped<INotificarDiarioBordoObservacaoUseCase, NotificarDiarioBordoObservacaoUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDiarioBordoUseCase, ExcluirNotificacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterAnosLetivosPAPUseCase, ObterAnosLetivosPAPUseCase>();

            services.TryAddScoped<IBuscarTiposCalendarioPorDescricaoUseCase, BuscarTiposCalendarioPorDescricaoUseCase>();

            services.TryAddScoped<IPendenciaAulaUseCase, PendenciaAulaUseCase>();
            services.TryAddScoped<IPendenciaAulaDiarioBordoUseCase, PendenciaAulaDiarioBordoUseCase>();
            services.TryAddScoped<IPendenciaAulaAvaliacaoUseCase, PendenciaAulaAvaliacaoUseCase>();
            services.TryAddScoped<IPendenciaAulaFrequenciaUseCase, PendenciaAulaFrequenciaUseCase>();
            services.TryAddScoped<IPendenciaAulaPlanoAulaUseCase, PendenciaAulaPlanoAulaUseCase>();

            services.TryAddScoped<ISalvarPlanejamentoAnualUseCase, SalvarPlanejamentoAnualUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase, ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase, ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponenteUseCase, ObterPlanejamentoAnualPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase, ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase>();
            services.TryAddScoped<IObterPeriodoLetivoTurmaUseCase, ObterPeriodoLetivoTurmaUseCase>();

            // Frequência
            services.TryAddScoped<IConciliacaoFrequenciaTurmasSyncUseCase, ConciliacaoFrequenciaTurmasSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasPorPeriodoUseCase, ConciliacaoFrequenciaTurmasPorPeriodoUseCase>();
            services.TryAddScoped<IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase, ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoFrequenciaPorAnoUseCase, ExecutaConsolidacaoFrequenciaPorAnoUseCase>();
            services.TryAddScoped<IInserirFrequenciaListaoUseCase, InserirFrequenciaListaoUseCase>();

            // Notificações

            services.TryAddScoped<INotificacaoFrequenciaUeUseCase, NotificacaoFrequenciaUeUseCase>();
            services.TryAddScoped<INotificacaoAndamentoFechamentoUseCase, NotificacaoAndamentoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoUeFechamentosInsuficientesUseCase, NotificacaoUeFechamentosInsuficientesUseCase>();
            services.TryAddScoped<INotificacaoInicioFimPeriodoFechamentoUseCase, NotificacaoInicioFimPeriodoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoInicioPeriodoFechamentoUEUseCase, NotificacaoInicioPeriodoFechamentoUEUseCase>();
            services.TryAddScoped<INotificacaoFimPeriodoFechamentoUEUseCase, NotificacaoFimPeriodoFechamentoUEUseCase>();


            //Notificação Resultado Insatisfatorio 
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();

            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();

            //Objetivo Curricular
            services.TryAddScoped<IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase, ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase>();

            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterObjetivosPorDisciplinaUseCase, ObterObjetivosPorDisciplinaUseCase>();

            // Parecer Conclusivo
            services.TryAddScoped<IReprocessarParecerConclusivoPorAnoUseCase, ReprocessarParecerConclusivoPorAnoUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorDreUseCase, ReprocessarParecerConclusivoPorDreUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorUeUseCase, ReprocessarParecerConclusivoPorUeUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorTurmaUseCase, ReprocessarParecerConclusivoPorTurmaUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoAlunoUseCase, ReprocessarParecerConclusivoAlunoUseCase>();

            // Pendencias
            services.TryAddScoped<IObterPendenciasUseCase, ObterPendenciasUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisUseCase, ExecutaVerificacaoPendenciasGeraisUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisAulaUseCase, ExecutaVerificacaoPendenciasGeraisAulaUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisCalendarioUseCase, ExecutaVerificacaoPendenciasGeraisCalendarioUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisEventosUseCase, ExecutaVerificacaoPendenciasGeraisEventosUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAulaUseCase, ExecutarExclusaoPendenciasAulaUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciaDiasLetivosInsuficientes, ExecutarExclusaoPendenciaDiasLetivosInsuficientes>();
            services.TryAddScoped<IExecutarExclusaoPendenciaParametroEvento, ExecutarExclusaoPendenciaParametroEvento>();

            services.TryAddScoped<ITratarAtribuicaoPendenciasUsuariosUseCase, TratarAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<ICargaAtribuicaoPendenciasPerfilUsuarioUseCase, CargaAtribuicaoPendenciasPerfilUsuarioUseCase>();

            // Pendencias Professor
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase, ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase, ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase>();

            // Pendencias Ausencia Fechamento
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase, ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaFechamentoUseCase, ExecutarExclusaoPendenciasAusenciaFechamentoUseCase>();

            // Pendencias Ausencia Registro Individual
            services.TryAddScoped<IGerarPendenciaAusenciaRegistroIndividualUseCase, GerarPendenciaAusenciaRegistroIndividualUseCase>();
            services.TryAddScoped<IAtualizarPendenciaRegistroIndividualUseCase, AtualizarPendenciaRegistroIndividualUseCase>();

            //Notificação Resultado Insatisfatorio 
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();

            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();

            // Periodo Fechamento
            services.TryAddScoped<IObterPeriodoFechamentoVigenteUseCase, ObterPeriodoFechamentoVigenteUseCase>();
            services.TryAddScoped<IVerificaPendenciasFechamentoUseCase, VerificaPendenciasFechamentoUseCase>();

            //PeriodoEscolar
            services.TryAddScoped<IObterPeriodosPorComponenteUseCase, ObterPeriodosPorComponenteUseCase>();

            // Plano AEE
            services.TryAddScoped<IObterPlanoAEEPorIdUseCase, ObterPlanoAEEPorIdUseCase>();
            services.TryAddScoped<IObterQuestoesPlanoAEEPorVersaoUseCase, ObterQuestoesPlanoAEEPorVersaoUseCase>();
            services.TryAddScoped<IObterPlanoAEEPorCodigoEstudanteUseCase, ObterPlanoAEEPorCodigoEstudanteUseCase>();
            services.TryAddScoped<IVerificarExistenciaPlanoAEEPorEstudanteUseCase, VerificarExistenciaPlanoAEEPorEstudanteUseCase>();
            services.TryAddScoped<IGerarPendenciaValidadePlanoAEEUseCase, GerarPendenciaValidadePlanoAEEUseCase>();


            // Plano Aula
            services.TryAddScoped<IObterPlanoAulaUseCase, ObterPlanoAulaUseCase>();
            services.TryAddScoped<IExcluirPlanoAulaUseCase, ExcluirPlanoAulaUseCase>();
            services.TryAddScoped<IMigrarPlanoAulaUseCase, MigrarPlanoAulaUseCase>();
            services.TryAddScoped<ISalvarPlanoAulaUseCase, SalvarPlanoAulaUseCase>();
            services.TryAddScoped<IObterPlanoAulasPorTurmaEComponentePeriodoUseCase, ObterPlanoAulasPorTurmaEComponentePeriodoUseCase>();            

            // Relatórios
            services.TryAddScoped<IRelatorioPlanoAulaUseCase, RelatorioPlanoAulaUseCase>();
            services.TryAddScoped<IRelatorioUsuariosUseCase, RelatorioUsuariosUseCase>();


            //Sincronismo CC Eol
            services.TryAddScoped<IListarComponentesCurricularesEolUseCase, ListarComponentesCurricularesEolUseCase>();
            services.TryAddScoped<ISincronizarComponentesCurricularesUseCase, SincronizarComponentesCurricularesUseCase>();

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
            services.TryAddScoped<ICalculoFrequenciaTurmaDisciplinaUseCase, CalculoFrequenciaTurmaDisciplinaUseCase>();

            services.TryAddScoped<ITrataNotificacoesNiveisCargosUseCase, TrataNotificacoesNiveisCargosUseCase>();

            services.TryAddScoped<IRelatorioAEAdesaoUseCase, RelatorioAEAdesaoUseCase>();

            services.TryAddScoped<IRelatorioLeituraComunicadosUseCase, RelatorioLeituraComunicadosUseCase>();
            services.TryAddScoped<IRelatorioPlanejamentoDiarioUseCase, RelatorioPlanejamentoDiarioUseCase>();

            services.TryAddScoped<IRemoveConexaoIdleUseCase, RemoveConexaoIdleUseCase>();

            // EncaminhamentoAEE
            services.TryAddScoped<IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase, ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterQuestionarioEncaminhamentoAeeUseCase, ObterQuestionarioEncaminhamentoAeeUseCase>();
            services.TryAddScoped<IExcluirArquivoAeeUseCase, ExcluirArquivoAeeUseCase>();
            services.TryAddScoped<IExcluirEncaminhamentoAEEUseCase, ExcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterInstrucoesModalUseCase, ObterInstrucoesModalUseCase>();
            services.TryAddScoped<IObterEncaminhamentosAEEUseCase, ObterEncaminhamentosAEEUseCase>();
            services.TryAddScoped<IEncerrarEncaminhamentoAEEUseCase, EncerrarEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IEnviarParaAnaliseEncaminhamentoAEEUseCase, EnviarParaAnaliseEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IVerificaPodeCadstrarEncaminhamentoAEEParaEstudanteUseCase, VerificaPodeCadstrarEncaminhamentoAEEParaEstudanteUseCase>();
            services.TryAddScoped<IConcluirEncaminhamentoAEEUseCase, ConcluirEncaminhamentoAEEUseCase>();
            services.TryAddScoped<IObterResponsaveisEncaminhamentosAEE, ObterResponsaveisEncaminhamentosAEE>();
            services.TryAddScoped<IDevolverEncaminhamentoUseCase, DevolverEncaminhamentoUseCase>();


            // Plano AEE 
            services.TryAddScoped<IObterPlanosAEEUseCase, ObterPlanosAEEUseCase>();
            services.TryAddScoped<IObterSituacaoEncaminhamentoPorEstudanteUseCase, ObterSituacaoEncaminhamentoPorEstudanteUseCase>();
            services.TryAddScoped<ISalvarPlanoAEEUseCase, SalvarPlanoAEEUseCase>();
            services.TryAddScoped<IObterVersoesPlanoAEEUseCase, ObterVersoesPlanoAEEUseCase>();
            services.TryAddScoped<IObterRestruturacoesPlanoAEEPorIdUseCase, ObterRestruturacoesPlanoAEEPorIdUseCase>();

            services.TryAddScoped<IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase, ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase>();
            services.TryAddScoped<IEncerrarPlanosAEEEstudantesInativosUseCase, EncerrarPlanosAEEEstudantesInativosUseCase>();
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
            services.TryAddScoped<INotificacaoSalvarItineranciaUseCase, NotificacaoSalvarItineranciaUseCase>();
            services.TryAddScoped<IRelatorioItineranciasUseCase, RelatorioItineranciasUseCase>();

            // Plano AEE
            services.TryAddScoped<ICadastrarParecerCPPlanoAEEUseCase, CadastrarParecerCPPlanoAEEUseCase>();
            services.TryAddScoped<ICadastrarParecerPAAIPlanoAEEUseCase, CadastrarParecerPAAIPlanoAEEUseCase>();
            services.TryAddScoped<IAtribuirResponsavelPlanoAEEUseCase, AtribuirResponsavelPlanoAEEUseCase>();
            services.TryAddScoped<INotificarPlanosAEEExpiradosUseCase, NotificarPlanosAEEExpiradosUseCase>();
            services.TryAddScoped<INotificarPlanosAEEEmAbertoUseCase, NotificarPlanosAEEEmAbertoUseCase>();

            services.TryAddScoped<IEnviarNotificacaoReestruturacaoPlanoAEEUseCase, EnviarNotificacaoReestruturacaoPlanoAEEUseCase>();
            services.TryAddScoped<IEnviarNotificacaoCriacaoPlanoAEEUseCase, EnviarNotificacaoCriacaoPlanoAEEUseCase>();
            services.TryAddScoped<IEnviarNotificacaoEncerramentoPlanoAEEUseCase, EnviarNotificacaoEncerramentoPlanoAEEUseCase>();

            services.TryAddScoped<IObterInformacoesDeFrequenciaAlunosPorBimestreUseCase, ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase>();
            services.TryAddScoped<IObterAlunosAtivosPorUeENomeUseCase, ObterAlunosAtivosPorUeENomeUseCase>();

            services.TryAddScoped<ICriarPlanoAEEObservacaoUseCase, CriarPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IAlterarPlanoAEEObservacaoUseCase, AlterarPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IExcluirPlanoAEEObservacaoUseCase, ExcluirPlanoAEEObservacaoUseCase>();
            services.TryAddScoped<IDevolverPlanoAEEUseCase, DevolverPlanoAEEUseCase>();

            //Notificacoes EncaminhamentoAEE
            services.TryAddScoped<INotificacaoConclusaoEncaminhamentoAEEUseCase, NotificacaoConclusaoEncaminhamentoAEEUseCase>();
            services.TryAddScoped<INotificacaoEncerramentoEncaminhamentoAEEUseCase, NotificacaoEncerramentoEncaminhamentoAEEUseCase>();
            services.TryAddScoped<INotificacaoDevolucaoEncaminhamentoAEEUseCase, NotificacaoDevolucaoEncaminhamentoAEEUseCase>();

            services.TryAddScoped<IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase, ObterInformacoesDeFrequenciaAlunoPorSemestreUseCase>();


            services.TryAddScoped<IRelatorioDevolutivasUseCase, RelatorioDevolutivasUseCase>();
            services.TryAddScoped<IObterBimestreAtualPorTurmaIdUseCase, ObterBimestreAtualPorTurmaIdUseCase>();
            services.TryAddScoped<IObterPeriodoLetivoTurmaUseCase, ObterPeriodoLetivoTurmaUseCase>();

            services.TryAddScoped<IObterEstudanteFotoUseCase, ObterEstudanteFotoUseCase>();
            services.TryAddScoped<ISalvarFotoEstudanteUseCase, SalvarFotoEstudanteUseCase>();
            services.TryAddScoped<IExcluirEstudanteFotoUseCase, ExcluirEstudanteFotoUseCase>();

            services.TryAddScoped<IExecutarSincronizacaoInstitucionalDreSyncUseCase, ExecutarSincronizacaoInstitucionalDreSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalDreTratarUseCase, ExecutarSincronizacaoInstitucionalDreTratarUseCase>();

            services.TryAddScoped<IExecutarSincronizacaoInstitucionalUeTratarUseCase, ExecutarSincronizacaoInstitucionalUeTratarUseCase>();

            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase, ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase, ExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalCicloSyncUseCase, ExecutarSincronizacaoInstitucionalCicloSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalCicloTratarUseCase, ExecutarSincronizacaoInstitucionalCicloTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTurmaSyncUseCase, ExecutarSincronizacaoInstitucionalTurmaSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTurmaTratarUseCase, ExecutarSincronizacaoInstitucionalTurmaTratarUseCase>();


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

            services.TryAddScoped<IRelatorioRegistroIndividualUseCase, RelatorioRegistroIndividualUseCase>();
            services.TryAddScoped<IObterTurmasFechamentoAcompanhamentoUseCase, ObterTurmasFechamentoAcompanhamentoUseCase>();

            services.TryAddScoped<IObterDashboardFrequenciaPorAnoUseCase, ObterDashboardFrequenciaPorAnoUseCase>();
            services.TryAddScoped<IObterDadosDashboardFrequenciaPorDreUseCase, ObterDadosDashboardFrequenciaPorDreUseCase>();
            services.TryAddScoped<IObterDashboardFrequenciaAusenciasPorMotivoUseCase, ObterDashboardFrequenciaAusenciasPorMotivoUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaConselhoClasseUseCase, ExecutarConsolidacaoTurmaConselhoClasseUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaFechamentoUseCase, ExecutarConsolidacaoTurmaFechamentoUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaUseCase, ExecutarConsolidacaoTurmaUseCase>();

            services.TryAddScoped<IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase, ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase>();

            services.TryAddScoped<IObterDashboardInformacoesEscolaresPorMatriculaUseCase, ObterDashboardInformacoesEscolaresPorMatriculaUseCase>();
            services.TryAddScoped<IObterDashboardInformacoesEscolaresPorTurmaUseCase, ObterDashboardInformacoesEscolaresPorTurmaUseCase>();
            services.TryAddScoped<IObterDataConsolidacaoInformacoesEscolaresUseCase, ObterDataConsolidacaoInformacoesEscolaresUseCase>();
            services.TryAddScoped<IObterModalidadeAnoItineranciaProgramaUseCase, ObterModalidadeAnoItineranciaProgramaUseCase>();

            services.TryAddScoped<IAlterarAulaFrequenciaTratarUseCase, AlterarAulaFrequenciaTratarUseCase>();
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

            services.TryAddScoped<IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase, ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase>();
            services.TryAddScoped<ICarregarRegistroFrequenciaAlunosUseCase, CarregarRegistroFrequenciaAlunosUseCase>();


            services.TryAddScoped<IRabbitDeadletterSgpTratarUseCase, RabbitDeadletterSgpTratarUseCase>();

            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosCronUseCase, ConciliacaoFrequenciaTurmasAlunosCronUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosSyncUseCase, ConciliacaoFrequenciaTurmasAlunosSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosBuscarUseCase, ConciliacaoFrequenciaTurmasAlunosBuscarUseCase>();

            services.TryAddScoped<IObterNotasParaAvaliacoesUseCase, ObterNotasParaAvaliacoesUseCase>();
            services.TryAddScoped<IObterNotasParaAvaliacoesListaoUseCase, ObterNotasParaAvaliacoesListaoUseCase>();
            services.TryAddScoped<IObterPeriodosParaConsultaNotasUseCase, ObterPeriodosParaConsultaNotasUseCase>();


            services.TryAddScoped<INotificarAlunosFaltososUseCase, NotificarAlunosFaltososUseCase>();

            // Sincronização de Devolutivas
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaUseCase, ConsolidarDevolutivasPorTurmaUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaInfantilUseCase, ConsolidarDevolutivasPorTurmaInfantilUseCase>();

            // Consolidação de Diarios de Bordo
            services.TryAddScoped<IConsolidarDiariosBordoCarregarUseCase, ConsolidarDiariosBordoCarregarUseCase>();
            services.TryAddScoped<IConsolidarDiariosBordoPorUeTratarUseCase, ConsolidarDiariosBordoPorUeTratarUseCase>();

            // Consolidação de Registros Pedagógicos
            services.TryAddScoped<IConsolidarRegistrosPedagogicosUseCase, ConsolidarRegistrosPedagogicosUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorUeTratarUseCase, ConsolidarRegistrosPedagogicosPorUeTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase, ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase>();
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


            //Aulas automáticas regencia
            services.TryAddScoped<ICarregarUesTurmasRegenciaAulaAutomaticaUseCase, CarregarUesTurmasRegenciaAulaAutomaticaUseCase>();
            services.TryAddScoped<ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase, SincronizarUeTurmaAulaRegenciaAutomaticaUseCase>();
            services.TryAddScoped<ISincronizarAulasRegenciaAutomaticamenteUseCase, SincronizarAulasRegenciaAutomaticamenteUseCase>();

            services.TryAddScoped<IObterNotasPorBimestresUeAlunoTurmaUseCase, ObterNotasPorBimestresUeAlunoTurmaUseCase>();
            services.TryAddScoped<IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase, ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase>();

            // Sincronização de Média dos Registros Individuais
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase, ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisUseCase, ConsolidacaoMediaRegistrosIndividuaisUseCase>();

            // Consolidação Matrícula Turma
            services.TryAddScoped<IRepositorioConsolidacaoMatriculaTurma, RepositorioConsolidacaoMatriculaTurma>();
            services.TryAddScoped<ICarregarDresConsolidacaoMatriculaUseCase, CarregarDresConsolidacaoMatriculaUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase, ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase>();
            services.TryAddScoped<ICarregarMatriculaTurmaUseCase, CarregarMatriculaTurmaUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase, ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase>();

            services.TryAddScoped<IObterTurmaModalidadesPorCodigosUseCase, ObterTurmaModalidadesPorCodigosUseCase>();

            services.TryAddScoped<IObterBimestrePorModalidadeUseCase, ObterBimestrePorModalidadeUseCase>();
            services.TryAddScoped<IObterSituacoesFechamentoUseCase, ObterSituacoesFechamentoUseCase>();
            services.TryAddScoped<IObterSituacoesConselhoClasseUseCase, ObterSituacoesConselhoClasseUseCase>();

            services.TryAddScoped<IRelatorioAcompanhamentoFechamentoUseCase, RelatorioAcompanhamentoFechamentoUseCase>();

            // Sincronização de Acompanhamento de Aprendizagem 
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase>();

            services.TryAddScoped<IListarAtribuicaoEsporadicaUseCase, ListarAtribuicaoEsporadicaUseCase>();
            services.TryAddScoped<IObterPeriodoAtribuicaoPorUeUseCase, ObterPeriodoAtribuicaoPorUeUseCase>();
            services.TryAddScoped<IExcluirAtribuicaoEsporadicaUseCase, ExcluirAtribuicaoEsporadicaUseCase>();


            services.TryAddScoped<IObterAnosLetivosAtribuicaoCJUseCase, ObterAnosLetivosAtribuicaoCJUseCase>();
            services.TryAddScoped<IObterProfessoresTitularesECjsUseCase, ObterProfessoresTitularesECjsUseCase>();
            services.TryAddScoped<ISalvarAtribuicaoCJUseCase, SalvarAtribuicaoCJUseCase>();
            services.TryAddScoped<IListarAtribuicoesCJPorFiltroUseCase, ListarAtribuicoesCJPorFiltroUseCase>();
            services.TryAddScoped<IObterFiltroSemanaUseCase, ObterFiltroSemanaUseCase>();
            // Dashboard Frequencia Aluno
            services.TryAddScoped<IObterDadosDashboardFrequenciaPorAnoTurmaUseCase, ObterDadosDashboardFrequenciaPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterDadosDashboardFrequenciaPorAnoTurmaUseCase, ObterDadosDashboardFrequenciaPorAnoTurmaUseCase>();
            services.TryAddScoped<IObterFrequenciasPorPeriodoUseCase, ObterFrequenciasPorPeriodoUseCase>();

            //  Dashboard Compensação ausência
            services.TryAddScoped<IObterDadosDashboardTotalAusenciasCompensadasUseCase, ObterDadosDashboardTotalAusenciasCompensadasUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDashBoardFrequenciaUseCase, ExecutaConsolidacaoDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase, ExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoMensalDashBoardFrequenciaUseCase, ExecutaConsolidacaoMensalDashBoardFrequenciaUseCase>();

            // Rotas Agendamento Sync
            services.TryAddScoped<IRotasAgendamentoTratarUseCase, RotasAgendamentoTratarUseCase>();
            services.TryAddScoped<IExcluirWorkflowAprovacaoPorIdUseCase, ExcluirWorkflowAprovacaoPorIdUseCase>();
            services.TryAddScoped<IExcluirNotificacoesPorAulaIdUseCase, ExcluirNotificacoesPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirFrequenciaPorAulaIdUseCase, ExcluirFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirPlanoAulaPorAulaIdUseCase, ExcluirPlanoAulaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirAnotacoesFrequenciaPorAulaIdUseCase, ExcluirAnotacoesFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirDiarioBordoPorAulaIdUseCase, ExcluirDiarioBordoPorAulaIdUseCase>();

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

            services.TryAddScoped<IImportarNotaAtividadeAvaliativaGsaUseCase, ImportarNotaAtividadeAvaliativaGsaUseCase>();


            // Dias Letivos
            services.TryAddScoped<IObterDiasLetivosPorUeETurnoUseCase, ObterDiasLetivosPorUeETurnoUseCase>();
            services.TryAddScoped<IRelatorioAtaBimestralUseCase, RelatorioAtaBimestralUseCase>();

            //Componentes curriculares integração
            services.TryAddScoped<IObterComponenteCurricularLancaNotaUseCase, ObterComponenteCurricularLancaNotaUseCase>();

            //Período escolar integração
            services.TryAddScoped<IObterPeriodoEscolarAtualPorTurmaUseCase, ObterPeriodoEscolarAtualPorTurmaUseCase>();

            services.TryAddScoped<INotificacaoFrequencia, NotificacaoFrequenciaUseCase>();
            services.TryAddScoped<IExecutarTipoCalendario, ExecutarTipoCalendarioUseCase>();
            services.TryAddScoped<IExecutarGravarRecorrencia, ExecutarGravarRecorrenciaUseCase>();
            services.TryAddScoped<IGerarNotificacaoAlteracaoLimiteDias, GerarNotificacaoAlteracaoLimiteDiasUseCase>();
            services.TryAddScoped<IVerificarPendenciasFechamentoTurmaDisciplina, VerificarPendenciasFechamentoTurmaDisciplinaUseCase>();
            services.TryAddScoped<IAlterarPeriodosComHierarquiaInferiorFechamento, AlterarPeriodosComHierarquiaInferiorFechamentoUseCase>();
            services.TryAddScoped<IAlterarRecorrenciaEventos, AlterarRecorrenciaEventosUseCase>();

            services.TryAddScoped<IExecutarNotificacaoRegistroFrequenciaUseCase, ExecutarNotificacaoRegistroFrequenciaUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoObjetivosComJuremaUseCase, ExecutarSincronizacaoObjetivosComJuremaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFaltososBimestreUseCase, ExecutarNotificacaoAlunosFaltososBimestreUseCase>();
            services.TryAddScoped<IExecutarTratamentoNotificacoesNiveisCargosUseCase, ExecutarTratamentoNotificacoesNiveisCargosUseCase>();
            services.TryAddScoped<IExecutarSincronismoComponentesCurricularesUseCase, ExecutarSincronismoComponentesCurricularesUseCase>();
            services.TryAddScoped<IExecutaSincronismoComponentesCurricularesEolUseCase, ExecutaSincronismoComponentesCurricularesEolUseCase>();
            services.TryAddScoped<IExecutarSyncGeralGoogleClassroomUseCase, ExecutarSyncGeralGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutaSyncGsaGoogleClassroomUseCase, ExecutaSyncGsaGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutarSyncSerapEstudantesProvasUseCase, ExecutarSyncSerapEstudantesProvasUseCase>();
            services.TryAddScoped<IPendenciasGeraisUseCase, PendenciasGeraisUseCase>();

            //Remoção da Atribuição de Pendencia Usuário
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUseCase, RemoverAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeUseCase, RemoverAtribuicaoPendenciasUsuariosUeUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase, RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase>();

            services.TryAddScoped<IObterDatasDiarioBordoPorPeriodoUseCase, ObterDatasDiarioBordoPorPeriodoUseCase>();
            services.TryAddScoped<IInserirAlterarDiarioBordoUseCase, InserirAlterarDiarioBordoUseCase>();

            services.TryAddScoped<IListarFechamentoTurmaBimestreUseCase, ListarFechamentoTurmaBimestreUseCase>();
        }
    }
}