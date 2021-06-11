import { InboxOutlined } from '@ant-design/icons';
import { Upload } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';
import { erro, erros, sucesso } from '~/servicos';
import ServicoArmazenamento from '~/servicos/Componentes/ServicoArmazenamento';
import { downloadBlob, permiteInserirFormato } from '~/utils/funcoes/gerais';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;

  cursor: ${props =>
    props.desabilitarUpload ? 'not-allowed' : 'pointer'} !important;

  .ant-upload-btn {
    pointer-events: ${props =>
      props.desabilitarUpload ? 'none' : 'auto'} !important;

    opacity: ${props => (props.desabilitarUpload ? '0.6' : '1')} !important;
  }

  border-color: ${props =>
    props.campoObrigadorio ? Base.Vermelho : '#d9d9d9'} !important;
`;

export const ContainerUpload = styled.div`
  .ant-upload-list-item-card-actions {
    opacity: 1 !important;
  }
`;

export const MensagemCampoObrigatorio = styled.span`
  color: ${Base.Vermelho} !important;
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
    onChangeListaArquivos,
    defaultFileList,
    urlUpload,
    tiposArquivosPermitidos,
    desabilitarUpload,
    desabilitarGeral,
    form,
    name,
    id,
  } = props;

  const [listaDeArquivos, setListaDeArquivos] = useState([...defaultFileList]);

  useEffect(() => {
    if (defaultFileList?.length) {
      const novoMap = defaultFileList.map(item => {
        return { ...item, status: 'done' };
      });
      setListaDeArquivos(novoMap);
    } else {
      setListaDeArquivos([]);
    }
  }, [defaultFileList]);

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
    if (!permiteInserirFormato(arquivo, tiposArquivosPermitidos)) {
      erro('Formato não permitido');
      return false;
    }

    if (excedeuLimiteMaximo(arquivo)) {
      erro('Tamanho máximo 100 MB');
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

    ServicoArmazenamento.fazerUploadArquivo(fmData, config, urlUpload)
      .then(resposta => {
        onSuccess(file, resposta.data);
      })
      .catch(e => {
        onError({ event: e });
        erros(e);
      });
  };

  const atualizaListaArquivos = (fileList, file) => {
    const novaLista = fileList.filter(item => item.uid !== file.uid);
    const novoMap = [...novaLista];
    setListaDeArquivos(novoMap);
    onChangeListaArquivos(novoMap);

    if (form && form.setFieldValue) {
      form.setFieldValue(name, novoMap);
      form.setFieldTouched(name, true);
    }
  };

  const onChange = ({ file, fileList }) => {
    const { status } = file;

    if (excedeuLimiteMaximo(file)) {
      atualizaListaArquivos(fileList, file);
      return;
    }

    if (!permiteInserirFormato(file)) {
      atualizaListaArquivos(fileList, file);
      return;
    }

    const novoMap = [...fileList];

    if (status === 'done') {
      sucesso(`${file.name} arquivo carregado com sucesso`);
    } else if (status === 'error') {
      atualizaListaArquivos(fileList, file);
      return;
    }
    if (status === 'done' || status === 'removed') {
      if (form && form.setFieldValue) {
        form.setFieldValue(name, novoMap);
        form.setFieldTouched(name, true);
      }
    }

    setListaDeArquivos(novoMap);
    onChangeListaArquivos(novoMap);
  };

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
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
    defaultFileList,
    accept: tiposArquivosPermitidos,
    disabled: desabilitarGeral,
    campoObrigadorio: possuiErro(),
  };

  const campoUpload = () => {
    return (
      <ContainerDragger {...propsDragger} desabilitarUpload={desabilitarUpload}>
        <p className="ant-upload-drag-icon">
          <InboxOutlined />
        </p>
        <p className="ant-upload-text">{textoUpload}</p>
        <p className="ant-upload-hint">
          {tiposArquivosPermitidos
            ? textoFormatoUpload.replace(
                'Todos os formatos',
                `Os formatos ${tiposArquivosPermitidos}`
              )
            : textoFormatoUpload}
        </p>
      </ContainerDragger>
    );
  };

  const campoComValidacoes = () => {
    return (
      <Field name={name} id={id}>
        {() => campoUpload()}
      </Field>
    );
  };

  const campoSemValidacoes = () => {
    return campoUpload();
  };

  const obterErros = () => {
    return form && form.touched[name] && form.errors[name] ? (
      <MensagemCampoObrigatorio>
        {form && form.errors[name]}
      </MensagemCampoObrigatorio>
    ) : (
      ''
    );
  };

  return (
    <ContainerUpload>
      <>
        {form ? campoComValidacoes() : campoSemValidacoes()}
        {obterErros()}
      </>
    </ContainerUpload>
  );
};

UploadArquivos.propTypes = {
  urlAction: PropTypes.string,
  multiplosArquivos: PropTypes.bool,
  textoErroUpload: PropTypes.string,
  textoUpload: PropTypes.string,
  textoFormatoUpload: PropTypes.string,
  customRequest: PropTypes.func,
  beforeUpload: PropTypes.func,
  showUploadList: PropTypes.oneOfType([PropTypes.object]),
  onRemove: PropTypes.func,
  onChangeListaArquivos: PropTypes.func,
  urlUpload: PropTypes.string,
  defaultFileList: PropTypes.oneOfType([PropTypes.array]),
  tiposArquivosPermitidos: PropTypes.string,
  desabilitarUpload: PropTypes.bool,
  desabilitarGeral: PropTypes.bool,
  form: PropTypes.oneOfType([PropTypes.object, PropTypes.any]),
  name: PropTypes.string,
  id: PropTypes.string,
};

UploadArquivos.defaultProps = {
  urlAction: '',
  multiplosArquivos: false,
  textoErroUpload: 'Erro ao carregar arquivo',
  textoUpload: 'Clique ou arraste para fazer o upload do arquivo',
  textoFormatoUpload: 'Todos os formatos são suportados no limite de 100mb',
  customRequest: null,
  beforeUpload: null,
  showUploadList: {
    showRemoveIcon: true,
    showDownloadIcon: true,
    showPreviewIcon: false,
  },
  onRemove: null,
  onChangeListaArquivos: () => {},
  urlUpload: '',
  defaultFileList: [],
  tiposArquivosPermitidos: '',
  desabilitarUpload: false,
  desabilitarGeral: false,
  form: null,
  name: '',
  id: '',
};

export default UploadArquivos;
