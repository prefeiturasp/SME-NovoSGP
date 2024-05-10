using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
