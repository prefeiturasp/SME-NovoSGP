using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace SME.SGP.Dados.Mapeamentos
{
    public static class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
           {
               config.AddMap(new PlanoCicloMap());
               config.AddMap(new ObjetivoDesenvolvimentoMap());
               config.AddMap(new ObjetivoDesenvolvimentoPlanoMap());
               config.AddMap(new MatrizSaberMap());
               config.AddMap(new MatrizSaberPlanoMap());
               config.AddMap(new AuditoriaMap());
               config.AddMap(new CicloMap());
               config.AddMap(new PlanoAnualMap());
               config.AddMap(new PeriodoEscolarMap());
               config.AddMap(new ObjetivoAprendizagemPlanoMap());
               config.AddMap(new DisciplinaPlanoMap());
               config.AddMap(new ComponenteCurricularMap());
               config.AddMap(new SupervisorEscolaDreMap());
               config.AddMap(new NotificacaoMap());
               config.AddMap(new WorkflowAprovacaoMap());
               config.AddMap(new WorkflowAprovacaoNivelMap());
               config.AddMap(new WorkflowAprovacaoNivelNotificacaoMap());
               config.AddMap(new WorkflowAprovacaoNivelUsuarioMap());
               config.AddMap(new UsuarioMap());
               config.AddMap(new PrioridadePerfilMap());
               config.AddMap(new ConfiguracaoEmailMap());
               config.AddMap(new TipoCalendarioMap());
               config.AddMap(new FeriadoCalendarioMap());
               config.AddMap(new EventoMap());
               config.AddMap(new EventoTipoMap());
               config.AddMap(new AulaMap());
               config.AddMap(new GradeMap());
               config.AddMap(new GradeFiltroMap());
               config.AddMap(new GradeDisciplinaMap());
               config.AddMap(new RegistroFrequenciaMap());
               config.AddMap(new RegistroAusenciaAlunoMap());
               config.AddMap(new PlanoAulaMap());
               config.AddMap(new ObjetivoAprendizagemAulaMap());
               config.AddMap(new AtribuicaoEsporadicaMap());
               config.AddMap(new AtividadeAvaliativaMap());
               config.AddMap(new TipoAvaliacaoMap());
               config.AddMap(new AtribuicaoCJMap());
               config.AddMap(new DreMap());
               config.AddMap(new UeMap());
               config.AddMap(new TurmaMap());
               config.AddMap(new AbrangenciaMap());
               config.AddMap(new FrequenciaAlunoMap());
               config.AddMap(new AtividadeAvaliativaRegenciaMap());
               config.AddMap(new NotificacaoFrequenciaMap());
               config.AddMap(new EventoMatriculaMap());
               config.AddMap(new NotaConceitoMap());
               config.AddMap(new NotaTipoValorMap());
               config.AddMap(new NotaParametroMap());
               config.AddMap(new ConceitoValorMap());
               config.AddMap(new NotaConceitoCicloParametroMap());
               config.AddMap(new AulaPrevistaMap());
               config.AddMap(new NotificacaoAulaPrevistaMap());
               config.AddMap(new AulaPrevistaBimestreMap());
               config.AddMap(new RegistroPoaMap());
               config.AddMap(new AtividadeAvaliativaDisciplinaMap());
               config.AddMap(new FechamentoReaberturaMap());
               config.AddMap(new FechamentoReaberturaBimestreMap());
               config.AddMap(new FechamentoReaberturaNotificacaoMap());
               config.AddMap(new CompensacaoAusenciaMap());
               config.AddMap(new CompensacaoAusenciaAlunoMap());
               config.AddMap(new CompensacaoAusenciaDisciplinaRegenciaMap());
               config.AddMap(new ProcessoExecutandoMap());
               config.AddMap(new PeriodoFechamentoMap());
               config.AddMap(new PeriodoFechamentoBimestreMap());
               config.AddMap(new NotificacaoCompensacaoAusenciaMap());
               config.AddMap(new EventoFechamentoMap());
               config.AddMap(new FechamentoTurmaDisciplinaMap());
               config.AddMap(new NotaConceitoBimestreMap());
               config.AddMap(new RecuperacaoParalelaMap());
               config.AddMap(new RecuperacaoParalelaPeriodoMap());
               config.AddMap(new RecuperacaoParalelaPeriodoObjetivoRespostaMap());
               config.AddMap(new EixoMap());
               config.AddMap(new ObjetivoaMap());
               config.AddMap(new RespostaMap());
               config.AddMap(new NotificacaoAulaMap());
               config.AddMap(new FechamentoFinalMap());
               config.AddMap(new HistoricoEmailUsuarioMap());
               config.AddMap(new PendenciaMap());
               config.AddMap(new PendenciaFechamentoMap());
               config.AddMap(new SinteseValorMap());
               config.ForDommel();
           });
        }
    }
}