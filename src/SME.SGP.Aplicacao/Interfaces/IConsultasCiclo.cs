using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCiclo
    {
        CicloDto Selecionar(int ano);
    }
}