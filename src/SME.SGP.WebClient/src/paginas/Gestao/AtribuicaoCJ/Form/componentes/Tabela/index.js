import React from 'react';

// Ant
import { Switch } from 'antd';

// Componentes
import { DataTable } from '~/componentes';

function Tabela() {
  const colunas = [
    {
      title: 'Disciplina',
      dataIndex: 'disciplina',
    },
    {
      title: 'Professor Titular',
      dataIndex: 'professorTitular',
    },
    {
      title: 'Substituir',
      dataIndex: 'substituir',
      width: '10%',
      align: 'center',
      render: (texto, linha) => {
        return <Switch />;
      },
    },
  ];
  return (
    <DataTable
      id="lista-disciplinas"
      // selectedRowKeys={idTiposSelecionados}
      // onSelectRow={onSelectRow}
      // onClickRow={onClickRow}
      columns={colunas}
      dataSource={[
        {
          disciplina: 'Matematica',
          professorTitular: 'Italo Maio',
          substituir: 'teste',
        },
      ]}
      selectMultipleRows
      pagination={false}
    />
  );
}

export default Tabela;
