﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAQuery : IRequest<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>>
    {
        public ObterEncaminhamentosNAAPAQuery(FiltroEncaminhamentoNAAPADto filtro)
        {
            ExibirHistorico = filtro.ExibirHistorico;
            DreId = filtro.DreId;
            TurmaId = filtro.TurmaId;
            CodigoUe = filtro.CodigoUe;
            Situacao = filtro.Situacao;
            CodigoNomeAluno = filtro.CodigoNomeAluno;
            AnoLetivo = filtro.AnoLetivo;
            DataAberturaQueixaInicio = filtro.DataAberturaQueixaInicio;
            DataAberturaQueixaFim = filtro.DataAberturaQueixaFim;
            Prioridade = filtro.Prioridade;
            ExibirEncerrados = filtro.ExibirEncerrados;
            Ordenacao = filtro.Ordenacao;
            if (Ordenacao.NaoPossuiRegistros())
                Ordenacao = new OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] { OrdenacaoListagemPaginadaEncaminhamentoNAAPA.DataEntradaQueixa };
        }

        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string CodigoNomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public int Prioridade { get; set; }
        public bool ExibirEncerrados { get; set; }
        public OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] Ordenacao { get; set; }
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
