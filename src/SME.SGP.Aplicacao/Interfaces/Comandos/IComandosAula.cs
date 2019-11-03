using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAula
    {
        void Inserir(AulaDto dto);
        void Alterar(AulaDto dto, long id);
        void Excluir(long id);
    }
}
