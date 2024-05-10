using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

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
