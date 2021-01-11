import React from 'react';
// import { useDispatch } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import LocalizarEstudanteDados from './localizarEstudanteDados';

const LocalizarEstudanteCollapse = () => {
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
      <LocalizarEstudanteDados />
    </CardCollapse>
  );
};

export default LocalizarEstudanteCollapse;
