import React from 'react';
import t from 'prop-types';

// Ant
import { Switch } from 'antd';

// Componentes
import { DataTable } from '~/componentes';

function Tabela({ carregando, lista, onChangeSubstituir }) {
  const colunas = [
    {
      title: 'Componente curricular',
      dataIndex: 'disciplina',
      key: 'disciplina',
    },
    {
      title: 'Professor Titular',
      dataIndex: 'professorTitular',
      key: 'professorTitular',
    },
    {
      title: 'Substituir',
      dataIndex: 'substituir',
      key: 'substituir',
      width: '25%',
      align: 'center',
      render: (valor, registro) => {
        return (
          <Switch
            checked={registro.substituir}
            onChange={() => onChangeSubstituir(registro)}
          />
        );
      },
    },
  ];

  return (
    <DataTable
      id="lista-disciplinas"
      rowKey="disciplina"
      columns={colunas}
      dataSource={lista}
      pagination={false}
      loading={carregando}
    />
  );
}

Tabela.propTypes = {
  carregando: t.bool,
  lista: t.oneOfType([t.array]),
  onChangeSubstituir: t.func,
};

Tabela.defaultProps = {
  carregando: false,
  lista: [],
  onChangeSubstituir: null,
};

export default Tabela;
