import React from 'react';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card } from '~/componentes';
import ButtonGroup from './componentes/ButtonGroup';
import Filtro from './componentes/Filtro';

function AtribuicaoEsporadicaLista() {
  return (
    <>
      <Cabecalho pagina="Atribuição esporádica" />
      <Card mx="mx-0">
        <ButtonGroup />
        <Filtro />
      </Card>
    </>
  );
}

export default AtribuicaoEsporadicaLista;
