using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrenciaAluno : RepositorioBase<OcorrenciaAluno>, IRepositorioOcorrenciaAluno
    {
        public RepositorioOcorrenciaAluno(ISgpContext conexao) : base(conexao) { }

        public override async Task<long> SalvarAsync(OcorrenciaAluno entidade)
        {
            if (entidade.Id > 0)
            {
                await database.Conexao.UpdateAsync(entidade);
                await AuditarAsync(entidade.Id, "A");
            }
            else
            {
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
                await AuditarAsync(entidade.Id, "I");
            }

            return entidade.Id;
        }
    }
}
