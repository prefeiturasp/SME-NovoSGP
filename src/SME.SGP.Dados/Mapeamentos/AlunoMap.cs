using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AlunoMap : DommelEntityMap<Aluno>
    {
        public AlunoMap()
        {
            ToTable("tabela_aluno");
            //Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.Nome).ToColumn("nome_aluno", caseSensitive: true);
        }
    }
}