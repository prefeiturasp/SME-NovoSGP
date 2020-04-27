using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ConsultasSecaoRelatorioSemestral : IConsultasSecaoRelatorioSemestral
    {
        public ConsultasSecaoRelatorioSemestral()
        {
        }

        public Task<IEnumerable<SecaoRelatorioSemestral>> ObterSecoesVigentes(DateTime dataReferencia)
        {
            throw new NotImplementedException();
        }
    }
}
