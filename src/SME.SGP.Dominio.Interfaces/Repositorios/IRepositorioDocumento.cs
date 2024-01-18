using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDocumento : IRepositorioBase<Documento>
    {
        Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, long anoLetivo,long documentoId);
        Task<PaginacaoResultadoDto<DocumentoResumidoDto>> ObterPorDreUeTipoEClassificacaoPaginada(long? dreId, long? ueId, long tipoDocumentoId, long classificacaoId, int? anoLetivo, Paginacao paginacao);
        Task<bool> RemoverReferenciaArquivo(long documentoId, long arquivoId);
        Task<bool> ExcluirDocumentoPorId(long id);
        Task<ObterDocumentoResumidoDto> ObterPorIdCompleto(long documentoId);
    }
}