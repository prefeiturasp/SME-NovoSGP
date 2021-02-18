using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand : IRequest<bool>
    {
        public ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand(int diasParaGeracaoDePendencia)
        {
            DiasParaGeracaoDePendencia = diasParaGeracaoDePendencia;
        }

        public int DiasParaGeracaoDePendencia { get; set; }
    }
}
