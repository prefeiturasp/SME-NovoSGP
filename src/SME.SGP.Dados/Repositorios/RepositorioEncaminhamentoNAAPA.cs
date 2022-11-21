using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoNAAPA>, IRepositorioEncaminhamentoNAAPA
    {
        public RepositorioEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> ListarPaginado(bool exibirHistorico, 
            int anoLetivo, long dreId, string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, int prioridade, string[] turmasCodigos, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, exibirHistorico, anoLetivo, dreId, codigoUe, 
                nomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim, situacao,prioridade , turmasCodigos);

            var parametros = new { anoLetivo, codigoUe, dreId, nomeAluno, turmasCodigos, situacao, 
                prioridade = prioridade.ToString(), dataAberturaQueixaInicio, dataAberturaQueixaFim };
            
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<EncaminhamentoNAAPAResumoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
        private string MontaQueryCompleta(Paginacao paginacao, bool exibirHistorico, int anoLetivo, long dreId, 
            string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, int prioridade, string[] turmasCodigos)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasCodigos);
            
            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasCodigos);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string nomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, int prioridade, 
            string[] turmasCodigos)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, nomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim,situacao, prioridade, turmasCodigos);
            
            if (!contador && (dataAberturaQueixaInicio.HasValue || dataAberturaQueixaFim.HasValue))
                sql.AppendLine(" order by to_date(enr.texto,'dd/MM/YYYY') desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine("count(np.id) ");
            else
            {
                sql.AppendLine(@"np.id EncaminhamentoNAAPAId
                                ,ue.nome UeNome 
                                ,ue.tipo_escola TipoEscola
                                ,np.aluno_codigo as CodigoAluno
                                ,np.aluno_nome as NomeAluno 
                                ,np.situacao 
                                ,case when q.nome = 'Prioridade' then enr.texto else null end Prioridade 
                                ,case when q.nome = 'Data de entrada da queixa' then enr.texto else null end DataAberturaQueixaInicio 
                ");
            }

            sql.AppendLine(@" from encaminhamento_naapa np              
                                join turma t on t.id = np.turma_id
                                join ue on t.ue_id = ue.id
                                join encaminhamento_naapa_secao ens on np.id = ens.encaminhamento_naapa_id  
                                join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id 
                                join questao q on enq.questao_id = q.id 
                                join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
            ");
        }

        private void ObterFiltro(StringBuilder sql, string nomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, int prioridade, string[] turmasCodigos)
        {
            sql.AppendLine(@" where not np.excluido 
                                    and t.ano_letivo = @anoLetivo 
                                    and ue.ue_id = @codigoUe 
                                    and ue.dre_Id = @dreId");

            if (!string.IsNullOrEmpty(nomeAluno))
                sql.AppendLine(" and enp.aluno_nome = @nomeAluno ");
            
            if (turmasCodigos.Any())
                sql.AppendLine(" and t.id = ANY(@turmasCodigos) ");
            
            if (situacao > 0)
                sql.AppendLine(" and np.situacao = @situacao ");
            
            if (prioridade > 0)
                sql.AppendLine(" and q.nome = 'Prioridade' and enr.texto = @prioridade ");

            if (dataAberturaQueixaInicio.HasValue || dataAberturaQueixaFim.HasValue)
            {
                sql.AppendLine(" and q.nome = 'Data de entrada da queixa' ");
               
                if (dataAberturaQueixaInicio.HasValue)
                    sql.AppendLine(" and to_date(enr.texto,'dd/MM/YYYY') >= @dataAberturaQueixaInicio ");
                
                if (dataAberturaQueixaFim.HasValue)
                    sql.AppendLine(" and to_date(enr.texto,'dd/MM/YYYY') <= @dataAberturaQueixaFim  ");
            }
        }
    }
}
