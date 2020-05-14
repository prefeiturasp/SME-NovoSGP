using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDeCalendarioDaTurmaQuery : IRequest<TipoCalendario>
    {
        public Turma Turma { get; set; }

    }
}
