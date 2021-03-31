import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import TabsDashboardAEE from './TabsDashboardAEE/tabsDashboardAEE';

const DashboardAEE = () => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);

  const [anoAtual] = useState(moment().format('YYYY'));
  const [anoLetivo, setAnoLetivo] = useState(anoAtual);
  const [codigoDre, setCodigoDre] = useState(undefined);
  const [codigoUe, setCodigoUe] = useState(undefined);

  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnosLetivos(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos()
      .catch(e => {
        erros(e);
        setCarregandoAnosLetivos(false);
      })
      .finally(() => setCarregandoAnosLetivos(false));

    if (anosLetivo?.data) {
      const mapAnosLetivos = anosLetivo.data.map(ano => {
        return { desc: ano, valor: ano };
      });

      setAnoLetivo(mapAnosLetivos[0].valor);
      setListaAnosLetivo(mapAnosLetivos);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

  const obterUes = useCallback(async dre => {
    if (dre) {
      setCarregandoUes(true);
      const retorno = await ServicoFiltroRelatorio.obterUes(dre)
        .catch(e => {
          erros(e);
          setCarregandoUes(false);
        })
        .finally(() => setCarregandoUes(false));

      if (retorno?.data?.length) {
        const mapUes = retorno.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (mapUes?.length === 1) {
          setCodigoUe(mapUes[0].valor);
        }
        setListaUes(mapUes);
      } else {
        setListaUes([]);
      }
    }
  }, []);

  const onChangeDre = dre => {
    setCodigoDre(dre);

    setListaUes([]);
    setCodigoUe(undefined);
  };

  const obterDres = async () => {
    setCarregandoDres(true);
    const retorno = await ServicoFiltroRelatorio.obterDres()
      .catch(e => {
        erros(e);
        setCarregandoDres(false);
      })
      .finally(() => setCarregandoDres(false));

    if (retorno?.data?.length) {
      setListaDres(retorno.data);

      if (retorno.data.length === 1) {
        setCodigoDre(retorno.data[0].codigo);
      }
    } else {
      setListaDres([]);
    }
  };

  useEffect(() => {
    if (codigoDre) {
      obterUes(codigoDre);
    } else {
      setCodigoUe(undefined);
      setListaUes([]);
    }
  }, [codigoDre, obterUes]);

  // useEffect(() => {
  //   if (anoLetivo && codigoDre && codigoUe) {

  //   }
  // }, [anoLetivo, codigoDre, codigoUe]);

  useEffect(() => {
    obterAnosLetivos();
    obterDres();
  }, [obterAnosLetivos]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUe = ue => {
    setCodigoUe(ue);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivo(ano);
  };

  return (
    <>
      <Cabecalho pagina="Dashboard AEE" />

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
                onClick={onClickVoltar}
              />
            </div>
            <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
              <Loader loading={carregandoAnosLetivos}>
                <SelectComponent
                  id="ano-letivo"
                  label="Ano Letivo"
                  lista={listaAnosLetivo}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaAnosLetivo?.length === 1}
                  onChange={onChangeAnoLetivo}
                  valueSelect={anoLetivo}
                  placeholder="Selecione o ano"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-9 col-xl-5 mb-2">
              <Loader loading={carregandoDres}>
                <SelectComponent
                  id="dre"
                  label="DRE"
                  lista={listaDres}
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaDres?.length === 1}
                  onChange={onChangeDre}
                  valueSelect={codigoDre}
                  placeholder="Diretoria Regional de Educação (DRE)"
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-12 col-lg-12 col-xl-5 mb-2">
              <Loader loading={carregandoUes}>
                <SelectComponent
                  id="ue"
                  label="Unidade Escolar (UE)"
                  lista={listaUes}
                  valueOption="valor"
                  valueText="desc"
                  disabled={listaUes?.length === 1}
                  onChange={onChangeUe}
                  valueSelect={codigoUe}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
          </div>
          <div className="row">
            <div className="col-md-12 mt-2">
              <TabsDashboardAEE
                anoLetivo={anoLetivo}
                codigoDre={codigoDre}
                codigoUe={codigoUe}
              />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default DashboardAEE;
