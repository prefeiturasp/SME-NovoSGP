using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery : IRequest<IEnumerable<int>>
    {
        public ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery(string login, Guid perfil, int modalidade, bool consideraHistorico, int anoLetivo, string ueCodigo)
        {
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public string Login { get; set; }
        public Guid Perfil { get; set; }
        public int Modalidade { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }
    public class ObterSemestresPorAnoLetivoModalidadeEUeCodigoQueryValidator : AbstractValidator<ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery>
    {
        public ObterSemestresPorAnoLetivoModalidadeEUeCodigoQueryValidator()
        {
            RuleFor(a => a.Login)
               .NotEmpty()
               .WithMessage("O login deve ser informado");

            RuleFor(a => a.Perfil)
               .NotEmpty()
               .WithMessage("O perfil deve ser informado");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado");

            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada");
        }
    }
}
