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
        public ExecutarVerificacaoPendenciaAusenciaFechamentoCommand(int diasParaGeracaoDePendencia, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            DiasParaGeracaoDePendencia = diasParaGeracaoDePendencia;
            Modalidade = modalidade;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public int DiasParaGeracaoDePendencia { get; set; }
        public Modalidade Modalidade { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }
}
