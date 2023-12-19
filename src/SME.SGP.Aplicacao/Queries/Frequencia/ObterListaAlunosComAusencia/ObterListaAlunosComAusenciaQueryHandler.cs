using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAlunosComAusenciaQueryHandler : IRequestHandler<ObterListaAlunosComAusenciaQuery, IEnumerable<AlunoAusenteDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterListaAlunosComAusenciaQueryHandler(
                                        IMediator mediator,
                                        IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<AlunoAusenteDto>> Handle(ObterListaAlunosComAusenciaQuery request, CancellationToken cancellationToken)
        {
            var alunosAusentesDto = new List<AlunoAusenteDto>();
            // Busca dados da turma
            var turma = await BuscaTurma(request.TurmaId);

            // Busca periodo
            var periodo = await BuscaPeriodo(turma, request.Bimestre);

            var alunosEOL = await mediator
                .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)), cancellationToken);

            var alunosAtivos = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (periodo.PeriodoInicio, periodo.PeriodoFim)), cancellationToken);
            
            if (alunosAtivos.NaoEhNulo() && alunosAtivos.Any())
                alunosAtivos = alunosAtivos.OrderByDescending(a => a.DataSituacao).ToList().DistinctBy(a => a.CodigoAluno);
            else
                throw new NegocioException("Não foram localizados alunos com matrícula ativa na turma, no período escolar selecionado.");

            var componentesCurricularesId = new List<long>() { long.Parse(request.DisciplinaId) };

            var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurricularesId.ToArray()), cancellationToken);

            if (disciplinasEOL.Any(d => d.TerritorioSaber))
                componentesCurricularesId.AddRange(disciplinasEOL.Where(d => d.TerritorioSaber).Select(d => d.CodigoComponenteCurricularTerritorioSaber));

            if (disciplinasEOL.EhNulo() || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado não localizado.");

            var quantidadeMaximaCompensacoes = int.Parse(await mediator
                .Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia, DateTime.Today.Year), cancellationToken));

            var percentualFrequenciaAlerta = int.Parse(await mediator
                .Send(new ObterValorParametroSistemaTipoEAnoQuery(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year), cancellationToken));                       
            
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunosAtivos.Select(x => x.CodigoAluno).ToArray(), turma);
            foreach (var alunoEOL in alunosAtivos)
            {               
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo
                    .ObterPorAlunoDisciplinaPeriodo(alunoEOL.CodigoAluno, componentesCurricularesId.Select(cc => cc.ToString()).ToArray(), periodo.Id, turma.CodigoTurma);

                if (frequenciaAluno.EhNulo() || frequenciaAluno.NumeroFaltasNaoCompensadas <= 0 || frequenciaAluno.PercentualFrequencia == 100)
                    continue;

                var faltasNaoCompensadas = int.Parse(frequenciaAluno.NumeroFaltasNaoCompensadas.ToString());

                alunosAusentesDto.Add(new AlunoAusenteDto()
                {
                    Id = alunoEOL.CodigoAluno,
                    Nome = alunoEOL.NomeAluno,
                    QuantidadeFaltasTotais = faltasNaoCompensadas,
                    MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > faltasNaoCompensadas ? faltasNaoCompensadas : quantidadeMaximaCompensacoes,
                    PercentualFrequencia = frequenciaAluno.PercentualFrequencia,
                    Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == alunoEOL.CodigoAluno)
                });
            }

            return alunosAusentesDto;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, Turma turma)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosCodigos));
        }
        private async Task<Turma> BuscaTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
            if (turma.EhNulo())
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private async Task<PeriodoEscolar> BuscaPeriodo(Turma turma, int bimestre)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi possível localizar o tipo de calendário da turma");

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi possível localizar os períodos escolares da turma");

            var periodoEscolar = periodosEscolares?.FirstOrDefault(p => p.Bimestre == bimestre);
            if (periodoEscolar.EhNulo())
                throw new NegocioException($"Período escolar do {bimestre}º Bimestre não localizado para a turma");

            return periodoEscolar;
        }
    }
}
