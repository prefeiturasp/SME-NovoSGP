using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery : IRequest<InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto>
    {
        public ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery(string codigoAluno, int anoLetivo)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
        }

        public string CodigoAluno { get; }
        public int AnoLetivo { get; }
    }

    public class ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryValidator : AbstractValidator<ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery>
    {
        public ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado para a pesquisa de informações atualizadas para o mapeamento de estudante ");
            
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado para a pesquisa de informações atualizadas para o mapeamento de estudante ");
        }
    }
}
