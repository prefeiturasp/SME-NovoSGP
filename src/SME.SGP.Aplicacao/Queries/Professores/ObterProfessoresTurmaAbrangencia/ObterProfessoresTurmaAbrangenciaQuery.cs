using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTurmaAbrangenciaQuery : IRequest<IEnumerable<string>>
    {
        public string TurmaCodigo { get; set; }

        public ObterProfessoresTurmaAbrangenciaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }
    }
}
