using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoAnual
    {
        void Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto);

        void Salvar(PlanoAnualDto planoAnualDto);
    }
}