using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoReaberturaPorDataTurmaQuery : IRequest<FechamentoReabertura>
    {
        public DateTime DataParaVerificar { get; internal set; }
        public long UeId { get; internal set; }
        public long TipoCalendarioId { get; set; }
    }
}
