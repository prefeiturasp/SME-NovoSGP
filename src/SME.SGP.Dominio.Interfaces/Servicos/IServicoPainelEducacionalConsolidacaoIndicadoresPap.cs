using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces.Servicos
{
    public interface IServicoPainelEducacionalConsolidacaoIndicadoresPap
    {
        (IList<PainelEducacionalConsolidacaoPapSme> Sme,
         IList<PainelEducacionalConsolidacaoPapDre> Dre,
         IList<PainelEducacionalConsolidacaoPapUe> Ue) ConsolidarDados(
            IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunosTurmasPap,
            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadoresPap,
            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> dadosFrequencia);
    }
}