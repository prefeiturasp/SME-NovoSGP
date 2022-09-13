﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoTurmaUseCase : AbstractUseCase, ISalvarAcompanhamentoTurmaUseCase
    {
        public SalvarAcompanhamentoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AcompanhamentoTurma> Executar(AcompanhamentoTurmaDto dto)
        {
            var acompanhamentoTurma = await MapearAcompanhamentoTurma(dto);

            return acompanhamentoTurma;
        }

        private async Task<AcompanhamentoTurma> MapearAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var acompanhamentoTurma = dto.AcompanhamentoTurmaId > 0 ?
                await AtualizaApanhadoTurma(dto) :
                await GerarAcompanhamentoTurma(dto);

            return acompanhamentoTurma;
        }

        private async Task<AcompanhamentoTurma> AtualizaApanhadoTurma(AcompanhamentoTurmaDto dto)
        {
            var acompanhamento = await ObterAcompanhamentoTurmaPorId(dto.AcompanhamentoTurmaId);
            await MoverRemoverExcluidos(dto, acompanhamento);
            acompanhamento.ApanhadoGeral = dto.ApanhadoGeral;
            return await mediator.Send(new SalvarAcompanhamentoTurmaCommand(acompanhamento));
        }

        private async Task MoverRemoverExcluidos(AcompanhamentoTurmaDto dto, AcompanhamentoTurma acompanhamento)
        {
            if (!string.IsNullOrEmpty(dto.ApanhadoGeral))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, acompanhamento.ApanhadoGeral, dto.ApanhadoGeral));
                dto.ApanhadoGeral = moverArquivo;
            }
            if (!string.IsNullOrEmpty(acompanhamento.ApanhadoGeral))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(acompanhamento.ApanhadoGeral, dto.ApanhadoGeral, TipoArquivo.AcompanhamentoAluno.Name()));
            }
        }

        private async Task<AcompanhamentoTurma> ObterAcompanhamentoTurmaPorId(long acompanhamentoTurmaId)
            => await mediator.Send(new ObterAcompanhamentoTurmaPorIdQuery(acompanhamentoTurmaId));

        private async Task<AcompanhamentoTurma> GerarAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            await MoverRemoverExcluidos(dto, new AcompanhamentoTurma() { ApanhadoGeral = string.Empty });
            return await mediator.Send(new GerarAcompanhamentoTurmaCommand(turma.Id, dto.Semestre, dto.ApanhadoGeral));
        }
    }
}
