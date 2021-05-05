import { Pie } from '@ant-design/charts';
import PropTypes from 'prop-types';
import React from 'react';
import { CoresGraficos } from '~/componentes/colors';

const GraficoPizza = props => {
  const { data, angleField, colorField, percentage } = props;

  const config = {
    data,
    angleField,
    colorField,
    label: {
      appendPadding: 30,
      type: 'spider',
      labelHeight: 28,
      content: percentage ? '{percentage}' : '{value}',
    },
    interactions: [{ type: 'element-selected' }, { type: 'element-active' }],
    color: CoresGraficos,
  };

  return data?.length ? <Pie {...config} /> : '';
};

GraficoPizza.propTypes = {
  data: PropTypes.oneOfType(PropTypes.array),
  angleField: PropTypes.string,
  colorField: PropTypes.string,
  percentage: PropTypes.bool,
  colors: PropTypes.oneOfType(PropTypes.array),
};

GraficoPizza.defaultProps = {
  data: [],
  angleField: 'quantidade',
  colorField: 'descricao',
  percentage: false,
  colors: [],
};

export default GraficoPizza;
