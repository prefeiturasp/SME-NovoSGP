using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasSecaoRelatorioSemestralPAP : IConsultasSecaoRelatorioSemestralPAP
    {
        public ConsultasSecaoRelatorioSemestralPAP()
        {
        }

        public Task<IEnumerable<SecaoRelatorioSemestralPAP>> ObterSecoesVigentes(DateTime dataReferencia)
        {
            throw new NotImplementedException();
        }
    }
}
