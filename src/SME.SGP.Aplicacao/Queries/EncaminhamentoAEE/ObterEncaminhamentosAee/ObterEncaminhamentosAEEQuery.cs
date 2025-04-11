using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEQuery : IRequest<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>>
    {
        public ObterEncaminhamentosAEEQuery(FiltroPesquisaEncaminhamentosAEEDto filtro)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            TurmaId = filtro.TurmaId;
            AlunoCodigo = filtro.AlunoCodigo;
            Situacao = filtro.Situacao;
            ResponsavelRf = filtro.ResponsavelRf;
            AnoLetivo = filtro.AnoLetivo;
            ExibirEncerrados = filtro.ExibirEncerrados;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoAEE? Situacao { get; }
        public string ResponsavelRf { get; }
        public int AnoLetivo { get; }
        public bool ExibirEncerrados { get; set; }
    }

    public class ObterEncaminhamentosAEEQueryValidator : AbstractValidator<ObterEncaminhamentosAEEQuery>
    {
        public ObterEncaminhamentosAEEQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa de Encaminhamentos AEE");
        }
    }
}
