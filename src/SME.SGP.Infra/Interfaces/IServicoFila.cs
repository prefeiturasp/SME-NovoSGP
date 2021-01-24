using SME.SGP.Infra.Dtos;
using System;

namespace SME.SGP.Infra.Interfaces
{
    public interface IServicoFila
    {
        [Obsolete("Utilizar: mediator.Send(new PublicaFilaWorkerServidorRelatoriosCommand(...")]
        void PublicaFilaWorkerServidorRelatorios(PublicaFilaRelatoriosDto adicionaFilaDto);

        [Obsolete("Utilizar: mediator.Send(new PublicaFilaWorkerSgpCommand(...")]
        void PublicaFilaWorkerSgp(PublicaFilaSgpDto publicaFilaSgpDto);
    }
}
