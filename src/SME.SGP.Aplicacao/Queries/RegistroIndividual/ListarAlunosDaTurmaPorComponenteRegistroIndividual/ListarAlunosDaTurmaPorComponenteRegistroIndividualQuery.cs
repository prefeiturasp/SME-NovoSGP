using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosDaTurmaPorComponenteRegistroIndividualQuery : IRequest<IEnumerable<AlunoDadosBasicosDto>>
    {
        public ListarAlunosDaTurmaPorComponenteRegistroIndividualQuery(Turma turma, long componenteCurricularId)
        {
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
        }

        public Turma Turma { get; set; }

        public long ComponenteCurricularId { get; set; }
    }
}
