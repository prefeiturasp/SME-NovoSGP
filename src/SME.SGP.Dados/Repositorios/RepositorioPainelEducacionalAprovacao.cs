using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAprovacao : IRepositorioPainelEducacionalAprovacao
    {
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalAprovacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ConsolidacaoAprovacaoDto>> ObterIndicadores(long[] turmasIds)
        {
            var sql = @"select 
                                cccat.turma_id as TurmaId,
                                cccat.aluno_codigo as CodigoAluno,        
                                cccat.parecer_conclusivo_id as ParecerConclusivoId,
                                ccp.nome as ParecerDescricao,
                                ccpa.ano_turma as SerieAno
                        from consolidado_conselho_classe_aluno_turma cccat
                        inner join conselho_classe_parecer ccp  on cccat.parecer_conclusivo_id = ccp.id
                        where cccat.turma_id = any(@turmasIds)
                        and cccat.excluido = false
                        and cccat.parecer_conclusivo_id is not null 
                        and status = 2"
            ;

            return await database.QueryAsync<ConsolidacaoAprovacaoDto>(sql, new { turmasIds });
        }
    }
}
