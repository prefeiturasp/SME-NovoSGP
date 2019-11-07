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
               config.ForDommel();
           });
        }
    }
}