import React from 'react';
import t from 'prop-types';

// Nivo
import { ResponsiveBar } from '@nivo/bar';

function Barras({
  dados,
  indice,
  chaves,
  legendaBaixo,
  legendaEsquerda,
  porcentagem,
  groupMode,
  legendsTranslateX,
  showAxisBottom,
  customProps,
  removeLegends,
  customMargins,
  labelSkipWidth,
  labelSkipHeight,
}) {
  const format = v => `${Math.round(v, 2)}%`;

  return (
    <ResponsiveBar
      borderRadius={4}
      data={dados}
      keys={chaves}
      indexBy={indice}
      margin={customMargins || { top: 50, right: 130, bottom: 50, left: 60 }}
      padding={0.3}
      innerPadding={1}
      groupMode={groupMode}
      color={{ scheme: 'set3' }}
      borderColor={{ from: 'color', modifiers: [['darker', 1.6]] }}
      axisTop={null}
      axisRight={null}
      axisBottom={
        showAxisBottom
          ? {
              tickSize: 5,
              tickPadding: 5,
              tickRotation: 0,
              legend: legendaBaixo,
              legendPosition: 'middle',
              legendOffset: 32,
            }
          : null
      }
      axisLeft={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        legend: legendaEsquerda,
        legendPosition: 'middle',
        legendOffset: -40,
      }}
      labelSkipWidth={labelSkipWidth}
      labelSkipHeight={labelSkipHeight}
      labelTextColor={{ from: 'color', modifiers: [['darker', 3]] }}
      legends={[
        {
          format: porcentagem ? format : null,
          dataFrom: 'keys',
          anchor: 'bottom-right',
          direction: 'column',
          justify: false,
          translateX: legendsTranslateX,
          translateY: 0,
          itemsSpacing: 2,
          itemWidth: 100,
          itemHeight: 20,
          itemDirection: 'left-to-right',
          itemOpacity: removeLegends ? 0 : 0.85,
          symbolSize: 20,
          effects: [
            {
              on: 'hover',
              style: {
                itemOpacity: removeLegends ? 0 : 1,
              },
            },
          ],
        },
      ]}
      animate
      motionStiffness={90}
      motionDamping={15}
      labelFormat={porcentagem ? format : ''}
      tooltipFormat={porcentagem ? format : ''}
      {...customProps}
    />
  );
}

Barras.propTypes = {
  dados: t.oneOfType([t.any]),
  indice: t.string,
  chaves: t.oneOfType([t.array]),
  legendaBaixo: t.string,
  legendaEsquerda: t.string,
  porcentagem: t.bool,
  groupMode: t.string,
  legendsTranslateX: t.number,
  showAxisBottom: t.bool,
  customProps: t.oneOfType([t.object]),
  removeLegends: t.bool,
  customMargins: t.oneOfType([t.any]),
  labelSkipWidth: t.number,
  labelSkipHeight: t.number,
};

Barras.defaultProps = {
  dados: [],
  indice: '',
  chaves: [],
  legendaBaixo: '',
  legendaEsquerda: '',
  porcentagem: false,
  groupMode: 'grouped',
  legendsTranslateX: 120,
  showAxisBottom: true,
  customProps: {},
  removeLegends: false,
  customMargins: null,
  labelSkipWidth: 12,
  labelSkipHeight: 12,
};

export default Barras;
