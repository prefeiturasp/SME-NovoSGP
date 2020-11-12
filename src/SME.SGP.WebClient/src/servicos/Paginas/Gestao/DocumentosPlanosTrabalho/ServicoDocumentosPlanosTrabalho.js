import api from '~/servicos/api';

const urlPadrao = 'v1/armazenamento/documentos';

class ServicoDocumentosPlanosTrabalho {
  obterArquivoParaDownload = codigoArquivo => {
    return api.get(`${urlPadrao}/${codigoArquivo}`, {
      responseType: 'arraybuffer',
    });
  };

  obterTiposDeDocumentos = () => {
    return api.get(`${urlPadrao}/tipos`);
  };

  salvarDocumento = params => {
    const url = `${urlPadrao}`;
    return api.post(url, params);
  };

  obterDocumento = documentoId => {
    return api.get(`${urlPadrao}/${documentoId}`);
  };
}

export default new ServicoDocumentosPlanosTrabalho();
