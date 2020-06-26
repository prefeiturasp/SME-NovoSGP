using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorTurmaQuery: IRequest<long>
    {
        public ObterTipoCalendarioIdPorTurmaQuery(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; set; }
    }
}
