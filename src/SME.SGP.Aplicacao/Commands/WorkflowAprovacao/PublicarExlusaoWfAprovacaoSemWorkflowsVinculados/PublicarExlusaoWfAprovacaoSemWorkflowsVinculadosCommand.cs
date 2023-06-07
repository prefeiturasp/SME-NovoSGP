using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand : IRequest<bool>
    {
        public PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand(long wfAprovacaoId, string tabelaVinculada, long wfIgnoradoId = 0)
        {
            WfAprovacaoId = wfAprovacaoId;
            WfIgnoradoId = wfIgnoradoId;
            TabelaVinculada = tabelaVinculada;
        }

        public long WfAprovacaoId { get; set; }
        public long WfIgnoradoId { get; set; }
        public string TabelaVinculada { get; set; }
    }

    public class PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommandValidator : AbstractValidator<PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommand>
    {
        public PublicarExlusaoWfAprovacaoSemWorkflowsVinculadosCommandValidator()
        {
            RuleFor(a => a.WfAprovacaoId)
                .NotEmpty()
                .WithMessage("É necessário informar o Id do workflow a excluir");
            
        }
    }
}
