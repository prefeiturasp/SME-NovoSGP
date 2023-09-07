﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmaDoEncaminhamentoNAAPAUseCase : AbstractUseCase, IAtualizarTurmaDoEncaminhamentoNAAPAUseCase
    {
        public AtualizarTurmaDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamento = param.ObterObjetoMensagem<EncaminhamentoNAAPADto>();
            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamento.AlunoCodigo), true));
            var alunosTurmas = alunosEol?.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular 
                                                      && turma.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                                                      && turma.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date); 

            if (alunosTurmas != null) 
               await AtualizarTurmaDoEncaminhamento(encaminhamento, alunosTurmas);

            return true;
        }

        private async Task AtualizarTurmaDoEncaminhamento(EncaminhamentoNAAPADto encaminhamento, IEnumerable<TurmasDoAlunoDto> alunosTurmas)
        {
            foreach(var alunoTurma in alunosTurmas)
            {
                var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(alunoTurma.CodigoTurma.ToString()));

                if (turmaId != 0)
                {
                    if (turmaId != encaminhamento.TurmaId)
                    {
                        await AtualizarEncaminhamento(encaminhamento.Id.GetValueOrDefault(), turmaId);
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoTransferenciaUeDreDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
                    }
                    if (alunoTurma.CodigoSituacaoMatricula != (int)(encaminhamento.SituacaoMatriculaAluno ?? 0))
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoAtualizacaoSituacaoAlunoDoEncaminhamentoNAAPA, encaminhamento, Guid.NewGuid(), null));
                }
            }
        }

        private async Task AtualizarEncaminhamento(long encaminhamentoNAAPAId, long turmaId)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAComTurmaPorIdQuery(encaminhamentoNAAPAId));

            encaminhamentoNAAPA.TurmaId = turmaId;

            await mediator.Send(new SalvarEncaminhamentoNAAPACommand(encaminhamentoNAAPA));
        }
    }
}
