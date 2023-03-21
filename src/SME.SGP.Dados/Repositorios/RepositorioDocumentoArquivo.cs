using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

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

        public async Task<IEnumerable<DocumentoArquivoDto>> ObterDocumentosArquivosPorDocumentoIdAsync(long documentoId)
        {
            const string query = @"select da.Id, da.documento_id documentoId, da.arquivo_id arquivoId, a.codigo  
                                   from documento_arquivo da 
                                     join arquivo a on da.arquivo_id = a.id 
                                    where documento_id = @documentoId";

            return await database.Conexao.QueryAsync<DocumentoArquivoDto>(query, new { documentoId });
        }
    }
}