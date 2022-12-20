using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDocumentoArquivo : IRepositorioDocumentoArquivo
    {
        protected readonly ISgpContext database;

        public RepositorioDocumentoArquivo(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(DocumentoArquivo documentoArquivo)
        {
            if (documentoArquivo.Id > 0)
                await database.Conexao.UpdateAsync(documentoArquivo);
            else
                documentoArquivo.Id = (long)await database.Conexao.InsertAsync(documentoArquivo);

            return documentoArquivo.Id;
        }
    }
}