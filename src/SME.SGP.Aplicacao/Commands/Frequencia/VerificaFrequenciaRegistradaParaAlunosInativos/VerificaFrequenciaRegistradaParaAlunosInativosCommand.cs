using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaFrequenciaRegistradaParaAlunosInativosCommand : IRequest<bool>
    {
        public string TurmaCodigo { get; set; }
    }
}
