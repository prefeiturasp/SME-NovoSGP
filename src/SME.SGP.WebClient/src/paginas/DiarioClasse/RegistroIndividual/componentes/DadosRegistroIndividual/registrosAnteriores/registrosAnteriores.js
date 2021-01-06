import React from 'react';
import shortid from 'shortid';

import { CardCollapse } from '~/componentes';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';

import RegistrosAnterioresConteudo from './registrosAnterioresConteudo/registrosAnterioresConteudo';

const RegistrosAnteriores = () => {
  const idCollapse = shortid.generate();

  return (
    <div key={shortid.generate()} className="px-4 pt-4">
      <CardCollapse
        configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
        styleCardBody={{ paddingTop: 12 }}
        key={`${idCollapse}-collapse-key`}
        titulo="Registros anteriores"
        indice={`${idCollapse}-collapse-indice`}
        alt={`${idCollapse}-alt`}
      >
        <RegistrosAnterioresConteudo />
      </CardCollapse>
    </div>
  );
};

export default RegistrosAnteriores;
