using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery : IRequest<Aula>
    {
        public ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery(string componenteCurricularId, string turmaId, DateTime data)
        {
            ComponenteCurricularId = componenteCurricularId;
            TurmaId = turmaId;
            Data = data;
        }

        public string ComponenteCurricularId { get; set; }
        public string TurmaId { get; set; }
        public DateTime Data { get; set; }

    }
}
