using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoNotaUseCase : ISalvarConselhoClasseAlunoNotaUseCase
    {
        private readonly IMediator mediator;
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;

        public SalvarConselhoClasseAlunoNotaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Executar(SalvarConselhoClasseAlunoNotaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(dto.CodigoTurma)) ?? throw new NegocioException("Turma não encontrada");

            var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(dto.FechamentoTurmaId, dto.CodigoAluno));

            FechamentoTurmaDisciplina fechamentoTurmaDisciplina;

            var idFechamentoTurmaDisciplina = dto.Bimestre > 0 ? (await mediator
                .Send(new ObterFechamentoTurmaDisciplinaDTOQuery(turma.CodigoTurma, dto.ConselhoClasseNotaDto.CodigoComponenteCurricular, dto.Bimestre, null)))?.Id ?? 0 : 0;

            var periodoEscolar = new PeriodoEscolar();

            if (fechamentoTurma.EhNulo())
            {
                if (!ehAnoAnterior)
                    throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, dto.Bimestre));

                if (periodoEscolar.EhNulo() && dto.Bimestre > 0)
                    throw new NegocioException("Período escolar não encontrado");

                fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = turma.Id,
                    Migrado = false,
                    PeriodoEscolarId = periodoEscolar?.Id,
                    Turma = turma,
                    PeriodoEscolar = periodoEscolar
                };
            }
            else
            {
                if (fechamentoTurma.PeriodoEscolarId.NaoEhNulo())
                {
                    periodoEscolar =
                        await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
                }
            }

            fechamentoTurmaDisciplina = idFechamentoTurmaDisciplina > 0 ?
                await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(idFechamentoTurmaDisciplina)) : new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = dto.ConselhoClasseNotaDto.CodigoComponenteCurricular
                };

            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var periodoEscolarValidacao = await ObtenhaPeriodoEscolar(periodoEscolar, turma, dto.Bimestre);

            await ValidaProfessorPodePersistirTurma(turma, periodoEscolarValidacao, usuario);

            var matriculasDoAlunoNaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(Convert.ToInt32(turma.CodigoTurma), Convert.ToInt32(dto.CodigoAluno)));

            var alunoConselho = matriculasDoAlunoNaTurma?.OrderByDescending(m=> m.DataSituacao)?.FirstOrDefault(t => t.DataMatricula < periodoEscolarValidacao.PeriodoFim) ?? matriculasDoAlunoNaTurma.FirstOrDefault();

            if(alunoConselho.EhNulo())
                throw new NegocioException("Não foi possível encontrar a(s) matrícula(s) do(a) aluno(a) na turma");

            await VerificaSePodeEditarNota(periodoEscolarValidacao, turma, alunoConselho, dto.Bimestre);
            await ValidarConceitoOuNota(dto, fechamentoTurma, alunoConselho, periodoEscolarValidacao);

            await mediator.Send(new GravarFechamentoTurmaConselhoClasseCommand(
                fechamentoTurma, fechamentoTurmaDisciplina, periodoEscolar?.Bimestre));

            return await mediator.Send(new GravarConselhoClasseCommad(fechamentoTurma, dto.ConselhoClasseId, dto.CodigoAluno,
                dto.ConselhoClasseNotaDto, periodoEscolar?.Bimestre, usuario));
        }

        private async Task<PeriodoEscolar> ObtenhaPeriodoEscolar(PeriodoEscolar periodo, Turma turma, int bimestre)
        {
            if (periodo.PeriodoFim == DateTime.MinValue)
            {
                if (bimestre == 0)
                    bimestre = turma.ModalidadeTipoCalendario.EhEjaOuCelp() ? BIMESTRE_2 : BIMESTRE_4;

                return await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            }

            return periodo;
        }

        private async Task VerificaSePodeEditarNota(PeriodoEscolar periodoEscolar, Turma turma, AlunoPorTurmaResposta alunoConselho, int bimestre)
        {
            DateTime? periodoInicio = periodoEscolar?.PeriodoInicio;
            DateTime? periodoFim = periodoEscolar?.PeriodoFim;

            if (bimestre == 0)
            {
                var periodosLetivos = await ObtenhaListaDePeriodoLetivo(turma);

                if (periodosLetivos.NaoEhNulo() || periodosLetivos.Any())
                {
                    periodoInicio = periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
                    periodoFim = periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;
                }
            }

            var visualizaNotas = (periodoEscolar is null && !alunoConselho.Inativo) ||
                (!alunoConselho.Inativo && alunoConselho.DataMatricula.Date <= periodoFim) ||
                (alunoConselho.Inativo && alunoConselho.DataSituacao.Date > periodoInicio);

            if (!visualizaNotas || !await mediator.Send(new VerificaSePodeEditarNotaQuery(alunoConselho.CodigoAluno, turma, periodoEscolar)))
                throw new NegocioException(MensagemNegocioFechamentoNota.NOTA_ALUNO_NAO_PODE_SER_INSERIDA_OU_ALTERADA_NO_PERIODO);
        }

        private async Task<List<PeriodoEscolar>> ObtenhaListaDePeriodoLetivo(Turma turma)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));

            if (tipoCalendario.NaoEhNulo())
                return (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id))).ToList();

            return null;
        }

        private async Task<bool> PossuiPermissaoTurma(Turma turma, PeriodoEscolar periodo, Usuario usuarioLogado)
        {
            if (usuarioLogado.EhProfessorCj())
                return await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue?.Dre?.CodigoDre, turma.Ue?.CodigoUe, turma.Id.ToString(), usuarioLogado.CodigoRf));

            return await mediator.Send(new ProfessorPodePersistirTurmaQuery(usuarioLogado.CodigoRf, turma.CodigoTurma, periodo.PeriodoFim));
        }

        private async Task ValidaProfessorPodePersistirTurma(Turma turma, PeriodoEscolar periodo, Usuario usuarioLogado)
        {
            if (!usuarioLogado.EhGestorEscolar() && !await PossuiPermissaoTurma(turma, periodo, usuarioLogado))
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private async Task ValidarConceitoOuNota(SalvarConselhoClasseAlunoNotaDto dto, FechamentoTurma fechamentoTurma,
            AlunoPorTurmaResposta alunoConselho, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurma.Turma.EhNulo())
                return;

            var notaTipoValor = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(fechamentoTurma.Turma));
            if (notaTipoValor.EhNulo())
                return;

            if (notaTipoValor.TipoNota == TipoNota.Nota && dto.ConselhoClasseNotaDto.Nota.NaoEhNulo() && dto.ConselhoClasseNotaDto.Nota > 10)
                throw new NegocioException(MensagemNegocioConselhoClasse.NOTA_NUMERICA_DEVE_SER_IGUAL_OU_INFERIOR_A_10);

            var turmasCodigos = new[] { dto.CodigoTurma };

            var dataSituacao = alunoConselho.PossuiSituacaoAtiva() ? periodoEscolar?.PeriodoFim : alunoConselho.DataSituacao;

            var notasFechamentoAluno = (fechamentoTurma is { PeriodoEscolarId: { } } ?
                await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, dto.CodigoAluno,
                    dto.Bimestre, alunoConselho.DataMatricula, dataSituacao, fechamentoTurma.Turma.AnoLetivo)) :
                await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, dto.CodigoAluno,
                    alunoConselho.DataMatricula, dataSituacao, dto.Bimestre))).ToList();

            var notaFechamentoAluno = notasFechamentoAluno.FirstOrDefault(c =>
                c.ComponenteCurricularCodigo == dto.ConselhoClasseNotaDto.CodigoComponenteCurricular &&
                c.AlunoCodigo == dto.CodigoAluno &&
                c.Bimestre == dto.Bimestre);

            switch (notaTipoValor.TipoNota)
            {
                case TipoNota.Conceito when dto.ConselhoClasseNotaDto.Conceito.EhNulo():
                    {
                        if (!(notaFechamentoAluno?.ConceitoId).HasValue)
                            return;

                        throw new NegocioException(MensagemNegocioConselhoClasse.CONCEITO_POS_CONSELHO_DEVE_SER_INFORMADO);
                    }
                case TipoNota.Nota when dto.ConselhoClasseNotaDto.Nota.EhNulo():
                    {
                        if (!(notaFechamentoAluno?.Nota).HasValue)
                            return;

                        throw new NegocioException(MensagemNegocioConselhoClasse.NOTA_POS_CONSELHO_DEVE_SER_INFORMADA);
                    }
            }
        }
    }
}