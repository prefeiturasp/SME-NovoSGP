import React from 'react';
import PropTypes from 'prop-types';

// Nivo
import { ResponsiveBar } from '@nivo/bar';

function Barras({
  dados,
  indice,
  chaves,
  legendaBaixo,
  legendaEsquerda,
  porcentagem,
}) {
  const format = v => `${Math.round(v, 2)}%`;

  return (
    <ResponsiveBar
      data={dados}
      keys={chaves}
      indexBy={indice}
      margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
      padding={0.3}
      innerPadding={1}
      groupMode="grouped"
      color={{ scheme: 'set3' }}
      borderColor={{ from: 'color', modifiers: [['darker', 1.6]] }}
      axisTop={null}
      axisRight={null}
      axisBottom={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        legend: legendaBaixo,
        legendPosition: 'middle',
        legendOffset: 32,
      }}
      axisLeft={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        legend: legendaEsquerda,
        legendPosition: 'middle',
        legendOffset: -40,
      }}
      labelSkipWidth={12}
      labelSkipHeight={12}
      labelTextColor={{ from: 'color', modifiers: [['darker', 1.6]] }}
      legends={[
        {
          format: porcentagem ? format : null,
          dataFrom: 'keys',
          anchor: 'bottom-right',
          direction: 'column',
          justify: false,
          translateX: 120,
          translateY: 0,
          itemsSpacing: 2,
          itemWidth: 100,
          itemHeight: 20,
          itemDirection: 'left-to-right',
          itemOpacity: 0.85,
          symbolSize: 20,
          effects: [
            {
              on: 'hover',
              style: {
                itemOpacity: 1,
              },
            },
          ],
        },
      ]}
      animate
      motionStiffness={90}
      motionDamping={15}
      labelFormat={porcentagem && format}
      tooltipFormat={porcentagem && format}
    />
  );
}

Barras.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
  indice: PropTypes.string,
  chaves: PropTypes.oneOfType([PropTypes.array]),
  legendaBaixo: PropTypes.string,
  legendaEsquerda: PropTypes.string,
  porcentagem: PropTypes.bool,
};

Barras.defaultProps = {
  dados: [],
  indice: '',
  chaves: [],
  legendaBaixo: '',
  legendaEsquerda: '',
  porcentagem: false,
};

export default Barras;
