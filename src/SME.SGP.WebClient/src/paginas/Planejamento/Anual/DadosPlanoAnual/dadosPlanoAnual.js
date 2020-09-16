import React from 'react';
import { useSelector } from 'react-redux';
import ListaObjetivos from './ListaObjetivos/listaObjetivos';

const DadosPlanoAnual = () => {
  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  return <>{componenteCurricular ? <ListaObjetivos /> : ''}</>;
};

export default DadosPlanoAnual;
