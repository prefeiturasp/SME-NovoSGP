using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDiarioBordo : RepositorioBase<PendenciaDiarioBordo>, IRepositorioPendenciaDiarioBordo
    {
        public RepositorioPendenciaDiarioBordo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task Excluir(long pendenciaId)
        {
            await database.Conexao.ExecuteScalarAsync("delete from pendencia_diario_bordo where id = @pendenciaId", new { pendenciaId }, commandTimeout: 60);
        }
    }
}
