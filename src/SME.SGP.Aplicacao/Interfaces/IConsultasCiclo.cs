using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCiclo
    {
        IEnumerable<CicloDto> Listar(IEnumerable<string> Ano, string AnoSelecionado, int Modalidade);

        CicloDto Selecionar(int ano);
    }
}