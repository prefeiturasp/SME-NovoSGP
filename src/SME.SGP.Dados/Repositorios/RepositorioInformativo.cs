using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Informes;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInformativo : RepositorioBase<Informativo>, IRepositorioInformativo
    {
        public RepositorioInformativo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<Informativo> ObterInformes(long id)
        {
            var sql = new StringBuilder();
            sql.AppendLine(ObterQueryInforme());
            sql.AppendLine(" WHERE inf.id = @id");

            var informativo = new Informativo();

            await database.Conexao
                .QueryAsync<Informativo, Dre, Ue, InformativoPerfil, Informativo>(
                    sql.ToString(), (informes, dre, ue, informativoPerfil) =>
                    {
                        if (informativo.Id == 0)
                        {
                            informativo = informes;
                            informativo.Dre = dre;
                            informativo.Ue = ue;
                            informativo.Perfis = new List<InformativoPerfil>();
                        }

                        informativo.Perfis.Add(informativoPerfil);

                        return informativo;
                    }, new { id });

            return informativo;
        }

        public async Task<bool> RemoverAsync(long id)
        {
            var query = @"delete from informativo where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }

        public async Task<PaginacaoResultadoDto<Informativo>> ObterInformesPaginado(InformeFiltroDto filtro, Paginacao paginacao)
        {
            var parametros = new
            {
                filtro.DreId,
                filtro.UeId,
                filtro.DataEnvioInicio,
                filtro.DataEnvioFim,
                titulo = string.IsNullOrEmpty(filtro.Titulo) ? string.Empty : $"%{filtro.Titulo.ToUpper()}%",
                perfils = filtro.Perfis.NaoEhNulo() ? filtro.Perfis.ToArray() : null
            };

            var totalRegistros = await database.Conexao.QueryFirstOrDefaultAsync<int>(ObterQueryInformesQuantitativo(filtro), parametros);
            var retorno = new PaginacaoResultadoDto<Informativo>();
            var itens = new List<Informativo>();
            var informativo = new Informativo();
            var queryPaginado = ObterQueryInformesPaginado(filtro, paginacao);

            await database.Conexao
                .QueryAsync<Informativo, Dre, Ue, InformativoPerfil, Informativo>(
                    queryPaginado, (informes, dre, ue, informativoPerfil) =>
                    {
                        if (informativo.Id != informes.Id)
                        {
                            informativo = informes;
                            informativo.Dre = dre;
                            informativo.Ue = ue;
                            informativo.Perfis = new List<InformativoPerfil>();
                            itens.Add(informativo);
                        }

                        informativo.Perfis.Add(informativoPerfil);

                        return informativo;
                    }, parametros);

            retorno.Items = itens;
            retorno.TotalRegistros = totalRegistros;
            retorno.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string ObterQueryInformesPaginado(InformeFiltroDto filtro, Paginacao paginacao)
        {
            var sql = new StringBuilder();
            sql.AppendLine(ObterQueryInforme());
            sql.AppendLine(" WHERE 1 = 1");
            sql.AppendLine(ObterQueryFiltro(filtro));
            sql.AppendLine(" ORDER BY inf.id");
            sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");

            return sql.ToString();
        }

        private string ObterQueryInformesQuantitativo(InformeFiltroDto filtro)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT COUNT(distinct inf.id) 
                            FROM informativo inf
                            INNER JOIN informativo_perfil inf_p ON inf_p.informativo_id = inf.id
                            LEFT JOIN dre ON dre.id = inf.dre_id
                            LEFT JOIN ue ON ue.id = inf.ue_id");
            sql.AppendLine(" WHERE 1 = 1");
            sql.AppendLine(ObterQueryFiltro(filtro));

            return sql.ToString();
        }

        private string ObterQueryInforme()
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT inf.id, inf.titulo, inf.texto, inf.data_envio,
                            inf.criado_em, inf.criado_por, inf.alterado_em,
                            inf.alterado_por, inf.criado_rf, inf.alterado_rf,
                            dre.id, dre.nome, dre.abreviacao,
                            ue.id, ue.nome, ue.tipo_escola,
                            inf_p.id, inf_p.informativo_id, inf_p.codigo_perfil
                            FROM informativo inf
                            INNER JOIN informativo_perfil inf_p ON inf_p.informativo_id = inf.id
                            LEFT JOIN dre ON dre.id = inf.dre_id
                            LEFT JOIN ue ON ue.id = inf.ue_id ");

            return sql.ToString();
        }

        private string ObterQueryFiltro(InformeFiltroDto filtro)
        {
            var sql = new StringBuilder();
            sql.AppendLine(filtro.DreId.HasValue ? " AND dre.id = @dreId" : string.Empty);
            sql.AppendLine(filtro.UeId.HasValue ? " AND ue.id = @ueId" : string.Empty);
            sql.AppendLine(filtro.DataEnvioInicio.HasValue ? " AND inf.data_envio BETWEEN @dataEnvioInicio::date AND @dataEnvioFim::date" : string.Empty);
            sql.AppendLine(filtro.Perfis.NaoEhNulo() && filtro.Perfis.Any() ? " AND inf_p.codigo_perfil = ANY(@perfils)" : string.Empty);
            sql.AppendLine(!string.IsNullOrEmpty(filtro.Titulo) ? " AND upper(f_unaccent(inf.titulo)) LIKE @titulo" : string.Empty);

            return sql.ToString();
        }
    }
}
