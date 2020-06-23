using SME.SGP.Infra.Dtos;

namespace SME.SGP.Infra.Interfaces
{
    public interface IServicoFila
    {
        void AdicionaFilaWorkerServidorRelatorios(AdicionaFilaDto adicionaFilaDto);
        void AdicionaFilaWorkerSgp(AdicionaFilaDto adicionaFilaDto);
    }
}
