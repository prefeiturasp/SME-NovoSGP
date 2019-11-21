using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplina(string turma, string disciplina)
        {
            var query = @"select professor_id, quantidade, data_aula 
                 from aula 
                where not excluido 
                  and turma_id = @turma 
                  and disciplina_id = @disciplina";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                disciplina
            });
        }

        public bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade)
        {
            var query = new StringBuilder("select 1 from v_abrangencia where turma_id = @turmaId and ue_codigo = @ueId ");

            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                query.AppendLine($"and modalidade_codigo = {(int)Modalidade.EJA} ");
            }
            else
            {
                query.AppendLine($"and (modalidade_codigo = {(int)Modalidade.Fundamental} or modalidade_codigo = {(int)Modalidade.Medio}) ");
            }

            return database.Conexao.QueryFirstOrDefault<bool>(query.ToString(), new
            {
                aula.TurmaId,
                aula.UeId
            });
        }

        public Aula ObterPorWorkflowId(long workflowId)
        {
            var query = @"select a.id,
                                 a.ue_id,
                                 a.disciplina_id,
                                 a.turma_id,
                                 a.tipo_calendario_id,
                                 a.professor_id,
                                 a.quantidade,
                                 a.data_aula,
                                 a.recorrencia_aula,
                                 a.tipo_aula,
                                 a.criado_em,
                                 a.criado_por,
                                 a.alterado_em,
                                 a.alterado_por,
                                 a.criado_rf,
                                 a.alterado_rf,
                                 a.excluido,
                                 a.migrado,
                                 a.aula_pai_id,
                                 a.wf_aprovacao_id,
                                 a.status
                             from  aula a
                            where a.excluido = false
                              and a.migrado = false
                              and tipo_aula = 2
                              and a.wf_aprovacao_id = @workflowId";

            return database.Conexao.QueryFirst<Aula>(query.ToString(), new
            {
                workflowId
            });
        }
    }
}