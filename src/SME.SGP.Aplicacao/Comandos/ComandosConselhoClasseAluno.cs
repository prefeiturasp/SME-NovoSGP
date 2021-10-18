using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseAluno : IComandosConselhoClasseAluno
    {
        private readonly IConsultasConselhoClasseAluno consultasConselhoClasseAluno;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IServicoConselhoClasse servicoConselhoClasse;
        private readonly IMediator mediator;

        public ComandosConselhoClasseAluno(IConsultasConselhoClasseAluno consultasConselhoClasseAluno,
                                           IConsultasConselhoClasse consultasConselhoClasse,
                                           IServicoConselhoClasse servicoConselhoClasse, IMediator mediator)
        {
            this.consultasConselhoClasseAluno = consultasConselhoClasseAluno ?? throw new ArgumentNullException(nameof(consultasConselhoClasseAluno));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.servicoConselhoClasse = servicoConselhoClasse ?? throw new ArgumentNullException(nameof(servicoConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ParecerConclusivoDto> GerarParecerConclusivoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
            => await servicoConselhoClasse.GerarParecerConclusivoAlunoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo);

        public async Task<ConselhoClasseAluno> SalvarAsync(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto)
            => await servicoConselhoClasse.SalvarConselhoClasseAluno(await MapearParaEntidade(conselhoClasseAlunoDto));

        private async Task<ConselhoClasseAluno> MapearParaEntidade(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto)
        {
            var conselhoClasseAluno = await consultasConselhoClasseAluno.ObterPorConselhoClasseAsync(conselhoClasseAlunoDto.ConselhoClasseId, conselhoClasseAlunoDto.AlunoCodigo);
            if (conselhoClasseAluno == null)
            {
                ConselhoClasse conselhoClasse = conselhoClasseAlunoDto.ConselhoClasseId == 0 ?
                    new ConselhoClasse() { FechamentoTurmaId = conselhoClasseAlunoDto.FechamentoTurmaId } :
                    consultasConselhoClasse.ObterPorId(conselhoClasseAlunoDto.ConselhoClasseId);

                conselhoClasseAluno = new ConselhoClasseAluno()
                {
                    ConselhoClasse = conselhoClasse,
                    ConselhoClasseId = conselhoClasse.Id,
                    AlunoCodigo = conselhoClasseAlunoDto.AlunoCodigo
                };
            }

            MoverAnotacoesPedagogicas(conselhoClasseAlunoDto, conselhoClasseAluno);
            MoverRecomendacoesAluno(conselhoClasseAlunoDto, conselhoClasseAluno);
            MoverRecomendacoesFamilia(conselhoClasseAlunoDto, conselhoClasseAluno);
            conselhoClasseAluno.AnotacoesPedagogicas = conselhoClasseAlunoDto.AnotacoesPedagogicas;
            conselhoClasseAluno.RecomendacoesAluno = conselhoClasseAlunoDto.RecomendacaoAluno;
            conselhoClasseAluno.RecomendacoesFamilia = conselhoClasseAlunoDto.RecomendacaoFamilia;

            return conselhoClasseAluno;
        }

        private void MoverAnotacoesPedagogicas(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.AnotacoesPedagogicas))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.AnotacoesPedagogicas, conselhoClasseAlunoDto.AnotacoesPedagogicas));
                conselhoClasseAlunoDto.AnotacoesPedagogicas = moverArquivo.Result;
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.AnotacoesPedagogicas))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.AnotacoesPedagogicas, conselhoClasseAlunoDto.AnotacoesPedagogicas, TipoArquivo.ConselhoClasse.Name()));
            }
        }
        private void MoverRecomendacoesAluno(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.RecomendacaoAluno))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.RecomendacoesAluno, conselhoClasseAlunoDto.RecomendacaoAluno));
                conselhoClasseAlunoDto.RecomendacaoAluno = moverArquivo.Result;
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.RecomendacoesAluno, conselhoClasseAlunoDto.RecomendacaoAluno, TipoArquivo.ConselhoClasse.Name()));
            }
        }
        private void MoverRecomendacoesFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.RecomendacaoFamilia))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.RecomendacoesFamilia, conselhoClasseAlunoDto.RecomendacaoFamilia));
                conselhoClasseAlunoDto.RecomendacaoFamilia = moverArquivo.Result;
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.RecomendacoesFamilia, conselhoClasseAlunoDto.RecomendacaoFamilia, TipoArquivo.ConselhoClasse.Name()));
            }
        }
    }
}