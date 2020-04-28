using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestral : IConsultasRelatorioSemestral
    {
        public ConsultasRelatorioSemestral()
        {

        }

        public Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new NotImplementedException();
        }
    }
}
