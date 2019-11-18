using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPeriodoEscolar
    {
        void Salvar(PeriodoEscolarListaDto periodosDto);
    }
}