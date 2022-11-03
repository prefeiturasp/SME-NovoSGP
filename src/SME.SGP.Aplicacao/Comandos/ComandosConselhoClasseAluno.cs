﻿using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System.Linq;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseAluno : IComandosConselhoClasseAluno
    {
        private readonly IConsultasConselhoClasseAluno consultasConselhoClasseAluno;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IMediator mediator;
        private const int BIMESTRE_FINAL_FUNDAMENTAL_MEDIO = 4;
        private const int BIMESTRE_FINAL_EJA = 2;

        public ComandosConselhoClasseAluno(IConsultasConselhoClasseAluno consultasConselhoClasseAluno,
                                           IConsultasConselhoClasse consultasConselhoClasse,
                                           IMediator mediator)
        {
            this.consultasConselhoClasseAluno = consultasConselhoClasseAluno ?? throw new ArgumentNullException(nameof(consultasConselhoClasseAluno));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAluno> SalvarAsync(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto)
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaCompletoPorIdQuery(conselhoClasseAlunoDto.FechamentoTurmaId));

            if (fechamentoTurma == null)
                throw new NegocioException(MensagemNegocioFechamentoNota.FECHAMENTO_TURMA_NAO_LOCALIZADO);
            
            var bimestre = fechamentoTurma.PeriodoEscolarId.HasValue ? fechamentoTurma.PeriodoEscolar.Bimestre : 
                fechamentoTurma.Turma.EhEJA() ? BIMESTRE_FINAL_EJA : BIMESTRE_FINAL_FUNDAMENTAL_MEDIO;

            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(fechamentoTurma.Turma, dataAtual.Date, bimestre,
                fechamentoTurma.Turma.AnoLetivo == dataAtual.Year));

            if (!periodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar ?? 
                                 await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(fechamentoTurma.Turma, bimestre));

            if (periodoEscolar == null)
                throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(fechamentoTurma.Turma.CodigoTurma));
            var alunoConselho = alunos.FirstOrDefault(x => x.CodigoAluno == conselhoClasseAlunoDto.AlunoCodigo);

            if (alunoConselho == null)
                throw new NegocioException(MensagemNegocioConselhoClasse.ALUNO_NAO_ENCONTRADO_PARA_SALVAR_CONSELHO_CLASSE);            
            
            if (fechamentoTurma.Turma.AnoLetivo == dataAtual.Year && alunoConselho.EstaAtivo(periodoEscolar.PeriodoFim))
            {
                var periodoReaberturaCorrespondente = await mediator.Send(new ObterFechamentoReaberturaPorDataTurmaQuery
                {
                    DataParaVerificar = dataAtual, 
                    TipoCalendarioId = periodoEscolar.TipoCalendarioId, 
                    UeId = fechamentoTurma.Turma.UeId
                });
                
                var permiteEdicao = (alunoConselho.PossuiSituacaoAtiva() && alunoConselho.DataMatricula.Date <= periodoEscolar.PeriodoFim) ||
                                    (alunoConselho.Inativo && alunoConselho.DataSituacao.Date > periodoEscolar.PeriodoFim);
                
                if (!permiteEdicao)
                       throw new NegocioException(MensagemNegocioFechamentoNota.ALUNO_INATIVO_ANTES_PERIODO_ESCOLAR);
            }
            
            var existeConselhoClasseBimestre = await mediator
                .Send(new VerificaNotasTodosComponentesCurricularesQuery(alunoConselho.CodigoAluno, fechamentoTurma.Turma, bimestre, periodoEscolar: periodoEscolar));

            if (!existeConselhoClasseBimestre)
                throw new NegocioException(MensagemNegocioFechamentoNota.EXISTE_COMPONENTES_SEM_NOTA_INFORMADA);

            var conselhoClasseAluno = await MapearParaEntidade(conselhoClasseAlunoDto);

            conselhoClasseAluno.Id = await mediator.Send(new SalvarConselhoClasseAlunoCommand(conselhoClasseAluno));

            await SalvarRecomendacoesAlunoFamilia(conselhoClasseAlunoDto.RecomendacaoAlunoIds, conselhoClasseAlunoDto.RecomendacaoFamiliaIds,
                conselhoClasseAluno.Id);

            return conselhoClasseAluno;
        }

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

            await MoverAnotacoesPedagogicas(conselhoClasseAlunoDto, conselhoClasseAluno);
            await MoverRecomendacoesAluno(conselhoClasseAlunoDto, conselhoClasseAluno);
            await MoverRecomendacoesFamilia(conselhoClasseAlunoDto, conselhoClasseAluno);
            conselhoClasseAluno.AnotacoesPedagogicas = conselhoClasseAlunoDto.AnotacoesPedagogicas;
            conselhoClasseAluno.RecomendacoesAluno = conselhoClasseAlunoDto.RecomendacaoAluno;
            conselhoClasseAluno.RecomendacoesFamilia = conselhoClasseAlunoDto.RecomendacaoFamilia;

            return conselhoClasseAluno;
        }

        private async Task SalvarRecomendacoesAlunoFamilia(IEnumerable<long> recomendacoesAlunoId, IEnumerable<long> recomendacoesFamiliaId, long conselhoClasseAlunoId)
            => await mediator.Send(new SalvarConselhoClasseAlunoRecomendacaoCommand(recomendacoesAlunoId, recomendacoesFamiliaId, conselhoClasseAlunoId));

        private async Task MoverAnotacoesPedagogicas(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.AnotacoesPedagogicas))
            {
                conselhoClasseAlunoDto.AnotacoesPedagogicas = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.AnotacoesPedagogicas, conselhoClasseAlunoDto.AnotacoesPedagogicas));
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.AnotacoesPedagogicas))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.AnotacoesPedagogicas, conselhoClasseAlunoDto.AnotacoesPedagogicas, TipoArquivo.ConselhoClasse.Name()));
            }
        }
        private async Task MoverRecomendacoesAluno(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.RecomendacaoAluno))
            {
                conselhoClasseAlunoDto.RecomendacaoAluno = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.RecomendacoesAluno, conselhoClasseAlunoDto.RecomendacaoAluno));
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.RecomendacoesAluno, conselhoClasseAlunoDto.RecomendacaoAluno, TipoArquivo.ConselhoClasse.Name()));
            }
        }
        private async Task MoverRecomendacoesFamilia(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto, ConselhoClasseAluno conselhoClasseAluno)
        {
            if (!string.IsNullOrEmpty(conselhoClasseAlunoDto.RecomendacaoFamilia))
            {
                conselhoClasseAlunoDto.RecomendacaoFamilia = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.ConselhoClasse, conselhoClasseAluno.RecomendacoesFamilia, conselhoClasseAlunoDto.RecomendacaoFamilia));
            }
            if (!string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(conselhoClasseAluno.RecomendacoesFamilia, conselhoClasseAlunoDto.RecomendacaoFamilia, TipoArquivo.ConselhoClasse.Name()));
            }
        }
    }
}