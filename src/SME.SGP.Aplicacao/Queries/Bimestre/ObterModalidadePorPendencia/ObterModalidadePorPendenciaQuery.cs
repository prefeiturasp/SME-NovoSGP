using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadePorPendenciaQuery : IRequest<int>
    {
        public long PendenciaId { get; set; }
        public DateTime DataPendencia { get; set; }
        public long TurmaId { get; set; }

        public ObterModalidadePorPendenciaQuery(long pendenciaId, long turmaId, DateTime dataPendencia)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
            DataPendencia = dataPendencia;
        }
    }

    public class ObterBimestrePorPendenciaQueryValidator : AbstractValidator<ObterModalidadePorPendenciaQuery>
    {
        public ObterBimestrePorPendenciaQueryValidator()
        {
            RuleFor(a => a.PendenciaId)
                .NotEmpty()
                .WithMessage("É preciso informar o Id da pendência para consultar o bimestre");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É preciso informar o Id da pendência para consultar o bimestre");
            RuleFor(a => a.DataPendencia)
                .NotEmpty()
                .WithMessage("É preciso informar a data da pendência para consultar o bimestre");
        }
    }
}
