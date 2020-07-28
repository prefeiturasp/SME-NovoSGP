import React, { useState, useMemo, useEffect, useCallback } from 'react';
import { Table, Radio, Divider } from 'antd';
import Container from './ListaAlunos.css';
import { Linha } from '~/componentes/EstilosGlobais';
import { confirmar, erro, sucesso, erros } from '~/servicos/alertas';
import {
  Loader,
  Card,
  ButtonGroup,
  Grid,
  SelectComponent,
  CampoTexto,
  CampoData,
  Label,
  momentSchema,
  Base,
  Editor,
  ModalConteudoHtml,
  Button,
} from '~/componentes';

const ListaAlunos = props => {
  var {
    dadosAlunos,
    alunosSelecionados,
    alunosLoader,
    ObterAlunos,
    onClose,
    onConfirm,
    refForm,
    modoEdicaoConsulta,
  } = props;

  const [linhasSelecionadas, setLinhasSelecionadas] = useState();
  const [modalAlunosVisivel, setModalAlunosVisivel] = useState(false);
  const [alunos, setAlunos] = useState([]);

  const columns = [
    {
      title: '#',
      dataIndex: 'numeroAlunoChamada',
    },
    {
      title: 'Nome do Estudante',
      dataIndex: 'nomeAluno',
      render: text => <a>{text}</a>,
    },
  ];

  const MapearAlunos = useCallback(() => {
    if (!dadosAlunos || dadosAlunos.length === 0) return;
    const alunosLista = dadosAlunos.map(x => {
      return {
        key: x.codigoAluno,
        nomeAluno: x.nomeAluno,
        numeroAlunoChamada: x.numeroAlunoChamada,
        codigoAluno: x.codigoAluno,
        selecionado: alunosSelecionados.find(z => z === x.codigoAluno)
          ? true
          : false,
      };
    });

    setAlunos(alunosLista.filter(x => !x.selecionado));
  }, [dadosAlunos, alunosSelecionados]);

  useEffect(() => MapearAlunos(), [MapearAlunos]);

  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      setLinhasSelecionadas(selectedRows);
    },
    getCheckboxProps: record => ({
      name: record.name,
    }),
  };

  const onClickSelecionarEstudantes = async () => {
    if (
      (refForm?.state?.values?.turmas?.length <= 0 ?? true) ||
      refForm.state.values.turmas.length <= 0
    ) {
      erro('Deve ser selecionada uma turma antes da seleção dos alunos');
      return;
    }

    if (refForm.state.values.turmas.length > 1) {
      erro('Somente é possivel selecionar alunos de uma unica turma');
      return;
    }

    var retorno = await ObterAlunos(
      refForm.state.values.turmas[0],
      refForm.state.values.anoLetivo
    );

    setModalAlunosVisivel(retorno);
  };

  return (
    <Linha className="row">
      <Grid cols={12}>
        <Label
          className="float-left"
          text="Estudantes selecionados que receberão a mensagem"
        ></Label>
        <Button
          type="button"
          color="Roxo"
          label="Selecionar Estudantes"
          className="float-right"
          disabled={modoEdicaoConsulta}
          onClick={onClickSelecionarEstudantes}
        />
      </Grid>
      <Grid cols={12}>
        <ModalConteudoHtml
          titulo="Lista de Estudantes"
          visivel={modalAlunosVisivel}
          onClose={() => {
            setModalAlunosVisivel(false);
            onClose();
          }}
          onConfirmacaoSecundaria={() => {
            setModalAlunosVisivel(false);
            onClose();
          }}
          onConfirmacaoPrincipal={() => {
            onConfirm(linhasSelecionadas.map(x => x.codigoAluno));
            setModalAlunosVisivel(false);
          }}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          closable
          loader={alunosLoader}
          width="50%"
          fecharAoClicarFora
          fecharAoClicarEsc
        >
          <Loader loading={alunosLoader}>
            <Label text="Selecione os estudantes"></Label>
            <Table
              showHeader={false}
              rowSelection={{
                type: 'checkbox',
                ...rowSelection,
              }}
              columns={columns}
              dataSource={alunos}
            />
          </Loader>
        </ModalConteudoHtml>
      </Grid>
    </Linha>
  );
};

export default ListaAlunos;
