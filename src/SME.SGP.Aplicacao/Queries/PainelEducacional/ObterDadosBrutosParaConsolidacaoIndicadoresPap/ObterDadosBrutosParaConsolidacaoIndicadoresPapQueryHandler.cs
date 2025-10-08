using MediatR;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap;
using SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio.Entidades.MapeamentoPap;
using SME.SGP.Infra.Dtos.Frequencia;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap
{
    public class ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandler : IRequestHandler<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery, (IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunoTurma, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> dificuldadesPap, IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> frequenciaBaixa)>
    {
        private readonly IMediator mediator;
        public ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<(
            IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunoTurma,
            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> dificuldadesPap,
            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> frequenciaBaixa)> Handle(ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            var dadosAlunosTurmasPap = await mediator.Send(new ObterAlunosTurmaPapQuery(request.AnoLetivo));
            if (dadosAlunosTurmasPap == null || !dadosAlunosTurmasPap.Any())
                return (null, null, null);

            dadosAlunosTurmasPap = DefinirTipoPap(dadosAlunosTurmasPap);
            var indicadoresPap = await mediator.Send(new ObterIndicadoresPapSgpConsolidadoQuery(dadosAlunosTurmasPap));
            var dadosFrequencia = await mediator.Send(new ObterQuantidadeAbaixoFrequenciaPorTurmaQuery(request.AnoLetivo));

            return (dadosAlunosTurmasPap, indicadoresPap, dadosFrequencia);
        }

        private static IEnumerable<DadosMatriculaAlunoTipoPapDto> DefinirTipoPap(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosMatriculaAlunos)
        {
            foreach (var dado in dadosMatriculaAlunos)
            {
                dado.TipoPap = PapComponenteCurricularMap.ObterTipoPapPorComponente(dado.ComponenteCurricularId);
                yield return dado;
            }
        }
    }
}