import React from 'react';
import { useSelector } from 'react-redux';

const SituacaoEncaminhamentoAEE = () => {
  const planoAEESituacaoEncaminhamentoAEE = useSelector(
    store => store.planoAEE.planoAEESituacaoEncaminhamentoAEE
  );

  return planoAEESituacaoEncaminhamentoAEE?.situacao ? (
    <>
      <strong>Encaminhamento AEE: </strong>
      {planoAEESituacaoEncaminhamentoAEE?.situacao}
    </>
  ) : (
    ''
  );
};

export default SituacaoEncaminhamentoAEE;
