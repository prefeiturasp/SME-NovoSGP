using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAQuery : IRequest<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>>
    {
        public ObterEncaminhamentosNAAPAQuery(bool exibirHistorico,int anoLetivo, long dreId, string codigoUe, long turmaId, string nomeAluno,
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, int prioridade, bool exibirEncerrados)
        {
            ExibirHistorico = exibirHistorico;
            DreId = dreId;
            TurmaId = turmaId;
            CodigoUe = codigoUe;
            Situacao = situacao;
            NomeAluno = nomeAluno;
            AnoLetivo = anoLetivo;
            DataAberturaQueixaInicio = dataAberturaQueixaInicio;
            DataAberturaQueixaFim = dataAberturaQueixaFim;
            Prioridade = prioridade;
            ExibirEncerrados = exibirEncerrados;
        }

        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string NomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public int Prioridade { get; set; }
        public bool ExibirEncerrados { get; set; }
    }

    public class ObterEncaminhamentosNAAPAQueryValidator : AbstractValidator<ObterEncaminhamentosNAAPAQuery>
    {
        public ObterEncaminhamentosNAAPAQueryValidator()
        {
            RuleFor(c => c.AnoLetivo).NotEmpty().WithMessage("O ano letivo deve ser informado para pesquisa de Encaminhamentos NAAPA");
            RuleFor(c => c.DreId).NotEmpty().WithMessage("O identificador da DRE deve ser informado para pesquisa de Encaminhamentos NAAPA");
        }
    }
}
