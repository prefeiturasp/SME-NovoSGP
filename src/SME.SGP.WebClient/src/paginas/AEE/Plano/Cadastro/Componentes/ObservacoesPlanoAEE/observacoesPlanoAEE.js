import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosObservacoesPlanoAEE from './montarDadosObservacoesPlanoAEE';

const ObservacoesPlanoAEE = () => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const key = 'secao-observacoes';

  return planoAEEDados?.id ? (
    <CardCollapse
      key={`${key}-collapse-key`}
      titulo="Observações"
      indice={`${key}-collapse-indice`}
      alt={`${key}-alt`}
    >
      <MontarDadosObservacoesPlanoAEE />
    </CardCollapse>
  ) : (
    ''
  );
};

export default ObservacoesPlanoAEE;
