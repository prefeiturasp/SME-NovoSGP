using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotasConceitos : RepositorioBase<NotaConceito>, IRepositorioNotasConceitos
    {
        public RepositorioNotasConceitos(ISgpContext sgpContext) : base(sgpContext)
        {
        }

        public async Task<double> ObterNotaEmAprovacao(string codigoAluno, long disciplinaId, long turmaFechamentoId)
        {
            var sql = $@"select coalesce(coalesce(w.nota,w.conceito_id),-1)
                            from fechamento_turma ft 
                            inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                            inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                            inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                            inner join wf_aprovacao_nota_fechamento w on w.fechamento_nota_id = fn.id
                            where ft.id = @turmaFechamentoId and fn.disciplina_id = @disciplinaId and fa.aluno_codigo = @codigoAluno
                        order by w.id desc";

            return await database.QueryFirstOrDefaultAsync<double>(sql, new { turmaFechamentoId,disciplinaId, codigoAluno });
        }
    }
}