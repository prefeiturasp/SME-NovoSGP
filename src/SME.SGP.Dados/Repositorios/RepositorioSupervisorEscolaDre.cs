using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSupervisorEscolaDre : RepositorioBase<SupervisorEscolaDre>, IRepositorioSupervisorEscolaDre
    {
        public RepositorioSupervisorEscolaDre(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObtemPorDreESupervisor(string dreId, string supervisorId, bool excluidos = false)
        {
            StringBuilder query = new();

            query.AppendLine("select id as AtribuicaoSupervisorId, dre_id, escola_id, supervisor_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido as AtribuicaoExcluida, tipo as TipoAtribuicao ");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where 1 = 1");

            if (!excluidos)
                query.AppendLine("and excluido = false");

            if (!string.IsNullOrEmpty(supervisorId))
                query.AppendLine("and sed.supervisor_id = @supervisorId");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and sed.dre_id = @dreId");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { supervisorId, dreId });
        }

        public async Task<IEnumerable<ExisteAtribuicaoExcluidaDto>> VerificarSeJaExisteAtribuicaoExcluida(string dreCodigo, string[]uesCodigos,int tipoAtribuicao)
        {
            StringBuilder query = new(@"SELECT 
                                            Id,
                                            escola_id UeCodigo,
                                            criado_por AS CriadoPor,
                                            criado_rf AS CriadoRF
                                        FROM supervisor_escola_dre sed 
                                        WHERE excluido  
                                        AND sed.dre_id = @dreCodigo 
                                        AND sed.escola_id = ANY(@uesCodigos)
                                        AND sed.tipo = @tipoAtribuicao ");

            return await database.Conexao.QueryAsync<ExisteAtribuicaoExcluidaDto>(query.ToString(), new { dreCodigo, uesCodigos, tipoAtribuicao });
        }

        public async Task<int> VerificarSeJaExisteAtribuicaoAtivaComOutroResponsavelParaAqueleTipoUe(int tipo, string ueCodigo, string dreCodigo, string responsavelCodigo)
        {
            StringBuilder query = new(@"SELECT
                                            count(*)
                                        FROM
                                         supervisor_escola_dre sed
                                        WHERE  NOT sed.excluido  
                                        AND tipo = @tipo
                                        AND sed.dre_id = @dreCodigo
                                        AND sed.escola_id = @ueCodigo
                                        AND sed.supervisor_id  <> @responsavelCodigo ");

            return await database.Conexao.QueryFirstAsync<int>(query.ToString(), new {tipo,ueCodigo,dreCodigo,responsavelCodigo });
        }

        public async Task<IEnumerable<UnidadeEscolarSemAtribuicaolDto>> ObterListaUEsParaNovaAtribuicaoPorCodigoDre(string dreCodigo)
        {
            StringBuilder query = new(@"SELECT
                                            u.ue_id AS Codigo,
                                            u.nome AS UeNome,
                                            u.tipo_escola AS TipoEscola,
                                            tipo AS TipoAtribuicao,
                                            CASE
                                            WHEN sed.excluido IS NOT NULL
                                            THEN sed.excluido 
                                            ELSE false 
                                            END AS AtribuicaoExcluida
                                        FROM
                                            ue u
                                            INNER JOIN dre d ON u.dre_id = d.id
                                            LEFT JOIN supervisor_escola_dre sed ON u.ue_id = sed.escola_id 
                                        WHERE d.dre_id = :dreCodigo
                                        GROUP BY u.ue_id,u.nome,u.tipo_escola,tipo,excluido  
                                        ORDER BY u.nome ");
            return await database.Conexao.QueryAsync<UnidadeEscolarSemAtribuicaolDto>(query.ToString(), new { dreCodigo });
        }


        public async Task<IEnumerable<UnidadeEscolarResponsavelDto>> ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(string dreId, string supervisoresId, int tipoResponsavel)
        {
            StringBuilder query = new(@"SELECT        
                                            sed.escola_id AS Codigo,
                                            u.nome AS UeNome,
                                            u.tipo_escola AS TipoEscola,
                                            sed.tipo,
                                            sed.criado_em AS CriadoEm,
                                            sed.criado_por AS CriadoPor,
                                            sed.alterado_em AS AlteradoEm,
                                            sed.alterado_por AS AlteradoPor,
                                            sed.criado_rf AS CriadoRf,
                                            sed.alterado_rf AS AlteradoRf
                                       FROM supervisor_escola_dre sed
                                        INNER JOIN dre d ON sed.dre_id = d.dre_id
                                        INNER JOIN ue u ON u.ue_id  = sed.escola_id 
                                        where excluido = false 
                                        and sed.supervisor_id = @supervisoresId 
                                        and sed.tipo = @tipoResponsavel
                                         and sed.dre_id = @dreId ");

            return await database.Conexao.QueryAsync<UnidadeEscolarResponsavelDto>(query.ToString(), new { dreId, supervisoresId, tipoResponsavel });
        }
        public async Task<List<SupervisorEscolasDreDto>> ObterTodosAtribuicaoResponsavelPorDreCodigo(string dreCodigo)
        {
            StringBuilder query = new();

            query.AppendLine(@"
            SELECT 
                sed.id as AtribuicaoSupervisorId,
                 sed.dre_id AS DreId,
                 u.ue_id AS UeId,
                 sed.supervisor_id AS SupervisorId,
                 sed.criado_em AS CriadoEm,
                 sed.criado_por AS CriadoPor,
                 sed.alterado_em AS AlteradoEm,
                 sed.alterado_por AS AlteradoPor,
                 sed.criado_rf AS CriadoRf,
                 sed.alterado_rf AS AlteradoRf,
                 sed.excluido AtribuicaoExcluida,
                 sed.tipo AS TipoAtribuicao,
                 d.abreviacao  AS DreNome,
                 u.nome AS UeNome,
                 u.tipo_escola AS TipoEscola
            FROM ue u
            INNER JOIN dre d ON u.dre_id =d.id
            LEFT JOIN supervisor_escola_dre sed ON u.ue_id  = sed.escola_id and not sed.excluido
            WHERE d.dre_id = @dreCodigo
            ORDER BY u.tipo_escola ,u.nome ");

            var consulta = await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { dreCodigo });
            return consulta.ToList();
        }
        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObterAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro)
        {
            StringBuilder query = new();

            query.AppendLine(@"SELECT sed.id as AtribuicaoSupervisorId,
                                 sed.dre_id AS DreId,
                                 sed.escola_id AS UeId,
                                 sed.supervisor_id AS SupervisorId,
                                 sed.criado_em AS CriadoEm,
                                 sed.criado_por AS CriadoPor,
                                 sed.alterado_em AS AlteradoEm,
                                 sed.alterado_por AS AlteradoPor,
                                 sed.criado_rf AS CriadoRf,
                                 sed.alterado_rf AS AlteradoRf,
                                 sed.excluido AtribuicaoExcluida,
                                 sed.tipo AS TipoAtribuicao,
                                 d.abreviacao  AS DreNome,
                                 u.nome AS UeNome,
                                 u.tipo_escola AS TipoEscola
                            FROM supervisor_escola_dre sed
                             INNER JOIN dre d ON sed.dre_id = d.dre_id
                             INNER JOIN ue u ON u.ue_id  = sed.escola_id 
                            WHERE sed.dre_id = @dre ");

            if (!string.IsNullOrEmpty(filtro.UeCodigo))
                query.AppendLine(" and sed.escola_id = @ue ");

            if (filtro.TipoCodigo > 0)
                query.AppendLine(" and sed.tipo = @tipo ");

            if(!string.IsNullOrEmpty(filtro.SupervisorId))
                query.AppendLine(" AND sed.supervisor_id = ANY(@supervisor)  AND sed.excluido = False ");

            if (filtro.SupervisorId?.Length == 0 || filtro.SupervisorId.EhNulo() && filtro.UESemResponsavel)
                query.AppendLine(" AND sed.excluido = true ");

            var parametros = new
            {
                dre = filtro.DreCodigo,
                ue = filtro.UeCodigo,
                tipo = filtro.TipoCodigo,
                supervisor = filtro.SupervisorId?.Split(",")
            };

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorDreAsync(string codigoDre,
            TipoResponsavelAtribuicao? tipoResponsavelAtribuicao)
        {
            StringBuilder query = new();

            query.AppendLine("select id as AtribuicaoSupervisorId, dre_id as DreId, escola_id as EscolaId, supervisor_id as SupervisorId, criado_em as CriadoEm," +
                "criado_por as CriadoPor, alterado_em as AlteradoEm, alterado_por as AlteradoPor, criado_rf as CriadoRF, " +
                "alterado_rf as AlteradoRF, excluido as AtribuicaoExcluida, tipo as TipoAtribuicao");

            query.AppendLine("from supervisor_escola_dre sed");

            query.AppendLine("where dre_id = @codigoDre " +
                "and excluido = false ");

            if (tipoResponsavelAtribuicao.NaoEhNulo())
                query.AppendLine("and tipo = @tipoResponsavelAtribuicao");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { codigoDre, tipoResponsavelAtribuicao });
        }
        
        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUe(string ueId)
        {
            StringBuilder query = new();

            query.AppendLine("select id as AtribuicaoSupervisorId, dre_id as DreId, escola_id as UeId, supervisor_id as SupervisorId, criado_em as CriadoEm, criado_por as CriadoPor, alterado_em as AlteradoEm, alterado_por as AlteradoPor, criado_rf as CriadoRF, alterado_rf as AlteradoRF, excluido as AtribuicaoExcluida, tipo as TipoAtribuicao");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where escola_id = @ueId and excluido = false and tipo = 1");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { ueId });
        }
        
        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObterSupervisoresPorUeTipo(string ueId, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            StringBuilder query = new();

            query.AppendLine(@"select sed.id as AtribuicaoSupervisorId, dre_id as DreId, escola_id as UeId, supervisor_id as SupervisorId, sed.criado_em as CriadoEm, 
                                      sed.criado_por as CriadoPor, sed.alterado_em as AlteradoEm, sed.alterado_por as AlteradoPor, sed.criado_rf as CriadoRF, 
                                      sed.alterado_rf as AlteradoRF, excluido as AtribuicaoExcluida, tipo as TipoAtribuicao, u.nome as NomeResponsavel 
                               from supervisor_escola_dre sed
                                left join usuario u on login = sed.supervisor_id 
                               where escola_id = @ueId and excluido = false and tipo = @tipoResponsavelAtribuicao");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { ueId,tipoResponsavelAtribuicao });
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObtemSupervisoresPorUeAsync(string ueId)
        {
            StringBuilder query = new();

            query.AppendLine("select id as AtribuicaoSupervisorId, dre_id as DreId, escola_id as UeId, supervisor_id as SupervisorId, criado_em as CriadoEm, criado_por as CriadoPor, alterado_em as AlteradoEm, alterado_por as AlteradoPor, criado_rf as CriadoRF, alterado_rf as AlteradoRF, excluido as AtribuicaoExcluida, tipo as TipoAtribuicao");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where escola_id = @ueId and excluido = false");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new { ueId });
        }

        public async Task<IEnumerable<DadosAbrangenciaSupervisorDto>> ObterDadosAbrangenciaSupervisor(string rfSupervisor, bool consideraHistorico, int anoLetivo, string codigoUe = null)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select distinct vact.modalidade_codigo Modalidade,");
            sqlQuery.AppendLine("                vact.dre_abreviacao AbreviacaoDre,");
            sqlQuery.AppendLine("                vact.dre_codigo CodigoDre,");
            sqlQuery.AppendLine("                vact.dre_nome DreNome,");
            sqlQuery.AppendLine("                vact.dre_id DreId,");
            sqlQuery.AppendLine("                 vact.ue_codigo CodigoUe,");
            sqlQuery.AppendLine("                 vact.ue_nome UeNome,");
            sqlQuery.AppendLine("                 ue.tipo_escola TipoEscola,");
            sqlQuery.AppendLine("                 vact.ue_id UeId,");
            sqlQuery.AppendLine("                 vact.turma_semestre Semestre,");
            sqlQuery.AppendLine("                 vact.turma_codigo CodigoTurma,");
            sqlQuery.AppendLine("                vact.turma_ano TurmaAno,");
            sqlQuery.AppendLine("                vact.turma_ano_letivo TurmaAnoLetivo,");
            sqlQuery.AppendLine("                vact.turma_nome TurmaNome,");
            sqlQuery.AppendLine("                vact.qt_duracao_aula QuantidadeDuracaoAula,");
            sqlQuery.AppendLine("                vact.tipo_turno TipoTurno,");
            sqlQuery.AppendLine("                vact.ensino_especial EnsinoEspecial,");
            sqlQuery.AppendLine("                vact.turma_id TurmaId,");
            sqlQuery.AppendLine("                vact.tipo_turma TipoTurma,");
            sqlQuery.AppendLine("                vact.nome_filtro NomeFiltro");
            sqlQuery.AppendLine("    from supervisor_escola_dre sed");
            sqlQuery.AppendLine("        inner join ue");
            sqlQuery.AppendLine("            on sed.escola_id = ue.ue_id");
            sqlQuery.AppendLine("        inner join v_abrangencia_cadeia_turmas vact");
            sqlQuery.AppendLine("            on ue.id = vact.ue_id");
            sqlQuery.AppendLine("where sed.supervisor_id = @rfSupervisor and");
            sqlQuery.AppendLine("     not sed.excluido and");
            sqlQuery.AppendLine("    vact.turma_historica = @consideraHistorico and");
            
            if(anoLetivo > 0)
                sqlQuery.AppendLine("    vact.turma_ano_letivo = @anoLetivo and");

            sqlQuery.AppendLine("    sed.Tipo = @tipoResponsavelAtribuicao");

            if (codigoUe.NaoEhNulo())
                sqlQuery.AppendLine(" and vact.ue_codigo = @codigoUe");

            var tipoResponsavelAtribuicao = (int)TipoResponsavelAtribuicao.SupervisorEscolar;

            return await database.Conexao
                .QueryAsync<DadosAbrangenciaSupervisorDto>(sqlQuery.ToString(),
                new
                {
                    rfSupervisor,
                    consideraHistorico,
                    anoLetivo,
                    tipoResponsavelAtribuicao,
                    codigoUe
                });
        }

        public async Task<IEnumerable<ListaUesConsultaAtribuicaoResponsavelDto>> ObterListaDeUesFiltroPrincipal(string dreCodigo)
        {
            var sql = new StringBuilder(@"SELECT 
                               u.id ,
                               u.ue_id AS Codigo,
                               u.nome AS NomeSimples,
                               u.tipo_escola AS TipoEscola 
                           FROM ue u
                           INNER JOIN dre d ON u.dre_id =d.id
                           LEFT JOIN supervisor_escola_dre sed ON u.ue_id  = sed.escola_id 
                           WHERE d.dre_id  = @dreCodigo
                           GROUP BY u.id 
                           ORDER BY u.tipo_escola,u.nome ");

            return await database.Conexao.QueryAsync<ListaUesConsultaAtribuicaoResponsavelDto>(sql.ToString(), new {dreCodigo});
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsavelAtribuidoUePorUeTipo(string codigoUe, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var query = @" select sed.supervisor_id as codigoRf
                            from supervisor_escola_dre sed
                            where sed.escola_id = @codigoUe
                            and sed.tipo = @tipoResponsavelAtribuicao
                            and not sed.excluido";

            return (await database.Conexao.QueryAsync<UsuarioEolRetornoDto>(query, new { codigoUe, tipoResponsavelAtribuicao }));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> ObterResponsaveisPorDreUeTiposAtribuicaoAsync(string codigoDre, string codigoUe, TipoResponsavelAtribuicao[] tiposResponsavelAtribuicao)
        {
            StringBuilder query = new();

            query.AppendLine("select sed.id as AtribuicaoSupervisorId, sed.dre_id as DreId, sed.escola_id as UeId, sed.supervisor_id as SupervisorId, sed.criado_em as CriadoEm, sed.criado_por as CriadoPor, sed.alterado_em as AlteradoEm, sed.alterado_por as AlteradoPor, sed.criado_rf as CriadoRF, sed.alterado_rf as AlteradoRF, sed.excluido as AtribuicaoExcluida, sed.tipo as TipoAtribuicao, u.nome as SupervisorNome");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("join usuario u on u.rf_codigo = supervisor_id ");
            query.AppendLine("where not excluido");

            if (!string.IsNullOrEmpty(codigoDre))
                query.AppendLine("and dre_id = @codigoDre");
            if (!string.IsNullOrEmpty(codigoUe))
                query.AppendLine("and escola_id = @codigoUe ");
            if (tiposResponsavelAtribuicao.NaoEhNulo() && tiposResponsavelAtribuicao.Any())
                query.AppendLine("and tipo = any (@tiposResponsavelAtribuicao) ");

            return await database.Conexao.QueryAsync<SupervisorEscolasDreDto>(query.ToString(), new
            {
                codigoDre,
                codigoUe,
                tiposResponsavelAtribuicao = tiposResponsavelAtribuicao.ToIntegerArray()
            });
            
            
        }
        public async Task RemoverAtribuicoesEmLote(IEnumerable<long> atribuicoesIds)
        {
            await RemoverLogico(atribuicoesIds.ToArray());
        }
    }
}