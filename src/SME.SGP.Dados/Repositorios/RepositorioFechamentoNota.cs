using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoNota : RepositorioBase<FechamentoNota>, IRepositorioFechamentoNota
    {
        public RepositorioFechamentoNota(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        const string queryPorFechamento = @"with lista as (select fa.aluno_codigo as AlunoCodigo
                                            , n.disciplina_id as ComponenteCurricularId
                                            , coalesce(wf.nota, n.nota) as Nota
                                            , coalesce(wf.conceito_id, n.conceito_id) as ConceitoId
                                            , pe.bimestre
                                            , wf.id as EmAprovacao
                                            , row_number() over (partition by ft.turma_id, fa.aluno_codigo, pe.id, n.disciplina_id order by n.id desc) sequencia
                                         from fechamento_nota n
                                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                                        inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                                        inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                                         left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                         left join wf_aprovacao_nota_fechamento wf on wf.fechamento_nota_id = n.id and not wf.excluido
                                        where fa.fechamento_turma_disciplina_id = ANY(@fechamentosTurmaDisciplinaId) and not n.excluido";

        public Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentosTurma(long[] fechamentosTurmaDisciplinaId)
        {
            var query = string.Concat(queryPorFechamento, ") select * from lista where sequencia = 1;");
            return database.Conexao.QueryAsync<FechamentoNotaAlunoAprovacaoDto>(query, new { fechamentosTurmaDisciplinaId });
        }
    }
}