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
               config.AddMap(new ObjetivoAprendizagemPlanoMap());
               config.AddMap(new DisciplinaPlanoMap());
               config.AddMap(new ComponenteCurricularMap());
               config.ForDommel();
           });
        }
    }
}