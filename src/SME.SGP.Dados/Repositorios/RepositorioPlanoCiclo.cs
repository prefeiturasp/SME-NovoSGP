using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoCiclo : RepositorioBase<PlanoCiclo>, IRepositorioPlanoCiclo
    {
        public RepositorioPlanoCiclo(SgpContext conexao) : base(conexao)
        {
        }

        public override PlanoCiclo ObterPorId(long id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("select msp.id, msp.matriz_id, msp.plano_id, m.*\n");
            query.Append("from \n");
            query.Append("	plano_ciclo p \n");
            query.Append("inner join matriz_saber_plano msp on \n");
            query.Append("	msp.plano_id = p.id \n");
            query.Append("inner join matriz_saber m on \n");
            query.Append("	msp.matriz_id = m.id ");
            query.Append("where p.id = @Id");

            var matrizes = database.Connection().Query<PlanoCiclo, MatrizSaber, MatrizSaberPlano, PlanoCiclo>(query.ToString(),
                   map: (plano, matriz, matrizSaber) =>
                   {
                       matrizSaber.MatrizSaber = matriz;
                       plano.Matrizes.Add(matrizSaber);
                       return plano;
                   },
                   splitOn: "id,id", param: new { Id = id });
            return new PlanoCiclo();
        }
    }
}