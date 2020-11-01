import React from 'react';

import { ResponsivePie } from '@nivo/pie';

const Pie = ({ data }) => (
  <ResponsivePie
    data={data}
    margin={{ top: 40, right: 80, bottom: 80, left: 10 }}
    enableRadialLabels={false}
    isInteractive={false}
    colors={d => d.color}
    legends={[
      {
        anchor: 'right',
        direction: 'column',
        itemWidth: 200,
        itemHeight: 20,

        symbolShape: 'square',
      },
    ]}
  />
);

export default Pie;
