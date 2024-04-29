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
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQueryHandler : IRequestHandler<ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery, AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;
        private readonly IRepositorioMapeamentoEstudante repositorioMapeamento;

        private const string HIPOTESE_ESCRITA_ALFABETICO = "A";
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
            var alunosTurma = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTimeExtension.HorarioBrasilia().Date));
            var matriculadosTurmaPAP = await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosTurma.Select(x => x.CodigoAluno).ToArray()));
            var alunosComMapeamento = await repositorioMapeamento.ObterCodigosAlunosComMapeamentoEstudanteBimestre(request.TurmaId, request.Bimestre);

            var retorno = new List<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto>();

            var alunosSondagemProvaSPInsuficientes = new List<string>();

            var tasks = alunosTurma
                .AsParallel()
                .WithDegreeOfParallelism(10)
                .Select(async aluno =>
                {
                    var sondagem = await mediator.Send(new ObterSondagemLPAlunoQuery(turma.CodigoTurma, aluno.CodigoAluno));
                    var avaliacoesExternasProvaSP = await mediator.Send(new ObterAvaliacoesExternasProvaSPAlunoQuery(aluno.CodigoAluno, turma.AnoLetivo));
                    if ((sondagem.ObterHipoteseEscrita(request.Bimestre) != HIPOTESE_ESCRITA_ALFABETICO 
                         && !string.IsNullOrEmpty(sondagem.ObterHipoteseEscrita(request.Bimestre)))
                        || avaliacoesExternasProvaSP.Any(psp => psp.Nivel.ToUpper() == RESULTADO_ABAIXO_BASICO_PROVA_SP.ToUpper()))
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
                    retorno.Add(new AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(aluno.CodigoAluno));
            }
            
            foreach (var alunoMapeado in alunosComMapeamento)
            {
                var alunoAlerta = retorno.FirstOrDefault(r => r.CodigoAluno.Equals(alunoMapeado));
                if (alunoAlerta.NaoEhNulo())
                    alunoAlerta.PossuiMapeamentoEstudante = true;
                else
                    retorno.Add(new AlunoSinalizadoPrioridadeMapeamentoEstudanteDto(alunoMapeado, true));
            } 

            return retorno.ToArray();
        }

    }
}
