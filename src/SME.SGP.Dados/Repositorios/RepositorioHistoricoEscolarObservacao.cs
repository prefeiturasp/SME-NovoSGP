using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoEscolarObservacao : RepositorioBase<HistoricoEscolarObservacao>, IRepositorioHistoricoEscolarObservacao
    {
        public RepositorioHistoricoEscolarObservacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<HistoricoEscolarObservacaoDto> ObterPorCodigoAlunoAsync(string alunoCodigo)
        {
            var query = @"select aluno_codigo as AlunoCodigo, observacao as Observacao
                          from historico_escolar_observacao 
                          where aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<HistoricoEscolarObservacaoDto>(query, new { alunoCodigo });
        }
    }
}
