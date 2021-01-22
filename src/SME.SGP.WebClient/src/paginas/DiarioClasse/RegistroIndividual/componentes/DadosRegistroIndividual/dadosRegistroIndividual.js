import React from 'react';
import { useSelector } from 'react-redux';

import NovoRegistroIndividual from './novoRegistroIndividual/novoRegistroIndividual';
import RegistrosAnteriores from './registrosAnteriores/registrosAnteriores';

const DadosRegistroIndividual = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );

  return (
    <>
      {!!Object.keys(dadosAlunoObjectCard).length && (
        <>
          <NovoRegistroIndividual />
          <RegistrosAnteriores />
        </>
      )}
    </>
  );
};

export default DadosRegistroIndividual;
