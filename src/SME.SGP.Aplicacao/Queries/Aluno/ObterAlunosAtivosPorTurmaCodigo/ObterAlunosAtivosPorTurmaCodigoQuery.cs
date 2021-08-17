using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorTurmaCodigoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosAtivosPorTurmaCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }
}
