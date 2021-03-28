using MediatR;
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

        public async Task<AuditoriaDto> Executar(AcompanhamentoTurmaDto dto)
        {
            var acompanhamentoTurma = await MapearAcompanhamentoTurma(dto);

            return (AuditoriaDto)acompanhamentoTurma;
        }

        private async Task<AcompanhamentoTurma> MapearAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var acompanhamentoTurma = dto.AcompanhamentoTurmaId > 0 ?
                await AtualizaApanhadoTurma(dto.AcompanhamentoTurmaId, dto.ApanhadoGeral) :
                await GerarAcompanhamentoTurma(dto);

            return acompanhamentoTurma;
        }

        private async Task<AcompanhamentoTurma> AtualizaApanhadoTurma(long acompanhamentoTurmaId, string apanhadoGeral)
        {
            var acompanhamento = await ObterAcompanhamentoTurmaPorId(acompanhamentoTurmaId);
            acompanhamento.ApanhadoGeral = apanhadoGeral;

            return await mediator.Send(new SalvarAcompanhamentoTurmaCommand(acompanhamento));
        }

        private async Task<AcompanhamentoTurma> ObterAcompanhamentoTurmaPorId(long acompanhamentoTurmaId)
            => await mediator.Send(new ObterAcompanhamentoTurmaPorIdQuery(acompanhamentoTurmaId));

        private async Task<AcompanhamentoTurma> GerarAcompanhamentoTurma(AcompanhamentoTurmaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await mediator.Send(new GerarAcompanhamentoTurmaCommand(turma.Id, dto.Semestre, dto.ApanhadoGeral));
        }
    }
}
