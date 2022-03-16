using FluentValidation;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiarioBordoObservacaoDto
    {
        public string Observacao { get; set; }
        public long DiarioBordoId { get; set; }
        public string UsuarioNomeDiarioBordo { get; set; }
        public string UsuarioCodigoRfDiarioBordo { get; set; }
    }
}
