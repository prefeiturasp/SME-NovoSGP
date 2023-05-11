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
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterListaAlunosComAusenciaQueryHandler(
                                        IMediator mediator,
                                        IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                        IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
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
                .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma)));

            var alunosAtivos = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (periodo.PeriodoInicio, periodo.PeriodoFim)));

            var usuarioLogado = await mediator
                .Send(new ObterUsuarioLogadoQuery());

            var codigosTerritoriosEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(long.Parse(request.DisciplinaId), turma.CodigoTurma, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null));

            var componentesCurricularesId = new List<long>() { long.Parse(request.DisciplinaId) };

            var professor = string.Empty;
            if (codigosTerritoriosEquivalentes != null && codigosTerritoriosEquivalentes.Any())
            {
                componentesCurricularesId.AddRange(codigosTerritoriosEquivalentes.Select(c => long.Parse(c.codigoComponente)).Except(componentesCurricularesId));
                professor = codigosTerritoriosEquivalentes.First().professor;
            }

            var disciplinasEOL = await repositorioComponenteCurricular
                .ObterDisciplinasPorIds(componentesCurricularesId.ToArray()); 

            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado não localizado.");

            var quantidadeMaximaCompensacoes = int.Parse(await mediator
                .Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia, DateTime.Today.Year)));

            var percentualFrequenciaAlerta = int.Parse(await mediator
                .Send(new ObterValorParametroSistemaTipoEAnoQuery(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year)));                       

            foreach (var alunoEOL in alunosAtivos)
            {               
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo
                    .ObterPorAlunoDisciplinaPeriodo(alunoEOL.CodigoAluno, componentesCurricularesId.Select(cc => cc.ToString()).ToArray(), periodo.Id, turma.CodigoTurma, professor);

                if (frequenciaAluno == null || frequenciaAluno.NumeroFaltasNaoCompensadas <= 0 || frequenciaAluno.PercentualFrequencia == 100)
                    continue;

                var faltasNaoCompensadas = int.Parse(frequenciaAluno.NumeroFaltasNaoCompensadas.ToString());

                alunosAusentesDto.Add(new AlunoAusenteDto()
                {
                    Id = alunoEOL.CodigoAluno,
                    Nome = alunoEOL.NomeAluno,
                    QuantidadeFaltasTotais = faltasNaoCompensadas,
                    MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > faltasNaoCompensadas ? faltasNaoCompensadas : quantidadeMaximaCompensacoes,
                    PercentualFrequencia = frequenciaAluno.PercentualFrequencia,
                    Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta
                });
            }

            return alunosAusentesDto;
        }

        private async Task<Turma> BuscaTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private async Task<PeriodoEscolar> BuscaPeriodo(Turma turma, int bimestre)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (tipoCalendario == null)
                throw new NegocioException("Não foi possível localizar o tipo de calendário da turma");

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi possível localizar os períodos escolares da turma");

            var periodoEscolar = periodosEscolares?.FirstOrDefault(p => p.Bimestre == bimestre);
            if (periodoEscolar == null)
                throw new NegocioException($"Período escolar do {bimestre}º Bimestre não localizado para a turma");

            return periodoEscolar;
        }        
    }
}
