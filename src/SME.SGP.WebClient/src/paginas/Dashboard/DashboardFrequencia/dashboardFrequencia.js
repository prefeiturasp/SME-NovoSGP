import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Cabecalho } from '~/componentes-sgp';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import { limparDadosDashboardFrequencia } from '~/redux/modulos/dashboardFrequencia/actions';
import history from '~/servicos/history';
import GraficosFrequencia from './DadosDashboardFrequencia/graficosFrequencia';
import DashboardFrequenciaFiltros from './dashboardFrequenciaFiltros';

const DashboardFrequencia = () => {
  const dispatch = useDispatch();

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  useEffect(() => {
    return () => {
      dispatch(limparDadosDashboardFrequencia());
    };
  }, [dispatch]);

  return (
    <>
      <Cabecalho pagina="Dashboard frequência" />
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
          </div>
          <DashboardFrequenciaFiltros />
          <div className="row">
            <div className="col-md-12 mt-2">
              <GraficosFrequencia />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default DashboardFrequencia;
