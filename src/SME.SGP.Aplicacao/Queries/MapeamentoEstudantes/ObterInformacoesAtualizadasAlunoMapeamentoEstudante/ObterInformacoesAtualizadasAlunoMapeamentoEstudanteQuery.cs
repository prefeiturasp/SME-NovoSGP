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
    public class ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery : IRequest<InformacoesAtualizadasMapeamentoEstudanteAlunoDto>
    {
        public ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery(string codigoAluno, int anoLetivo, int bimestre)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
        }

        public string CodigoAluno { get; }
        public int AnoLetivo { get; }
        public int Bimestre { get; }
    }

    public class ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryValidator : AbstractValidator<ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery>
    {
        public ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado para a pesquisa de informações atualizadas para o mapeamento de estudante ");
            
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado para a pesquisa de informações atualizadas para o mapeamento de estudante ");

            RuleFor(c => c.Bimestre)
            .NotEmpty()
            .WithMessage("O bimestre deve ser informado para a pesquisa de informações atualizadas para o mapeamento de estudante ");
        }
    }
}
