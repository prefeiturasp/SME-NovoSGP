import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import RadioGroupButton from '~/componentes/radioGroupButton';
import CampoTexto from '~/componentes/campoTexto';
import SelectComponent from '~/componentes/select';
import { Colors, Label } from '~/componentes';
import history from '~/servicos/history';
import api from '~/servicos/api';
import TextEditor from '~/componentes/textEditor';
import { Div, Titulo } from './avaliacaoStyled';
import RotasDTO from '~/dtos/rotasDto';
import ServicoAvaliacao from '~/servicos/Paginas/Calendario/ServicoAvaliacao';
import { erro, sucesso, confirmar } from '~/servicos/alertas';

const AvaliacaoForm = ({ match }) => {
  const botaoCadastrarRef = useRef(null);

  const clicouBotaoVoltar = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    if (confirmado) {
      botaoCadastrarRef.current.click();
    } else {
      history.push(RotasDTO.CALENDARIO_PROFESSOR);
    }
  };

  const clicouBotaoExcluir = async () => {};

  const clicouBotaoCadastrar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
  };

  const [idAvaliacao, setIdAvaliacao] = useState('');

  const eventoAulaCalendarioEdicao = useSelector(
    store => store.calendarioProfessor.eventoAulaCalendarioEdicao
  );

  const diaAvaliacao = useSelector(
    store => store.calendarioProfessor.diaSelecionado
  );

  const [descricao, setDescricao] = useState('');

  const cadastrarAvaliacao = async dados => {
    const avaliacao = {};
    avaliacao.dreId = eventoAulaCalendarioEdicao.dre;
    avaliacao.turmaId = eventoAulaCalendarioEdicao.turma;
    avaliacao.ueId = eventoAulaCalendarioEdicao.unidadeEscolar;
    avaliacao.dataAvaliacao = window.moment(diaAvaliacao).format();
    avaliacao.descricao = descricao;

    ServicoAvaliacao.salvar(idAvaliacao, { ...dados, ...avaliacao })
      .then(() => {
        sucesso(
          `Avaliação ${idAvaliacao ? 'atualizada' : 'cadastrada'} com sucesso!`
        );
        history.push(RotasDTO.CALENDARIO_PROFESSOR);
      })
      .catch(() => {
        erro(`Erro ao ${idAvaliacao ? 'atualizar' : 'cadastrar'} a avaliação!`);
      });
  };

  const [validacoes] = useState(
    Yup.object({
      categoriaId: Yup.string().required('Selecione a categoriaId'),
      disciplinaId: Yup.string().required('Selecione o componente curricular'),
      tipoAvaliacaoId: Yup.string().required(
        'Selecione o tipo de atividade avaliativa'
      ),
      nome: Yup.string().required('Preencha o nome da atividade avaliativa'),
    })
  );

  const usuario = useSelector(store => store.usuario);

  const [dataAvaliacao, setdataAvaliacao] = useState();

  const [listaCategorias] = useState([
    { label: 'Normal', value: 1 },
    { label: 'Interdisciplinar', value: 2 },
  ]);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);

  const campoNomeRef = useRef(null);
  const textEditorRef = useRef(null);

  const aoTrocarTextEditor = valor => {
    setDescricao(valor);
  };

  const [dadosAvaliacao, setDadosAvaliacao] = useState();
  const inicial = {
    categoriaId: 1,
    disciplinaId: undefined,
    nome: '',
    tipoAvaliacaoId: undefined,
  };

  const clicouBotaoCancelar = () => {
    setDadosAvaliacao(inicial);
  };

  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const obterDisciplinas = async () => {
    const disciplinas = await api.get(
      `v1/professores/${usuario.rf}/turmas/${turmaId}/disciplinas`
    );
    if (disciplinas.data) setListaDisciplinas(disciplinas.data);
  };

  const [disciplinaDesabilitada, setDisciplinaDesabilitada] = useState(false);

  useEffect(() => {
    if (listaDisciplinas.length === 1) {
      setDadosAvaliacao({
        ...dadosAvaliacao,
        disciplinaId: listaDisciplinas[0].codigoComponenteCurricular.toString(),
      });
      setDisciplinaDesabilitada(true);
    }
  }, [listaDisciplinas]);

  const [listaTiposAvaliacao, setListaTiposAvaliacao] = useState([]);

  const obterlistaTiposAvaliacao = async () => {
    const tipos = await api.get('v1/atividade-avaliativa/tipos/listar');
    if (tipos.data && tipos.data.items) {
      const lista = [];
      tipos.data.items.forEach(tipo => {
        lista.push({ nome: tipo.nome, id: tipo.id });
      });
      setListaTiposAvaliacao(lista);
    }
  };

  useEffect(() => {
    setdataAvaliacao(window.moment(diaAvaliacao));
    setDadosAvaliacao(inicial);
    obterDisciplinas();
    obterlistaTiposAvaliacao();

    if (match && match.params && match.params.id)
      setIdAvaliacao(match.params.id);
  }, []);

  useEffect(() => {
    let avaliacao;
    if (idAvaliacao) avaliacao = ServicoAvaliacao.buscar(idAvaliacao);
    if (avaliacao && avaliacao.data) setDadosAvaliacao(avaliacao.data);
  }, [idAvaliacao]);

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">
          {`Cadastro de avaliação - ${
            dataAvaliacao ? dataAvaliacao.format('dddd') : ''
          }, ${dataAvaliacao ? dataAvaliacao.format('DD/MM/YYYY') : ''} `}
        </Titulo>
      </Grid>
      <Formik
        enableReinitialize
        initialValues={dadosAvaliacao}
        onSubmit={dados => cadastrarAvaliacao(dados)}
        validationSchema={validacoes}
        validateOnBlur={false}
        validateOnChange={false}
      >
        {form => (
          <Card className="rounded mb-4 mx-auto">
            <Grid cols={12} className="d-flex justify-content-end mb-3">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                onClick={clicouBotaoVoltar}
                border
                className="mr-3"
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                onClick={clicouBotaoCancelar}
                border
                bold
                className="mr-3"
              />
              <Button
                label="Excluir"
                color={Colors.Vermelho}
                border
                className="mr-3"
                disabled
                onClick={clicouBotaoExcluir}
              />
              <Button
                label="Cadastrar"
                color={Colors.Roxo}
                onClick={e => clicouBotaoCadastrar(form, e)}
                ref={botaoCadastrarRef}
                border
                bold
              />
            </Grid>
            <Form>
              <Div className="row">
                <Grid cols={12} className="mb-4">
                  <RadioGroupButton
                    id="categoriaId"
                    name="categoriaId"
                    label="Categoria"
                    opcoes={listaCategorias}
                    form={form}
                  />
                </Grid>
              </Div>
              <Div className="row">
                <Grid cols="4" className="mb-4">
                  <SelectComponent
                    id="disciplinaId"
                    name="disciplinaId"
                    label="Componente curricular"
                    lista={listaDisciplinas}
                    valueOption="codigoComponenteCurricular"
                    valueText="nome"
                    disabled={disciplinaDesabilitada}
                    placeholder="Disciplina"
                    form={form}
                  />
                </Grid>
                <Grid cols="4" className="mb-4">
                  <SelectComponent
                    id="tipoAvaliacaoId"
                    name="tipoAvaliacaoId"
                    label="Tipo de Atividade Avaliativa"
                    lista={listaTiposAvaliacao}
                    valueOption="id"
                    valueText="nome"
                    placeholder="Atividade Avaliativa"
                    form={form}
                  />
                </Grid>
                <Grid cols="4" className="mb-4">
                  <Label text="Nome da Atividade Avaliativa" />
                  <CampoTexto
                    name="nome"
                    id="nome"
                    maxlength={50}
                    placeholder="Nome"
                    type="input"
                    form={form}
                    ref={campoNomeRef}
                    icon
                  />
                </Grid>
              </Div>
              <Div className="row">
                <Grid cols={12}>
                  <Label text="Descrição" />
                  <TextEditor
                    ref={textEditorRef}
                    id="descricao"
                    onBlur={aoTrocarTextEditor}
                    value={descricao}
                    maxlength={500}
                  />
                </Grid>
              </Div>
            </Form>
          </Card>
        )}
      </Formik>
    </Div>
  );
};

AvaliacaoForm.propTypes = {
  match: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

AvaliacaoForm.defaultProps = {
  match: {},
};

export default AvaliacaoForm;
