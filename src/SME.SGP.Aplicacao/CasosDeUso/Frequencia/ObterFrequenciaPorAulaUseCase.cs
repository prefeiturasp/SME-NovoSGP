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
            var aula = await mediator
                .Send(new ObterAulaPorIdQuery(param.AulaId));

            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var turma = await mediator
                .Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));

            if (turma == null)
                throw new NegocioException(MensagensNegocioFrequencia.TURMA_NAO_ENCONTRADA_POR_CODIGO);

            var alunosDaTurmaNaData = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(aula.TurmaId, (aula.DataAula, aula.DataAula)));

            if (alunosDaTurmaNaData == null || !alunosDaTurmaNaData.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            var registroFrequenciaDto = await ObterRegistroFrequencia(aula, turma);

            var frequenciaAlunos = await mediator.Send(new ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(aula.Id));
            if (frequenciaAlunos == null)
                frequenciaAlunos = new List<FrequenciaAlunoSimplificadoDto>();

            var compensacaoAusenciaAlunoAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery(aula.Id));
            if (compensacaoAusenciaAlunoAulas == null)
                compensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>();

            var periodoEscolar = await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula));

            if (periodoEscolar == null)
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");

            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;

            registroFrequenciaDto.TemPeriodoAberto = await mediator
                .Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodoEscolar.Bimestre, mesmoAnoLetivo));

            var parametroPercentualCritico = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaCritico, periodoEscolar.PeriodoInicio.Year));

            if (parametroPercentualCritico == null)
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");

            var percentualCritico = int.Parse(parametroPercentualCritico.Valor);

            var parametroPercentualAlerta = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaAlerta, periodoEscolar.PeriodoInicio.Year));

            if (parametroPercentualAlerta == null)
                throw new NegocioException("Parâmetro de percentual de frequência em alerta não encontrado contate a SME.");

            var percentualAlerta = int.Parse(parametroPercentualAlerta.Valor);

            var usuarioLogado = await mediator
                .Send(new ObterUsuarioLogadoQuery());

            var professorConsiderado = usuarioLogado.Login;

            var codigosTerritorioEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(param.ComponenteCurricularId ?? long.Parse(aula.DisciplinaId), turma.CodigoTurma, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null,turma.Id));

            var codigosComponentesConsiderados = new List<long>() { param.ComponenteCurricularId ?? long.Parse(aula.DisciplinaId) };

            if (codigosTerritorioEquivalentes != default)
            {
                codigosComponentesConsiderados.AddRange(codigosTerritorioEquivalentes.Select(c => long.Parse(c.codigoComponente)).Except(codigosComponentesConsiderados));
                professorConsiderado = codigosTerritorioEquivalentes.First().professor;
            }

            var componenteCurricularAula = await mediator
                .Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(codigosComponentesConsiderados.ToArray(), codigoTurma: turma.CodigoTurma));

            if (componenteCurricularAula == null || !componenteCurricularAula.Any())
                throw new NegocioException("Componente curricular da aula não encontrado");

            var anotacoesTurma = await mediator
                .Send(new ObterAlunosComAnotacaoNaAulaQuery(aula.Id));

            var frequenciaAlunosRegistrada = await mediator
                .Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, codigosComponentesConsiderados.ToArray(), periodoEscolar.Id, professorConsiderado));

            var turmaPossuiFrequenciaRegistrada = await mediator
                .Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, codigosComponentesConsiderados.Select(c => c.ToString()).ToArray(), periodoEscolar.Id, professorConsiderado));

            foreach (var aluno in alunosDaTurmaNaData.OrderBy(a => a.NomeSocialAluno ?? a.NomeAluno))
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
                    Desabilitado = aluno.DeveMostrarNaChamada(aula.DataAula, periodoEscolar.PeriodoInicio) && (aluno.EstaInativo(aula.DataAula) || (aluno.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido) && aula.EhDataSelecionadaFutura && !aula.PermiteRegistroFrequencia(turma))) && turma.TipoTurma != TipoTurma.Programa,
                    PermiteAnotacao = aluno.EstaAtivo(aula.DataAula),
                    PossuiAnotacao = anotacoesTurma.Any(a => a == aluno.CodigoAluno),
                    NomeResponsavel = aluno.NomeResponsavel,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    CelularResponsavel = aluno.CelularResponsavel,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                    TipoFrequenciaPreDefinido = tipoFrequenciaPreDefinida.ShortName()
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
                        return TipoResponsavel.Filicacao1.Name();
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
            return TipoResponsavel.Filicacao1.ToString();
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(FrequenciaAluno frequenciaAluno, int percentualAlerta, int percentualCritico, bool turmaComFrequenciasRegistradas)
        {
            double percentualFrequencia = Double.MinValue;

            if (turmaComFrequenciasRegistradas && frequenciaAluno != null)
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
            if (registroFrequencia == null)
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