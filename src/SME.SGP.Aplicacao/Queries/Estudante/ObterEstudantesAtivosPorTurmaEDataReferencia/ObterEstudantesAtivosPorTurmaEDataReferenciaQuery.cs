using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudantesAtivosPorTurmaEDataReferenciaQuery : IRequest<IEnumerable<EstudanteDto>>
    {
        public ObterEstudantesAtivosPorTurmaEDataReferenciaQuery(string turmaCodigo, DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
            TurmaCodigo = turmaCodigo;

        }

        public string TurmaCodigo { get; set; }
        public DateTime DataReferencia { get; set; }
        
    }
}
