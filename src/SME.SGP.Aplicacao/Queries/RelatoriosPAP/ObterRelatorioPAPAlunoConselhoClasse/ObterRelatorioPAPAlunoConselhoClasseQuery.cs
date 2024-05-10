using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPAPAlunoConselhoClasseQuery : IRequest<RelatorioPAPAlunoConselhoClasseDto>
    {
        public ObterRelatorioPAPAlunoConselhoClasseQuery(int anoLetivo, string alunoCodigo, int bimestre, ModalidadeTipoCalendario modalidade)
        {
            AnoLetivo = anoLetivo;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
    }

    public class ObterRelatorioPAPAlunoConselhoClasseQueryValidator : AbstractValidator<ObterRelatorioPAPAlunoConselhoClasseQuery>
    {
        public ObterRelatorioPAPAlunoConselhoClasseQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para busca do relatório pap do aluno.");
            RuleFor(x => x.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade do tipo de calendário deve ser informada para busca do relatório pap do aluno.");
            RuleFor(x => x.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para busca do relatório pap do aluno.");
            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para busca do relatório pap do aluno.");
        }
    }
}
