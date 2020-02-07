import React, { useMemo } from 'react';
import t from 'prop-types';

// Ant
import { Table } from 'antd';

// Componentes
import { Base } from '~/componentes';

// Estilos
import { ContainerTabela } from './styles';

const dadosBackend = [
  {
    key: '0',
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
    key: '1',
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
    key: '2',
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
    key: '3',
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

function TabelaFrequencia({ dados }) {
  const renderizarCor = descricao => {
    const mapa = {
      Frequente: Base.Laranja,
      'Pouco frequente': Base.Vermelho,
      'Não comparece': Base.Azul,
      Total: Base.Verde,
    };
    return mapa[descricao];
  };

  const colunasBase = [
    {
      title: 'Ano',
      dataIndex: 'FrequenciaDescricao',
      colSpan: 2,
      // fixed: 'left',
      width: 150,
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            rowSpan: index % 2 === 0 ? 2 : 0,
            style: {
              borderLeft: `7px solid ${renderizarCor(row.FrequenciaDescricao)}`,
              fontWeight: 'bold',
            },
          },
        };
      },
    },
    {
      title: 'TipoDado',
      dataIndex: 'TipoDado',
      colSpan: 0,
      width: 150,
      // fixed: 'left',
      render: (text, row, index) => {
        return {
          children: text,
          props: {
            style: {
              fontWeight: 'bold',
            },
          },
        };
      },
    },
  ];

  const colunasTabela = useMemo(() => {
    if (!dados) return colunasBase;

    const colunasParaExcluir = [
      'TipoDado',
      'FrequenciaDescricao',
      'key',
      'Descricao',
    ];

    const colunasParaRenderizar = Object.keys(dados[0]).filter(
      item => colunasParaExcluir.indexOf(item) === -1
    );

    const colunasParaIncluir = colunasParaRenderizar.map(item => {
      const novaColuna = { title: item, dataIndex: item };
      if (item !== 'Total') return novaColuna;
      return {
        ...novaColuna,
        width: 100,
        // fixed: 'right',
        // render: (text, row, index) => {
        //   return {
        //     children: text,
        //     props: {
        //       style: {
        //         backgroundColor: Base.Roxo,
        //         color: 'white',
        //       },
        //     },
        //   };
        // },
      };
    });

    return [...colunasBase, ...colunasParaIncluir];
  }, [colunasBase, dados]);

  return (
    <>
      <ContainerTabela>
        <Table
          pagination={false}
          columns={colunasTabela}
          dataSource={dados || []}
          bordered
          rowKey="key"
          key="key"
          size="small"
        />
      </ContainerTabela>
      {/* {dadosBackend && (
        <div style={{ height: 400 }}>
          <Graficos.Barras
            dados={dadosBackend.filter(x => x.TipoDado === 'Porcentagem')}
            indice="DescricaoFrequencia"
            chaves={['3C', '4C', '4E', '5C', '6C', '6B']}
            legendaBaixo="teste"
            legendaEsquerda="teste2"
          />
        </div>
      )} */}
    </>
  );
}

TabelaFrequencia.propTypes = {
  dados: t.oneOfType([t.any]),
};

TabelaFrequencia.defaultProps = {
  dados: [],
};

export default TabelaFrequencia;
