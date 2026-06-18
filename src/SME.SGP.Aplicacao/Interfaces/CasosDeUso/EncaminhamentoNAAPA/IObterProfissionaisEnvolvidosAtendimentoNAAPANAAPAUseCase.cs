using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase : IUseCase<FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA, IEnumerable<FuncionarioUnidadeDto>>
    { }

}
