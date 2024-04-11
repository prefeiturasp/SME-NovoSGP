using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase : IUseCase<FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA, IEnumerable<FuncionarioUnidadeDto>>
    {}

}
