using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoAtividadeUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorFuncaoAtividadeUeQuery(string ueId, long funcaoAtividadeId)
        {
            UeId = ueId;
            FuncaoAtividadeId = funcaoAtividadeId;
        }

        public string UeId { get; set; }
        public long FuncaoAtividadeId { get; set; }
    }

    public class ObterFuncionariosPorFuncaoAtividadeUeQueryValidator : AbstractValidator<ObterFuncionariosPorFuncaoAtividadeUeQuery>
    {
        public ObterFuncionariosPorFuncaoAtividadeUeQueryValidator()
        {
            RuleFor(x => x.UeId).NotEmpty().WithMessage("Informe a UE para Obter os Funcionarios Por Cargo e Ue");
            RuleFor(x => x.FuncaoAtividadeId).NotEmpty().WithMessage("Informe a Função Atividade para Obter os Funcionarios Por Função Atividade e Ue");
        }
    }
}