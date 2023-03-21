using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesPorRFEAnoQuery : IRequest<IEnumerable<AtribuicaoEsporadica>>
    {
        public ObterAtribuicoesPorRFEAnoQuery(string professorRf, bool somenteInfantil, int anoLetivo, string dreCodigo = "", string ueCodigo = "", bool historico = false)
        {
            AnoLetivo = anoLetivo;
            SomenteInfantil = somenteInfantil;
            ProfessorRf = professorRf;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Historico = historico;
        }

        public int AnoLetivo { get; set; }
        public bool SomenteInfantil { get; set; }
        public string ProfessorRf { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public bool Historico { get; set; }
    }
}
