using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DadosCriacaoAulasAutomaticasDto
    {
        public DadosCriacaoAulasAutomaticasDto(string ueCodigo, long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasLetivosENaoLetivos, IEnumerable<DateTime> diasForaDoPeriodoEscolar, Modalidade modalidade, IEnumerable<DadosTurmaAulasAutomaticaDto> dadosTurmas)
        {
            UeCodigo = ueCodigo;
            TipoCalendarioId = tipoCalendarioId;
            DiasLetivosENaoLetivos = diasLetivosENaoLetivos;
            DiasForaDoPeriodoEscolar = diasForaDoPeriodoEscolar;
            Modalidade = modalidade;
            DadosTurmas = dadosTurmas;
        }
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public IEnumerable<DiaLetivoDto> DiasLetivosENaoLetivos { get; set; }
        public IEnumerable<DateTime> DiasForaDoPeriodoEscolar { get; set; }
        public Modalidade Modalidade { get; set; }
        public IEnumerable<DadosTurmaAulasAutomaticaDto> DadosTurmas { get; set; }
    }
}
