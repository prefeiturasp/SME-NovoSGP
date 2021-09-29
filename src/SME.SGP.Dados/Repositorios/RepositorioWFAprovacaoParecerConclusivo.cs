using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoParecerConclusivo : IRepositorioWFAprovacaoParecerConclusivo
    {
        private readonly ISgpContext database;

        public RepositorioWFAprovacaoParecerConclusivo(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Salvar(WFAprovacaoParecerConclusivo entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }
    }
}
