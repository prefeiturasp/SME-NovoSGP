using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;
using Utilities;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoReflexoFrequenciaBuscaAtiva : RepositorioBase<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>, IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva
    {
        public RepositorioConsolidacaoReflexoFrequenciaBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirConsolidacoes(string ueCodigo, int mes, int anoLetivo)
        {
            await database.Conexao.ExecuteAsync("delete from consolidacao_reflexo_frequencia_busca_ativa where ue_id = @ueCodigo and mes = @mes and ano_letivo = @anoLetivo",
                                                new { ueCodigo, mes, anoLetivo } );
        }

        public Task<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno> ObterIdConsolidacao(string turmaCodigo, string alunoCodigo, int mes, int anoLetivo)
        {
            return database.Conexao.QueryFirstOrDefaultAsync<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>(@"select * from consolidacao_reflexo_frequencia_busca_ativa 
                                                        where turma_id = @turmaCodigo 
                                                              and aluno_codigo = @alunoCodigo
                                                              and mes = @mes
                                                              and ano_letivo = @anoLetivo;",
                                                new { turmaCodigo, alunoCodigo, mes, anoLetivo });
        }
    }
}