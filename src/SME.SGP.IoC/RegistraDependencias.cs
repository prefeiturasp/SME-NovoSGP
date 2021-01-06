using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.CasosDeUso.Abrangencia;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Anos;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Anos;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
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

namespace SME.SGP.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            services.TryAddScoped<HangfireMediator>();

            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
            RegistrarServicos(services);
            RegistrarCasosDeUso(services);
            RegistrarMapeamentos.Registrar();
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
            services.TryAddScoped<IComandoFrequencia, ComandoFrequencia>();
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
            services.TryAddScoped<IComandosFechamentoAluno, ComandosFechamentoAluno>();
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
            services.TryAddScoped<IConsultaGrupoComunicacao, ConsultaGrupoComunicacao>();
            services.TryAddScoped<IConsultaComunicado, ConsultaComunicado>();
            services.TryAddScoped<IConsultasConselhoClasseRecomendacao, ConsultasConselhoClasseRecomendacao>();
            services.TryAddScoped<IConsultasRelatorioSemestralTurmaPAP, ConsultasRelatorioSemestralTurmaPAP>();
            services.TryAddScoped<IConsultasRelatorioSemestralPAPAluno, ConsultasRelatorioSemestralPAPAluno>();
            services.TryAddScoped<IConsultasRelatorioSemestralPAPAlunoSecao, ConsultasRelatorioSemestralPAPAlunoSecao>();
            services.TryAddScoped<IConsultasSecaoRelatorioSemestralPAP, ConsultasSecaoRelatorioSemestralPAP>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultaRecuperacaoParalelaPeriodo, ConsultaRecuperacaoParalelaPeriodo>();
            services.TryAddScoped<IConsultasPlanoAnualTerritorioSaber, ConsultasPlanoAnualTerritorioSaber>();
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();
            services.TryAddScoped<ISgpContext, SgpContext>();
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
            services.TryAddScoped<IRepositorioComponenteCurricular, RepositorioComponenteCurricular>();
            services.TryAddScoped<IRepositorioSupervisorEscolaDre, RepositorioSupervisorEscolaDre>();
            services.TryAddScoped<IRepositorioNotificacao, RepositorioNotificacao>();
            services.TryAddScoped<IRepositorioWorkflowAprovacao, RepositorioWorkflowAprovacao>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivelNotificacao, RepositorioWorkflowAprovaNivelNotificacao>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivel, RepositorioWorkflowAprovacaoNivel>();
            services.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddScoped<IRepositorioWorkflowAprovacaoNivelUsuario, RepositorioWorkflowAprovacaoNivelUsuario>();
            services.TryAddScoped<IRepositorioPrioridadePerfil, RepositorioPrioridadePerfil>();
            services.TryAddScoped<IRepositorioConfiguracaoEmail, RepositorioConfiguracaoEmail>();
            services.TryAddScoped<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.TryAddScoped<IRepositorioTipoCalendario, RepositorioTipoCalendario>();
            services.TryAddScoped<IRepositorioFeriadoCalendario, RepositorioFeriadoCalendario>();
            services.TryAddScoped<IRepositorioPeriodoEscolar, RepositorioPeriodoEscolar>();
            services.TryAddScoped<IRepositorioEvento, RepositorioEvento>();
            services.TryAddScoped<IRepositorioEventoTipo, RepositorioEventoTipo>();
            services.TryAddScoped<IRepositorioParametrosSistema, RepositorioParametrosSistema>();
            services.TryAddScoped<IRepositorioAula, RepositorioAula>();
            services.TryAddScoped<IRepositorioGrade, RepositorioGrade>();
            services.TryAddScoped<IRepositorioGradeFiltro, RepositorioGradeFiltro>();
            services.TryAddScoped<IRepositorioGradeDisciplina, RepositorioGradeDisciplina>();
            services.TryAddScoped<IRepositorioFrequencia, RepositorioFrequencia>();
            services.TryAddScoped<IRepositorioRegistroAusenciaAluno, RepositorioRegistroAusenciaAluno>();
            services.TryAddScoped<IRepositorioAtribuicaoEsporadica, RepositorioAtribuicaoEsporadica>();
            services.TryAddScoped<IRepositorioFrequenciaAlunoDisciplinaPeriodo, RepositorioFrequenciaAlunoDisciplinaPeriodo>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativa, RepositorioAtividadeAvaliativa>();
            services.TryAddScoped<IRepositorioTipoAvaliacao, RepositorioTipoAvaliacao>();
            services.TryAddScoped<IRepositorioPlanoAula, RepositorioPlanoAula>();
            services.TryAddScoped<IRepositorioObjetivoAprendizagemAula, RepositorioObjetivoAprendizagemAula>();
            services.TryAddScoped<IRepositorioAtribuicaoCJ, RepositorioAtribuicaoCJ>();
            services.TryAddScoped<IRepositorioDre, RepositorioDre>();
            services.TryAddScoped<IRepositorioUe, RepositorioUe>();
            services.TryAddScoped<IRepositorioTurma, RepositorioTurma>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativaRegencia, RepositorioAtividadeAvaliativaRegencia>();
            services.TryAddScoped<IRepositorioNotasConceitos, RepositorioNotasConceitos>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativa, RepositorioAtividadeAvaliativa>();
            services.TryAddScoped<IRepositorioConceito, RepositorioConceito>();
            services.TryAddScoped<IRepositorioNotaParametro, RepositorioNotaParametro>();
            services.TryAddScoped<IRepositorioNotaTipoValor, RepositorioNotaTipoValor>();
            services.TryAddScoped<IRepositorioNotificacaoFrequencia, RepositorioNotificacaoFrequencia>();
            services.TryAddScoped<IRepositorioEventoMatricula, RepositorioEventoMatricula>();
            services.TryAddScoped<IRepositorioAulaPrevista, RepositorioAulaPrevista>();
            services.TryAddScoped<IRepositorioNotificacaoAulaPrevista, RepositorioNotificacaoAulaPrevista>();
            services.TryAddScoped<IRepositorioAulaPrevistaBimestre, RepositorioAulaPrevistaBimestre>();
            services.TryAddScoped<IRepositorioRegistroPoa, RepositorioRegistroPoa>();
            services.TryAddScoped<IRepositorioAtividadeAvaliativaDisciplina, RepositorioAtividadeAvaliativaDisciplina>();
            services.TryAddScoped<IRepositorioFechamentoReabertura, RepositorioFechamentoReabertura>();
            services.TryAddScoped<IRepositorioCompensacaoAusencia, RepositorioCompensacaoAusencia>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaAluno, RepositorioCompensacaoAusenciaAluno>();
            services.TryAddScoped<IRepositorioCompensacaoAusenciaDisciplinaRegencia, RepositorioCompensacaoAusenciaDisciplinaRegencia>();
            services.TryAddScoped<IRepositorioProcessoExecutando, RepositorioProcessoExecutando>();
            services.TryAddScoped<IRepositorioPeriodoFechamento, RepositorioPeriodoFechamento>();
            services.TryAddScoped<IRepositorioPeriodoFechamentoBimestre, RepositorioPeriodoFechamentoBimestre>();
            services.TryAddScoped<IRepositorioNotificacaoCompensacaoAusencia, RepositorioNotificacaoCompensacaoAusencia>();
            services.TryAddScoped<IRepositorioEventoFechamento, RepositorioEventoFechamento>();
            services.TryAddScoped<IRepositorioFechamentoTurmaDisciplina, RepositorioFechamentoTurmaDisciplina>();
            services.TryAddScoped<IRepositorioFechamentoNota, RepositorioFechamentoNota>();
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
            services.TryAddScoped<IRepositorioFechamentoTurma, RepositorioFechamentoTurma>();
            services.TryAddScoped<IRepositorioConselhoClasse, RepositorioConselhoClasse>();
            services.TryAddScoped<IRepositorioConselhoClasseAluno, RepositorioConselhoClasseAluno>();
            services.TryAddScoped<IRepositorioConselhoClasseNota, RepositorioConselhoClasseNota>();
            services.TryAddScoped<IRepositorioGrupoComunicacao, RepositorioGrupoComunicacao>();
            services.TryAddScoped<IRepositorioComunicado, RepositorioComunicado>();
            services.TryAddScoped<IRepositorioWfAprovacaoNotaFechamento, RepositorioWfAprovacaoNotaFechamento>();
            services.TryAddScoped<IRepositorioComunicadoGrupo, RepositorioComunicacaoGrupo>();
            services.TryAddScoped<IRepositorioConselhoClasseRecomendacao, RepositorioConselhoClasseRecomendacao>();
            services.TryAddScoped<IRepositorioCicloEnsino, RepositorioCicloEnsino>();
            services.TryAddScoped<IRepositorioTipoEscola, RepositorioTipoEscola>();
            services.TryAddScoped<IRepositorioRelatorioSemestralTurmaPAP, RepositorioRelatorioSemestralTurmaPAP>();
            services.TryAddScoped<IRepositorioRelatorioSemestralPAPAluno, RepositorioRelatorioSemestralPAPAluno>();
            services.TryAddScoped<IRepositorioRelatorioSemestralPAPAlunoSecao, RepositorioRelatorioSemestralPAPAlunoSecao>();
            services.TryAddScoped<IRepositorioSecaoRelatorioSemestralPAP, RepositorioSecaoRelatorioSemestralPAP>();
            services.TryAddScoped<IRepositorioObjetivoAprendizagem, RepositorioObjetivoAprendizagem>();
            services.TryAddScoped<IRepositorioConselhoClasseParecerConclusivo, RepositorioConselhoClasseParecerConclusivo>();
            services.TryAddScoped<IRepositorioPlanoAnualTerritorioSaber, RepositorioPlanoAnualTerritorioSaber>();
            services.TryAddScoped<IRepositorioCorrelacaoRelatorio, RepositorioCorrelacaoRelatorio>();
            services.TryAddScoped<IRepositorioCorrelacaoRelatorioJasper, RepositorioRelatorioCorrelacaoJasper>();
            //services.TryAddScoped<IRepositorioTestePostgre, RepositorioTestePostgre>();
            services.TryAddScoped<IRepositorioFechamentoReaberturaBimestre, RepositorioFechamentoReaberturaBimestre>();
            services.TryAddScoped<IRepositorioHistoricoReinicioSenha, RepositorioHistoricoReinicioSenha>();
            services.TryAddScoped<IRepositorioComunicadoAluno, RepositorioComunicadoAluno>();
            services.TryAddScoped<IRepositorioComunicadoTurma, RepositorioComunicadoTurma>();
            services.TryAddScoped<IRepositorioDiarioBordo, RepositorioDiarioBordo>();
            services.TryAddScoped<IRepositorioDevolutiva, RepositorioDevolutiva>();
            services.TryAddScoped<IRepositorioAnoEscolar, RepositorioAnoEscolar>();
            services.TryAddScoped<IRepositorioCartaIntencoes, RepositorioCartaIntencoes>();
            services.TryAddScoped<IRepositorioDiarioBordoObservacao, RepositorioDiarioBordoObservacao>();
            services.TryAddScoped<IRepositorioAnotacaoFrequenciaAluno, RepositorioAnotacaoFrequenciaAluno>();
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



            // Pendencias
            services.TryAddScoped<IRepositorioPendencia, RepositorioPendencia>();
            services.TryAddScoped<IRepositorioPendenciaFechamento, RepositorioPendenciaFechamento>();
            services.TryAddScoped<IRepositorioPendenciaAula, RepositorioPendenciaAula>();
            services.TryAddScoped<IRepositorioPendenciaUsuario, RepositorioPendenciaUsuario>();
            services.TryAddScoped<IRepositorioPendenciaCalendarioUe, RepositorioPendenciaCalendarioUe>();
            services.TryAddScoped<IRepositorioPendenciaParametroEvento, RepositorioPendenciaParametroEvento>();
            services.TryAddScoped<IRepositorioPendenciaProfessor, RepositorioPendenciaProfessor>();

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
            services.TryAddScoped<IServicoDiaLetivo, ServicoDiaLetivo>();
            services.TryAddScoped<IServicoLog, ServicoLog>();
            services.TryAddScoped<IServicoFrequencia, ServicoFrequencia>();
            services.TryAddScoped<IServicoAtribuicaoEsporadica, ServicoAtribuicaoEsporadica>();
            services.TryAddScoped<IServicoCalculoFrequencia, ServicoCalculoFrequencia>();
            services.TryAddScoped<IServicoDeNotasConceitos, ServicoDeNotasConceitos>();
            services.TryAddScoped<IServicoNotificacaoFrequencia, ServicoNotificacaoFrequencia>();
            services.TryAddScoped<IServicoNotificacaoAulaPrevista, ServicoNotificacaoAulaPrevista>();
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
            services.TryAddScoped<IServicoCalculoParecerConclusivo, ServicoCalculoParecerConclusivo>();
            services.TryAddScoped<IServicoObjetivosAprendizagem, ServicoObjetivosAprendizagem>();
            services.TryAddScoped<IServicoFila, FilaRabbit>();

        }

        private static void RegistrarCasosDeUso(IServiceCollection services){
            services.TryAddScoped<IObterUltimaVersaoUseCase, ObterUltimaVersaoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseAlunoUseCase, ImpressaoConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IImpressaoConselhoClasseTurmaUseCase, ImpressaoConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IReceberRelatorioProntoUseCase, ReceberRelatorioProntoUseCase>();
            services.TryAddScoped<IBoletimUseCase, BoletimUseCase>();
            services.TryAddScoped<IObterListaAlunosDaTurmaUseCase, ObterListaAlunosDaTurmaUseCase>();
            services.TryAddScoped<IReceberDadosDownloadRelatorioUseCase, ReceberDadosDownloadRelatorioUseCase>();
            services.TryAddScoped<IRelatorioConselhoClasseAtaFinalUseCase, RelatorioConselhoClasseAtaFinalUseCase>();
            services.TryAddScoped<IGamesUseCase, GamesUseCase>();
            services.TryAddScoped<IReiniciarSenhaUseCase, ReiniciarSenhaUseCase>();
            services.TryAddScoped<IInserirAulaUseCase, InserirAulaUseCase>();
            services.TryAddScoped<IAlterarAulaUseCase, AlterarAulaUseCase>();
            services.TryAddScoped<IExcluirAulaUseCase, ExcluirAulaUseCase>();
            services.TryAddScoped<IPodeCadastrarAulaUseCase, PodeCadastrarAulaUseCase>();
            services.TryAddScoped<IObterFuncionariosUseCase, ObterFuncionariosUseCase>();
            services.TryAddScoped<IExcluirAulaRecorrenteUseCase, ExcluirAulaRecorrenteUseCase>();
            services.TryAddScoped<IInserirAulaRecorrenteUseCase, InserirAulaRecorrenteUseCase>();
            services.TryAddScoped<IAlterarAulaRecorrenteUseCase, AlterarAulaRecorrenteUseCase>();
            services.TryAddScoped<INotificarUsuarioUseCase, NotificarUsuarioUseCase>();
            services.TryAddScoped<IReceberRelatorioComErroUseCase, ReceberRelatorioComErroUseCase>();
            services.TryAddScoped<IUsuarioPossuiAbrangenciaAdmUseCase, UsuarioPossuiAbrangenciaAdmUseCase>();
            services.TryAddScoped<IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase, ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase>();
            services.TryAddScoped<IHistoricoEscolarUseCase, HistoricoEscolarUseCase>();
            services.TryAddScoped<IObterAlunosPorCodigoEolNomeUseCase, ObterAlunosPorCodigoEolNomeUseCase>();
            services.TryAddScoped<IGerarRelatorioFaltasFrequenciaUseCase, GerarRelatorioFaltasFrequenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosDresPorAbrangenciaUseCase, ObterFiltroRelatoriosDresPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosUesPorAbrangenciaUseCase, ObterFiltroRelatoriosUesPorAbrangenciaUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosModalidadesPorUeUseCase, ObterFiltroRelatoriosModalidadesPorUeUseCase>();
            services.TryAddScoped<IRelatorioPendenciasFechamentoUseCase, RelatorioPendenciasFechamentoUseCase>();
            services.TryAddScoped<IObterAnotacaoFrequenciaAlunoUseCase, ObterAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase, ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeUseCase>();
            services.TryAddScoped<ICopiarCodigoCorrelacaoUseCase, CopiarCodigoCorrelacaoUseCase>();
            services.TryAddScoped<IInserirCodigoCorrelacaoUseCase, InserirCodigoCorrelacaoUseCase>();
            services.TryAddScoped<IObterDiarioBordoUseCase, ObterDiarioBordoUseCase>();
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

            services.TryAddScoped<IExcluirDevolutivaUseCase, ExcluirDevolutivaUseCase>();
            services.TryAddScoped<IObterListaDevolutivasPorTurmaComponenteUseCase, ObterListaDevolutivasPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterDiariosBordoPorDevolutiva, ObterDiariosBordoPorDevolutiva>();
            services.TryAddScoped<IObterDevolutivaPorIdUseCase, ObterDevolutivaPorIdUseCase>();
            services.TryAddScoped<IObterDiariosDeBordoPorPeriodoUseCase, ObterDiariosDeBordoPorPeriodoUseCase>();
            services.TryAddScoped<IObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase, ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteUseCase>();
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
            services.TryAddScoped<ISincronizarAulasInfantilUseCase, SincronizarAulasInfantilUseCase>();
            services.TryAddScoped<INotificarExclusaoAulaComFrequenciaUseCase, NotificarExclusaoAulaComFrequenciaUseCase>();
            services.TryAddScoped<ISalvarAnotacaoFrequenciaAlunoUseCase, SalvarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IAlterarAnotacaoFrequenciaAlunoUseCase, AlterarAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IExcluirAnotacaoFrequenciaAlunoUseCase, ExcluirAnotacaoFrequenciaAlunoUseCase>();
            services.TryAddScoped<IObterMotivosAusenciaUseCase, ObterMotivosAusenciaUseCase>();

            services.TryAddScoped<IObterDashBoardUseCase, ObterDashBoardUseCase>();
            services.TryAddScoped<IInserirDevolutivaUseCase, InserirDevolutivaUseCase>();
            services.TryAddScoped<IAlterarDevolutivaUseCase, AlterarDevolutivaUseCase>();
            services.TryAddScoped<IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase, ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase>();
            services.TryAddScoped<IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase, ObterUltimasNotificacoesNaoLidasPorUsuarioUseCase>();

            //Carta Intenções Observacao
            services.TryAddScoped<IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase, ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase>();
            services.TryAddScoped<ISalvarCartaIntencoesObservacaoUseCase, SalvarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IAlterarCartaIntencoesObservacaoUseCase, AlterarCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirCartaIntencoesObservacaoUseCase, ExcluirCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<ISalvarNotificacaoCartaIntencoesObservacaoUseCase, SalvarNotificacaoCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirNotificacaoCartaIntencoesObservacaoUseCase, ExcluirNotificacaoCartaIntencoesObservacaoUseCase>();

            // Componentes Curriculares
            services.TryAddScoped<IObterComponentesCurricularesPorTurmaECodigoUeUseCase, ObterComponentesCurricularesPorTurmaECodigoUeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorUeAnosModalidadeUseCase, ObterComponentesCurricularesPorUeAnosModalidadeUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorAnoEscolarUseCase, ObterComponentesCurricularesPorAnoEscolarUseCase>();
            services.TryAddScoped<IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase, ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase>();

            // Conselho de Classe
            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterPareceresConclusivosUseCase, ObterPareceresConclusivosUseCase>();
            services.TryAddScoped<IAtualizarSituacaoConselhoClasseUseCase, AtualizarSituacaoConselhoClasseUseCase>();

            //Notificacao Devolutivoa
            services.TryAddScoped<ISalvarNotificacaoDevolutivaUseCase, SalvarNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDevolutivaUseCase, ExcluirNotificacaoDevolutivaUseCase>();


            services.TryAddScoped<IObterUsuarioPorCpfUseCase, ObterUsuarioPorCpfUseCase>();
            services.TryAddScoped<ISolicitarReiniciarSenhaEscolaAquiUseCase, SolicitarReiniciarSenhaEscolaAquiUseCase>();
            services.TryAddScoped<INotificarDiarioBordoObservacaoUseCase, NotificarDiarioBordoObservacaoUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDiarioBordoUseCase, ExcluirNotificacaoDiarioBordoUseCase>();
            services.TryAddScoped<IObterAnosLetivosPAPUseCase, ObterAnosLetivosPAPUseCase>();

            services.TryAddScoped<IBuscarTiposCalendarioPorDescricaoUseCase, BuscarTiposCalendarioPorDescricaoUseCase>();
            services.TryAddScoped<IPendenciaAulaUseCase, PendenciaAulaUseCase>();
            services.TryAddScoped<IExecutaPendenciaAulaUseCase, ExecutaPendenciaAulaUseCase>();

            services.TryAddScoped<ISalvarPlanejamentoAnualUseCase, SalvarPlanejamentoAnualUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase, ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase, ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase>();
            services.TryAddScoped<IObterPlanejamentoAnualPorTurmaComponenteUseCase, ObterPlanejamentoAnualPorTurmaComponenteUseCase>();
            services.TryAddScoped<IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase, ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase>();

            // Armazenamento de arquivos
            services.TryAddScoped<IUploadDeArquivoUseCase, UploadDeArquivoUseCase>();
            services.TryAddScoped<IDownloadDeArquivoUseCase, DownloadDeArquivoUseCase>();
            services.TryAddScoped<IExcluirArquivoUseCase, ExcluirArquivoUseCase>();


            // Grade Curricular
            services.TryAddScoped<IRelatorioControleGradeUseCase, RelatorioControleGradeUseCase>();

            //Sincronismo CC Eol
            services.TryAddScoped<IListarComponentesCurricularesEolUseCase, ListarComponentesCurricularesEolUseCase>();
            services.TryAddScoped<ISincronizarComponentesCurricularesUseCase, SincronizarComponentesCurricularesUseCase>();
            services.TryAddScoped<IExecutaSincronismoComponentesCurricularesEolUseCase, ExecutaSincronismoComponentesCurricularesEolUseCase>();

            services.TryAddScoped<IObterComponentesCurricularesRegenciaPorTurmaUseCase, ObterComponentesCurricularesRegenciaPorTurmaUseCase>();
            services.TryAddScoped<IObterPeriodoEscolarPorTurmaUseCase, ObterPeriodoEscolarPorTurmaUseCase>();

            services.TryAddScoped<IRelatorioResumoPAPUseCase, RelatorioResumoPAPUseCase>();
            services.TryAddScoped<IRelatorioGraficoPAPUseCase, RelatorioGraficoPAPUseCase>();

            //Objetivo Curricular
            services.TryAddScoped<IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase, ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase>();

            services.TryAddScoped<IObterBimestresComConselhoClasseTurmaUseCase, ObterBimestresComConselhoClasseTurmaUseCase>();
            services.TryAddScoped<IObterObjetivosPorDisciplinaUseCase, ObterObjetivosPorDisciplinaUseCase>();

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
            services.TryAddScoped<IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase, ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase>();
            services.TryAddScoped<IListarEventosPorCalendarioUseCase, ListarEventosPorCalendarioUseCase>();
            services.TryAddScoped<IObterDadosDeLeituraDeComunicadosUseCase, ObterDadosDeLeituraDeComunicadosUseCase>();


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

            


            // Dias Letivos
            services.TryAddScoped<IObterDiasLetivosPorCalendarioUseCase, ObterDiasLetivosPorCalendarioUseCase>();

            // Notificações
            services.TryAddScoped<IExecutaNotificacaoAndamentoFechamentoUseCase, ExecutaNotificacaoAndamentoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoAndamentoFechamentoUseCase, NotificacaoAndamentoFechamentoUseCase>();
            services.TryAddScoped<IExecutaNotificacaoUeFechamentosInsuficientesUseCase, ExecutaNotificacaoUeFechamentosInsuficientesUseCase>();
            services.TryAddScoped<INotificacaoUeFechamentosInsuficientesUseCase, NotificacaoUeFechamentosInsuficientesUseCase>();
            services.TryAddScoped<IExecutaNotificacaoPeriodoFechamentoUseCase, ExecutaNotificacaoPeriodoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoPeriodoFechamentoUseCase, NotificacaoPeriodoFechamentoUseCase>();
            services.TryAddScoped<IExecutaNotificacaoInicioFimPeriodoFechamentoUseCase, ExecutaNotificacaoInicioFimPeriodoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoInicioFimPeriodoFechamentoUseCase, NotificacaoInicioFimPeriodoFechamentoUseCase>();


            // Dias Letivos
            services.TryAddScoped<IObterDiasLetivosPorCalendarioUseCase, ObterDiasLetivosPorCalendarioUseCase>();


            // Notificação
            services.TryAddScoped<IExecutaNotificacaoFrequenciaUeUseCase, ExecutaNotificacaoFrequenciaUeUseCase>();
            services.TryAddScoped<INotificacaoFrequenciaUeUseCase, NotificacaoFrequenciaUeUseCase>();

            //Notificação Resultado Insatisfatorio 
            services.TryAddScoped<IExecutaNotificacaoResultadoInsatisfatorioUseCase, ExecutaNotificacaoResultadoInsatisfatorioUseCase>();
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();

            // Pendencias
            services.TryAddScoped<IObterPendenciasUseCase, ObterPendenciasUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisUseCase, ExecutaVerificacaoPendenciasGeraisUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAulaUseCase, ExecutarExclusaoPendenciasAulaUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciaDiasLetivosInsuficientes, ExecutarExclusaoPendenciaDiasLetivosInsuficientes>();
            services.TryAddScoped<IExecutarExclusaoPendenciaParametroEvento, ExecutarExclusaoPendenciaParametroEvento>();
            services.TryAddScoped<IPendenciasGeraisUseCase, PendenciasGeraisUseCase>();

            // Pendencias Professor
            services.TryAddScoped<IExecutaPendenciasProfessorAvaliacaoUseCase, ExecutaPendenciasProfessorAvaliacaoUseCase>();
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase, ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase, ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase>();

            // Pendencias Ausencia Fechamento
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase, ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase>();
            services.TryAddScoped<IExecutaPendenciasAusenciaFechamentoUseCase, ExecutaPendenciasAusenciaFechamentoUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaFechamentoUseCase, ExecutarExclusaoPendenciasAusenciaFechamentoUseCase>();

            //Notificação Resultado Insatisfatorio 
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();

            services.TryAddScoped<IExecutaNotificacaoReuniaoPedagogicaUseCase, ExecutaNotificacaoReuniaoPedagogicaUseCase>();
            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();


            // Plano Aula
            services.TryAddScoped<IObterPlanoAulaUseCase, ObterPlanoAulaUseCase>();
            services.TryAddScoped<IExcluirPlanoAulaUseCase, ExcluirPlanoAulaUseCase>();
            services.TryAddScoped<IMigrarPlanoAulaUseCase, MigrarPlanoAulaUseCase>();
            services.TryAddScoped<ISalvarPlanoAulaUseCase, SalvarPlanoAulaUseCase>();
            
            //Editor
            services.TryAddScoped<IUploadArquivoEditorUseCase, UploadArquivoEditorUseCase>();

            // Relatórios
            services.TryAddScoped<IRelatorioPlanoAulaUseCase, RelatorioPlanoAulaUseCase>();
            services.TryAddScoped<IRelatorioUsuariosUseCase, RelatorioUsuariosUseCase>();

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

            services.TryAddScoped<IUsuarioPossuiAbrangenciaAcessoSondagemUseCase, UsuarioPossuiAbrangenciaAcessoSondagemUseCase>();

            services.TryAddScoped<ITrataNotificacoesNiveisCargosUseCase, TrataNotificacoesNiveisCargosUseCase>();
            services.TryAddScoped<IExecutaTrataNotificacoesNiveisCargosUseCase, ExecutaTrataNotificacoesNiveisCargosUseCase>();

            services.TryAddScoped<IRelatorioAEAdesaoUseCase, RelatorioAEAdesaoUseCase>();

            services.TryAddScoped<IRelatorioLeituraComunicadosUseCase, RelatorioLeituraComunicadosUseCase>();
            services.TryAddScoped<IRelatorioPlanejamentoDiarioUseCase, RelatorioPlanejamentoDiarioUseCase>();

            services.TryAddScoped<IRemoveConexaoIdleUseCase, RemoveConexaoIdleUseCase>();

            services.TryAddScoped<IAlterarRegistroIndividualUseCase, AlterarRegistroIndividualUseCase>();
            services.TryAddScoped<IInserirRegistroIndividualUseCase, InserirRegistroIndividualUseCase>();
            services.TryAddScoped<IExcluirRegistroIndividualUseCase, ExcluirRegistroIndividualUseCase>();
            services.TryAddScoped<IListarAlunosDaTurmaUseCase, ListarAlunosDaTurmaUseCase>();
            services.TryAddScoped<IObterRegistroIndividualPorAlunoDataUseCase, ObterRegistroIndividualPorAlunoDataUseCase>();
            services.TryAddScoped<IObterRegistroIndividualUseCase, ObterRegistroIndividualUseCase>();
            services.TryAddScoped<IObterRegistrosIndividuaisPorAlunoPeriodoUseCase, ObterRegistrosIndividuaisPorAlunoPeriodoUseCase>();

            services.TryAddScoped<IListarOcorrenciasUseCase, ListarOcorrenciasUseCase>();
            services.TryAddScoped<IListarTiposOcorrenciaUseCase, ListarTiposOcorrenciaUseCase>();
            services.TryAddScoped<IObterOcorrenciaUseCase, ObterOcorrenciaUseCase>();
            services.TryAddScoped<IAlterarOcorrenciaUseCase, AlterarOcorrenciaUseCase>();
            services.TryAddScoped<IExcluirOcorrenciaUseCase, ExcluirOcorrenciaUseCase>();
            services.TryAddScoped<IInserirOcorrenciaUseCase, InserirOcorrenciaUseCase>();
        }
    }
}