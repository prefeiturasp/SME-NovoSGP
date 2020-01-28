import React from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import { Loader } from '~/componentes';
import Card from '~/componentes/card';
import { useState } from 'react';
import { Tabs } from 'antd';

const FechamentoBismestre = () => {

  const { TabPane } = Tabs;
  const [carregandoBimestres, setCarregandoBimestres] = useState(false);

  return (
    <>
      <Cabecalho pagina="Fechamento" />
      <Loader loading={carregandoBimestres}>
        <Card>

        </Card>
      </Loader>
    </>
  );
}

export default FechamentoBismestre;
