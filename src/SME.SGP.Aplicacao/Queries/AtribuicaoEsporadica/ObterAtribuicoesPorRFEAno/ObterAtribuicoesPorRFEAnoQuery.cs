using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesPorRFEAnoQuery : IRequest<IEnumerable<AtribuicaoEsporadica>>
    {
        public ObterAtribuicoesPorRFEAnoQuery(string professorRf, bool somenteInfantil, int anoLetivo)
        {
            AnoLetivo = anoLetivo;
            SomenteInfantil = somenteInfantil;
            ProfessorRf = professorRf;
        }

        public int AnoLetivo { get; set; }


        public bool SomenteInfantil { get; set; }

        public string ProfessorRf { get; set; }

    }
}
