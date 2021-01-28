import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import SecaoLocalizarEstudanteDados from './secaoLocalizarEstudanteDados';

const SecaoLocalizarEstudanteCollapse = () => {
  return (
    <CardCollapse
      key="localizar-estudante-collapse-key"
      titulo="Localizar estudante"
      indice="localizar-estudante-collapse-indice"
      alt="localizar-estudante-alt"
      show
    >
      <SecaoLocalizarEstudanteDados />
    </CardCollapse>
  );
};

export default SecaoLocalizarEstudanteCollapse;
