using MediatR;
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

            registrosFrequencias.CarregarAulas(request.Aulas, request.RegistrosFrequenciaAlunos);
            registrosFrequencias.CarregarAuditoria(request.RegistrosFrequenciaAlunos);

            foreach (var aluno in request.AlunosDaTurma
                                    .Where(a => a.DeveMostrarNaChamada(request.DataInicio))
                                    .OrderBy(c => c.NomeAluno))
            {
                // Apos o bimestre da inatividade o aluno não aparece mais na lista de frequencia ou
                // se a matrícula foi ativada após a data da aula                
                if (aluno.EstaInativo(request.DataInicio) || 
                   (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo && aluno.DataMatricula > request.DataFim))
                    continue;

                var frequenciaPreDefinida = request.FrequenciasPreDefinidas.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, request.Turma.AnoLetivo));

                var registroFrequenciaAluno = new AlunoRegistroFrequenciaDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.NumeroAlunoChamada,
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    NomeResponsavel = aluno.NomeResponsavel,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    CelularResponsavel = aluno.CelularResponsavel,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                };

                var frequenciaAluno = request.FrequenciaAlunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = await mediator.Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, request.PeriodoEscolar, request.Turma.ModalidadeCodigo));
                // Indicativo de Frequencia (%)
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(frequenciaAluno, request.PercentualAlerta, request.PercentualCritico, request.TurmaPossuiFrequenciaRegistrada);

                if (RegistraFrequencia(request.RegistraFrequencia, request.Aulas, request.Turma))
                {
                    var registrosFrequenciaAluno = request.RegistrosFrequenciaAlunos.Where(a => a.AlunoCodigo == aluno.CodigoAluno);
                    registroFrequenciaAluno.CarregarAulas(request.Aulas, registrosFrequenciaAluno, aluno, request.AnotacoesTurma, frequenciaPreDefinida);
                }

                registrosFrequencias.Alunos.Add(registroFrequenciaAluno);
            }

            return registrosFrequencias;
        }

        private bool RegistraFrequencia(bool registraFrequencia, IEnumerable<Aula> aulas, Turma turma)
        {
            var aula = aulas.First();

            return registraFrequencia 
                && aula.PermiteRegistroFrequencia(turma);
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(FrequenciaAluno frequenciaAluno, int percentualAlerta, int percentualCritico, bool turmaComFrequenciasRegistradas)
        {
            var percentualFrequencia = turmaComFrequenciasRegistradas ?
                (int)Math.Round(frequenciaAluno != null ? frequenciaAluno.PercentualFrequencia : 100) :
                int.MinValue;

            var percentualFrequenciaLabel = percentualFrequencia < 0 ? null : percentualFrequencia.ToString();

            // Critico
            if (percentualFrequencia <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequenciaLabel };

            // Alerta
            if (percentualFrequencia <= percentualAlerta)
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
