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
  const [dre, setDre] = useState(undefined);
  const [ue, setUe] = useState(undefined);

  const [carregandoAnosLetivos, setCarregandoAnosLetivos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);

  const OPCAO_TODOS = '-99';

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

  const obterUes = useCallback(async codigoDre => {
    if (codigoDre) {
      setCarregandoUes(true);
      const retorno = await ServicoFiltroRelatorio.obterUes(codigoDre)
        .catch(e => {
          erros(e);
          setCarregandoUes(false);
        })
        .finally(() => setCarregandoUes(false));

      if (retorno?.data?.length) {
        setListaUes(retorno.data);
        if (retorno.data?.length === 1) {
          setUe(retorno.data[0]);
        }
      } else {
        setListaUes([]);
      }
    }
  }, []);

  const onChangeDre = codigoDre => {
    if (codigoDre) {
      const dreAtual = listaDres?.find(item => item.codigo === codigoDre);
      if (dreAtual) {
        setDre(dreAtual);
      }
    } else {
      setDre();
    }
    setListaUes([]);
    setUe(undefined);
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
        setDre(retorno.data[0]);
      }
    } else {
      setListaDres([]);
    }
  };

  useEffect(() => {
    if (dre?.codigo) {
      obterUes(dre.codigo);
    } else {
      setUe(undefined);
      setListaUes([]);
    }
  }, [dre, obterUes]);

  useEffect(() => {
    obterAnosLetivos();
    obterDres();
  }, [obterAnosLetivos]);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onChangeUe = codigoUe => {
    if (codigoUe) {
      const ueAtual = listaUes?.find(item => item.codigo === codigoUe);
      if (ueAtual) {
        setUe(ueAtual);
      }
    } else {
      setUe();
    }
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
                  valueSelect={dre?.codigo}
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
                  valueOption="codigo"
                  valueText="nome"
                  disabled={listaUes?.length === 1}
                  onChange={onChangeUe}
                  valueSelect={ue?.codigo}
                  placeholder="Unidade Escolar (UE)"
                />
              </Loader>
            </div>
          </div>
          <div className="row">
            <div className="col-md-12 mt-2">
              {dre && ue ? (
                <TabsDashboardAEE
                  anoLetivo={anoLetivo}
                  dreId={OPCAO_TODOS === dre?.codigo ? OPCAO_TODOS : dre?.id}
                  ueId={OPCAO_TODOS === ue?.codigo ? OPCAO_TODOS : ue?.id}
                  dreCodigo={
                    OPCAO_TODOS === dre?.codigo ? OPCAO_TODOS : dre?.codigo
                  }
                  ueCodigo={
                    OPCAO_TODOS === ue?.codigo ? OPCAO_TODOS : ue?.codigo
                  }
                />
              ) : (
                ''
              )}
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default DashboardAEE;
