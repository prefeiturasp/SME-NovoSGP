import React from 'react';
// import { useDispatch } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import SecaoLocalizarEstudanteDados from './secaoLocalizarEstudanteDados';

const SecaoLocalizarEstudanteCollapse = () => {
  // const dispatch = useDispatch();

  const onClickCardCollapse = () => {
    // dispatch(setClicouNoBimestre({ bimestre }));
  };

  return (
    <CardCollapse
      key="localizar-estudante-collapse-key"
      titulo="Localizar estudante"
      indice="localizar-estudante-collapse-indice"
      alt="localizar-estudante-alt"
      onClick={onClickCardCollapse}
    >
      <SecaoLocalizarEstudanteDados />
    </CardCollapse>
  );
};

export default SecaoLocalizarEstudanteCollapse;
