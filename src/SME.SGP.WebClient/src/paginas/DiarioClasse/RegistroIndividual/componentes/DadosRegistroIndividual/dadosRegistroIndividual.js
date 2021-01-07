import React from 'react';
import { useSelector } from 'react-redux';

import { Loader } from '~/componentes';

import NovoRegistroIndividual from './novoRegistroIndividual/novoRegistroIndividual';
import RegistrosAnteriores from './registrosAnteriores/registrosAnteriores';

const DadosRegistroIndividual = () => {
  const {
    dadosAlunoObjectCard,
    exibirLoaderGeralRegistroIndividual,
  } = useSelector(store => store.registroIndividual);

  return (
    <>
      {!!Object.keys(dadosAlunoObjectCard).length && (
        <Loader ignorarTip loading={exibirLoaderGeralRegistroIndividual}>
          <NovoRegistroIndividual dadosAlunoObjectCard={dadosAlunoObjectCard} />
          <RegistrosAnteriores />
        </Loader>
      )}
    </>
  );
};

export default DadosRegistroIndividual;
