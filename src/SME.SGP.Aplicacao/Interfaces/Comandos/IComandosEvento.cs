using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosEvento
    {
        void Salvar(EventoDto eventoDto);
    }
}