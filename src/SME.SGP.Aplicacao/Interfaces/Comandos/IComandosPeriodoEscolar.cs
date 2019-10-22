using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosPeriodoEscolar
    {
        void Salvar(PeriodoEscolarListaDto periodosDto);
    }
}