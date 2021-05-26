using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
   public class ObterComponentesCurricularesRegenciaPorTurmaQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesRegenciaPorTurmaQuery(Turma turma, long componenteCurricularId)
        {
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
        }

        public Turma Turma { get; set; }

        public long ComponenteCurricularId { get; set; }
    }
}
