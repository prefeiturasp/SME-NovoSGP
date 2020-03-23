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

            var query = new StringBuilder(@"select p.descricao, p.situacao, ftd.disciplina_id as DisciplinaId
                                  from pendencia_fechamento pf 
                                 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
                                 inner join turma t on t.id = ftd.turma_id 
                                 inner join periodo_escolar pe on pe.id = ftd.periodo_escolar_id 
                                 inner join pendencia p on p.id = pf.pendencia_id 
                                  where t.turma_id = @turmaCodigo ");
            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");
            if (componenteCurricularId > 0)
                query.AppendLine(" and ftd.disciplina_id = @componenteCurricularId");

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new {turmaCodigo, bimestre, componenteCurricularId }))
            {
                retorno.Items = multi.Read<PendenciaFechamentoResumoDto>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
    }
}