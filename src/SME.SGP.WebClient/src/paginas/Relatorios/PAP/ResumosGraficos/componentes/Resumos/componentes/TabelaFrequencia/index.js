import React, { useState, useEffect } from 'react';
import axios from 'axios';

// Ant
import { Table } from 'antd';

// Componentes
import { Base } from '~/componentes';

const servico = axios.create({
  baseURL: 'http://demo7314211.mockable.io/api',
});

const test = [
  {
    QuantidadeTotal: 90,
    PorcentagemTotal: 90,
    Frequencia: [
      {
        Descricao: 'Frequente',
        Quantidade: 36,
        Porcentagem: 40,
        Anos: [
          {
            CodigoAno: 1,
            DescricaoAno: '1B',
            Quantidade: 9,
            Porcentagem: 22,
          },
          {
            CodigoAno: 1,
            DescricaoAno: '2C',
            Quantidade: 9,
            Porcentagem: 22,
          },
          {
            CodigoAno: 1,
            DescricaoAno: '4D',
            Quantidade: 9,
            Porcentagem: 22,
          },
        ],
      },
    ],
  },
];

const dadosBackend = [
  {
    id: 0,
    DescricaoFrequencia: 'Frequente',
    TipoDado: 'Quantidade',
    Cor: Base.Laranja,
    '3C': 11,
    '4C': 15,
    '4E': 20,
    '5C': 25,
    '6C': 25,
    '6B': 25,
    Total: 36,
  },
  {
    id: 1,
    DescricaoFrequencia: 'Frequente',
    TipoDado: 'Porcentagem',
    Cor: Base.Laranja,
    '3C': 11,
    '4C': 15,
    '4E': 20,
    '5C': 25,
    '6C': 25,
    '6B': 25,
    Total: 36,
  },
  {
    id: 2,
    DescricaoFrequencia: 'Pouco frequente',
    TipoDado: 'Quantidade',
    Cor: Base.Vermelho,
    '3C': 11,
    '4C': 15,
    '4E': 20,
    '5C': 25,
    '6C': 25,
    '6B': 25,
    Total: 36,
  },
  {
    id: 3,
    DescricaoFrequencia: 'Pouco frequente',
    TipoDado: 'Porcentagem',
    Cor: Base.Vermelho,
    '3C': 11,
    '4C': 15,
    '4E': 20,
    '5C': 25,
    '6C': 25,
    '6B': 25,
    Total: 36,
  },
];

function TabelaFrequencia() {
  const [dadosTabela, setDadosTabela] = useState([]);

  const cores = [Base.Laranja, Base.Vermelho, Base.Azul, Base.Verde];

  const colunas = [
    {
      title: 'Ano',
      dataIndex: 'DescricaoFrequencia',
      colSpan: 2,
      fixed: 'left',
      width: 150,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan: index % 2 === 0 ? 2 : 0,
            style: { borderLeft: `7px solid ${row.Cor}` },
          },
        };
      },
    },
    {
      title: 'TipoDado',
      dataIndex: 'TipoDado',
      colSpan: 0,
      width: 150,
      fixed: 'left',
    },
    {
      title: '3C',
      dataIndex: '3C',
    },
    {
      title: '4C',
      dataIndex: '3C',
    },
    {
      title: '4E',
      dataIndex: '3C',
    },
    {
      title: '5C',
      dataIndex: '5C',
    },
    {
      title: '6C',
      dataIndex: '6C',
    },
    {
      title: '6B',
      dataIndex: '6B',
    },
    {
      title: 'Total',
      dataIndex: 'Total',
      width: 100,
      fixed: 'right',
    },
  ];

  // const colunas = () => {
  // 	return
  // }

  useEffect(async () => {
    async function buscarDados() {
      const { data, status } = await servico.get(
        '/v1/recuperacao-paralela/resumos/frequencia'
      );
      if (data && status === 200) {
        setDadosTabela(data[0]);
      }
    }
    buscarDados();
  }, []);

  return (
    <>
      <Table
        pagination={false}
        columns={colunas}
        dataSource={dadosBackend || []}
        rowKey="id"
        key="id"
        bordered
        size="small"
      />
    </>
  );
}

export default TabelaFrequencia;
