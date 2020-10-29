import React, { useCallback, useEffect, useState } from 'react';
import { Tabs, Row } from 'antd';
import { Loader, SelectComponent, Graficos } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import FiltroHelper from '~componentes-sgp/filtro/helper';

import { ContainerTabs } from './style';

const { TabPane } = Tabs;

const data = [
  {
    "id": 21234,
    "nomeCompletoDre": null,
    "nomeCompletoUe": null,
    "codigoturma": 0,
    "totalUsuariosComCpfInvalidos": 21956,
    "totalUsuariosPrimeiroAcessoIncompleto": 7,
    "totalUsuariosSemAppInstalado": 1194399,
    "totalUsuariosValidos": 59,
    "label": "Total de usuários",
    "value": 59,
    "color": "hsl(181, 70%, 50%)"
  },
  {
    "id": "hack",
    "label": "hack",
    "value": 111,
    "color": "hsl(181, 70%, 50%)"
  },
  {
    "id": "ruby",
    "label": "ruby",
    "value": 9,
    "color": "hsl(115, 70%, 50%)"
  },
  {
    "id": "sass",
    "label": "sass",
    "value": 344,
    "color": "hsl(75, 70%, 50%)"
  },
  {
    "id": "erlang",
    "label": "erlang",
    "value": 482,
    "color": "hsl(199, 70%, 50%)"
  },
  {
    "id": "scala",
    "label": "scala",
    "value": 154,
    "color": "hsl(123, 70%, 50%)"
  }
];


const DashboardEscolaAqui = () => {
  const [anoLetivo, setAnoLetivo] = useState(undefined);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [listaDres, setListaDres] = useState([]);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [listaUes, setListaUes] = useState([]);

  const [dreId, setDreId] = useState(undefined);
  const [ueId, setUeId] = useState(undefined);

  const onChangeDre = valor => {
    setDreId(valor);
    setUeId();
    setUeId(undefined);
  };

  const onChangeUe = valor => {
    setUeId(valor);
  };

  // usar para ano letivo
  const [anoAtual] = useState(window.moment().format('YYYY'));

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const { data } = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`
      );
      if (data && data.length) {
        const lista = data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDreId(lista[0].valor);
        }
      } else {
        setListaDres([]);
        setDreId(undefined);
      }
      setCarregandoDres(false);
    }
  }, [anoLetivo]);

  useEffect(() => {
    obterDres();
  }, [obterDres]);

  const obterUes = useCallback(async (dre, ano) => {
    if (dre) {
      setCarregandoUes(true);
      const { data } = await AbrangenciaServico.buscarUes(
        dre,
        `v1/abrangencias/false/dres/${dre}/ues?anoLetivo=${ano}`,
        true
      );
      if (data) {
        const lista = data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista && lista.length && lista.length === 1) {
          setUeId(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
      setCarregandoUes(false);
    }
  }, []);

  useEffect(() => {
    if (dreId) {
      obterUes(dreId, anoLetivo);
    } else {
      setUeId();
      setListaUes([]);
    }
  }, [dreId, anoLetivo, obterUes]);

  return (
    <>
      <Cabecalho pagina="Dashboard" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
              <Button
                id="btn-voltar-rel-pendencias"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={() => {
                  history.push('/');
                }}
              />
            </div>

            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
              <Loader loading={carregandoDres} tip="">
                <SelectComponent
                  id="drop-dre-rel-pendencias"
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!anoLetivo || (listaDres && listaDres.length === 1)}
                  onChange={onChangeDre}
                  valueSelect={dreId}
                  placeholder="Diretoria Regional De Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
              <Loader loading={carregandoUes} tip="">
                <SelectComponent
                  id="drop-ue-rel-pendencias"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={!dreId || (listaUes && listaUes.length === 1)}
                  onChange={onChangeUe}
                  valueSelect={ueId}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>

            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12">
              <ContainerTabs type="card" defaultActiveKey="1">
                <TabPane tab="Adesão" key="1">
                  <div style={{ height: 400 }}>
                    <Graficos.Pie data={data} />
                  </div>
                </TabPane>
                <TabPane tab="Comunicados Totais" key="2">
                  <p>Teste 2</p>
                </TabPane>
                <TabPane tab="Comunicados Leitura" key="3">
                  <p>Teste 3</p>
                </TabPane>
              </ContainerTabs>
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default DashboardEscolaAqui;
