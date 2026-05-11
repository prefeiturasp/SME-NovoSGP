using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSemNotasRecomendacoesUseCase : IObterAlunosSemNotasRecomendacoesUseCase
    {
        private readonly IMediator mediator;
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;

        public ObterAlunosSemNotasRecomendacoesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<InconsistenciasAlunoFamiliaDto>> Executar(FiltroInconsistenciasAlunoFamiliaDto param)
        {
            var turmaRegular = await mediator.Send(new ObterTurmaPorIdQuery(param.TurmaId));

            if (turmaRegular.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var retorno = new List<InconsistenciasAlunoFamiliaDto>();
            var turmasCodigo = new List<string>();

            PeriodoEscolar periodoEscolar;
            if (turmaRegular.ModalidadeTipoCalendario.NaoEhEjaOuCelp())
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turmaRegular, param.Bimestre == 0 ? BIMESTRE_4 : param.Bimestre));
            else
                periodoEscolar = await ObterPeriodoEscolarTurmaEJA(turmaRegular, param.Bimestre);

            turmasCodigo.Add(turmaRegular.CodigoTurma);

            var tipoCalendarioTurma = await mediator
                .Send(new ObterTipoCalendarioIdPorTurmaQuery(turmaRegular));

            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(tipoCalendarioTurma, turmaRegular.EhTurmaInfantil, param.Bimestre));

            var alunosDaTurma = (await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(turmaRegular.CodigoTurma, (periodoFechamento?.InicioDoFechamento ?? periodoEscolar.PeriodoInicio, periodoFechamento?.FinalDoFechamento ?? periodoEscolar.PeriodoFim), false, true)))?.DistinctBy(x => x.NomeAluno);

            var turmaComplementares = await mediator
                .Send(new ObterTurmasComplementaresPorAlunoQuery(alunosDaTurma.Select(x => x.CodigoAluno).ToArray()));

            var turmasComplementaresFiltradas = turmaComplementares
                .Where(t => t.TurmaRegularCodigo == turmaRegular.CodigoTurma && t.Semestre == turmaRegular.Semestre);

            if (turmasComplementaresFiltradas.Any())
                turmasCodigo.AddRange(turmasComplementaresFiltradas.Select(s => s.CodigoTurma));

            var turmasItinerarioEnsinoMedio = await mediator
                .Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance);

            var codigosItinerarioEnsinoMedio = await ObterTurmasCodigosItinerarioEnsinoMedio(turmaRegular, turmasItinerarioEnsinoMedio, periodoEscolar, param.Bimestre);

            if (codigosItinerarioEnsinoMedio.NaoEhNulo())
                turmasCodigo.AddRange(codigosItinerarioEnsinoMedio);

            if (turmaRegular.EhTurmaEnsinoMedio)
            {
                var turmasAluno = await mediator
                    .Send(new ObterTurmasFechamentoConselhoPorAlunosQuery(alunosDaTurma
                        .Select(a => Convert.ToInt64(a.CodigoAluno)).ToArray(), turmaRegular.AnoLetivo));

                if (!turmasAluno.Any())
                    throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

                var turmasRegularesCodigo = turmasAluno
                    .Where(x => x.TipoTurma == TipoTurma.Regular)?.Select(t => t.TurmaCodigo)?.Distinct();

                if (turmasRegularesCodigo.NaoEhNulo() && turmasRegularesCodigo.Any())
                    turmasCodigo.AddRange(turmasRegularesCodigo);
            }

            var turmasDosAlunos = turmasCodigo.Distinct().ToArray();

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var perfil = await mediator.Send(ObterPerfilAtualQuery.Instance);

            var componentesCurricularesPorTurma = (await mediator
                .Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasDosAlunos, perfil, usuarioLogado.CodigoRf, turmaRegular.EnsinoEspecial, turmaRegular.TurnoParaComponentesCurriculares)))
                .Where(w => w.LancaNota)
                .ToArray();

            var fechamentoTurma = await mediator
                .Send(new ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery(param.Bimestre, turmaRegular.CodigoTurma, turmaRegular.AnoLetivo, turmaRegular.Semestre, tipoCalendarioTurma));

            switch (fechamentoTurma)
            {
                case null when !turmaRegular.EhAnoAnterior():
                    {
                        if (param.Bimestre > 0)
                            throw new NegocioException(string.Format(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO_BIMESTRE, param.Bimestre));

                        throw new NegocioException(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO);
                    }
                case null:
                    throw new NegocioException(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO);
            }

            var existeConselhoParaTurma = await mediator
                .Send(new ExisteConselhoClasseParaTurmaQuery(turmasDosAlunos, param.Bimestre));

            if (!existeConselhoParaTurma)
                throw new NegocioException(MensagemNegocioConselhoClasse.NAO_FOI_ENCONTRADO_CONSELHO_CLASSE_PRA_NENHUM_ESTUDANTE);

            var obterRecomendacoes = await mediator
                .Send(new VerificarSeExisteRecomendacaoPorTurmaQuery(turmasDosAlunos, param.Bimestre));

            var obterConselhoClasseAlunoNota = await mediator
                .Send(new ObterConselhoClasseAlunoNotaQuery(turmasDosAlunos, param.Bimestre));

            await MapearRetorno(retorno, obterRecomendacoes, obterConselhoClasseAlunoNota, alunosDaTurma, periodoEscolar, componentesCurricularesPorTurma);

            return retorno
                .Where(w => w.Inconsistencias.Any())
                .OrderBy(o => o.AlunoNome);
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolarTurmaEJA(Turma turma, int bimestre)
        {
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == 0)
                throw new NegocioException(string.Format(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO_TURMA, turma.CodigoTurma));

            var periodosLetivos = (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId))).ToList();
            return periodosLetivos.FirstOrDefault(pl => pl.Bimestre == (bimestre == 0 ? BIMESTRE_2 : bimestre));
        }

        private async Task MapearRetorno(List<InconsistenciasAlunoFamiliaDto> retorno, IEnumerable<AlunoTemRecomandacaoDto> obterRecomendacoes, IEnumerable<ConselhoClasseAlunoNotaDto> obterConselhoClasseAlunoNota, IEnumerable<AlunoPorTurmaResposta> alunoPorTurmaRespostas,
            PeriodoEscolar periodoEscolar, IEnumerable<DisciplinaDto> componentesCurricularesPorTurma)
        {
            foreach (var aluno in alunoPorTurmaRespostas)
            {
                var item = new InconsistenciasAlunoFamiliaDto
                {
                    NumeroChamada = aluno.NumeroAlunoChamada,
                    AlunoNome = aluno.NomeAluno,
                    AlunoCodigo = aluno.CodigoAluno
                };

                var componentesComNotasDoAluno = obterConselhoClasseAlunoNota.Where(x => x.AlunoCodigo.Equals(aluno.CodigoAluno) && !string.IsNullOrEmpty(x.Nota)).Select(s => s.ComponenteCurricularId);

                var turmas = componentesCurricularesPorTurma.Where(w => !string.IsNullOrEmpty(w.TurmaCodigo)).Select(x => x.TurmaCodigo).Distinct().ToArray();
                var turmasComMatriculaValida = await mediator.Send(new ObterTurmasComMatriculaValidasParaValidarConselhoQuery(aluno.CodigoAluno, turmas, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim));

                var componentesSemNota = componentesCurricularesPorTurma.Where(x => turmasComMatriculaValida.Contains(x.TurmaCodigo)
                                                                                    && !componentesComNotasDoAluno.Contains(x.CodigoComponenteCurricular)).Select(s => s.Nome).Distinct();

                foreach (var componente in componentesSemNota)
                    item.Inconsistencias.Add(string.Format(MensagemNegocioConselhoClasse.AUSENCIA_DA_NOTA_NO_COMPONENTE, componente));

                var existeRecomendacao = obterRecomendacoes.Where(x => x.AluncoCodigo == aluno.CodigoAluno && x.TemRecomendacao);
                if (!existeRecomendacao.Any())
                    item.Inconsistencias.Add(MensagemNegocioConselhoClasse.SEM_RECOMENDACAO_FAMILIA_ESTUDANDE);

                retorno.Add(item);
            }
        }

        private async Task<string[]> ObterTurmasCodigosItinerarioEnsinoMedio(Turma turma, IEnumerable<TurmaItinerarioEnsinoMedioDto> turmasItinerarioEnsinoMedio, PeriodoEscolar periodoEscolar, int bimestre)
        {
            string[] turmasCodigos = null;
            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                && !(bimestre == 0 && turma.EhEJA() && !turma.EhTurmaRegular()))
            {
                var ue = await mediator.Send(new ObterUePorIdQuery(turma.UeId));
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();
                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery(turma.AnoLetivo, tiposParaConsulta, false, ue.CodigoUe, turma.Semestre, periodoEscolar?.PeriodoFim));
                if (!turmasCodigos.Any())
                    turmasCodigos = new string[1] { turma.CodigoTurma };
                else if (!turmasCodigos.Contains(turma.CodigoTurma))
                    turmasCodigos = turmasCodigos.Concat(new[] { turma.CodigoTurma }).ToArray();
            }
            else turmasCodigos = new string[] { turma.CodigoTurma };

            return turmasCodigos;
        }
    }
}