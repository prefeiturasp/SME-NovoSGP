using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterResumoProfessorPorRFAnoLetivoQuery : IRequest<ProfessorResumoDto>
    {
        public ObterResumoProfessorPorRFAnoLetivoQuery(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false)
        {
            CodigoRF = codigoRF;
            AnoLetivo = anoLetivo;
            BuscarOutrosCargos = buscarOutrosCargos;
        }

        public string CodigoRF { get; set; }
        public int AnoLetivo { get; set; }
        public bool BuscarOutrosCargos { get; set; }
    }
}
