import { Upload } from 'antd';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import Card from '~/componentes/card';
import { erro, sucesso } from '~/servicos';
import UploadArquivos from './uploadArquivos';
import { urlBase } from '~/servicos/variaveis';

const { Dragger } = Upload;

export const ContainerDragger = styled(Dragger)`
  height: auto !important;
`;

const PocUploadArquivos = () => {
  const [url, setUrl] = useState('');

  useEffect(() => {
    urlBase().then(resposta => setUrl(resposta));
  }, []);

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
    urlAction: `https://localhost:5001/api/v1/armazenamento/upload`,
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
