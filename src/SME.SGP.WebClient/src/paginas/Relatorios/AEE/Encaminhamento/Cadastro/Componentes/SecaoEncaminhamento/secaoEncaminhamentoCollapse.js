import React from 'react';
// import { useDispatch } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import DadosSecaoEncaminhamento from './DadosSecaoEncaminhamento/dadosSecaoEncaminhamento';
import ObjectCardEncaminhamento from './objectCardEncaminhamento';

const SecaoEncaminhamentoCollapse = () => {
  // const dispatch = useDispatch();

  const onClickCardCollapse = () => {
    // dispatch(setClicouNoBimestre({ bimestre }));
  };

  return (
    <CardCollapse
      key="secao-encaminhamento-collapse-key"
      titulo="Encaminhamento"
      indice="secao-encaminhamento-collapse-indice"
      alt="secao-encaminhamento-alt"
      onClick={onClickCardCollapse}
    >
      <ObjectCardEncaminhamento />
      <DadosSecaoEncaminhamento />
    </CardCollapse>
  );
};

export default SecaoEncaminhamentoCollapse;
