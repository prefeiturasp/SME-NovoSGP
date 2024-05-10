using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery : IRequest<List<AbrangenciaTurmaRetornoEolDto>>
    {
        public ObterTurmasAbrangenciaHistoricaEOLAnoProfessorQuery(int anoLetivo, string professorRf)
        {
            AnoLetivo = anoLetivo;
            ProfessorRf = professorRf;
        }

        public int AnoLetivo { get; }
        public string ProfessorRf { get; }
    }
}
