import React, { useState, useMemo, useCallback, useEffect } from 'react';
import { Table, Radio, Divider } from 'antd';
import Container from './ListaAlunos.css';
import { Linha } from '~/componentes/EstilosGlobais';
import { confirmar, erro, sucesso, erros } from '~/servicos/alertas';
import { Label } from '~/componentes';
import Icon from '~/componentes/icon';

const ListaAlunosSelecionados = props => {
  var { dadosAlunos, alunosSelecionados, onRemove, modoEdicaoConsulta } = props;

  const [alunos, setAlunos] = useState();

  const columns = [
    {
      title: 'Nome do Estudante',
      dataIndex: 'nomeAluno',
      render: text => <a>{text}</a>,
    },
    {
      title: 'Action',
      key: 'action',
      render: (text, record) => (
        <a
          className="float-right"
          onClick={() => {
            if (!modoEdicaoConsulta) onRemove(record.codigoAluno);
          }}
        >
          <Icon icon="fa-2x fa-times"></Icon>
        </a>
      ),
    },
  ];

  console.log(alunosSelecionados);

  const ObterAlunos = useCallback(() => {
    if (!dadosAlunos || dadosAlunos.length === 0) return;

    const alunosLista = dadosAlunos.map(x => {
      return {
        key: `alunos${x.codigoAluno}selecionados`,
        nomeAluno: x.nomeAluno,
        numeroAlunoChamada: x.numeroAlunoChamada,
        codigoAluno: x.codigoAluno,
        selecionado: alunosSelecionados.find(z => z === x.codigoAluno)
          ? true
          : false,
      };
    });

    setAlunos(alunosLista.filter(x => x.selecionado));
  }, [dadosAlunos, alunosSelecionados]);

  useEffect(() => {
    ObterAlunos();
  }, [ObterAlunos, alunosSelecionados, dadosAlunos]);

  return (
    <>
      <Label text="Selecione os estudantes"></Label>
      <Table
        showHeader={false}
        columns={columns}
        dataSource={alunos}
        locale={{ emptyText: 'Nenhum estudante selecionado' }}
      />
    </>
  );
};

export default ListaAlunosSelecionados;
