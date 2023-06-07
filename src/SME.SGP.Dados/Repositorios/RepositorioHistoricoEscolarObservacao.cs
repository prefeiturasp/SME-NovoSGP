using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoEscolarObservacao : RepositorioBase<HistoricoEscolarObservacao>, IRepositorioHistoricoEscolarObservacao
    {
        public RepositorioHistoricoEscolarObservacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<HistoricoEscolarObservacao> ObterPorCodigoAlunoAsync(string alunoCodigo)
        {
            var query = @"select id, aluno_codigo, observacao, criado_em, criado_por, alterado_em, alterado_por, alterado_rf, criado_rf
                          from historico_escolar_observacao 
                          where aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<HistoricoEscolarObservacao>(query, new { alunoCodigo });
        }

        public Task<IEnumerable<HistoricoEscolarObservacao>> ObterPorCodigosAlunosAsync(string[] codigosAlunos)
        {
            var query = @"select id, aluno_codigo, observacao, criado_em, criado_por, alterado_em, alterado_por, alterado_rf, criado_rf
                        from historico_escolar_observacao 
                        where aluno_codigo = any(@codigosAlunos)";

            return database.Conexao.QueryAsync<HistoricoEscolarObservacao>(query, new { codigosAlunos });
        }
    }
}
