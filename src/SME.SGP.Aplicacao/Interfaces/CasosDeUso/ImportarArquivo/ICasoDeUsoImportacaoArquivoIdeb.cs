using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo
{
    public interface ICasoDeUsoImportacaoArquivoIdeb
    {
        Task<RetornoDto> Executar(IFormFile arquivo);
    }
}
