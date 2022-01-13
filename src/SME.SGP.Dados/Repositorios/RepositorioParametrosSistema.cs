using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistema : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistema
    {
        public RepositorioParametrosSistema(ISgpContext database) : base(database)
        {
        }

        public async Task AtualizarValorPorTipoAsync(TipoParametroSistema tipo, string valor, int? ano = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("update parametros_sistema set valor = @valor");
            query.AppendLine("where ativo and tipo = @tipo");
            if (ano.HasValue)
                query.AppendLine("and ano = @ano");

            await database.Conexao.ExecuteAsync(query.ToString(), new { tipo, valor, ano });
        }

        public async Task ReplicarParametrosAnoAnteriorAsync(int anoAtual, int anoAnterior)
        {
            var query = @"insert into parametros_sistema
                                 (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
                           select nome, tipo, descricao, valor, @anoAtual, ativo, now(), 'SISTEMA', null, '', 'SISTEMA', ''
                             from parametros_sistema
                            where ano = @anoAnterior                              
                              and tipo not in (select tipo 
                                                 from parametros_sistema 
                                                where ano = @anoAtual)";

            await database.Conexao.ExecuteAsync(query.ToString(), new { anoAtual, anoAnterior });
        }        
    }
}