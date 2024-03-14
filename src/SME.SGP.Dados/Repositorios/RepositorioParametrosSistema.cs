using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistema : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistema
    {
        public RepositorioParametrosSistema(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
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

        public async Task<int> ReplicarParametrosAnoAnteriorAsync(int anoAtual, int anoAnterior)
        {
            var query = @"insert into parametros_sistema
                                 (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
                           select nome, tipo, descricao, valor, @anoAtual, ativo, now(), 'SISTEMA', null, '', 'SISTEMA', ''
                             from parametros_sistema
                            where ano = @anoAnterior                              
                              and tipo not in (select tipo 
                                                 from parametros_sistema 
                                                where ano = @anoAtual)";

            return await database.Conexao.ExecuteAsync(query.ToString(), new { anoAtual, anoAnterior });
        }


        public async Task<int> CriarParametrosPeriodosConfiguracaoRelatorioPeriodicoPAPAnoAtualAsync(int anoAtual)
        {
            var query = @"DO $$
                          DECLARE
                              configuracaoId bigint;
                              periodoRelatorioId bigint;
                              periodoDB record;
                          BEGIN
                          	  configuracaoId := (select id from configuracao_relatorio_pap where inicio_vigencia <= NOW() and tipo_periodicidade = 'B' and (fim_vigencia is null or fim_vigencia >= NOW()));
                              FOR periodoDB IN
                                  SELECT pe.id, pe.bimestre 
                                  FROM periodo_escolar pe 
                                  INNER JOIN tipo_calendario tc ON tc.id = pe.tipo_calendario_id
                                  WHERE ano_letivo = @anoAtual AND modalidade = 1 AND NOT tc.excluido
                                        and coalesce(configuracaoId, 0) > 0
                                  ORDER BY pe.bimestre
                              LOOP
                                  INSERT INTO periodo_relatorio_pap (configuracao_relatorio_pap_id, periodo, criado_em, criado_por, criado_rf)
                                      select configuracaoId, periodoDB.bimestre, NOW(), 'SISTEMA', '0' 
                                      	where not exists(SELECT 1 FROM periodo_relatorio_pap prp 
		                          			            INNER JOIN periodo_escolar_relatorio_pap per ON prp.id = per.periodo_relatorio_pap_id
		                          			            WHERE prp.configuracao_relatorio_pap_id = configuracaoId 
		                          			            AND per.periodo_escolar_id = periodoDB.id)
                                      RETURNING id INTO periodoRelatorioId;

                                  INSERT INTO periodo_escolar_relatorio_pap(periodo_relatorio_pap_id, periodo_escolar_id, criado_em, criado_por, criado_rf)
                                    select periodoRelatorioId, periodoDB.id, NOW(), 'SISTEMA', '0' where coalesce(periodoRelatorioId, 0) > 0;
                              END LOOP;
                          END $$;";

            return await database.Conexao.ExecuteAsync(query.ToString(), new { anoAtual });
        }
    }
}