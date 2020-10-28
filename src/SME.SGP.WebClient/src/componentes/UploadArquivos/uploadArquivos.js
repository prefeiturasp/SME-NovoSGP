import { InboxOutlined } from '@ant-design/icons';
import { Upload } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;
`;

const UploadArquivos = props => {
  const {
    onChange,
    urlAction,
    multiplosArquivos,
    mensagemErroUpload,
    textoUpload,
    textoFormatoUpload,
  } = props;

  const propsDragger = {
    locale: { uploadError: mensagemErroUpload },
    name: 'file',
    multiple: multiplosArquivos,
    action: urlAction,
    onChange,
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
  onChange: PropTypes.func,
  urlAction: PropTypes.string,
  multiplosArquivos: PropTypes.bool,
  mensagemErroUpload: PropTypes.string,
  textoUpload: PropTypes.string,
  textoFormatoUpload: PropTypes.string,
};

UploadArquivos.defaultProps = {
  onChange: () => {},
  urlAction: '',
  multiplosArquivos: false,
  mensagemErroUpload: '',
  textoUpload: '',
  textoFormatoUpload: '',
};

export default UploadArquivos;
