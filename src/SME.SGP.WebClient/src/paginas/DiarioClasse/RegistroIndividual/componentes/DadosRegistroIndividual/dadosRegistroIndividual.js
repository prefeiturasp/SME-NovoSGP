import React from 'react';
import { useSelector } from 'react-redux';

import { Loader } from '~/componentes';

import NovoRegistroIndividual from './novoRegistroIndividual/novoRegistroIndividual';
import RegistrosAnteriores from './registrosAnteriores/registrosAnteriores';

const DadosRegistroIndividual = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const exibirLoaderGeralRegistroIndividual = useSelector(
    store => store.registroIndividual.exibirLoaderGeralRegistroIndividual
  );

  return (
    <>
      {!!Object.keys(dadosAlunoObjectCard).length && (
        <Loader loading={exibirLoaderGeralRegistroIndividual}>
          <NovoRegistroIndividual dadosAlunoObjectCard={dadosAlunoObjectCard} />
          <RegistrosAnteriores />
        </Loader>
      )}
    </>
  );
};

export default DadosRegistroIndividual;
