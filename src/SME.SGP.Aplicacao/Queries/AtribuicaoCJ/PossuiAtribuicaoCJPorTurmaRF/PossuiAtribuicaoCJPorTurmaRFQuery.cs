using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
   public class PossuiAtribuicaoCJPorTurmaRFQuery : IRequest<bool>
    {
        public PossuiAtribuicaoCJPorTurmaRFQuery(string turmaCodigo, string rfProfessor, long disciplinaId)
        {
            TurmaCodigo = turmaCodigo;
            RFProfessor = rfProfessor;
            DisciplinaId = disciplinaId;
        }

        public string TurmaCodigo { get; set; }

        public string RFProfessor { get; set; }

        public long DisciplinaId{ get; set; }

    }
}
