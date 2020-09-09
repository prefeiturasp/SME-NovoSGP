import React from 'react';
import { Tabs } from 'antd';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';

import { Colors } from '~/componentes/colors';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import Card from '../../../../componentes/card';
import ReiniciarSenha from '../reiniciarSenha';
import ReiniciarSenhaEA from '../ReiniciarSenhaEA';

import { ContainerTabs } from './style';

const { TabPane } = Tabs;

export default function TabsReiniciarSenha() {
  const onClickVoltar = () => history.push(URL_HOME);

  return (
    <>
      <Cabecalho pagina="Reiniciar senha" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
        </div>

        <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12">
          <ContainerTabs type="card" defaultActiveKey="1">
            <TabPane tab="SGP" key="1">
              <ReiniciarSenha />
            </TabPane>
            <TabPane tab="Escola Aqui" key="2">
              <ReiniciarSenhaEA />
            </TabPane>
          </ContainerTabs>
        </div>
      </Card>
    </>
  );
}
