import { Upload } from 'antd';
import React from 'react';
import styled from 'styled-components';
import Card from '~/componentes/card';
import { erro, sucesso } from '~/servicos';
import UploadArquivos from './uploadArquivos';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;
`;

const PocUploadArquivos = () => {
  const onChange = info => {
    const { status } = info.file;
    if (status !== 'uploading') {
      console.log(info.file, info.fileList);
    }
    if (status === 'done') {
      sucesso(`${info.file.name} arquivo carregado com sucesso`);
    } else if (status === 'error') {
      erro(`${info.file.name} erro ao carregar arquivo`);
    }
  };

  const props = {
    onChange,
    urlAction: 'https://www.mocky.io/v2/5cc8019d300000980a055e76',
    multiplosArquivos: true,
    texoErroUpload: 'Erro ao carregar arquivo',
    textoUpload:
      'Clique ou arraste para fazer o upload do arquivo de planejamento.',
    textoFormatoUpload: 'Formato suportado: .pdf',
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
