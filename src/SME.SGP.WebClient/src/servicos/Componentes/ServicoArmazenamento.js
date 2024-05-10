import api from '~/servicos/api';

const urlPadrao = 'v1/armazenamento';

class ServicoArmazenamento {
  obterArquivoParaDownload = codigoArquivo => {
    return api.get(`${urlPadrao}/${codigoArquivo}`, {
      responseType: 'arraybuffer',
    });
  };

  removerArquivo = codigoArquivo => {
    return api.delete(`${urlPadrao}/${codigoArquivo}`);
  };

  fazerUploadArquivo = (formData, configuracaoHeader, urlUpload) => {
    let url = `${urlPadrao}/upload`;
    if (urlUpload) {
      url = urlUpload;
    }
    return api.post(url, formData, configuracaoHeader);
  };
}

export default new ServicoArmazenamento();
