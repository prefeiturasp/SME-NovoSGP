using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicaoCJPorDreUeTurmaRFQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public string TurmaId { get; set; }

        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }

        public string ProfessorRf { get; set; }

        public ObterAtribuicaoCJPorDreUeTurmaRFQuery(string turmaId, string dreCodigo, string ueCodigo, string professorRf)
        {
            TurmaId = turmaId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            ProfessorRf = professorRf;
        }
    }
    
}
