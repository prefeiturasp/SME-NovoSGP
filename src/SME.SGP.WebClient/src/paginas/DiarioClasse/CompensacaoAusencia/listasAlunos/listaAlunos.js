import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { DataTable } from '~/componentes';

import { CardTabelaAlunos } from '../styles';

const ListaAlunos = props => {
  const { lista, idsAlunos, onSelectRow } = props;

  const colunasListaAlunos = [
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
    {
      title: 'Frequência',
      dataIndex: 'frequencia',
    },
    {
      title: 'Faltas',
      dataIndex: 'faltas',
    },
  ];

  const onSelectRowAlunos = ids => {
    onSelectRow(ids);
  };
  return (
    <CardTabelaAlunos>
      <DataTable
        scroll={{ y: 420 }}
        id="lista-alunos"
        idLinha="alunoCodigo"
        selectedRowKeys={idsAlunos}
        onSelectRow={onSelectRowAlunos}
        columns={colunasListaAlunos}
        dataSource={lista}
        selectMultipleRows
        onClickRow={() => {}}
        pagination={false}
        pageSize={9999}
      />
    </CardTabelaAlunos>
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
  onSelectRow: () => {},
};

export default ListaAlunos;
