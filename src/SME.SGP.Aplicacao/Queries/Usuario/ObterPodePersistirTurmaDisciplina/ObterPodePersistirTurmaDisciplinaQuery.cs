using System;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPodePersistirTurmaDisciplinaQuery : IRequest<bool>
    {
        public ObterPodePersistirTurmaDisciplinaQuery(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null)
        {
            CodigoRf = codigoRf;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            Data = data;
            Usuario = usuario;
        }

        public DateTime Data { get; set; }

        public string DisciplinaId { get; set; }

        public string TurmaId { get; set; }

        public string CodigoRf { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class ObterPodePersistirTurmaDisciplinaQueryValidator : AbstractValidator<ObterPodePersistirTurmaDisciplinaQuery>
    {
        public ObterPodePersistirTurmaDisciplinaQueryValidator()
        {
            RuleFor(x => x.CodigoRf).NotEmpty().WithMessage("Informe o Código RF para Obter se pode Persistir Turma e Disciplina");
            RuleFor(x => x.CodigoRf).NotEmpty().WithMessage("Informe o Turma ID para Obter se pode Persistir Turma e Disciplina");
            RuleFor(x => x.CodigoRf).NotEmpty().WithMessage("Informe a Disciplina para Obter se pode Persistir Turma e Disciplina");
            RuleFor(x => x.CodigoRf).NotEmpty().WithMessage("Informe a Data para Obter se pode Persistir Turma e Disciplina");
        }
    }
}