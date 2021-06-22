using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresEventoLiberacaoBoletimQuery : IRequest<int[]>
    {
        public ObterBimestresEventoLiberacaoBoletimQuery(DateTime dataRefencia)
        {
            DataRefencia = dataRefencia;
        }

        public DateTime DataRefencia { get; set; }
    }
}
