using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery(int anoLetivo, string codigoRf, int modalidadeTurma, string ano, long turmaIdReferencia)
        {
            AnoLetivo = anoLetivo;
            CodigoRf = codigoRf;
            ModalidadeTurma = modalidadeTurma;
            Ano = ano;
            TurmaIdReferencia = turmaIdReferencia;
        }

        public int AnoLetivo { get; set; }
        public string CodigoRf { get; set; }
        public int ModalidadeTurma { get; set; }
        public string Ano { get; set; }
        public long TurmaIdReferencia { get; set; }
    }

    public class ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQueryValidator : AbstractValidator<ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery>
    {
        public ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("É necessário informar o ano letivo para obter as turmas para cópia de avaliação");
            RuleFor(a => a.CodigoRf)
                .NotEmpty()
                .WithMessage("É necessário informar o rf do usuário para obter as turmas para cópia de avaliação");
            RuleFor(a => a.ModalidadeTurma)
                .NotEmpty()
                .WithMessage("É necessário informar a modalidade da turma para obter as turmas para cópia de avaliação");
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("É necessário informar o ano da turma para obter as turmas para cópia de avaliação");
            RuleFor(a => a.TurmaIdReferencia)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma de referência para obter as turmas para cópia de avaliação");
        }
    }
}
