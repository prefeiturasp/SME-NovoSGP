﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");

            var alunosDaTurmaFiltrados = await mediator
                .Send(new ObterAlunosAtivosPorTurmaCodigoQuery(aula.TurmaId, aula.DataAula));

            if (alunosDaTurmaFiltrados == null || !alunosDaTurmaFiltrados.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            var registroFrequenciaDto = await ObterRegistroFrequencia(aula, turma);

            var frequenciaAlunos = await mediator
                .Send(new ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(aula.Id));

            if (frequenciaAlunos == null)
                frequenciaAlunos = new List<FrequenciaAlunoSimplificadoDto>();

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

            var componenteCurricularAula = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { param.ComponenteCurricularId ?? Convert.ToInt64(aula.DisciplinaId) }, codigoTurma: turma.CodigoTurma));

            if (componenteCurricularAula == null || componenteCurricularAula.ToList().Count <= 0)
                throw new NegocioException("Componente curricular da aula não encontrado");

            var anotacoesTurma = await mediator
                .Send(new ObterAlunosComAnotacaoNaAulaQuery(aula.Id));

            var frequenciaAlunosRegistrada = await mediator
                .Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, new long[] { long.Parse(aula.DisciplinaId) }, periodoEscolar.Id));

            var turmaPossuiFrequenciaRegistrada = await mediator
                .Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, new string[] { aula.DisciplinaId }, periodoEscolar.Id));

            var alunosCondicaoFrequencia = Enumerable.Empty<AlunoPorTurmaResposta>();

            if (turma.ModalidadeCodigo == Modalidade.EJA)
            {
                alunosCondicaoFrequencia = alunosDaTurmaFiltrados
                    .Where(a => !a.Inativo || !a.SituacaoMatricula.Equals(SituacaoMatriculaAluno.VinculoIndevido) &&
                           (a.Inativo && a.DataSituacao >= aula.DataAula) && a.DataMatricula <= aula.DataAula)
                    .OrderBy(c => c.NomeAluno);
            }
            else
            {
                alunosCondicaoFrequencia = alunosDaTurmaFiltrados
                    .Where(a => a.EstaAtivo(aula.DataAula, aula.DataAula) || !a.SituacaoMatricula.Equals(SituacaoMatriculaAluno.VinculoIndevido) &&
                           (a.Inativo && a.DataSituacao >= aula.DataAula) && a.DataMatricula <= aula.DataAula)
                    .OrderBy(c => c.NomeAluno);
            }

            foreach (var aluno in alunosCondicaoFrequencia)
            {
                var tipoFrequenciaPreDefinida = await mediator
                    .Send(new ObterFrequenciaPreDefinidaPorAlunoETurmaQuery(turma.Id, long.Parse(aula.DisciplinaId), aluno.CodigoAluno));

                var alunoPossuiPlanoAEE = await mediator
                    .Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));

                var periodoDeCompensacaoAberto = new PeriodoDeCompensacaoAbertoUseCase(mediator);

                var ehAbertura = await periodoDeCompensacaoAberto
                    .VerificarPeriodoAberto(turma.CodigoTurma, periodoEscolar.Bimestre);

                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    Desabilitado = aluno.DeveMostrarNaChamada(aula.DataAula, periodoEscolar.PeriodoInicio) && (aluno.EstaInativo(aula.DataAula) || (aluno.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido) && aula.EhDataSelecionadaFutura && !aula.PermiteRegistroFrequencia(turma))) && turma.TipoTurma != TipoTurma.Programa,
                    PermiteAnotacao = aluno.EstaAtivo(aula.DataAula, aula.DataAula),
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

                if (!componenteCurricularAula.FirstOrDefault().RegistraFrequencia || !aula.PermiteRegistroFrequencia(turma))
                {
                    registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
                    continue;
                }

                for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                {
                    registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                    {
                        NumeroAula = numeroAula,
                        TipoFrequencia = ObterFrequenciaAluno(frequenciaAlunos, aluno.CodigoAluno, numeroAula, tipoFrequenciaPreDefinida)
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
            var percentualFrequencia = 0;

            if (turmaComFrequenciasRegistradas && frequenciaAluno != null)
                percentualFrequencia = (int)Math.Round(frequenciaAluno.PercentualFrequencia);

            var percentualFrequenciaLabel = percentualFrequencia < 0 ? null : percentualFrequencia.ToString();

            // Critico
            if (percentualFrequencia <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequenciaLabel };

            // Alerta
            if (percentualFrequencia <= percentualAlerta)
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