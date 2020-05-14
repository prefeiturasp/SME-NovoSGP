import { Base } from '~/componentes';
import React from 'react';
import styled from 'styled-components';

import { Table, Tooltip } from 'antd';

const ColunasFixas = () => [
  {
    title: 'Objetivo',
    dataIndex: 'Objetivo',
    colSpan: 1,
    width: 200,
    render: (text, row) => {
      let valor = text;
      if (valor.length > 100) valor = `${text.substr(0, 100)}...`;
      return {
        children: <Tooltip title={text}>{valor}</Tooltip>,
        props: {
          rowSpan: row.ObjetivoGrupo ? row.ObjetivoSize : 0,
          style: { fontWeight: 'bold' },
        },
      };
    },
  },
  {
    title: 'Resposta',
    dataIndex: 'Resposta',
    colSpan: 1,
    width: 150,
    render: text => {
      return {
        children: text,
        props: {
          rowSpan: 1,
          style: { fontWeight: 'bold' },
        },
      };
    },
  },
];

const Tabela = styled(Table)`
  th.headerTotal {
    background-color: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;

export { ColunasFixas, Tabela };
