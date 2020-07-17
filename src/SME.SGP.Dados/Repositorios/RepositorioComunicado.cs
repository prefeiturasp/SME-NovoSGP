using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicado : RepositorioBase<Comunicado>, IRepositorioComunicado
    {
        private readonly string fromComunicado = @"comunicado";

        private readonly string fromComunicadoGrupo =
                                                @"(SELECT
                                                    co.id,
                                                    co.titulo,
                                                    co.descricao,
                                                    co.data_envio,
                                                    co.data_expiracao,
                                                    co.criado_em,
                                                    co.criado_por,
                                                    co.alterado_em,
                                                    co.alterado_por,
                                                    co.criado_rf,
                                                    co.alterado_rf,
                                                    co.excluido
                                                from comunicado co inner join comunidado_grupo cgr
                                                    on cgr.comunicado_id = co.id
                                                {0}
                                                where co.excluido = false
                                                group by
                                                    co.id,
                                                    co.titulo,
                                                    co.descricao,
                                                    co.data_envio,
                                                    co.data_expiracao ,
                                                    co.criado_em,
                                                    co.criado_por,
                                                    co.alterado_em,
                                                    co.alterado_por,
                                                    co.criado_rf,
                                                    co.alterado_rf,
                                                    co.excluido
                                                order by co.id
                                                {1})";

        private string queryComunicado(bool ehListagem)
        {
            var query = new StringBuilder();

            query.AppendLine(@"
						SELECT
							{0}
						FROM {1} c
						LEFT JOIN comunidado_grupo cg
								on cg.comunicado_id = c.id
							LEFT join grupo_comunicado g
								on cg.grupo_comunicado_id = g.id
                        ");

            if (!ehListagem)
                query.AppendLine(@"
                        LEFT JOIN comunicado_aluno ca
                            on ca.comunicado_id = c.id");

            query.AppendLine(@"
						WHERE (c.excluido = false)
						{2}");

            return query.ToString();
        }

        public RepositorioComunicado(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();
            string where = MontaWhereListar(filtro);
            string from = "";
            var whereGrupo = " AND ({0}.grupo_comunicado_id = ANY(@gruposId))";

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);

            from = ObterFrom(filtro, paginacao, whereGrupo);

            query.AppendFormat(queryComunicado(true), Montarcampos(), from, where);

            var retornoPaginado = new PaginacaoResultadoDto<Comunicado>()
            {
                Items = await database.Conexao.QueryAsync<Comunicado, ComunicadoGrupo, GrupoComunicacao, ComunicadoAluno, Comunicado>(query.ToString(), (comunicado, g, grupo, ComunicadoAluno) =>
                {
                    comunicado.AdicionarGrupo(grupo);
                    comunicado.AdicionarAluno(ComunicadoAluno);

                    return comunicado;
                }, new
                {
                    filtro.DataEnvio,
                    filtro.DataExpiracao,
                    filtro.Titulo,
                    filtro.GruposId,
                    filtro.AnoLetivo,
                    filtro.Modalidade,
                    filtro.Semestre,
                    filtro.CodigoDre,
                    filtro.CodigoUe,
                    filtro.Turma
                },
            splitOn: "id,ComunicadoGrupoId,GrupoId,AlunoId")
            };

            var queryCount = new StringBuilder(string.Format(queryComunicado(true), "count(distinct c.id)", fromComunicado, $"{where}{(filtro.GruposId?.Length > 0 ? string.Format(whereGrupo, "cg") : "")}"));

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryAsync<int>(queryCount.ToString(), new
            {
                filtro.DataEnvio,
                filtro.DataExpiracao,
                filtro.Titulo,
                filtro.GruposId,
                filtro.AnoLetivo,
                filtro.Modalidade,
                filtro.Semestre,
                filtro.CodigoDre,
                filtro.CodigoUe,
                filtro.Turma
            })).Sum();

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        private string ObterFrom(FiltroComunicadoDto filtro, Paginacao paginacao, string whereGrupo)
        {
            string from;
            if (paginacao.QuantidadeRegistros != 0)
                from = string.Format(fromComunicadoGrupo, filtro.GruposId?.Length > 0 ? string.Format(whereGrupo, "cgr") : "", string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros));
            else
                from = string.Format(fromComunicadoGrupo, filtro.GruposId?.Length > 0 ? string.Format(whereGrupo, "cgr") : "", "");
            return from;
        }

        public async Task<IEnumerable<ComunicadoResultadoDto>> ObterResultadoPorComunicadoIdAsync(long id)
        {
            var where = "AND c.id = @id";
            var query = string.Format(queryComunicado(false), Montarcampos(false), fromComunicado, where);

            return await database.Conexao.QueryAsync<ComunicadoResultadoDto>(query, new { id });
        }

        private static string MontaWhereListar(FiltroComunicadoDto filtro)
        {
            var where = new StringBuilder();

            where.AppendLine("c.ano_letivo = @AnoLetivo");

            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                filtro.Titulo = $"%{filtro.Titulo.ToUpperInvariant()}%";
                where.AppendLine("AND (upper(f_unaccent(c.titulo)) LIKE @titulo)");
            }

            if (filtro.DataEnvio.HasValue)
                where.AppendLine("AND (date(c.data_envio) = @DataEnvio)");

            if (filtro.DataExpiracao.HasValue)
                where.AppendLine("AND (date(c.data_expiracao) = @DataExpiracao)");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoDre))
                where.AppendLine("AND c.codigo_dre = @CodigoDre");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoUe))
                where.AppendLine("AND c.codigo_ue = @CodigoUe");

            if (!string.IsNullOrWhiteSpace(filtro.Turma))
                where.AppendLine("AND c.turma = @Turma");

            if (filtro.Modalidade > 0)
                where.AppendLine("AND c.modalidade = @Modalidade");

            if (filtro.Semestre > 0)
                where.AppendLine("AND c.semestre = @Semestre");

            return where.ToString();
        }

        private string Montarcampos(bool EhListagem = true)
        {
            StringBuilder campos = new StringBuilder();
            campos.Append(@"c.id,
							c.titulo,
							c.descricao,
							c.data_envio as DataEnvio,
							c.data_expiracao as DataExpiracao,
							c.criado_em as CriadoEm,
							c.criado_por as CriadoPor,
							c.alterado_em as AlteradoEm,
							c.alterado_por as AlteradoPor,
							c.criado_rf as CriadoRf,
							c.alterado_rf as AlteradoRf,
                            c.ano_letivo as AnoLetivo,
                            c.modalidade as Modalidade,
                            c.semestre as Semestre,
                            c.tipo_comunicado as TipoComunicado,
                            c.codigo_dre as CodigoDre,
                            c.codigo_ue as CodigoUe,
                            c.turma as Turma,
                            c.alunos_especificados as AlunosEspecificados,");

            if (!EhListagem)
                campos.Append(@"
                                ca.id as AlunoId,
                                ca.id,
                                ca.aluno_codigo,
                                ca.comunicado_id,
                                ca.excluido,
                                ca.criado_em,
                                ca.alterado_em,
                                ca.criado_por,
                                ca.alterado_por,
                                ca.criado_rf,
                                ca.alterado_rf,
                                g.nome as Grupo,
                                g.id as GrupoId");
            else
                campos.Append(@"cg.id AS ComunicadoGrupoId,
                                cg.comunicado_id,
                                cg.grupo_comunicado_id,
                                g.id as GrupoId,
                                g.id,
                                g.nome");

            return campos.ToString();
        }
    }
}