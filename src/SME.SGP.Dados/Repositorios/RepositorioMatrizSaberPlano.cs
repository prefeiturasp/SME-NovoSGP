using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMatrizSaberPlano : RepositorioBase<MatrizSaberPlano>, IRepositorioMatrizSaberPlano
    {
        public RepositorioMatrizSaberPlano(SgpContext conexao) : base(conexao)
        {
        }

        public MatrizSaberPlano ObterPorMatrizSaberPlano(long matrizId, long idPlano)
        {
            if (idPlano == 0)
            {
                return null;
            }
            return database.Connection().Query<MatrizSaberPlano>("").SingleOrDefault();
        }

        public MatrizSaberPlano ObterMatrizesSaberDoPlano(IEnumerable<long> matrizesId, long idPlano)
        {
            if (idPlano == 0)
            {
                return null;
            }
            return database.Connection().Query<MatrizSaberPlano>("select * from matriz_saber_plano where plano_id in @Ids", new { Ids = matrizesId }).SingleOrDefault();
        }
    }
}