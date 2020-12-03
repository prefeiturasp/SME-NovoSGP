import PropTypes from 'prop-types';
import React from 'react';
import { Graficos } from '~/componentes';
import {
  ContainerGraficoBarras,
  TituloGrafico,
} from '../../dashboardEscolaAqui.css';
import {
  formataMilhar,
  tooltipCustomizadoDashboardEscolaAqui,
} from '../../dashboardEscolaAquiGraficosUtils';

const GraficoBarraDashboardEscolaAqui = props => {
  const {
    titulo,
    dadosGrafico,
    chavesGrafico,
    indice,
    groupMode,
    removeLegends,
    customPropsColors,
  } = props;

  return (
    <>
      <div className="scrolling-chart">
        <div className="col-md-12">
          <TituloGrafico>{titulo}</TituloGrafico>
          <ContainerGraficoBarras>
            <Graficos.Barras
              groupMode={groupMode || 'grouped'}
              dados={dadosGrafico}
              indice={indice}
              chaves={chavesGrafico}
              legendsTranslateX={105}
              removeLegends={removeLegends}
              customProps={{
                colors: customPropsColors || (item => item?.data?.color),
                tooltip: item => {
                  return tooltipCustomizadoDashboardEscolaAqui(item);
                },
                labelFormat: valor => (
                  <tspan y={-7}>{formataMilhar(valor)}</tspan>
                ),
              }}
            />
          </ContainerGraficoBarras>
        </div>
      </div>
    </>
  );
};

GraficoBarraDashboardEscolaAqui.propTypes = {
  titulo: PropTypes.string,
  dadosGrafico: PropTypes.oneOfType([PropTypes.array]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  indice: PropTypes.string,
  groupMode: PropTypes.string,
  removeLegends: PropTypes.bool,
  customPropsColors: PropTypes.oneOfType([PropTypes.any]),
};

GraficoBarraDashboardEscolaAqui.defaultProps = {
  titulo: '',
  dadosGrafico: [],
  chavesGrafico: [],
  indice: '',
  groupMode: '',
  removeLegends: false,
  customPropsColors: null,
};

export default GraficoBarraDashboardEscolaAqui;
