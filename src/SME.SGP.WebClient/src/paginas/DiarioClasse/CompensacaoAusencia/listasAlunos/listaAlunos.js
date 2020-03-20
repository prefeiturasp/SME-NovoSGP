import PropTypes from 'prop-types';
import React from 'react';
import { DataTable, Label } from '~/componentes';

import { CardTabelaAlunos } from '../styles';

const ListaAlunos = props => {
  const { lista, idsAlunos, onSelectRow } = props;

  const montaExibicaoPercentual = (frequencia, dadosAluno) => {
    const frequenciaArredondada = frequencia
      ? Number(frequencia).toFixed(2)
      : '0,00';
    if (dadosAluno.alerta) {
      return (
        <>
          {`${frequenciaArredondada}% `}
          <i
            className="fas fa-exclamation-triangle"
            style={{ color: '#b40c02' }}
          />
        </>
      );
    }
    return `${frequenciaArredondada}%`;
  };

  const colunasListaAlunos = [
    {
      title: 'Nome',
      dataIndex: 'nome',
      ellipsis: true,
    },
    {
      title: 'Frequência',
      dataIndex: 'percentualFrequencia',
      render: (frequencia, dadosAluno) =>
        montaExibicaoPercentual(frequencia, dadosAluno),
    },
    {
      title: 'Faltas',
      dataIndex: 'quantidadeFaltasTotais',
    },
  ];

  const onSelectRowAlunos = ids => {
    onSelectRow(ids);
  };
  return (
    <>
      <Label text="" />
      <CardTabelaAlunos>
        <DataTable
          scroll={{ y: 420 }}
          id="lista-alunos"
          selectedRowKeys={idsAlunos}
          onSelectRow={onSelectRowAlunos}
          columns={colunasListaAlunos}
          dataSource={lista}
          selectMultipleRows
          onClickRow={() => { }}
          pagination={false}
          pageSize={9999}
        />
      </CardTabelaAlunos>
    </>
  );
};

ListaAlunos.propTypes = {
  lista: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  idsAlunos: PropTypes.oneOfType([PropTypes.array, PropTypes.string]),
  onSelectRow: PropTypes.func,
};

ListaAlunos.defaultProps = {
  lista: [],
  idsAlunos: [],
  onSelectRow: () => { },
};

export default ListaAlunos;
