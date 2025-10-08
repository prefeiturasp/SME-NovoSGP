using MediatR;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;

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
}