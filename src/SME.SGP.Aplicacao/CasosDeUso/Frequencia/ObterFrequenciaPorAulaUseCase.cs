﻿using MediatR;
using SME.SGP.Dominio;
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
            var aula = await mediator.Send(new ObterAulaPorIdQuery(param.AulaId));
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma == null)
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEDataMatriculaQuery(aula.TurmaId, aula.DataAula));
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            FrequenciaDto registroFrequenciaDto = await ObterRegistroFrequencia(aula, turma);


            var frequenciaAlunos = await mediator.Send(new ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(aula.Id));

            if (frequenciaAlunos == null)
                frequenciaAlunos = new List<FrequenciaAlunoSimplificadoDto>();

            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(aula.TipoCalendarioId, aula.DataAula));
            if (periodoEscolar == null)
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");


            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;

            registroFrequenciaDto.TemPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodoEscolar.Bimestre, mesmoAnoLetivo));


            var parametroPercentualCritico = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaCritico, periodoEscolar.PeriodoInicio.Year));
            if (parametroPercentualCritico == null)
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");
            var percentualCritico = int.Parse(parametroPercentualCritico.Valor);

            var parametroPercentualAlerta = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaAlerta, periodoEscolar.PeriodoInicio.Year));
            if (parametroPercentualAlerta == null)
                throw new NegocioException("Parâmetro de percentual de frequência em alerta não encontrado contate a SME.");
            var percentualAlerta = int.Parse(parametroPercentualAlerta.Valor);


            var componenteCurricularAula = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] {
                param.ComponenteCurricularId.HasValue ? param.ComponenteCurricularId.Value : Convert.ToInt64(aula.DisciplinaId) }));

            if (componenteCurricularAula == null || componenteCurricularAula.ToList().Count <= 0)
                throw new NegocioException("Componente curricular da aula não encontrado");

            var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoNaAulaQuery(aula.Id));

            var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, long.Parse(aula.DisciplinaId), periodoEscolar.Id));

            var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, aula.DisciplinaId, periodoEscolar.Id));
            foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(aula.DataAula)).OrderBy(c => c.NomeAluno))
            {

                if (NaoExibirAlunoFrequencia(aluno, aula, periodoEscolar))
                    continue;

                var tipoFrequenciaPreDefinida = await mediator.Send(new ObterFrequenciaPreDefinidaPorAlunoETurmaQuery(turma.Id, long.Parse(aula.DisciplinaId), aluno.CodigoAluno));

                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));
                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.NumeroAlunoChamada,
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    Desabilitado = aluno.EstaInativo(aula.DataAula) || aula.EhDataSelecionadaFutura,
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

                registroFrequenciaAluno.Marcador = await mediator.Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, turma.ModalidadeCodigo));

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
            var tipoFrequencia = frequenciaAlunos.FirstOrDefault(a => a.NumeroAula == numeroAula && a.CodigoAluno == codigoAluno)?.TipoFrequencia;
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
            if (turmaComFrequenciasRegistradas)
            {
                percentualFrequencia = (int)Math.Round(frequenciaAluno != null ? frequenciaAluno.PercentualFrequencia : 100);
            }
            else
            {
                percentualFrequencia = int.MinValue;
            }
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

        private bool NaoExibirAlunoFrequencia(AlunoPorTurmaResposta aluno, Aula aula, PeriodoEscolar periodoEscolar)
        {
            DateTime dataSituacao = DateTime.Parse(aluno.DataSituacao.ToString("dd/MM/yyyy"));
            DateTime dataMatricula = DateTime.Parse(aluno.DataMatricula.ToString("dd/MM/yyyy"));
            return (aluno.EstaInativo(aula.DataAula) && (dataSituacao < periodoEscolar.PeriodoInicio || dataSituacao < aula.DataAula)) ||
                   (!aluno.Inativo && aula.DataAula < dataMatricula) ||
                   (aluno.Inativo && !(aula.DataAula >= dataMatricula && aula.DataAula <= dataSituacao));
        }
    }
}
// Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId, long? disciplinaId = null);

//public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId, long? disciplinaId = null)
//{

//    var ausencias = servicoFrequencia.ObterListaAusenciasPorAula(aulaId);
//    if (ausencias == null)
//        ausencias = new List<RegistroAusenciaAluno>();

//    var bimestre = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(aula.TipoCalendarioId, aula.DataAula);
//    if (bimestre == null)
//        throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");


//    registroFrequenciaDto.TemPeriodoAberto = await consultasTurma.TurmaEmPeriodoAberto(aula.TurmaId, DateTime.Today, bimestre.Bimestre);

//    var parametroPercentualCritico = await repositorioParametrosSistema.ObterValorPorTipoEAno(
//                                            TipoParametroSistema.PercentualFrequenciaCritico,
//                                            bimestre.PeriodoInicio.Year);
//    if (parametroPercentualCritico == null)
//        throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");

//    var percentualCritico = int.Parse(parametroPercentualCritico);
//    var percentualAlerta = int.Parse(await repositorioParametrosSistema.ObterValorPorTipoEAno(
//                                            TipoParametroSistema.PercentualFrequenciaAlerta,
//                                            bimestre.PeriodoInicio.Year));

//    var disciplinaAula = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId.HasValue ? disciplinaId.Value : Convert.ToInt64(aula.DisciplinaId) });

//    if (disciplinaAula == null || disciplinaAula.ToList().Count <= 0)
//        throw new NegocioException("Componente curricular da aula não encontrado");

//    var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoNaAulaQuery(aulaId));

//    foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(aula.DataAula)).OrderBy(c => c.NomeAluno))
//    {
//        // Apos o bimestre da inatividade o aluno não aparece mais na lista de frequencia ou
//        // se a matrícula foi ativada após a data da aula                
//        if ((aluno.EstaInativo(aula.DataAula) && aluno.DataSituacao < bimestre.PeriodoInicio) ||
//            (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo && aluno.DataSituacao > aula.DataAula))
//            continue;

//        if (aula.DataAula < aluno.DataMatricula.Date)
//            continue;

//        if (aluno.EstaInativo(aula.DataAula) && aluno.DataSituacao < aula.DataAula)
//            continue;

//        var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));
//        var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
//        {
//            CodigoAluno = aluno.CodigoAluno,
//            NomeAluno = aluno.NomeAluno,
//            NumeroAlunoChamada = aluno.NumeroAlunoChamada,
//            CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
//            SituacaoMatricula = aluno.SituacaoMatricula,
//            DataSituacao = aluno.DataSituacao,
//            DataNascimento = aluno.DataNascimento,
//            Desabilitado = aluno.EstaInativo(aula.DataAula) || aula.EhDataSelecionadaFutura,
//            PermiteAnotacao = aluno.EstaAtivo(aula.DataAula),
//            PossuiAnotacao = anotacoesTurma.Any(a => a == aluno.CodigoAluno),
//            NomeResponsavel = aluno.NomeResponsavel,
//            TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
//            CelularResponsavel = aluno.CelularResponsavel,
//            DataAtualizacaoContato = aluno.DataAtualizacaoContato,
//            EhAtendidoAEE = alunoPossuiPlanoAEE
//        };

//        // Marcador visual da situação
//        registroFrequenciaAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, bimestre, turma.EhTurmaInfantil);

//        // Indicativo de frequencia do aluno
//        aluno.CodigoTurma = long.Parse(turma.CodigoTurma);
//        registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(aluno, aula.DisciplinaId, bimestre, percentualAlerta, percentualCritico);

//        if (!disciplinaAula.FirstOrDefault().RegistraFrequencia)
//        {
//            registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
//            continue;
//        }

//        var ausenciasAluno = ausencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);

//        for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
//        {
//            registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
//            {
//                NumeroAula = numeroAula,
//                Compareceu = !ausenciasAluno.Any(c => c.NumeroAula == numeroAula)
//            });
//        }

//        registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
//    }

//    registroFrequenciaDto.Desabilitado = registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado) || aula.EhDataSelecionadaFutura;

//    return registroFrequenciaDto;
//}







//private static List<RegistroAusenciaAluno> ObtemListaDeAusencias(FrequenciaDto frequenciaDto)
//{
//    var registrosAusenciaAlunos = new List<RegistroAusenciaAluno>();

//    foreach (var frequencia in frequenciaDto.ListaFrequencia.Where(c => c.Aulas.Any(a => !a.Compareceu)))
//    {
//        foreach (var ausenciaNaAula in frequencia.Aulas.Where(c => !c.Compareceu))
//        {
//            registrosAusenciaAlunos.Add(new RegistroAusenciaAluno(frequencia.CodigoAluno, ausenciaNaAula.NumeroAula));
//        }
//    }

//    return registrosAusenciaAlunos;
//}
