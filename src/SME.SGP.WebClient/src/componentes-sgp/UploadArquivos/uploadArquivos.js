import { InboxOutlined } from '@ant-design/icons';
import { Upload } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import styled from 'styled-components';
import { erro, erros, sucesso } from '~/servicos';
import ServicoArmazenamento from '~/servicos/Componentes/ServicoArmazenamento';
import { downloadBlob } from '~/utils/funcoes/gerais';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;
`;

const TAMANHO_MAXIMO_UPLOAD = 100;

const UploadArquivos = props => {
  const {
    urlAction,
    multiplosArquivos,
    textoErroUpload,
    textoUpload,
    textoFormatoUpload,
    customRequest,
    beforeUpload,
    showUploadList,
    onRemove,
  } = props;

  const [listaDeArquivos, setLstaDeArquivos] = useState();

  const excedeuLimiteMaximo = arquivo => {
    const tamanhoArquivo = arquivo.size / 1024 / 1024;
    return tamanhoArquivo > TAMANHO_MAXIMO_UPLOAD;
  };

  const onRemoveDefault = async arquivo => {
    const codigoArquivo = arquivo.xhr;
    const resposta = await ServicoArmazenamento.removerArquivo(
      codigoArquivo
    ).catch(e => erros(e));

    if (resposta && resposta.status === 200) {
      sucesso(`Arquivo ${arquivo.name} excluído com sucesso`);
      return true;
    }
    return false;
  };

  const beforeUploadDefault = arquivo => {
    if (excedeuLimiteMaximo(arquivo)) {
      erro('Tamanho máximo 100mb');
      return false;
    }

    return true;
  };

  const onDownload = arquivo => {
    const codigoArquivo = arquivo.xhr;
    ServicoArmazenamento.obterArquivoParaDownload(codigoArquivo)
      .then(resposta => {
        downloadBlob(resposta.data, arquivo.name);
      })
      .catch(e => erros(e));
  };

  const customRequestDefault = options => {
    const { onSuccess, onError, file, onProgress } = options;

    const fmData = new FormData();
    const config = {
      headers: { 'content-type': 'multipart/form-data' },
      onUploadProgress: event => {
        onProgress({ percent: (event.loaded / event.total) * 100 }, file);
      },
    };
    fmData.append('file', file);

    ServicoArmazenamento.fazerUploadArquivo(fmData, config)
      .then(resposta => {
        onSuccess(file, resposta.data);
      })
      .catch(e => {
        onError({ event: e });
      });
  };

  const atualizaListaArquivos = (fileList, file) => {
    const novaLista = fileList.filter(item => item.uid !== file.uid);
    setLstaDeArquivos([...novaLista]);
  };

  const onChange = ({ file, fileList }) => {
    const { status } = file;

    if (excedeuLimiteMaximo(file)) {
      atualizaListaArquivos(fileList, file);
      return;
    }

    if (status === 'done') {
      sucesso(`${file.name} arquivo carregado com sucesso`);
    } else if (status === 'error') {
      atualizaListaArquivos(fileList, file);
      erro(`${file.name} erro ao carregar arquivo`);
      return;
    }

    setLstaDeArquivos([...fileList]);
  };

  const propsDragger = {
    locale: { uploadError: textoErroUpload },
    name: 'file',
    multiple: multiplosArquivos,
    action: urlAction,
    onChange,
    customRequest: customRequest || customRequestDefault,
    fileList: listaDeArquivos,
    beforeUpload: beforeUpload || beforeUploadDefault,
    onDownload,
    showUploadList,
    onRemove: onRemove || onRemoveDefault,
  };

  return (
    <ContainerDragger {...propsDragger}>
      <p className="ant-upload-drag-icon">
        <InboxOutlined />
      </p>
      <p className="ant-upload-text">{textoUpload}</p>
      <p className="ant-upload-hint">{textoFormatoUpload}</p>
    </ContainerDragger>
  );
};

UploadArquivos.propTypes = {
  urlAction: PropTypes.string,
  multiplosArquivos: PropTypes.bool,
  textoErroUpload: PropTypes.string,
  textoUpload: PropTypes.string,
  textoFormatoUpload: PropTypes.string,
  customRequest: PropTypes.func,
  fileList: PropTypes.oneOfType([PropTypes.array]),
  beforeUpload: PropTypes.func,
  showUploadList: PropTypes.oneOfType([PropTypes.object]),
  onRemove: PropTypes.func,
};

UploadArquivos.defaultProps = {
  urlAction: '',
  multiplosArquivos: false,
  textoErroUpload: 'Erro ao carregar arquivo',
  textoUpload:
    'Clique ou arraste para fazer o upload do arquivo de planejamento',
  textoFormatoUpload: 'Todos os formatos são suportados no limite de 100mb',
  customRequest: null,
  fileList: [],
  beforeUpload: null,
  showUploadList: {
    showRemoveIcon: true,
    showDownloadIcon: true,
    showPreviewIcon: false,
  },
  onRemove: null,
};

export default UploadArquivos;
