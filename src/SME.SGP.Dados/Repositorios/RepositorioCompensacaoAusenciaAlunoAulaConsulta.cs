using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaAlunoAulaConsulta : RepositorioBase<CompensacaoAusenciaAlunoAula>, IRepositorioCompensacaoAusenciaAlunoAulaConsulta
    {
        public RepositorioCompensacaoAusenciaAlunoAulaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoAulaDto>> ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidade(long aulaId, int quantidade)
        {
            var query = @"select caaa.id, caaa.compensacao_ausencia_aluno_id
                          from registro_frequencia_aluno rfa 
                              join compensacao_ausencia_aluno_aula caaa on caaa.registro_frequencia_aluno_id = rfa.id 
                              join aula a on a.id = rfa.aula_id 
                              join turma t on t.turma_id = a.turma_id 
                          where rfa.aula_id = @aulaId 
                                and caaa.numero_aula > @quantidade 
                                and not caaa.excluido";
            
            return await database.Conexao.QueryAsync<CompensacaoAusenciaAlunoAulaDto>(query, new { aulaId, quantidade });
        }
    }
}