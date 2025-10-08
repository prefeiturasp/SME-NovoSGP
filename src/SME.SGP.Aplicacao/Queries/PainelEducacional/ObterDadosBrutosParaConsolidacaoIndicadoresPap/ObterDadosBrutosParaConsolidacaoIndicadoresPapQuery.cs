using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap;
using SME.SGP.Aplicacao.Queries.ConsolidacaoFrequenciaTurma.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesIndicadoresPap;
using SME.SGP.Dominio.Entidades.MapeamentoPap;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap
{
    public class ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery : IRequest<(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunoTurma,
            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> dificuldadesPap,
            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> frequenciaBaixa)>
    {
        public int AnoLetivo { get; set; }

        public ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery(int anoLetivo)
        {
            this.AnoLetivo = anoLetivo;
        }
    }
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
            var dadosAlunosTurmasPap = await mediator.Send(new ObterAlunosTurmaPapQuery(request.AnoLetivo), cancellationToken);
            if (dadosAlunosTurmasPap == null || !dadosAlunosTurmasPap.Any())
                return (null, null, null);

            dadosAlunosTurmasPap = DefinirTipoPap(dadosAlunosTurmasPap);
            var indicadoresPap = await mediator.Send(new ObterDificuldadesIndicadoresPapQuery(dadosAlunosTurmasPap), cancellationToken);
            var dadosFrequencia = await mediator.Send(new ObterQuantidadeAbaixoFrequenciaPorTurmaQuery(request.AnoLetivo), cancellationToken);

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
    public class ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryValidator : AbstractValidator<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>
    {
        public ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE).WithMessage($"O ano letivo deve ser maior ou igual {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
        }
    }
}