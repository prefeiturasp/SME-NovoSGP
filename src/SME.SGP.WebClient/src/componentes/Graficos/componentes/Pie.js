import React from 'react';

import { ResponsivePie } from '@nivo/pie';

const Pie = ({ data }) => (
  <ResponsivePie
    data={data}
    margin={{ top: 40, right: 80, bottom: 80, left: 80 }}
    enableRadialLabels={false}
    isInteractive={false}
    colors={d => d.color}
    sliceLabel={item => `${item.value} mil`}
    legends={[
      {
        anchor: 'right',
        direction: 'column',
        itemWidth: 300,
        itemHeight: 20,

        symbolShape: 'square',
      },
    ]}
  />
);

export default Pie;
