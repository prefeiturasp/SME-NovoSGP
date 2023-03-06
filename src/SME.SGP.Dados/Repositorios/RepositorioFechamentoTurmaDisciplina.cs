using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>,IRepositorioFechamentoTurmaDisciplina
    {

        public RepositorioFechamentoTurmaDisciplina(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> AtualizarSituacaoFechamento(long fechamentoTurmaDisciplinaId, int situacaoFechamento)
        {
            var query = @"update fechamento_turma_disciplina 
                             set situacao = @situacaoFechamento
                         where id = @fechamentoTurmaDisciplinaId";

            await database.Conexao.ExecuteAsync(query, new { fechamentoTurmaDisciplinaId, situacaoFechamento });
            return true;
        }
    }
}