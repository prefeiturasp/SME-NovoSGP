using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ImportacaoBoletim
    {
        public int AnoLetivo { get; set; }
        public List<ImportacaoBoletimItens> Importacoes { get; set; }
    }

    public class ImportacaoBoletimItens
    {
        public string CodigoUe { get; set; }
        public IFormFile Boletim { get; set; }
    }
}
