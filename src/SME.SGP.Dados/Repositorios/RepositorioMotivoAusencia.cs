using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMotivoAusencia : IRepositorioMotivoAusencia
    {
        private readonly ISgpContext contexto;

        public RepositorioMotivoAusencia(ISgpContext contexto)
        {
            this.contexto = contexto;
        }
      
        public async Task<IEnumerable<MotivoAusencia>> Listar()
        {
            return await contexto.Conexao.QueryAsync<MotivoAusencia>("select id, descricao from motivo_ausencia");
        }
    }
}
