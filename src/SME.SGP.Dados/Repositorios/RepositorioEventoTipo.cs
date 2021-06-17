using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoTipo : RepositorioBase<EventoTipo>, IRepositorioEventoTipo
    {
        public RepositorioEventoTipo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<EventoTipo>> ListarTipos(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Guid perfilCodigo, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<EventoTipo>();

            StringBuilder sql = MontaQueryCompleta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao);

            var parametros = new 
            { 
                local_ocorrencia = eventoLocalOcorrencia, 
                letivo = eventoLetivo, 
                descricao = $"%{descricao?.ToLowerInvariant()}%",
                perfilCodigo
            };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<EventoTipo>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        public EventoTipo ObtenhaTipoEventoFeriado()
        {
            var sql = "select * from evento_tipo where descricao = @descricao";

            return database.Conexao.QuerySingleOrDefault<EventoTipo>(sql, new { descricao = "Feriado" });
        }

        public EventoTipo ObterTipoEventoPorTipo(TipoEvento tipoEvento)
        {
            var sql = "select * from evento_tipo where codigo = @tipoEvento";

            return database.Conexao.QuerySingleOrDefault<EventoTipo>(sql, new { tipoEvento });
            
        }

        public EventoTipo ObterPorCodigo(long id)
        {
            var sql = "select * from evento_tipo where codigo = @id";

            return database.Conexao.QuerySingleOrDefault<EventoTipo>(sql, new { id });
        }

        private static StringBuilder MontaQueryCompleta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao, sql, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao, sql, contador: true);
            return sql;
        }

        private static void MontaQueryConsulta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao, StringBuilder sql, bool contador)
        {
            ObtenhaCabecalhoConsulta(sql, contador);

            ObtenhaFiltroConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, sql);

            if (!contador)
                sql.AppendLine("order by et.id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
        }

        private static void ObtenhaCabecalhoConsulta(StringBuilder sql, bool contador)
        {
            sql.AppendLine($"select {(contador ? "count(et.*)" : "et.*")} from evento_tipo et");
            sql.AppendLine($"where not et.excluido");
        }

        private static void ObtenhaFiltroConsulta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, StringBuilder sql)
        {
            if (eventoLocalOcorrencia != 0)
                sql.AppendLine("and local_ocorrencia = @local_ocorrencia");

            if (eventoLetivo != 0)
                sql.AppendLine("and letivo = @letivo");

            if (!string.IsNullOrWhiteSpace(descricao))
                sql.AppendLine("and lower(f_unaccent(descricao)) like @descricao");

            sql.AppendLine(@" and (
                -- Tem acesso exclusivo ao tipo e permite cadastro
                exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = :perfilCodigo and pet.evento_tipo_id = et.id and pet.exclusivo)
                --ou (não tem acesso exclusivo pro perfil
                  or (not exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = :perfilCodigo and pet.exclusivo)
                  --e (tem permissao de cadastro pro tipo evento
                      and (exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.codigo_perfil = :perfilCodigo and pet.evento_tipo_id = et.id and pet.permite_cadastro)
                  --ou nao tem configuracao pro tipo evento))
                      or not exists (select 1 from perfil_evento_tipo pet where not pet.excluido and pet.evento_tipo_id = et.id))
                  ))");
        }

        public async Task<long> ObterIdPorCodigo(int codigo)
        {
            var query = "select id from evento_tipo where codigo = @codigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { codigo });
        }
    }
}