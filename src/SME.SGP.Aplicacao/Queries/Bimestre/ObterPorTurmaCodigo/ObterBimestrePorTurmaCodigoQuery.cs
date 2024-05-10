using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestrePorTurmaCodigoQuery : IRequest<int>
    {
        public ObterBimestrePorTurmaCodigoQuery(string turmaCodigo, DateTime data)
        {
            TurmaCodigo = turmaCodigo;
            Data = data;
        }

        public string TurmaCodigo { get; set; }
        public DateTime Data { get; set; }
    }
}
