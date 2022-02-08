using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoNotaPorTurmaIdQuery : IRequest<NotaTipoValor>
    {
        public ObterTipoNotaPorTurmaIdQuery(long turmaId, TipoTurma tipoTurma)
        {
            TurmaId = turmaId;
            TipoTurma = tipoTurma;
        }

        public long TurmaId { get; set; }
        public TipoTurma TipoTurma { get; set; }
    }
}
