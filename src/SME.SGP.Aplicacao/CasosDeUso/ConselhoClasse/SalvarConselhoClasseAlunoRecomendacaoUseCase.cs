﻿using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoRecomendacaoUseCase : AbstractUseCase, ISalvarConselhoClasseAlunoRecomendacaoUseCase
    {
        private const int BIMESTRE_FINAL_FUNDAMENTAL_MEDIO = 4;
        private const int BIMESTRE_FINAL_EJA_CELP = 2;
        private const int BIMESTRE_FINAL_CONSULTA_NOTA = 0;

        public SalvarConselhoClasseAlunoRecomendacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ConselhoClasseAluno> Executar(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto)
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaCompletoPorIdQuery(conselhoClasseAlunoDto.FechamentoTurmaId));

            if (fechamentoTurma.EhNulo())
                throw new NegocioException(MensagemNegocioFechamentoNota.FECHAMENTO_TURMA_NAO_LOCALIZADO);

            var bimestre = ObterBimestre(fechamentoTurma);

            var periodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(fechamentoTurma.Turma, dataAtual.Date, bimestre,
                fechamentoTurma.Turma.AnoLetivo == dataAtual.Year));

            if (!periodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar ??
                                 await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(fechamentoTurma.Turma, bimestre));

            if (periodoEscolar.EhNulo())
                throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(fechamentoTurma.Turma.CodigoTurma));
            var alunoConselho = alunos.FirstOrDefault(x => x.CodigoAluno == conselhoClasseAlunoDto.AlunoCodigo);

            if (alunoConselho.EhNulo())
                throw new NegocioException(MensagemNegocioConselhoClasse.ALUNO_NAO_ENCONTRADO_PARA_SALVAR_CONSELHO_CLASSE);

            var permiteEdicao = alunoConselho.EstaAtivo(periodoEscolar.PeriodoFim) || await EstaInativoDentroPeriodoAberturaReabertura(alunoConselho, bimestre, periodoEscolar.TipoCalendarioId, fechamentoTurma.Turma);

            if (!permiteEdicao)
                throw new NegocioException(MensagemNegocioFechamentoNota.ALUNO_INATIVO_ANTES_PERIODO_ESCOLAR);
            
            var bimestreParaValidacaoNotasPreenchidas = fechamentoTurma.PeriodoEscolarId.HasValue ? bimestre : BIMESTRE_FINAL_CONSULTA_NOTA;

            var existeConselhoClasseBimestre = await mediator
                .Send(new VerificaNotasTodosComponentesCurricularesQuery(alunoConselho.CodigoAluno, fechamentoTurma.Turma, bimestreParaValidacaoNotasPreenchidas, periodoEscolar: periodoEscolar));

            if (!existeConselhoClasseBimestre)
                throw new NegocioException(MensagemNegocioFechamentoNota.EXISTE_COMPONENTES_SEM_NOTA_INFORMADA);

            var conselhoClasseAluno = await MapearParaEntidade(conselhoClasseAlunoDto);

            conselhoClasseAluno.Id = await mediator.Send(new SalvarConselhoClasseAlunoCommand(conselhoClasseAluno));

            await mediator.Send(new SalvarConselhoClasseAlunoRecomendacaoCommand(conselhoClasseAlunoDto.RecomendacaoAlunoIds, conselhoClasseAlunoDto.RecomendacaoFamiliaIds, conselhoClasseAluno.Id));

            return conselhoClasseAluno;
        }

        private int ObterBimestre(FechamentoTurma fechamentoTurma)
        {
            if (fechamentoTurma.PeriodoEscolarId.HasValue)
                return fechamentoTurma.PeriodoEscolar.Bimestre;

            return fechamentoTurma.Turma.EhTurmaModalidadeSemestral() ? BIMESTRE_FINAL_EJA_CELP : BIMESTRE_FINAL_FUNDAMENTAL_MEDIO;
        }
        
        private async Task<bool> EstaInativoDentroPeriodoAberturaReabertura(AlunoPorTurmaResposta dadosAluno, int bimestre, long tipoCalendarioId, Turma turma)
        {
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, dadosAluno.DataSituacao.Date, bimestre, turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year, tipoCalendarioId));
        }

        private async Task<ConselhoClasseAluno> MapearParaEntidade(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto)
        {
            var conselhoClasseAluno = await mediator.Send(new ObterPorConselhoClasseAlunoCodigoQuery(conselhoClasseAlunoDto.ConselhoClasseId, conselhoClasseAlunoDto.AlunoCodigo));
            if (conselhoClasseAluno.EhNulo())
            {
                ConselhoClasse conselhoClasse = conselhoClasseAlunoDto.ConselhoClasseId == 0 ?
                    new ConselhoClasse() { FechamentoTurmaId = conselhoClasseAlunoDto.FechamentoTurmaId } :
                    await mediator.Send(new ObterConselhoClassePorIdQuery(conselhoClasseAlunoDto.ConselhoClasseId));

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
