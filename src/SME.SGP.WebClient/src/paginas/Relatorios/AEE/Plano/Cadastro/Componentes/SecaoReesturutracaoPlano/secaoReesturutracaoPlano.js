import React from 'react';
import { CardCollapse } from '~/componentes';

const SecaoReesturutracaoPlano = () => {
  return (
    <>
      <CardCollapse
        key="secao-1-semestre-plano-collapse-key"
        titulo="Reestruturações do 1º Semestre"
        show
        indice="secao-1-semestre-plano-collapse-indice"
        alt="secao-informacoes-plano-alt"
      >
        Reestruturações do 1º Semestre
      </CardCollapse>
      <CardCollapse
        key="secao-2-semestre-plano-collapse-key"
        titulo="Reestruturações do 2º Semestre"
        show
        indice="secao-2-semestre-plano-collapse-indice"
        alt="secao-informacoes-plano-alt"
      >
        Reestruturações do 2º Semestre
      </CardCollapse>
    </>
  );
};

export default SecaoReesturutracaoPlano;
