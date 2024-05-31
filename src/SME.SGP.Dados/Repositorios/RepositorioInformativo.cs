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
                .QueryAsync<Informativo, Dre, Ue, InformativoPerfil, InformativoModalidade, Informativo>(
                    sql.ToString(), (informes, dre, ue, informativoPerfil, informativoModalidade) =>
                    {
                        if (informativo.Id == 0)
                        {
                            informativo = informes;
                            informativo.Dre = dre;
                            informativo.Ue = ue;
                            informativo.Perfis = new List<InformativoPerfil>();
                            informativo.Modalidades = new List<InformativoModalidade>();
                        }

                        if (informativoPerfil.NaoEhNulo() &&
                            !informativo.Perfis.Exists(perfil => perfil.Id == informativoPerfil.Id))
                            informativo.Perfis.Add(informativoPerfil);

                        if (informativoModalidade.NaoEhNulo() &&
                            !informativo.Modalidades.Exists(modalidade => modalidade.Id == informativoModalidade.Id))
                            informativo.Modalidades.Add(informativoModalidade);

                        return informativo;
                    }, new { id });

            return informativo;
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
            var queryPaginado = ObterQueryInformesPaginado(filtro);

            await database.Conexao
                .QueryAsync<Informativo, Dre, Ue, InformativoPerfil, InformativoModalidade, Informativo>(
                    queryPaginado, (informes, dre, ue, informativoPerfil, informativoModalidade) =>
                    {
                        if (informativo.Id != informes.Id)
                        {
                            informativo = informes;
                            informativo.Dre = dre;
                            informativo.Ue = ue;
                            informativo.Perfis = new List<InformativoPerfil>();
                            informativo.Modalidades = new List<InformativoModalidade>();
                            itens.Add(informativo);
                        }

                        if (informativoPerfil.NaoEhNulo() &&
                            !informativo.Perfis.Exists(perfil => perfil.Id == informativoPerfil.Id))
                            informativo.Perfis.Add(informativoPerfil);

                        if (informativoModalidade.NaoEhNulo() &&
                            !informativo.Modalidades.Exists(modalidade => modalidade.Id == informativoModalidade.Id))
                            informativo.Modalidades.Add(informativoModalidade);

                        return informativo;
                    }, parametros);

            retorno.Items = itens.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros);
            retorno.TotalRegistros = totalRegistros;
            retorno.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string ObterQueryInformesPaginado(InformeFiltroDto filtro)
        {
            var sql = new StringBuilder();
            sql.AppendLine(ObterQueryInforme());
            sql.AppendLine(" WHERE 1 = 1");
            sql.AppendLine(ObterQueryFiltro(filtro));
            sql.AppendLine(" ORDER BY inf.id");

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
                            inf.criado_em, inf.criado_por, inf.alterado_em, inf.dre_id, inf.ue_id,
                            inf.alterado_por, inf.criado_rf, inf.alterado_rf,
                            dre.id, dre.nome, dre.abreviacao, dre.dre_id as CodigoDre,
                            ue.id, ue.nome, ue.tipo_escola, ue.ue_id as CodigoUe,
                            inf_p.id, inf_p.informativo_id, inf_p.codigo_perfil,
                            inf_m.id, inf_m.informativo_id, inf_m.modalidade_codigo
                            FROM informativo inf
                            INNER JOIN informativo_perfil inf_p ON inf_p.informativo_id = inf.id
                            LEFT JOIN informativo_modalidade inf_m ON inf_m.informativo_id = inf.id
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
            sql.AppendLine(filtro.Perfis.NaoEhNulo() && filtro.Perfis.Any() ? " AND EXISTS (SELECT 1 FROM informativo_perfil inf_p_i WHERE inf_p_i.codigo_perfil = ANY(@perfils) AND inf_p.informativo_id = inf_p_i.informativo_id)" : string.Empty);
            sql.AppendLine(!string.IsNullOrEmpty(filtro.Titulo) ? " AND upper(f_unaccent(inf.titulo)) LIKE @titulo" : string.Empty);
            sql.AppendLine(" AND not inf.excluido AND not inf_p.excluido ");

            return sql.ToString();
        }

        public async Task<bool> InformeFoiExcluido(long id)
        {
            return await database.Conexao.ExecuteScalarAsync<bool>("select count(1) from informativo where id=@id and excluido", new { id });
        }
    }
}
