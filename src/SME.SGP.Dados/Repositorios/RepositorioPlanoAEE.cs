using Dapper;
using Elastic.Apm.Api;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao)
        {
            var query = @"update plano_aee
                           set situacao = @situacao
                          where id in (select plano_aee_id from plano_aee_versao where id = @versaoId) ";

            return await database.Conexao.ExecuteAsync(query, new { versaoId, situacao });
        }        

        public async Task<int> AtualizarTurmaParaRegularPlanoAEE(long planoAEEId, long turmaId)
        {
            var query = @"update plano_aee 
                            set turma_id = @turmaId 
                            where id = @planoAEEId";

            return await database.Conexao.ExecuteAsync(query, new { planoAEEId, turmaId });                  
        }

        public Task<int> ObterQdadePlanosAEEAtivosAluno(string codigoAluno)
        => database.Conexao.QueryFirstOrDefaultAsync<int>(@$"select count(pa.id) from plano_aee pa 
                                            where pa.aluno_codigo = @codigoAluno 
                                            and not pa.excluido 
                                            and not situacao in({(int)SituacaoPlanoAEE.Encerrado}, {(int)SituacaoPlanoAEE.EncerradoAutomaticamente})", 
            new { codigoAluno });                  
    }
}
