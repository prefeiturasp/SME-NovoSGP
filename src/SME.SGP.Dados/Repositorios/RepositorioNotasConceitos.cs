using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotasConceitos : RepositorioBase<NotaConceito>, IRepositorioNotasConceitos
    {
        public RepositorioNotasConceitos(ISgpContext sgpContext, IServicoAuditoria servicoAuditoria) : base(sgpContext, servicoAuditoria)
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
        public Task<bool> SalvarListaNotaConceito(IEnumerable<NotaConceito> entidade, Usuario criadoPor)
        {

            var lancaNota = entidade.First().Nota.HasValue;

            var sql = @$"copy notas_conceito (
                                atividade_avaliativa,
	                            aluno_id,
	                            {(lancaNota ? "nota" : "conceito")},
	                            tipo_nota,
	                            criado_por,
	                            criado_rf,
	                            criado_em,
	                            disciplina_id,
	                            status_gsa)
                       from
                       stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var item in entidade)
                {

                    writer.StartRow();
                    writer.Write(item.AtividadeAvaliativaID, NpgsqlDbType.Bigint);
                    writer.Write(item.AlunoId, NpgsqlDbType.Varchar);

                    if (lancaNota)
                        writer.Write((double)item.Nota, NpgsqlDbType.Numeric);
                    else
                        writer.Write((long)item.ConceitoId, NpgsqlDbType.Bigint);

                    writer.Write((long)item.TipoNota, NpgsqlDbType.Bigint);
                    writer.Write(item.CriadoPor ?? criadoPor.Nome);
                    writer.Write(item.CriadoRF ?? criadoPor.Login);
                    writer.Write(item.CriadoEm);
                    writer.Write(item.DisciplinaId, NpgsqlDbType.Varchar);
                    writer.Write((int)item.StatusGsa, NpgsqlDbType.Integer);
                }
                writer.Complete();
            }

            return Task.FromResult(true);
        }
    }
}