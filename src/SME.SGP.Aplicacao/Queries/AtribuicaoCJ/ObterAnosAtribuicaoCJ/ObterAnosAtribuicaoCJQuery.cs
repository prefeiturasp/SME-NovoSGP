using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosAtribuicaoCJQuery : IRequest<IEnumerable<int>>
    {
        public ObterAnosAtribuicaoCJQuery(string professorRf, bool consideraHistorico)
        {
            ProfessorRF = professorRf;
            ConsideraHistorico = consideraHistorico;
        }

        public string ProfessorRF { get; set; }
        public bool ConsideraHistorico { get; }
    }
}
