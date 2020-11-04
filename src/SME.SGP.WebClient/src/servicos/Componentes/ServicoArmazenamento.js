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

  fazerUploadArquivo = (formData, configuracaoHeader) => {
    return api.post(`${urlPadrao}/upload`, formData, configuracaoHeader);
  };
}

export default new ServicoArmazenamento();
