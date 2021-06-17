import PropTypes from 'prop-types';
import { Column } from '@ant-design/charts';
import React from 'react';
import { Base, CoresGraficos } from '~/componentes/colors';

const GraficoBarras = props => {
  const {
    data,
    xAxisVisible,
    legendVisible,
    isGroup,
    xField,
    yField,
    seriesField,
    colors,
  } = props;

  const config = {
    data,
    isGroup,
    xField,
    yField,
    seriesField,
    xAxis: {
      visible: xAxisVisible,
      label: {
        style: {
          fontWeight: 'bold',
          fontSize: 12,
          fill: Base.CinzaMako,
        },
      },
    },
    columnStyle: {
      radius: [4, 4, 0, 0],
    },
    label: {
      position: 'top',
      offset: 0,
      style: {
        fill: Base.CinzaMako,
        textAlign: 'center',
        fontSize: 14,
        fontWeight: 400,
      },
    },
    legend: legendVisible
      ? {
          position: 'bottom',
          flipPage: false,
          itemWidth: 180,
          itemName: {
            style: {
              fontWeight: 'bold',
              fontSize: 12,
              fill: Base.CinzaMako,
            },
          },
          marker: {
            symbol: 'circle',
            style: {
              y: 5,
              r: 6,
            },
          },
        }
      : false,
    tooltip: {
      showTitle: false,
      domStyles: {
        'g2-tooltip-list': {
          textAlign: 'left',
        },
      },
    },
    appendPadding: [20, 0, 20, 0],
    color: colors?.length ? colors : CoresGraficos,
  };

  return data?.length ? <Column {...config} /> : '';
};

GraficoBarras.propTypes = {
  data: PropTypes.oneOfType(PropTypes.array),
  xAxisVisible: PropTypes.bool,
  legendVisible: PropTypes.bool,
  isGroup: PropTypes.bool,
  xField: PropTypes.string,
  yField: PropTypes.string,
  seriesField: PropTypes.string,
  colors: PropTypes.oneOfType(PropTypes.array),
};

GraficoBarras.defaultProps = {
  data: [],
  xAxisVisible: false,
  legendVisible: true,
  isGroup: false,
  xField: 'descricao',
  yField: 'quantidade',
  seriesField: 'descricao',
  colors: [],
};

export default GraficoBarras;
