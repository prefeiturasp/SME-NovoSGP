using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMatrizSaberPlano : RepositorioBase<MatrizSaberPlano>, IRepositorioMatrizSaberPlano
    {
        public RepositorioMatrizSaberPlano(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public IEnumerable<MatrizSaberPlano> ObterMatrizesPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<MatrizSaberPlano>("select * from matriz_saber_plano where plano_id = @Id", new { Id = idPlano });
        }
    }
}