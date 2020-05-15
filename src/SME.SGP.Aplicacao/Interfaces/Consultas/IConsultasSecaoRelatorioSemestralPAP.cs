using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasSecaoRelatorioSemestralPAP
    {
        Task<IEnumerable<SecaoRelatorioSemestralPAP>> ObterSecoesVigentes(DateTime dataReferencia);
    }
}
