using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Boletim
{
    public interface IBoletimIdebUseCase
    {
        Task<ImportacaoLogRetornoDto> Executar(int ano, IEnumerable<IFormFile> boletins);
    }
}
