import React, { useMemo } from 'react';
import PropTypes from 'prop-types';

// Ant
import { Table } from 'antd';

// Componentes
import { Base } from '~/componentes';

// Estilos
import { ContainerTabela } from './styles';

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
      className: 'primeirasColunas',
      colSpan: 2,
      fixed: 'left',
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
      className: 'primeirasColunas',
      colSpan: 0,
      width: 150,
      fixed: 'left',
      render: text => {
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
    if (!Object.entries(dados).length) return [];

    const colunasParaExcluir = [
      'TipoDado',
      'FrequenciaDescricao',
      'key',
      'Descricao',
      'indice',
    ];

    const colunasParaRenderizar = Object.keys(dados[0]).filter(
      item => colunasParaExcluir.indexOf(item) === -1
    );

    const colunasParaIncluir = colunasParaRenderizar
      .map(item => {
        if (item !== 'Total') return { title: item, dataIndex: item };
        return null;
      })
      .filter(item => item !== null);

    colunasParaIncluir.push({
      title: 'Total',
      dataIndex: 'Total',
      width: 100,
      className: 'headerTotal',
      fixed: 'right',
      render: text => {
        return {
          children: text || '0',
          className: 'itemColunaTotal',
        };
      },
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
          locale={{ emptyText: 'Sem dados' }}
        />
      </ContainerTabela>
    </>
  );
}

TabelaFrequencia.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
};

TabelaFrequencia.defaultProps = {
  dados: [],
};

export default TabelaFrequencia;
