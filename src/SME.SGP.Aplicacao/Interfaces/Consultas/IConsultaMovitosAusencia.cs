using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultaMovitosAusencia
    {
        Task<IEnumerable<MotivoAusencia>> Listar();
    }
}
