using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosRegistroPoa
    {
        void Atualizar(RegistroPoaDto registroPoaDto);

        void Cadastrar(RegistroPoaDto registroPoaDto);

        void Excluir(long id);
    }
}