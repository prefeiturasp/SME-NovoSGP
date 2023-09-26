using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorComponenteProfessorBimestreQuery : IRequest<IEnumerable<PendenciaAulaProfessorDto>>
    {
        public string ComponenteCurricularId { get; set; }
        public string CodigoRf { get; set; }
        public long PeriodoEscolarId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
        public string TurmaCodigo { get; set; }
        public long UeId { get; set; }

        public ObterPendenciaIdPorComponenteProfessorBimestreQuery(string componenteId, string codigoRf, long periodoEscolarId, TipoPendencia tipoPendencia, string turmaCodigo, long ueId)
        {
            ComponenteCurricularId = componenteId;
            CodigoRf = codigoRf;
            PeriodoEscolarId = periodoEscolarId;
            TipoPendencia = tipoPendencia;
            TurmaCodigo = turmaCodigo;
            UeId = ueId;
        }
    }

    public class ObterPendenciaIdPorComponenteProfessorBimestreQueryValidator : AbstractValidator<ObterPendenciaIdPorComponenteProfessorBimestreQuery>
    {
        public ObterPendenciaIdPorComponenteProfessorBimestreQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o RF para verificar se já existe pendência.");

            RuleFor(a => a.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("É necessário informar o id do período escolar para verificar se já existe pendência.");

            RuleFor(a => a.TipoPendencia)
                .NotEmpty()
                .WithMessage("É necessário informar o tipo da pendência para verificar se já existe a mesma.");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma para verificar se já existe a mesma.");

            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("É necessário informar o identificador da Ue para verificar se já existe a mesma.");
        }
    }
}
