using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;

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
            if (conselhoClasseAlunoId != null && conselhoClasseAlunoId > 0)
            {
                var wfAprovacaoParecerConclusivo = await mediator.Send(new ObterSePossuiParecerEmAprovacaoQuery(conselhoClasseAlunoId));

                if (wfAprovacaoParecerConclusivo != null)
                {
                    parecerConclusivoDto.Id = wfAprovacaoParecerConclusivo.ConselhoClasseParecerId.Value;
                    parecerConclusivoDto.Nome = wfAprovacaoParecerConclusivo.ConselhoClasseParecer.Nome;
                    parecerConclusivoDto.EmAprovacao = true;
                }
            }
        }
    }
}