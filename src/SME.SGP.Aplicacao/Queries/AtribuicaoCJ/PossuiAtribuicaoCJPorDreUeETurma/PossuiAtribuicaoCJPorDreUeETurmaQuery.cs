using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class PossuiAtribuicaoCJPorDreUeETurmaQuery : IRequest<bool>
    {
        public PossuiAtribuicaoCJPorDreUeETurmaQuery(string dreCodigo, string ueCodigo, string turmaId, string professorRf)
        {
            TurmaId = turmaId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            ProfessorRf = professorRf;
        }

        public string TurmaId { get; set; }

        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }

        public string ProfessorRf { get; set; }
    }
}
