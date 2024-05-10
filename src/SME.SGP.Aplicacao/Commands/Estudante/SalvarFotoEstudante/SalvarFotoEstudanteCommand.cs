using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoEstudanteCommand : IRequest<Guid>
    {
        public SalvarFotoEstudanteCommand(IFormFile file, string alunoCodigo)
        {
            File = file;
            AlunoCodigo = alunoCodigo;
        }

        public IFormFile File { get; set; }
        public string AlunoCodigo { get; set; }
    }

}
