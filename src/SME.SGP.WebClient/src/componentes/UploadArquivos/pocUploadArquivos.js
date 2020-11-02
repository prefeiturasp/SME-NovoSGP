import { Upload } from 'antd';
import React, { useState } from 'react';
import styled from 'styled-components';
import Card from '~/componentes/card';
import { api, erro, erros, sucesso } from '~/servicos';
import UploadArquivos from './uploadArquivos';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;
`;

const PocUploadArquivos = () => {
  const [listaDeArquivos, setLstaDeArquivos] = useState();

  const onChange = ({ file, fileList }) => {
    const { status } = file;

    const tamanhoArquivo = file.size / 1024 / 1024;
    if (tamanhoArquivo > 100) {
      const novaLista = fileList.filter(item => item.uid !== file.uid);
      setLstaDeArquivos([...novaLista]);
      return;
    }

    if (status === 'done') {
      sucesso(`${file.name} arquivo carregado com sucesso`);
    } else if (status === 'error') {
      erro(`${file.name} erro ao carregar arquivo`);
    }

    setLstaDeArquivos([...fileList]);
  };

  const customRequest = options => {
    const { onSuccess, onError, file, onProgress } = options;

    const fmData = new FormData();
    const config = {
      headers: { 'content-type': 'multipart/form-data' },
      onUploadProgress: event => {
        onProgress({ percent: (event.loaded / event.total) * 100 }, file);
      },
    };
    fmData.append('file', file);

    api
      .post('v1/armazenamento/upload', fmData, config)
      .then(resposta => {
        onSuccess(file, resposta.data);
      })
      .catch(e => {
        onError({ event: e });
      });
  };

  const beforeUpload = arquivo => {
    const tamanhoArquivo = arquivo.size / 1024 / 1024;
    if (tamanhoArquivo > 100) {
      erro('Tamanho máximo é de 100 mb');
      return false;
    }

    return true;
  };

  const downloadBlob = (data, fileName) => {
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.style = 'display: none';

    const blob = new Blob([data]);
    const url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);

    document.body.removeChild(a);
  };

  const onDownload = arquivo => {
    const codigoArquivo = arquivo.xhr;
    api
      .get(`v1/armazenamento/${codigoArquivo}`, { responseType: 'arraybuffer' })
      .then(resposta => {
        downloadBlob(resposta.data, arquivo.name);
      })
      .catch(e => erros(e));
  };

  const onRemove = async arquivo => {
    const codigoArquivo = arquivo.xhr;
    const resposta = await api
      .delete(`v1/armazenamento/${codigoArquivo}`)
      .catch(e => erros(e));

    if (resposta && resposta.status === 200) {
      sucesso(`Arquivo ${arquivo.name} removido com sucesso`);
      return true;
    }
    return false;
  };

  const props = {
    onChange,
    texoErroUpload: 'Erro ao carregar arquivo',
    textoUpload:
      'Clique ou arraste para fazer o upload do arquivo de planejamento.',
    textoFormatoUpload: 'Todos os formatos suportados no limite de 100 mb',
    customRequest,
    fileList: listaDeArquivos,
    beforeUpload,
    onRemove,
    onDownload,
    showUploadList: {
      showRemoveIcon: true,
      showDownloadIcon: true,
    },
  };

  return (
    <Card>
      <div className="col-md-12">
        <UploadArquivos {...props} />
      </div>
    </Card>
  );
};

export default PocUploadArquivos;
