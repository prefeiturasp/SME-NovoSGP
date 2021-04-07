import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { erros } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import TabsDashboardEscolaAqui from './TabsDashboardEscolaAqui/tabsDashboardEscolaAqui';

const DashboardEscolaAqui = () => {
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);

  const [codigoDre, setCodigoDre] = useState(undefined);
  const [codigoUe, setCodigoUe] = useState(undefined);

  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const obterUes = useCallback(async dre => {
    if (dre) {
      setCarregandoGeral(true);
      const retorno = await ServicoFiltroRelatorio.obterUes(dre, true).catch(
        e => {
          erros(e);
          setCarregandoGeral(false);
        }
      );
      if (retorno && retorno.data) {
        const lista = retorno.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista && lista.length && lista.length === 1) {
          setCodigoUe(lista[0].valor);
        }
        setListaUes(lista);
      } else {
        setListaUes([]);
      }
      setCarregandoGeral(false);
    }
  }, []);

  const onChangeDre = dre => {
    setCodigoDre(dre);

    setListaUes([]);
    setCodigoUe(undefined);
  };

  const obterDres = async () => {
    setCarregandoGeral(true);
    const retorno = await ServicoFiltroRelatorio.obterDres().catch(e => {
      erros(e);
      setCarregandoGeral(false);
    });
    if (retorno && retorno.data && retorno.data.length) {
      setListaDres(retorno.data);

      if (retorno && retorno.data.length && retorno.data.length === 1) {
        setCodigoDre(retorno.data[0].codigo);
      }
    } else {
      setListaDres([]);
    }
    setCarregandoGeral(false);
  };

  useEffect(() => {
    obterDres();
  }, []);

  useEffect(() => {
    if (codigoDre) {
      obterUes(codigoDre);
    } else {
      setCodigoUe(undefined);
      setListaUes([]);
    }
  }, [codigoDre, obterUes]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUe = ue => {
    setCodigoUe(ue);
  };

  return (
    <>
      <Cabecalho pagina="Dashboard" />
      <Loader loading={carregandoGeral}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id="btn-voltar"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                <SelectComponent
                  id="select-component-dre"
                  label="Diretoria Regional de Educação (DRE)"
                  lista={listaDres}
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaDres && listaDres.length === 1}
                  onChange={onChangeDre}
                  valueSelect={codigoDre}
                  placeholder="Diretoria Regional de Educação (DRE)"
                />
              </div>
              <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                <SelectComponent
                  id="select-component-ue"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaUes && listaUes.length === 1}
                  onChange={onChangeUe}
                  valueSelect={codigoUe}
                  placeholder="Unidade Escolar (UE)"
                />
              </div>
            </div>
            <div className="row">
              <div className="col-md-12">
                <TabsDashboardEscolaAqui
                  codigoDre={codigoDre}
                  codigoUe={codigoUe}
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default DashboardEscolaAqui;
