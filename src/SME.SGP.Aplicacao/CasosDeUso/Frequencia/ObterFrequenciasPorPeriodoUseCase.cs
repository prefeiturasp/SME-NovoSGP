using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorPeriodoUseCase : AbstractUseCase, IObterFrequenciasPorPeriodoUseCase
    {
        public ObterFrequenciasPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<RegistroFrequenciaPorDataPeriodoDto> Executar(FiltroFrequenciaPorPeriodoDto param)
        {

            List<FrequenciaDto> RetornoFrequencia = new List<FrequenciaDto>();

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEDataMatriculaQuery(param.TurmaId.ToString(), param.DataInicio));
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            var aulas = await mediator.Send(new ObterAulasPorDataPeriodoQuery(param.DataInicio, param.DataFim, param.TurmaId, param.DisciplinaId));
            if (aulas == null)
                throw new NegocioException("Aula não encontrada.");

            foreach (Aula aula in aulas)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
                if (turma == null)
                    throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");

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

                var componenteCurricularAula = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] {Convert.ToInt64(aula.DisciplinaId) }));

                if (componenteCurricularAula == null || componenteCurricularAula.ToList().Count <= 0)
                    throw new NegocioException("Componente curricular da aula não encontrado");

                var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoNaAulaQuery(aula.Id));

                var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, long.Parse(aula.DisciplinaId), periodoEscolar.Id));

                var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, aula.DisciplinaId, periodoEscolar.Id));
                foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(aula.DataAula)).OrderBy(c => c.NomeAluno))
                {
                    // Apos o bimestre da inatividade o aluno não aparece mais na lista de frequencia ou
                    // se a matrícula foi ativada após a data da aula                
                    if ((aluno.EstaInativo(aula.DataAula) && (aluno.DataSituacao < periodoEscolar.PeriodoInicio || aluno.DataSituacao < aula.DataAula)) ||
                        (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo && aluno.DataMatricula > aula.DataAula))
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

                RetornoFrequencia.Add(registroFrequenciaDto);
            }

            return await ObterListaFrequenciaAula(RetornoFrequencia, param.DisciplinaId, param.TurmaId, param.DataInicio, param.DataFim);
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

        private async Task<RegistroFrequenciaPorDataPeriodoDto> ObterListaFrequenciaAula(List<FrequenciaDto> frequeciasDia, string disciplinaId, string codigoTurma, DateTime dataInicio, DateTime dataFim)
        {
            RegistroFrequenciaPorDataPeriodoDto registroFrequenciaPorDataPeriodo = new RegistroFrequenciaPorDataPeriodoDto();

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEDataMatriculaQuery(codigoTurma, dataInicio));
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            foreach (var aluno in alunosDaTurma)
            {
                FrequeciaPorDataPeriodoDto frequenciaAluno = new FrequeciaPorDataPeriodoDto();

                frequenciaAluno.CodigoAluno = aluno.CodigoAluno;
                frequenciaAluno.NomeAluno = aluno.NomeAluno;
                frequenciaAluno.NumeroAlunoChamada = aluno.NumeroAlunoChamada;

                foreach (FrequenciaDto frequenciaDia in frequeciasDia)
                {
                    var aula = await mediator.Send(new ObterAulaPorIdQuery(frequenciaDia.AulaId));
                    if (aula == null)
                        throw new NegocioException("Aula não encontrada.");

                    FrequenciaAulaPorDataPeriodoDto frequenciaAlunoAula = new FrequenciaAulaPorDataPeriodoDto();

                    frequenciaAlunoAula.DataAula = aula.DataAula;

                    bool verificaSeAlunoTeveFrequencia = frequenciaDia.ListaFrequencia.Any(a => a.CodigoAluno == aluno.CodigoAluno);

                    if (verificaSeAlunoTeveFrequencia)
                    {
                        var tipoFrequencia = ObterTipoFrequencia(frequenciaDia.ListaFrequencia.Where(a => a.CodigoAluno == aluno.CodigoAluno).FirstOrDefault().Aulas);

                        if (tipoFrequencia != null)
                            frequenciaAlunoAula.TipoFrequencia = tipoFrequencia;
                    }


                    frequenciaAluno.Aulas.Add(frequenciaAlunoAula);
                }

                registroFrequenciaPorDataPeriodo.ListaFrequencia.Add(frequenciaAluno);
            }

            registroFrequenciaPorDataPeriodo = await ObterRegistroFrequenciaPorData(disciplinaId, codigoTurma, dataInicio, dataFim, registroFrequenciaPorDataPeriodo.ListaFrequencia);

            return registroFrequenciaPorDataPeriodo;
        }

        private string ObterTipoFrequencia(IEnumerable<FrequenciaAulaDto> aulas)
        {
            if (aulas == null)
                return null;
            else
            {
                if (aulas.Count() == 1)
                    return aulas.FirstOrDefault().TipoFrequencia;
                else
                {
                    var quantidadeDeFreqDiferentes = aulas.GroupBy(a => a.TipoFrequencia);

                    if (quantidadeDeFreqDiferentes.Count() > 1)
                        return null;
                    else
                        return aulas.FirstOrDefault().TipoFrequencia;
                }
            }
        }

        private async Task<RegistroFrequenciaPorDataPeriodoDto> ObterRegistroFrequenciaPorData(string disciplinaId, string turmaId, DateTime dataInicio, DateTime dataFim, IList<FrequeciaPorDataPeriodoDto> listaFrequencia)
        {
            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorDataEAulaIdQuery(disciplinaId, turmaId, dataInicio, dataFim));

            if (registroFrequencia == null)
            {
                return null;
            }
            else
            {
                var menorCriadoEm = registroFrequencia.OrderBy(a => a.CriadoEm).FirstOrDefault();
                var maiorAlteradoEm = registroFrequencia.OrderByDescending(a => a.CriadoEm).FirstOrDefault();

                if (maiorAlteradoEm == null)
                {
                    maiorAlteradoEm = menorCriadoEm;
                }

                var RegistroFrequenciaPorDataPeriodoDto = new RegistroFrequenciaPorDataPeriodoDto()
                {
                    AlteradoEm = maiorAlteradoEm.AlteradoEm,
                    AlteradoPor = maiorAlteradoEm.AlteradoPor,
                    AlteradoRF = maiorAlteradoEm.AlteradoPor,
                    CriadoEm = menorCriadoEm.CriadoEm,
                    CriadoPor = menorCriadoEm.CriadoPor,
                    CriadoRF = menorCriadoEm.CriadoRF,
                    ListaFrequencia = listaFrequencia
                };


                return RegistroFrequenciaPorDataPeriodoDto;
            }
        }
    }
}
