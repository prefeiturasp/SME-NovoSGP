using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ProfessoresTurmaDisciplinaQuery : IRequest<List<ProfessorAtribuidoTurmaDisciplinaDTO>>
    {
        public ProfessoresTurmaDisciplinaQuery(string codigoTurma, string disciplinaId, DateTime data)
        {
            CodigoTurma = codigoTurma;
            DisciplinaId = disciplinaId;
            Data = data;
        }

        public string CodigoTurma { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime Data { get; set; }
    }
}
