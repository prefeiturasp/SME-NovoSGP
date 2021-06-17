using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoEstudanteCommand : IRequest<bool>
    {
        public ExcluirFotoEstudanteCommand(string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }

        public string AlunoCodigo { get; set; }
    }

}
