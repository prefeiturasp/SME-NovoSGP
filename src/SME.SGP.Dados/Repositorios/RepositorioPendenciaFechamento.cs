using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamento : RepositorioBase<PendenciaFechamento>, IRepositorioPendenciaFechamento
    {
        public RepositorioPendenciaFechamento(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> ListarPaginada(Paginacao paginacao, string turmaCodigo, int bimestre, long componenteCurricularId)
        {
            var retorno = new PaginacaoResultadoDto<PendenciaFechamentoResumoDto>();

            var query = new StringBuilder(MontaQuery(paginacao, bimestre, componenteCurricularId));
            query.AppendLine(";");
            query.AppendLine(MontaQuery(paginacao, bimestre, componenteCurricularId, true));

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new {turmaCodigo, bimestre, componenteCurricularId }))
            {
                retorno.Items = multi.Read<PendenciaFechamentoResumoDto>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            var query = @"select p.id as PendenciaId, p.titulo as descricao, p.descricao as detalhamento
                                , p.situacao, ftd.disciplina_id as DisciplinaId, pe.bimestre, pf.fechamento_turma_disciplina_id as FechamentoId
                                , p.criado_em as CriadoEm, p.criado_por as CriadoPor, p.criado_rf as CriadoRf, p.alterado_em as AlteradoEm, p.alterado_por as AlteradoPor, p.alterado_rf as AlteradoRf
                          from pendencia_fechamento pf 
                         inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
                         inner join turma t on t.id = ftd.turma_id 
                         inner join periodo_escolar pe on pe.id = ftd.periodo_escolar_id 
                         inner join pendencia p on p.id = pf.pendencia_id  
                         where p.id = @pendenciaId";

            return await database.Conexao.QueryFirstAsync<PendenciaFechamentoCompletoDto>(query, new { pendenciaId });
        }

        public bool VerificaPendenciasAbertoPorFechamento(long fechamentoId)
        {
            var query = @"select count(p.id) 
                      from pendencia_fechamento pf 
                     inner join pendencia p on p.id = pf.pendencia_id 
                     where pf.fechamento_turma_disciplina_id = @fechamentoId
                       and p.situacao = 1";

            return database.Conexao.QueryFirst<int>(query, new { fechamentoId }) > 0;
        }

        private string MontaQuery(Paginacao paginacao, int bimestre, long componenteCurricularId, bool contador = false)
        {
            var fields = contador ? "count(p.id)" : "p.id as PendenciaId, p.titulo as descricao, p.situacao, ftd.disciplina_id as DisciplinaId";
            var query = new StringBuilder(string.Format(@"select {0}
                                  from pendencia_fechamento pf 
                                 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
                                 inner join turma t on t.id = ftd.turma_id 
                                 inner join periodo_escolar pe on pe.id = ftd.periodo_escolar_id 
                                 inner join pendencia p on p.id = pf.pendencia_id 
                                  where t.turma_id = @turmaCodigo ", fields));
            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");
            if (componenteCurricularId > 0)
                query.AppendLine(" and ftd.disciplina_id = @componenteCurricularId");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");

            return query.ToString();
        }
    }
}