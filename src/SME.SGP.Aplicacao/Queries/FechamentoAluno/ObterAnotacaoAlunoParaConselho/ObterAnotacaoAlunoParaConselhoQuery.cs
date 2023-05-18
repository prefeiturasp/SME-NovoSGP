using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoAlunoParaConselhoQuery : IRequest<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>>
    {
        public ObterAnotacaoAlunoParaConselhoQuery(string alunoCodigo, string[] turmasCodigos, long periodoId)
        {
            AlunoCodigo = alunoCodigo;
            TurmasCodigos = turmasCodigos;
            PeriodoId = periodoId;
        }
        
        public long PeriodoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string[] TurmasCodigos { get; set; }
    }

    public class ObterAnotacaoAlunoParaConselhoQueryValidator : AbstractValidator<ObterAnotacaoAlunoParaConselhoQuery>
    {
        public ObterAnotacaoAlunoParaConselhoQueryValidator()
        {
            RuleFor(a => a.PeriodoId)
                .NotEmpty()
                .WithMessage("O identificador do período deve ser informado para a busca de anotações do aluno para o conselho");
            
            RuleFor(a => a.AlunoCodigo)
             .NotEmpty()
             .WithMessage("O código do aluno deve ser informado para a busca de anotações do aluno para o conselho");
            
            RuleFor(a => a.TurmasCodigos)
                .NotNull()
                .WithMessage("Os códigos das turmas devem ser informados para a busca de anotações do aluno para o conselho");
        }
    }
}
