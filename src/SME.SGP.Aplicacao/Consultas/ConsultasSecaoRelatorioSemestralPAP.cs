using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;

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
