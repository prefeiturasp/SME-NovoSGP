import React from 'react';
import { useSelector } from 'react-redux';

const SituacaoEncaminhamentoAEE = () => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  return planoAEEDados?.encaminhamento ? (
    <>
      <strong>Encaminhamento AEE: </strong>
      {planoAEEDados?.encaminhamento?.situacao}
    </>
  ) : (
    ''
  );
};

export default SituacaoEncaminhamentoAEE;
