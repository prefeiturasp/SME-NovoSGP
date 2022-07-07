//using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
//using System.Collections.Generic;
using System.Threading.Tasks;

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

            return await database.QueryFirstOrDefaultAsync<double>(sql, new { turmaFechamentoId, disciplinaId, codigoAluno });
        }
        //public void SalvarListaNotaConceito(List<NotaConceito> entidade)
        //{
        //    var sql = @"copy notas_conceito (
        //                        atividade_avaliativa,
	       //                     aluno_id,
	       //                     nota,
	       //                     conceito,
	       //                     tipo_nota,
	       //                     criado_por,
	       //                     criado_rf,
	       //                     criado_em,
	       //                     alterado_por,
	       //                     alterado_rf,
	       //                     alterado_em,
	       //                     disciplina_id,
	       //                     status_gsa)
        //               from
        //               stdin (FORMAT binary)";
        //    using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
        //    {
        //        foreach (var item in entidade)
        //        {
        //            writer.StartRow();
        //            writer.Write(item.AtividadeAvaliativaID);
        //            writer.Write(item.AlunoId);
        //            writer.Write(item.Nota);
        //            writer.Write(item.ConceitoId);
        //            writer.Write(item.TipoNota);
        //            writer.Write(item.CriadoEm);
        //            writer.Write(item.CriadoPor ?? "Sistema");
        //            writer.Write(item.CriadoRF ?? "Sistema");
        //            writer.Write(item.DisciplinaId);
        //            writer.Write(item.StatusGsa);

        //        }
        //        writer.Complete();
        //    }
        //}
    }
}