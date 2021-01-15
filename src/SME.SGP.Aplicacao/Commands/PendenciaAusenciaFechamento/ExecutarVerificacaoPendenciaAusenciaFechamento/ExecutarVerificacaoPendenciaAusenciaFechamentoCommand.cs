using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAusenciaFechamentoCommand : IRequest<bool>
    {
        public ExecutarVerificacaoPendenciaAusenciaFechamentoCommand(int diasParaGeracaoDePendencia, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            DiasParaGeracaoDePendencia = diasParaGeracaoDePendencia;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public int DiasParaGeracaoDePendencia { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }
}
