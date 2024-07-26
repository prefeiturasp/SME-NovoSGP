using MediatR;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryHandler : IRequestHandler<ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery, AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IRepositorioMapeamentoEstudante repositorioMapeamento;

        private readonly string[] HIPOTESES_ESCRITA_NAO_ALFABETICAS = new string[] { "PS", "SSV", "SCV", "SA" };
        private const string RESULTADO_ABAIXO_BASICO_PROVA_SP = "Abaixo do básico";

        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE, 
                                                                                      IMediator mediator,
                                                                                      IRepositorioMapeamentoEstudante repositorioMapeamento)
        {
            this.repositorioMapeamento = repositorioMapeamento ?? throw new System.ArgumentNullException(nameof(repositorioMapeamento));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]> Handle(ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            var periodo = await BuscaPeriodo(turma, request.Bimestre);
            var alunosTurma = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (periodo.PeriodoInicio, periodo.PeriodoFim)));

            var matriculadosTurmaPAP = await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosTurma.Select(x => x.CodigoAluno).ToArray()));
            var alunosComMapeamento = await repositorioMapeamento.ObterCodigosAlunosComMapeamentoEstudanteBimestre(request.TurmaId, request.Bimestre);

            var retorno = new List<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto>();

            var alunosSondagemInsuficiente = new List<string>();
            var alunosProvaSPInsuficiente = new List<string>();

            var tasks = alunosTurma
                .AsParallel()
                .WithDegreeOfParallelism(10)
                .Select(async aluno =>
                {
                    var sondagem = await mediator.Send(new ObterSondagemLPAlunoQuery(turma.CodigoTurma, aluno.CodigoAluno));
                    var avaliacoesExternasProvaSP = await mediator.Send(new ObterAvaliacoesExternasProvaSPAlunoQuery(aluno.CodigoAluno, turma.AnoLetivo-1));
                    if (HIPOTESES_ESCRITA_NAO_ALFABETICAS.Contains(sondagem.ObterHipoteseEscrita(request.Bimestre)))
                        alunosSondagemInsuficiente.Add(aluno.CodigoAluno);
                    if (avaliacoesExternasProvaSP.Any(psp => psp.Nivel.ToUpper() == RESULTADO_ABAIXO_BASICO_PROVA_SP.ToUpper()))
                        alunosProvaSPInsuficiente.Add(aluno.CodigoAluno);
                });

            await Task.WhenAll(tasks);


            foreach (var aluno in alunosTurma)
            {
                var qdadePlanosAEE = await repositorioPlanoAEE.ObterQdadePlanosAEEAtivosAluno(aluno.CodigoAluno);
                var alertaLaranja = (matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                                    || qdadePlanosAEE > 0
                                    || alunosProvaSPInsuficiente.Contains(aluno.CodigoAluno));
                var alertaVermelho = alunosSondagemInsuficiente.Contains(aluno.CodigoAluno);


                if (alertaLaranja || alertaVermelho) 
                    retorno.Add(new AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(aluno.CodigoAluno, alertaLaranja, alertaVermelho));
            }
            
            foreach (var alunoMapeado in alunosComMapeamento)
            {
                var alunoAlerta = retorno.FirstOrDefault(r => r.CodigoAluno.Equals(alunoMapeado));
                if (alunoAlerta.NaoEhNulo())
                    alunoAlerta.PossuiMapeamentoEstudante = true;
                else
                    retorno.Add(new AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(alunoMapeado, possuiMapeamentoEstudante: true));
            } 

            return retorno.ToArray();
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
