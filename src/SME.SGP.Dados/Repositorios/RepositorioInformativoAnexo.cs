using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInformativoAnexo : IRepositorioInformativoAnexo
    {
        protected readonly ISgpContext database;

        public RepositorioInformativoAnexo(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(InformativoAnexo informativoAnexo)
        {
            if (informativoAnexo.Id > 0)
                await database.Conexao.UpdateAsync(informativoAnexo);
            else
                informativoAnexo.Id = (long)await database.Conexao.InsertAsync(informativoAnexo);

            return informativoAnexo.Id;
        }

        public async Task<IEnumerable<InformativoAnexoDto>> ObterAnexosPorInformativoIdAsync(long informativoId)
        {
            const string query = @"select da.Id, da.informativo_id informativoId, da.arquivo_id arquivoId, a.codigo, a.nome  
                                   from informativo_anexo da 
                                     join arquivo a on da.arquivo_id = a.id 
                                    where informativo_id = @informativoId";

            return await database.Conexao.QueryAsync<InformativoAnexoDto>(query, new { informativoId });
        }
    }
}