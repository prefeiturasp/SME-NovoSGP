﻿using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSePodeEditarNotaQueryHandler : IRequestHandler<VerificaSePodeEditarNotaQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public VerificaSePodeEditarNotaQueryHandler(IMediator mediator, IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }

        public async Task<bool> Handle(VerificaSePodeEditarNotaQuery request, CancellationToken cancellationToken)
        {
            var turmaFechamento = await this.mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(request.Turma.CodigoTurma, DateTimeExtension.HorarioBrasilia())); 

            if (turmaFechamento == null || !turmaFechamento.Any())
                throw new NegocioException($"Não foi possível obter os dados da turma {request.Turma.CodigoTurma}");

            var turmaFechamentoOrdenada = turmaFechamento.GroupBy(x => x.CodigoAluno).SelectMany(y => y.OrderByDescending(a => a.DataSituacao).Take(1));

            var aluno = turmaFechamentoOrdenada.Last(a => a.CodigoAluno == request.AlunoCodigo);

            if (aluno == null)
                throw new NegocioException($"Não foi possível obter os dados do aluno {request.AlunoCodigo}");

            var temPeriodoAberto = false;

            if (request.PeriodoEscolar != null)
                temPeriodoAberto = await this.consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(request.Turma.CodigoTurma, aluno.DataSituacao, request.PeriodoEscolar.Bimestre);

            return aluno.PodeEditarNotaConceitoNoPeriodo(request.PeriodoEscolar, temPeriodoAberto);
        }
    }
}
