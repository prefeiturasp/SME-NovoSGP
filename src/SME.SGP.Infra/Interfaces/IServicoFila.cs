using SME.SGP.Infra.Dtos;

namespace SME.SGP.Infra.Interfaces
{
    public interface IServicoFila
    {
        void PublicaFilaWorkerServidorRelatorios(PublicaFilaRelatoriosDto adicionaFilaDto);
        void PublicaFilaWorkerSgp(PublicaFilaSgpDto publicaFilaSgpDto);
    }
}
