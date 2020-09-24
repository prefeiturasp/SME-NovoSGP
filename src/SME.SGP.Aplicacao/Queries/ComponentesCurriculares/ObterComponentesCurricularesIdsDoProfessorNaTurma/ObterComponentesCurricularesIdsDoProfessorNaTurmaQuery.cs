using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesIdsDoProfessorNaTurmaQuery : IRequest<long[]>
    {
        public ObterComponentesCurricularesIdsDoProfessorNaTurmaQuery(string professorRF, long turmaCodigo, Guid perfil)
        {
            ProfessorRF = professorRF;
            TurmaCodigo = turmaCodigo;
            Perfil = perfil;
        }

        public string ProfessorRF { get; set; }
        public long TurmaCodigo { get; set; }
        public Guid Perfil { get; set; }

    }
}
