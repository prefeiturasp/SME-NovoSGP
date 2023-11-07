using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCadastroAcessoABAEConsulta : RepositorioBase<CadastroAcessoABAE>, IRepositorioCadastroAcessoABAEConsulta
    {
        public RepositorioCadastroAcessoABAEConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {}

        public Task<bool> ExisteCadastroAcessoABAEPorCpf(string cpf, long ueId)
        {
            return database.Conexao.QueryFirstOrDefaultAsync<bool>("select 1 from cadastro_acesso_abae where cpf = @cpf and not excluido and ue_id = @ueId", new {cpf, ueId });
        }

        public async Task<PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>> ObterPaginado(FiltroDreIdUeIdNomeSituacaoABAEDto filtro, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, filtro);

            var parametros = new {ueId = filtro.UeId, nome = filtro.Nome, situacao = filtro.Situacao};
            
            var retorno = new PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<DreUeNomeSituacaoTipoEscolaDataABAEDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int) Math.Ceiling((double) retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, FiltroDreIdUeIdNomeSituacaoABAEDto filtro)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, filtro);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, filtro, true);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, FiltroDreIdUeIdNomeSituacaoABAEDto filtro, bool ehContador = false)
        {
            ObterCabecalho(sql, ehContador);

            ObterFiltro(sql, filtro);

            if (!ehContador)
                sql.AppendLine(" order by coalesce(a.alterado_em, a.criado_em) desc ");

            if (paginacao.QuantidadeRegistros > 0 && !ehContador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }
        
        private static void ObterCabecalho(StringBuilder sql, bool EhContador)
        {
            var query = EhContador 
                            ? "select distinct count(a.id) " 
                            : @"select a.id, dre.nome as dre,
                              ue.tipo_escola tipoEscola,
                              ue.nome as Ue,
                              a.nome,
                              a.situacao,
                              coalesce(a.alterado_em, a.criado_em) as Data ";
                
            sql.AppendLine(query);

            query = @"from cadastro_acesso_abae a
                        join ue on ue.id = a.ue_id
                        join dre on dre.id = ue.dre_id ";
            
            sql.AppendLine(query);
        }

        private static void ObterFiltro(StringBuilder sql, FiltroDreIdUeIdNomeSituacaoABAEDto filtro)
        {
            sql.AppendLine(" where not excluido and a.situacao = @situacao ");

            if (filtro.UeId.EhMaiorQueZero())
                sql.AppendLine(" and ue.id = @ueId ");

            if (filtro.Nome.EstaPreenchido())
                sql.AppendLine(" and lower(f_unaccent(a.nome)) LIKE ('%' || lower(f_unaccent(@nome)) || '%') ");
        }
    }
}