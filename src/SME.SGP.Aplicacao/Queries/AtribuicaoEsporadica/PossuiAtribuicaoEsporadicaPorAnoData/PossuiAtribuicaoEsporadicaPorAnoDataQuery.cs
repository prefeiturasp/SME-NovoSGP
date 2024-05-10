using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class PossuiAtribuicaoEsporadicaPorAnoDataQuery : IRequest<bool>
    {
        public PossuiAtribuicaoEsporadicaPorAnoDataQuery(int? anoLetivo, string dreCodigo, string ueCodigo, string professorRf, DateTime? data)
        {
            AnoLetivo = anoLetivo;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            ProfessorRf = professorRf;
            Data = data;
        }

        public int? AnoLetivo { get; set; }

        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }

        public string ProfessorRf { get; set; }

        public DateTime? Data { get; set; }
    }
}
