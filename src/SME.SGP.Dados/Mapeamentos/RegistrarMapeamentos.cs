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
               config.AddMap(new AlunoMap());
               config.AddMap(new ProfessorMap());
               config.ForDommel();
           });
        }
    }
}