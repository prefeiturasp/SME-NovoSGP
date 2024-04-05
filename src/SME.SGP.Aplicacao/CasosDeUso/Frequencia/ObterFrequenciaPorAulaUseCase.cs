using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorAulaUseCase : AbstractUseCase, IObterFrequenciaPorAulaUseCase
    {
        public ObterFrequenciaPorAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<FrequenciaDto> Executar(FiltroFrequenciaDto param)
        {
            (Aula aula, Turma turma, IEnumerable<AlunoPorTurmaResposta> alunosDaTurmaNaData) = await ValidarParametros(param);
            var registroFrequenciaDto = await ObterRegistroFrequencia(aula, turma);
            var frequenciaAlunos = await mediator.Send(new ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(aula.Id))
                                   ?? Enumerable.Empty<FrequenciaAlunoSimplificadoDto>();
            var compensacaoAusenciaAlunoAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery(aula.Id)) 
                                                ?? Enumerable.Empty<CompensacaoAusenciaAlunoAulaSimplificadoDto>();
            
            var periodoEscolar = await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula))
                ?? throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");

            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;

            registroFrequenciaDto.TemPeriodoAberto = await mediator
                .Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodoEscolar.Bimestre, mesmoAnoLetivo));

            var parametroPercentualCritico = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaCritico, periodoEscolar.PeriodoInicio.Year))
                ?? throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");

            var percentualCritico = int.Parse(parametroPercentualCritico.Valor);

            var parametroPercentualAlerta = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaAlerta, periodoEscolar.PeriodoInicio.Year))
                ?? throw new NegocioException("Parâmetro de percentual de frequência em alerta não encontrado contate a SME.");   

            var percentualAlerta = int.Parse(parametroPercentualAlerta.Valor);
            var codigosComponentesConsiderados = new long[] { param.ComponenteCurricularId ?? long.Parse(aula.DisciplinaId) };
            var componenteCurricularAula = await ObterComponentesCurricularesTurma(codigosComponentesConsiderados, turma.CodigoTurma);

            var anotacoesTurma = await mediator
                .Send(new ObterAlunosComAnotacaoNaAulaQuery(aula.Id));

            var frequenciaAlunosRegistrada = await mediator
                .Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, codigosComponentesConsiderados, periodoEscolar.Id));

            var turmaPossuiFrequenciaRegistrada = await mediator
                .Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, codigosComponentesConsiderados.Select(c => c.ToString()).ToArray(), periodoEscolar.Id));

            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunosDaTurmaNaData, turma);
            foreach (var aluno in alunosDaTurmaNaData)
            {
                var tipoFrequenciaPreDefinida = await mediator
                    .Send(new ObterFrequenciaPreDefinidaPorAlunoETurmaQuery(turma.Id, long.Parse(aula.DisciplinaId), aluno.CodigoAluno));

                var alunoPossuiPlanoAEE = await mediator
                    .Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));

                var periodoDeCompensacaoAberto = new PeriodoDeCompensacaoAbertoUseCase(mediator);

                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeSocialAluno ?? aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    Desabilitado = EhAlunoDesabilitado(turma, aula, periodoEscolar, aluno),
                    PossuiAnotacao = anotacoesTurma.Any(a => a == aluno.CodigoAluno),
                    NomeResponsavel = aluno.NomeResponsavel,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    CelularResponsavel = aluno.CelularResponsavel,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                    TipoFrequenciaPreDefinido = tipoFrequenciaPreDefinida.ShortName(),
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                };

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = await mediator
                    .Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, turma.ModalidadeCodigo));

                aluno.CodigoTurma = long.Parse(turma.CodigoTurma);

                var frequenciaAluno = frequenciaAlunosRegistrada.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(frequenciaAluno, percentualAlerta, percentualCritico, turmaPossuiFrequenciaRegistrada);

                if (!componenteCurricularAula.Any(c => c.RegistraFrequencia) || !aula.PermiteRegistroFrequencia(turma))
                {
                    registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
                    continue;
                }

                for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                {
                    registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                    {
                        NumeroAula = numeroAula,
                        TipoFrequencia = ObterFrequenciaAluno(frequenciaAlunos, aluno.CodigoAluno, numeroAula, tipoFrequenciaPreDefinida),
                        PossuiCompensacao = compensacaoAusenciaAlunoAulas.Any(t => t.CodigoAluno == aluno.CodigoAluno && t.NumeroAula == numeroAula)
                    });
                }

                registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
            }

            registroFrequenciaDto.Desabilitado = registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado) || aula.EhDataSelecionadaFutura || !aula.PermiteRegistroFrequencia(turma);

            return registroFrequenciaDto;
        }

        private bool EhAlunoDesabilitado(Turma turma, Aula aula, PeriodoEscolar periodoEscolar, AlunoPorTurmaResposta aluno)
            => aluno.DeveMostrarNaChamada(aula.DataAula, periodoEscolar.PeriodoInicio) && (aluno.EstaInativo(aula.DataAula) ||
                   (aluno.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido) && aula.EhDataSelecionadaFutura && !aula.PermiteRegistroFrequencia(turma)))
                   && turma.TipoTurma != TipoTurma.Programa;

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesTurma(long[] componentesCurriculares, string codigoTurma)
        {
            var componenteCurricularAula = await mediator
                .Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(componentesCurriculares, codigoTurma));

            if (componenteCurricularAula.NaoPossuiRegistros())
                throw new NegocioException("Componente curricular da aula não encontrado");

            return componenteCurricularAula;
        }

        private async Task<(Aula aula, Turma turma, IEnumerable<AlunoPorTurmaResposta> alunosTurmaNoPeriodo)> ValidarParametros(FiltroFrequenciaDto param)
        {
            var aula = await mediator
                .Send(new ObterAulaPorIdQuery(param.AulaId)) ?? throw new NegocioException("Aula não encontrada.");
            var turma = await mediator
                .Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId)) ?? throw new NegocioException(MensagensNegocioFrequencia.TURMA_NAO_ENCONTRADA_POR_CODIGO);
            var alunosDaTurmaNaData = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(aula.TurmaId, (aula.DataAula, aula.DataAula)));
            if (alunosDaTurmaNaData.NaoPossuiRegistros())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            return (aula, turma, alunosDaTurmaNaData.OrderBy(a => a.NomeSocialAluno ?? a.NomeAluno));
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(IEnumerable<AlunoPorTurmaResposta> alunosDaTurmaNaData, Turma turma)
        {
            var alunosCodigos = alunosDaTurmaNaData.Select(x => x.CodigoAluno).ToArray();
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosCodigos));
        }

        private string ObterFrequenciaAluno(IEnumerable<FrequenciaAlunoSimplificadoDto> frequenciaAlunos, string codigoAluno, int numeroAula, TipoFrequencia tipoFrequenciaPreDefinida)
        {
            var tipoFrequencia = frequenciaAlunos.FirstOrDefault(a => a.NumeroAula == numeroAula && a.CodigoAluno == codigoAluno && a.TipoFrequencia > 0)?.TipoFrequencia;
            if (tipoFrequencia.HasValue)
                return tipoFrequencia.ShortName();
            return tipoFrequenciaPreDefinida.ShortName();
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            switch (tipoResponsavel)
            {
                case "1":
                    {
                        return TipoResponsavel.Filiacao1.Name();
                    }
                case "2":
                    {
                        return TipoResponsavel.Filiacao2.Name();
                    }
                case "3":
                    {
                        return TipoResponsavel.ResponsavelLegal.Name();
                    }
                case "4":
                    {
                        return TipoResponsavel.ProprioEstudante.Name();
                    }
            }
            return TipoResponsavel.Filiacao1.ToString();
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(FrequenciaAluno frequenciaAluno, int percentualAlerta, int percentualCritico, bool turmaComFrequenciasRegistradas)
        {
            double percentualFrequencia = Double.MinValue;

            if (turmaComFrequenciasRegistradas && frequenciaAluno.NaoEhNulo())
                percentualFrequencia = frequenciaAluno.PercentualFrequencia;

            var percentualFrequenciaLabel = percentualFrequencia < 0 ? null : FrequenciaAluno.FormatarPercentual(FrequenciaAluno.ArredondarPercentual(percentualFrequencia));

            // Critico
            if (percentualFrequencia <= (double)percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequenciaLabel };

            // Alerta
            if (percentualFrequencia <= (double)percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequenciaLabel };

            return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Info, Percentual = percentualFrequenciaLabel };
        }

        private async Task<FrequenciaDto> ObterRegistroFrequencia(Aula aula, Turma turma)
        {
            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id));
            if (registroFrequencia.EhNulo())
            {
                registroFrequencia = new RegistroFrequencia(aula);
            }
            var registroFrequenciaDto = new FrequenciaDto(aula.Id)
            {
                AlteradoEm = registroFrequencia.AlteradoEm,
                AlteradoPor = registroFrequencia.AlteradoPor,
                AlteradoRF = registroFrequencia.AlteradoRF,
                CriadoEm = registroFrequencia.CriadoEm,
                CriadoPor = registroFrequencia.CriadoPor,
                CriadoRF = registroFrequencia.CriadoRF,
                Id = registroFrequencia.Id,
                Desabilitado = !aula.PermiteRegistroFrequencia(turma)
            };
            return registroFrequenciaDto;
        }
    }
}