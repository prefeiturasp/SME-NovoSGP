﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class
        ObterFechamentoSituacaoPorEstudanteQueryHandler : IRequestHandler<ObterFechamentoSituacaoQuery,IEnumerable<FechamentoSituacaoQuantidadeDto>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorio;

        public ObterFechamentoSituacaoPorEstudanteQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> Handle(ObterFechamentoSituacaoQuery request,
            CancellationToken cancellationToken)
            => await repositorio.ObterSituacaoProcessoFechamento(request.UeId,
                request.Ano, request.DreId, request.Modalidade,
                request.Semestre, request.Bimestre);
    }
}