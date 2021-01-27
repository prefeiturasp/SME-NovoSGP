using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDocumento : IRepositorioBase<Documento>
    {
        Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, long documentoId);
        Task<PaginacaoResultadoDto<DocumentoDto>> ObterPorUeTipoEClassificacaoPaginada(long ueId, long tipoDocumentoId, long classificacaoId, Paginacao paginacao);
        Task<bool> RemoverReferenciaArquivo(long documentoId, long arquivoId);
        Task<bool> ExcluirDocumentoPorId(long id);
        Task<ObterDocumentoDto> ObterPorIdCompleto(long documentoId);
    }
}