using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioNotificacaoConsulta : RepositorioBase<Notificacao>, IRepositorioNotificacaoConsulta
    {
        public RepositorioNotificacaoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<Notificacao>> Obter(string dreId, string ueId, int statusId,
            string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, Paginacao paginacao)
        {
            var query = new StringBuilder();

            MontaQueryObterCompleta(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, paginacao, query,
                paginacao.QuantidadeRegistros, paginacao.QuantidadeRegistrosIgnorados);

            MontaQueryObterCount(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);

            var retornoPaginado = new PaginacaoResultadoDto<Notificacao>();

            if (!string.IsNullOrEmpty(titulo))
            {
                titulo = $"%{titulo}%";
            }

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new
            {
                dreId,
                ueId,
                turmaId,
                statusId,
                tipoId,
                usuarioRf,
                categoriaId,
                titulo,
                codigo,
                anoLetivo
            }))
            {
                retornoPaginado.Items = multi.Read<Notificacao>().ToList();
                retornoPaginado.TotalRegistros = multi.ReadFirst<int>();
            }

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        private static void MontaFiltrosObter(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, StringBuilder query)
        {
            query.AppendLine("where excluida = false");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and n.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and n.ue_id = @ueId");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and n.turma_id = @turmaId");

            if (statusId > 0)
                query.AppendLine("and n.status = @statusId");

            if (tipoId > 0)
                query.AppendLine("and n.tipo = @tipoId");

            if (!string.IsNullOrEmpty(usuarioRf))
                query.AppendLine("and (u.rf_codigo = @usuarioRf or u.login = @usuarioRf)");

            if (categoriaId > 0)
                query.AppendLine("and n.categoria = @categoriaId");

            if (codigo > 0)
                query.AppendLine("and n.codigo = @codigo");

            if (anoLetivo > 0)
                query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");

            if (!string.IsNullOrEmpty(titulo))
                query.AppendLine("and lower(f_unaccent(n.titulo)) LIKE lower(f_unaccent(@titulo))");
        }

        private static void MontaQueryObterCabecalho(StringBuilder query, bool EhParaCount)
        {
            query.Append(";");

            if (EhParaCount)
                query.AppendLine("select count(n.*) from notificacao n");
            else query.AppendLine("select n.* from notificacao n");

            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
        }

        private static void MontaQueryObterCompleta(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo,
            long codigo, int anoLetivo, Paginacao paginacao, StringBuilder query, int quantidadeRegistros, int quantidadeRegistrosIgnorados)
        {
            MontaQueryObterCabecalho(query, false);
            MontaFiltrosObter(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);
            query.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", quantidadeRegistrosIgnorados, quantidadeRegistros);
        }

        private static void MontaQueryObterCount(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo,
            long codigo, int anoLetivo, StringBuilder query)
        {
            MontaQueryObterCabecalho(query, true);
            MontaFiltrosObter(dreId, ueId, statusId, turmaId, usuarioRf, tipoId, categoriaId, titulo, codigo, anoLetivo, query);
        }

        public async Task<IEnumerable<Notificacao>> ObterNotificacoesPorAnoLetivoERf(int anoLetivo, string usuarioRf, int limite)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.* from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
            query.AppendLine("where u.rf_codigo = @usuarioRf");
            query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");
            query.AppendLine("and excluida = @excluida");
            query.AppendLine("order by n.status asc, n.criado_em desc");
            query.AppendLine("limit @limite");

            return await database.Conexao.QueryAsync<Notificacao>(query.ToString(), new { anoLetivo, usuarioRf, limite, excluida = false });
        }

        public override Notificacao ObterPorId(long id)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.*, wan.*, u.* from notificacao n");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wann");
            query.AppendLine("on wann.notificacao_id = n.id");
            query.AppendLine("left join wf_aprovacao_nivel wan");
            query.AppendLine("on wan.id = wann.wf_aprovacao_nivel_id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on u.id = n.usuario_id");

            query.AppendLine("where excluida = false ");
            query.AppendLine("and n.id = @id ");

            return database.Conexao.Query<Notificacao, WorkflowAprovacaoNivel, Usuario, Notificacao>(query.ToString(),
                (notificacao, workflowNivel, usuario) =>
                {
                    notificacao.WorkflowAprovacaoNivel = workflowNivel;
                    notificacao.Usuario = usuario;
                    notificacao.UsuarioId = usuario.Id;

                    return notificacao;
                }, param: new { id }).FirstOrDefault();
        }

        public async Task<Notificacao> ObterPorCodigo(long codigo)
        {
            var query = new StringBuilder();

            query.AppendLine("select n.*, wan.*, u.* from notificacao n");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wann");
            query.AppendLine("on wann.notificacao_id = n.id");
            query.AppendLine("left join wf_aprovacao_nivel wan");
            query.AppendLine("on wan.id = wann.wf_aprovacao_nivel_id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on u.id = n.usuario_id");

            query.AppendLine("where excluida = false ");
            query.AppendLine("and n.codigo = @codigo ");

            var notificacoes = await database.Conexao.QueryAsync<Notificacao, WorkflowAprovacaoNivel, Usuario, Notificacao>(query.ToString(),
                (notificacao, workflowNivel, usuario) =>
                {
                    notificacao.WorkflowAprovacaoNivel = workflowNivel;
                    notificacao.Usuario = usuario;
                    notificacao.UsuarioId = usuario.Id;

                    return notificacao;
                }, param: new { codigo });

            return notificacoes.FirstOrDefault();
        }

        public async Task<IEnumerable<Notificacao>> ObterPorWorkFlowAprovacaoId(long workFlowAprovacaoId)
        {
            var query = @"select distinct n.*
                            from wf_aprovacao wa
                            join wf_aprovacao_nivel wan on wan.wf_aprovacao_id = wa.id 
                            join wf_aprovacao_nivel_notificacao wann on wann.wf_aprovacao_nivel_id = wan.id 
                            join notificacao n on n.id = wann.notificacao_id 
                            join wf_aprovacao_nota_fechamento wfanc on wfanc.wf_aprovacao_id = wa.id
                            where wfanc.wf_aprovacao_id = @workFlowAprovacaoId";

            return await database.Conexao.QueryAsync<Notificacao>(query, new {workFlowAprovacaoId});
        }

        public int ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(int anoLetivo, string usuarioRf)
        {
            var query = new StringBuilder();

            query.AppendLine("select count(*) from notificacao n");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");
            query.AppendLine("where u.rf_codigo = @usuarioRf");
            query.AppendLine("and excluida = @excluida");
            query.AppendLine("and n.status = @naoLida");
            query.AppendLine("and EXTRACT(year FROM n.criado_em) = @anoLetivo");

            return database.Conexao.QueryFirst<int>(query.ToString(), new { anoLetivo, usuarioRf, excluida = false, naoLida = (int)NotificacaoStatus.Pendente });
        }

        public async Task<long> ObterUltimoCodigoPorAnoAsync(int ano)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT * from (");
            query.AppendLine("SELECT MAX(n.codigo) Codigo");
            query.AppendLine("FROM notificacao n");
            query.AppendLine($"where ano = @ano and n.categoria <> {(int)NotificacaoCategoria.Informe}");
            query.AppendLine("group by n.codigo, n.ano");
            query.AppendLine("order by n.codigo, n.ano desc) VALORES");
            query.AppendLine("order by Codigo desc");
            query.AppendLine("limit 1");

            var codigos = await database.Conexao.QueryAsync<int>(query.ToString(), new { ano }, commandTimeout: 90);
            return codigos.FirstOrDefault();
        }

        public async Task<int> ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(int anoLetivo, string codigoRf)
        {
            var sql = @"select
                            count(*)
                        from
                            notificacao n
                        left join usuario u on
                            n.usuario_id = u.id
                        where
                            (u.rf_codigo = @codigoRf or u.login = @codigoRf)
                            and not excluida
                            and n.status = @naoLida
                            and extract(year from n.criado_em) = @anoLetivo";

            return await database.Conexao.QueryFirstAsync<int>(sql, new { anoLetivo, codigoRf, naoLida = (int)NotificacaoStatus.Pendente });
       }

        public async Task<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>> ObterNotificacoesParaTratamentoCargosNiveis(string codigoUe)
        {
            var query = @"select 
                            wan.cargo,                                                         
                            n.ue_id as UECodigo,
                            n.dre_id as DRECodigo,                            
                            n.id as NotificacaoId,
                            wan.wf_aprovacao_id as WorkflowId
                            from wf_aprovacao_nivel wan
                                inner join wf_aprovacao_nivel_notificacao wann 
                                    on wann.wf_aprovacao_nivel_id  = wan.id
                                inner join notificacao n 
                                    on wann.notificacao_id  = n.id                            
                                inner join usuario u 
                                    on n.usuario_id  = u.id 
                                where n.status = 1
                                    and n.excluida = false
                                    and n.tipo in (1,2)
                                    and n.ue_id = @codigoUe";

            return await database.Conexao.QueryAsync<NotificacoesParaTratamentoCargosNiveisDto>(query, new { codigoUe });
        }
        public async Task<long> ObterCodigoPorId(long notificacaoId)
        {
            var query = @"select codigo from notificacao where id = @notificacaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { notificacaoId });
        }

        public long ObterUltimoCodigoPorAno(int ano)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT coalesce(max(n.codigo), 0)");
            query.AppendLine("FROM notificacao n");
            query.AppendLine("where EXTRACT(year FROM n.criado_em) = @ano");

            return database.Conexao.QueryFirstOrDefault<int>(query.ToString(), new { ano });
        }

        public Task<IEnumerable<NotificacaoUsuarioDto>> ObterUsuariosNotificacoesPorIds(long[] notificacoesIds)
        {
            var query = @"select 
                            n.id as Id,
                            n.codigo as Codigo,
                            n.status as Status,
                            u.rf_codigo as UsuarioRf
                         from notificacao n 
                         left join usuario u on u.id = n.usuario_id
                        where n.id = any(@notificacoesIds)";

            return database.Conexao.QueryAsync<NotificacaoUsuarioDto>(query, new { notificacoesIds });
        }

        public Task<string> ObterUsuarioNotificacaoPorId(long id)
        {
            var query = @"select u.rf_codigo
                         from notificacao n 
                         left join usuario u on u.id = n.usuario_id
                        where n.id = @id";

            return database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { id });
        }
    }
}
