using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasSecaoRelatorioSemestral
    {
        Task<IEnumerable<SecaoRelatorioSemestral>> ObterSecoesVigentes(DateTime dataReferencia);
    }
}
