using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAtribuicaoCJ
    {
        Task<IEnumerable<AtribuicaoCJListaRetornoDto>> Listar(AtribuicaoCJListaFiltroDto filtroDto);

        Task<AtribuicaoCJTitularesRetornoDto> ObterProfessoresTitularesECjs(string ueId, string turmaId,
                    string professorRf, Modalidade modalidadeId);
    }
}