using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoProfissionalAtendimentosNAAPAConsolidadoQuery : IRequest<ConsolidadoAtendimentoNAAPA>
    {
        public ObterAtendimentoProfissionalAtendimentosNAAPAConsolidadoQuery(long ueId, int mes, int anoLetivo, string rfProfissional, Modalidade modalidade)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            Mes = mes;
            RfProfissional = rfProfissional;
            Modalidade = modalidade;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string RfProfissional { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryValidator : AbstractValidator<ObterAtendimentoProfissionalAtendimentosNAAPAConsolidadoQuery>
    {
        public ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.Mes).GreaterThan(0).WithMessage("Informe o Mes do Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
            RuleFor(x => x.RfProfissional).NotEmpty().WithMessage("Informe o Profissional para realizar a consulta");
            RuleFor(x => x.Modalidade).NotEmpty().WithMessage("Informe a Modalidade para realizar a consulta");
        }
    }
}