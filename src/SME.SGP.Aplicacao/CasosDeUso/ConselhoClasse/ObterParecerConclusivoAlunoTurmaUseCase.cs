using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoAlunoTurmaUseCase : IObterParecerConclusivoAlunoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterParecerConclusivoAlunoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> Executar(string codigoTurma, string alunoCodigo)
        {
            ParecerConclusivoDto parecerConclusivoDto;
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            var conselhoClasseAluno = await mediator.Send(new ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery(codigoTurma, alunoCodigo, 0, true));
            if (conselhoClasseAluno is null)
                return new ParecerConclusivoDto();

            if (!turma.EhAnoAnterior() && !conselhoClasseAluno.ConselhoClasseParecerId.HasValue)
                parecerConclusivoDto = await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(
                                        conselhoClasseAluno.ConselhoClasseId,
                                        conselhoClasseAluno.ConselhoClasse.FechamentoTurmaId,
                                        alunoCodigo));
            else
                parecerConclusivoDto = new ParecerConclusivoDto()
                {
                    Id = conselhoClasseAluno?.ConselhoClasseParecerId ?? 0,
                    Nome = conselhoClasseAluno?.ConselhoClasseParecer?.Nome,
                    EmAprovacao = false
                };

            await VerificaEmAprovacaoParecerConclusivo(conselhoClasseAluno?.Id, parecerConclusivoDto);

            return parecerConclusivoDto;
        }

        private async Task VerificaEmAprovacaoParecerConclusivo(long? conselhoClasseAlunoId, ParecerConclusivoDto parecerConclusivoDto)
        {
            if (conselhoClasseAlunoId.NaoEhNulo() && conselhoClasseAlunoId > 0)
            {
                var wfAprovacaoParecerConclusivo = await mediator.Send(new ObterSePossuiParecerEmAprovacaoQuery(conselhoClasseAlunoId));

                if (wfAprovacaoParecerConclusivo.NaoEhNulo())
                {
                    parecerConclusivoDto.Id = wfAprovacaoParecerConclusivo.ConselhoClasseParecerId.Value;
                    parecerConclusivoDto.Nome = wfAprovacaoParecerConclusivo.ConselhoClasseParecer.Nome;
                    parecerConclusivoDto.EmAprovacao = true;
                }
            }
        }
    }
}