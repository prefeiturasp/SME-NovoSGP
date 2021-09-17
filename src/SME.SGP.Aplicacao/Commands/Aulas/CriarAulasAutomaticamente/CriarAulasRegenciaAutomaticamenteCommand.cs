using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasRegenciaAutomaticamenteCommand : IRequest<bool>
    {
        public CriarAulasRegenciaAutomaticamenteCommand(Modalidade modalidade, long tipoCalendarioId, string ueCodigo, IEnumerable<DiaLetivoDto> diasLetivos, IEnumerable<DadosTurmaAulasAutomaticaDto> dadosTurmas, IEnumerable<DateTime> diasForaDoPeriodoEscolar)
        {
            Modalidade = modalidade;
            TipoCalendarioId = tipoCalendarioId;
            DiasLetivos = diasLetivos;
            DadosTurmas = dadosTurmas;
            DiasForaDoPeriodoEscolar = diasForaDoPeriodoEscolar;
            UeCodigo = ueCodigo;
        }

        public Modalidade Modalidade { get; set; }
        public long TipoCalendarioId { get; set; }
        public IEnumerable<DiaLetivoDto> DiasLetivos { get; set; }
        public IEnumerable<DadosTurmaAulasAutomaticaDto> DadosTurmas { get; set; }
        public IEnumerable<DateTime> DiasForaDoPeriodoEscolar { get; set; }
        public string UeCodigo { get; set; }
    }
}
