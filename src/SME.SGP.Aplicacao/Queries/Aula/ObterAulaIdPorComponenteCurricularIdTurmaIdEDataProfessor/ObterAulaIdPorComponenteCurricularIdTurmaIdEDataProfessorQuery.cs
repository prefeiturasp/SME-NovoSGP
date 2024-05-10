using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQuery : IRequest<long?>
    {
        public ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQuery(string componenteCurricularId, string turmaId, DateTime data, string professorRf)
        {
            ComponenteCurricularId = componenteCurricularId;
            ProfessorRf = professorRf;
            TurmaId = turmaId;
            Data = data;
        }

        public string ComponenteCurricularId { get; set; }
        public string TurmaId { get; set; }
        public DateTime Data { get; set; }
        public string ProfessorRf { get; set; }

    }
}
