import React from 'react';
import Card from '~/componentes/card';
import UploadArquivos from './uploadArquivos';

const PocUploadArquivos = () => {
  return (
    <Card>
      <div className="col-md-12">
        <UploadArquivos />
      </div>
    </Card>
  );
};

export default PocUploadArquivos;
