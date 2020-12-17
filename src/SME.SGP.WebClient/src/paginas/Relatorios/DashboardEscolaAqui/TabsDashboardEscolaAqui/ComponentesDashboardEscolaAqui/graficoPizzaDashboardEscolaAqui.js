import PropTypes from 'prop-types';
import React from 'react';
import { Graficos } from '~/componentes';
import LegendaGrafico from '~/componentes-sgp/LegendaGrafico/legendaGrafico';
import { TituloGrafico } from '../../dashboardEscolaAqui.css';

const GraficoPizzaDashboardEscolaAqui = props => {
  const { titulo, dadosGrafico } = props;

  return (
    <>
      <TituloGrafico>{titulo}</TituloGrafico>
      <div className="row mb-5">
        <div className="col-md-6">
          <Graficos.Pie
            data={dadosGrafico}
            style={{ fontSize: '14px !important' }}
          />
        </div>
        <div className="col-md-6 d-flex align-items-center">
          <LegendaGrafico dados={dadosGrafico} />
        </div>
      </div>
    </>
  );
};

GraficoPizzaDashboardEscolaAqui.propTypes = {
  dadosGrafico: PropTypes.oneOfType([PropTypes.array]),
  titulo: PropTypes.string,
};

GraficoPizzaDashboardEscolaAqui.defaultProps = {
  dadosGrafico: [],
  titulo: '',
};

export default GraficoPizzaDashboardEscolaAqui;
