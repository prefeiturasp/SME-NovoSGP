using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDocumento : IRepositorioBase<Documento>
    {
        Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId);
        Task<IEnumerable<DocumentoDto>> ObterPorUeTipoEClassificacao(long ueId, long tipoDocumentoId, long classificacaoId);
    }
}