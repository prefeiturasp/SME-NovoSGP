using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidadoNota : IRepositorioConselhoClasseConsolidadoNota
    {
        protected readonly ISgpContext database;

        public RepositorioConselhoClasseConsolidadoNota(ISgpContext database)
        {
            this.database = database;
        }

        public Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(long consolidadoTurmaAlunoId, int bimestre)
        {
            var query = $@" select id,consolidado_conselho_classe_aluno_turma_id,bimestre,nota,conceito_id,componente_curricular_id    
                            from consolidado_conselho_classe_aluno_turma_nota
                            where consolidado_conselho_classe_aluno_turma_id = @consolidadoTurmaAlunoId and bimestre = @bimestre";

            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAlunoNota>(query, new { consolidadoTurmaAlunoId, bimestre });
        }

        public async Task<long> SalvarAsync(ConselhoClasseConsolidadoTurmaAlunoNota consolidadoNota)
        {
            try
            {
                if (consolidadoNota.Id > 0)
                {
                    var sucesso = await database.Conexao.UpdateAsync(consolidadoNota);
                    return sucesso ? consolidadoNota.Id : 0;
                }
                else
                    return (long)(await database.Conexao.InsertAsync(consolidadoNota));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
