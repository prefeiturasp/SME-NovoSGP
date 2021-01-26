import api from '~/servicos/api';

const urlPadrao = 'v1/armazenamento/documentos';

class ServicoDocumentosPlanosTrabalho {
  obterTiposDeDocumentos = () => {
    return api.get(`${urlPadrao}/tipos`);
  };

  salvarDocumento = (params, documentoId) => {
    if (documentoId) {
      return api.put(`${urlPadrao}/${documentoId}`, {
        codigoArquivo: params.arquivoCodigo,
      });
    }

    return api.post(`${urlPadrao}`, params);
  };

  obterDocumento = documentoId => {
    return api.get(`${urlPadrao}/${documentoId}`);
  };

  excluirDocumento = documentoId => {
    return api.delete(`${urlPadrao}/${documentoId}`);
  };

  validacaoUsuarioDocumento = (
    documentoId,
    tipoDocumentoId,
    classificacaoId,
    usuarioId,
    ueId
  ) => {
    const url = `${documentoId}/tipo-documento/${tipoDocumentoId}/classificacao/${classificacaoId}/usuario/${usuarioId}/ue/${ueId}`;
    return api.get(`${urlPadrao}/${url}`);
  };
}

export default new ServicoDocumentosPlanosTrabalho();
