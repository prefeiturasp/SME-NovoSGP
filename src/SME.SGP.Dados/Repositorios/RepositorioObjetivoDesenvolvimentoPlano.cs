using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoDesenvolvimentoPlano : RepositorioBase<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>, IRepositorioObjetivoDesenvolvimentoPlano
    {
        public RepositorioObjetivoDesenvolvimentoPlano(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public IEnumerable<RecuperacaoParalelaObjetivoDesenvolvimentoPlano> ObterObjetivosDesenvolvimentoPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>("select * from recuperacao_paralela_objetivo_desenvolvimento_plano where plano_id = @Id", new { Id = idPlano });
        }
    }
}