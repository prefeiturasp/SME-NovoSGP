using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoCJCommand : IRequest<bool>
    {
        public RemoverAtribuicaoCJCommand(string dreId, string ueId, string turmaId, string professorRf)
        {
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            ProfessorRf = professorRf;
        }

        public string DreId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public string ProfessorRf { get; set; }
    }
}
