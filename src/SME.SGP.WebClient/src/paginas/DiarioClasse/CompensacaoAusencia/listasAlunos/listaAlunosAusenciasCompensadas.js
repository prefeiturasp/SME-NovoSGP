import PropTypes from 'prop-types';
import React from 'react';
import { DataTable } from '~/componentes';

import { CardTabelaAlunos } from '../styles';

const ListaAlunosAusenciasCompensadas = props => {
  const {
    listaAusenciaCompensada,
    idsAlunosAusenciaCompensadas,
    onSelectRow,
  } = props;

  const colunasListaAlunosAusenciaCompensada = [
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
    {
      title: 'Compensações',
      dataIndex: 'qtdFaltasCompensadas',
    },
  ];

  const onSelectRowAlunos = ids => {
    onSelectRow(ids);
  };

  return (
    <CardTabelaAlunos>
      <DataTable
        scroll={{ y: 420 }}
        id="lista-alunos-ausencia-compensada"
        idLinha="alunoCodigo"
        selectedRowKeys={idsAlunosAusenciaCompensadas}
        onSelectRow={onSelectRowAlunos}
        columns={colunasListaAlunosAusenciaCompensada}
        dataSource={listaAusenciaCompensada}
        selectMultipleRows
        pagination={false}
        pageSize={9999}
      />
    </CardTabelaAlunos>
  );
};

ListaAlunosAusenciasCompensadas.propTypes = {
  listaAusenciaCompensada: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
  ]),
  idsAlunosAusenciaCompensadas: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.string,
  ]),
  onSelectRow: PropTypes.func,
};

ListaAlunosAusenciasCompensadas.defaultProps = {
  listaAusenciaCompensada: [],
  idsAlunosAusenciaCompensadas: [],
  onSelectRow: () => {},
};

export default ListaAlunosAusenciasCompensadas;
