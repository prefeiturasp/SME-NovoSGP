﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaFrequenciaAulasQueryHandler : IRequestHandler<ObterListaFrequenciaAulasQuery, RegistroFrequenciaPorDataPeriodoDto>
    {
        private readonly IMediator mediator;

        public ObterListaFrequenciaAulasQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RegistroFrequenciaPorDataPeriodoDto> Handle(ObterListaFrequenciaAulasQuery request, CancellationToken cancellationToken)
        {
            var codigoTurma = long.Parse(request.Turma.CodigoTurma);
            var registrosFrequencias = new RegistroFrequenciaPorDataPeriodoDto();
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            registrosFrequencias.CarregarAulas(request.Aulas, request.RegistrosFrequenciaAlunos, usuarioLogado.EhSomenteProfessorCj(), usuarioLogado.EhGestorEscolar());
            registrosFrequencias.CarregarAuditoria(request.RegistrosFrequenciaAlunos);
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(request.AlunosDaTurma.Select(x => x.CodigoAluno).ToArray(), request.Turma.AnoLetivo);
            foreach (var aluno in request.AlunosDaTurma)
            {
                var frequenciaPreDefinida = request.FrequenciasPreDefinidas
                    .LastOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                var alunoPossuiPlanoAEE = await mediator
                    .Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, request.Turma.AnoLetivo));

                var registroFrequenciaAluno = new AlunoRegistroFrequenciaDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeSocialAluno ?? aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.ObterNumeroAlunoChamada(),
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    NomeResponsavel = aluno.NomeResponsavel,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    CelularResponsavel = aluno.CelularResponsavel,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                };

                var frequenciaAluno = request.FrequenciaAlunos
                    .FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = await mediator
                    .Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, request.PeriodoEscolar, request.Turma.ModalidadeCodigo));

                // Indicativo de Frequencia (%)
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(frequenciaAluno, request.PercentualAlerta, request.PercentualCritico, request.TurmaPossuiFrequenciaRegistrada);

                if (request.Aulas.Any())
                {
                    if (RegistraFrequencia(request.RegistraFrequencia, request.Aulas, request.Turma))
                    {
                        var registrosFrequenciaAluno = request.RegistrosFrequenciaAlunos
                            .Where(a => a.AlunoCodigo == aluno.CodigoAluno);

                        var compensacoesAusenciaAluno = request.CompensacaoAusenciaAlunoAulas
                            .Where(t => t.CodigoAluno == aluno.CodigoAluno);

                        var anotacoesAluno = request.AnotacoesTurma
                            .Where(a => a.AlunoCodigo == aluno.CodigoAluno);

                        registroFrequenciaAluno
                            .CarregarAulas(request.Aulas, registrosFrequenciaAluno, compensacoesAusenciaAluno, aluno, anotacoesAluno, frequenciaPreDefinida, request.PeriodoEscolar.PeriodoFim);
                    }

                    registrosFrequencias.Alunos.Add(registroFrequenciaAluno);
                }
            }

            return registrosFrequencias.Aulas.Any() ? registrosFrequencias : null;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private bool RegistraFrequencia(bool registraFrequencia, IEnumerable<Aula> aulas, Turma turma)
        {
            var aula = aulas.First();

            return registraFrequencia
                && aula.PermiteRegistroFrequencia(turma);
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(FrequenciaAluno frequenciaAluno, int percentualAlerta, int percentualCritico, bool turmaComFrequenciasRegistradas)
        {
            var percentualFrequencia = frequenciaAluno != null ? frequenciaAluno.PercentualFrequencia : double.MinValue;

            var percentualFrequenciaLabel = percentualFrequencia < 0 ? null : FrequenciaAluno.FormatarPercentual(percentualFrequencia);

            // Critico
            if (percentualFrequencia <= (double)percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequenciaLabel };

            // Alerta
            if (percentualFrequencia <= (double)percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequenciaLabel };

            return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Info, Percentual = percentualFrequenciaLabel };
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

    }
}
