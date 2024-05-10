import PropTypes from 'prop-types';
import React from 'react';
import { Graficos } from '~/componentes';
import LegendaGrafico from '~/componentes-sgp/LegendaGrafico/legendaGrafico';
import { ContainerGraficoBarras, TituloGrafico } from '../style';
import {
  formataMilhar,
  tooltipCustomizadoDashboard,
} from './graficosDashboardUtils';

const GraficoBarraDashboard = props => {
  const {
    titulo,
    dadosGrafico,
    chavesGrafico,
    indice,
    groupMode,
    removeLegends,
    customPropsColors,
    dadosLegendaCustomizada,
    margemPersonalizada,
    showAxisBottom,
  } = props;

  const customMargins = {
    top: 50,
    right: dadosLegendaCustomizada?.length ? 0 : 130,
    bottom: 50,
    left: 60,
  };
  return (
    <>
      <div
        className="scrolling-chart  mt-3 mb-3"
        style={{
          flexDirection: dadosLegendaCustomizada?.length ? 'column' : 'row',
        }}
      >
        <div className="col-md-12">
          {titulo ? <TituloGrafico>{titulo}</TituloGrafico> : ''}
          <ContainerGraficoBarras>
            <Graficos.Barras
              groupMode={groupMode || 'grouped'}
              dados={dadosGrafico}
              indice={indice}
              chaves={chavesGrafico}
              legendsTranslateX={105}
              removeLegends={removeLegends}
              customMargins={margemPersonalizada || customMargins}
              labelSkipWidth={0}
              labelSkipHeight={0}
              customProps={{
                colors: customPropsColors || (item => item?.data?.color),
                tooltip: item => {
                  return tooltipCustomizadoDashboard(item);
                },
                labelFormat: valor => (
                  <tspan y={-7}>{formataMilhar(valor)}</tspan>
                ),
              }}
              showAxisBottom={showAxisBottom}
            />
          </ContainerGraficoBarras>
        </div>
        {dadosLegendaCustomizada?.length ? (
          <div className="col-md-12">
            <LegendaGrafico dados={dadosLegendaCustomizada} orizontal />
          </div>
        ) : (
          ''
        )}
      </div>
    </>
  );
};

GraficoBarraDashboard.propTypes = {
  titulo: PropTypes.string,
  dadosGrafico: PropTypes.oneOfType([PropTypes.array]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  indice: PropTypes.string,
  groupMode: PropTypes.string,
  removeLegends: PropTypes.bool,
  customPropsColors: PropTypes.oneOfType([PropTypes.any]),
  dadosLegendaCustomizada: PropTypes.oneOfType([PropTypes.array]),
  margemPersonalizada: PropTypes.oneOfType(PropTypes.any),
  showAxisBottom: PropTypes.bool,
};

GraficoBarraDashboard.defaultProps = {
  titulo: '',
  dadosGrafico: [],
  chavesGrafico: [],
  indice: '',
  groupMode: '',
  removeLegends: false,
  customPropsColors: null,
  dadosLegendaCustomizada: [],
  margemPersonalizada: null,
  showAxisBottom: true,
};

export default GraficoBarraDashboard;
