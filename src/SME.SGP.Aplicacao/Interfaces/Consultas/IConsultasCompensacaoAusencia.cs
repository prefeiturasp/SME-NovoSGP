using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCompensacaoAusencia
    {
        Task<IEnumerable<CompensacaoAusencia>> Listar(string disciplinaId, int bimestre, string nomeAtividade);
    }
}
