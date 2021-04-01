import PropTypes from 'prop-types';
import React from 'react';
import { Graficos } from '~/componentes';
import LegendaGrafico from '~/componentes-sgp/LegendaGrafico/legendaGrafico';
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
    dadosLegendaCustomizada,
    margemPersonalizada,
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
                  return tooltipCustomizadoDashboardEscolaAqui(item);
                },
                labelFormat: valor => (
                  <tspan y={-7}>{formataMilhar(valor)}</tspan>
                ),
              }}
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

GraficoBarraDashboardEscolaAqui.propTypes = {
  titulo: PropTypes.string,
  dadosGrafico: PropTypes.oneOfType([PropTypes.array]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  indice: PropTypes.string,
  groupMode: PropTypes.string,
  removeLegends: PropTypes.bool,
  customPropsColors: PropTypes.oneOfType([PropTypes.any]),
  dadosLegendaCustomizada: PropTypes.oneOfType([PropTypes.array]),
  margemPersonalizada: PropTypes.oneOfType(PropTypes.any),
};

GraficoBarraDashboardEscolaAqui.defaultProps = {
  titulo: '',
  dadosGrafico: [],
  chavesGrafico: [],
  indice: '',
  groupMode: '',
  removeLegends: false,
  customPropsColors: null,
  dadosLegendaCustomizada: [],
  margemPersonalizada: null,
};

export default GraficoBarraDashboardEscolaAqui;
