using MediatR;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryHandler : IRequestHandler<ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery, string[]>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private const string HIPOTESE_ESCRITA_ALFABETICO = "A";
        private const string RESULTADO_ABAIXO_BASICO_PROVA_SP = "Abaixo do básico";

        public ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE, IMediator mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string[]> Handle(ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            var alunosTurma = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTimeExtension.HorarioBrasilia().Date));
            var matriculadosTurmaPAP = await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosTurma.Select(x => x.CodigoAluno).ToArray()));

            var retorno = new List<string>();

            var alunosSondagemProvaSPInsuficientes = new List<string>();

            var tasks = alunosTurma
                .AsParallel()
                .WithDegreeOfParallelism(10)
                .Select(async aluno =>
                {
                    var sondagem = await mediator.Send(new ObterSondagemLPAlunoQuery(turma.CodigoTurma, aluno.CodigoAluno));
                    var avaliacoesExternasProvaSP = await mediator.Send(new ObterAvaliacoesExternasProvaSPAlunoQuery(aluno.CodigoAluno, turma.AnoLetivo));
                    if (SondagemHipoteseEscritaDiferenteAlfabetica(sondagem)
                        || avaliacoesExternasProvaSP.Any(psp => psp.Nivel == RESULTADO_ABAIXO_BASICO_PROVA_SP)
                    )
                        alunosSondagemProvaSPInsuficientes.Add(aluno.CodigoAluno);
                });

            await Task.WhenAll(tasks);


            foreach (var aluno in alunosTurma)
            {
                var qdadePlanosAEE = await repositorioPlanoAEE.ObterQdadePlanosAEEAtivosAluno(aluno.CodigoAluno);
                if (matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno)
                    || qdadePlanosAEE > 0
                    || alunosSondagemProvaSPInsuficientes.Contains(aluno.CodigoAluno)
                    )
                    retorno.Add(aluno.CodigoAluno);
            }

            return retorno.ToArray();
        }

        private bool SondagemHipoteseEscritaDiferenteAlfabetica(SondagemLPAlunoDto sondagem)
        {
            return sondagem.ObterHipoteseEscrita(1) != HIPOTESE_ESCRITA_ALFABETICO && !string.IsNullOrEmpty(sondagem.ObterHipoteseEscrita(1))
                    || sondagem.ObterHipoteseEscrita(1) != HIPOTESE_ESCRITA_ALFABETICO && !string.IsNullOrEmpty(sondagem.ObterHipoteseEscrita(2))
                    || sondagem.ObterHipoteseEscrita(1) != HIPOTESE_ESCRITA_ALFABETICO && !string.IsNullOrEmpty(sondagem.ObterHipoteseEscrita(3))
                    || sondagem.ObterHipoteseEscrita(1) != HIPOTESE_ESCRITA_ALFABETICO && !string.IsNullOrEmpty(sondagem.ObterHipoteseEscrita(4));
        }
    }
}
