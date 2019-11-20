using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAtribuicaoEsporadica
    {
        void Salvar(AtribuicaoEsporadicaCompletaDto atruibuicaoEsporadicaCompletaDto);
    }
}