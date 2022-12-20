using System.Collections.Generic;
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

        public async Task<IEnumerable<DocumentoArquivo>> ObterDocumentosArquivosPorDocumentoIdAsync(long documentoId)
        {
            const string query = @"select *
                                    from documento_arquivo
                                    where documento_id = @documentoId";

            return await database.Conexao.QueryAsync<DocumentoArquivo>(query, new { documentoId });
        }
    }
}