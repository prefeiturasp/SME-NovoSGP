using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery : IRequest<IEnumerable<OpcaoRespostaSimplesDto>>
    {
        public ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(string nomeComponente, TipoQuestionario tipoQuestionario)
        {
            NomeComponente = nomeComponente;
            TipoQuestionario = tipoQuestionario;
        }

        public string NomeComponente { get; }
        public TipoQuestionario TipoQuestionario { get; }
    }

    public class ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQueryValidator : AbstractValidator<ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery>
    {
        public ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQueryValidator()
        {
            RuleFor(a => a.NomeComponente)
                .NotEmpty()
                .WithMessage("O Nome Componente da Questão deve ser informado para retorno de suas opções de resposta.");
            RuleFor(a => a.NomeComponente)
                .NotEmpty()
                .WithMessage("O Tipo Questionário da Questão deve ser informado para retorno de suas opções de resposta.");
        }
    }
}
