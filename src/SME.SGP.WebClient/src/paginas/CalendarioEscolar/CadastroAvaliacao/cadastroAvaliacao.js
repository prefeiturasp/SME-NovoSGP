import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import styled from 'styled-components';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import RadioGroupButton from '~/componentes/radioGroupButton';
import CampoTexto from '~/componentes/campoTexto';
import SelectComponent from '~/componentes/select';
import { Base, Colors, Label } from '~/componentes';
import history from '~/servicos/history';
import api from '~/servicos/api';
import TextEditor from '~/componentes/textEditor';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;

const clicouBotaoVoltar = () => {
  history.push('/calendario-escolar/calendario-professor');
};

const clicouBotaoCancelar = () => {};

const clicouBotaoExcluir = async () => {};

const clicouBotaoCadastrar = (e, form) => {
  e.persist();
  form.validateForm().then(() => form.handleSubmit(e));
};

const cadastrarAvaliacao = async dados => {
  return dados;
};

const CadastroAvaliacao = () => {
  const [validacoes] = useState(
    Yup.object({
      categoria: Yup.string().required('Selecione a categoria'),
    })
  );

  const usuario = useSelector(store => store.usuario);

  const diaAvaliacao = useSelector(
    store => store.calendarioProfessor.diaSelecionado
  );
  const [dataAvaliacao, setdataAvaliacao] = useState();

  const [listaCategorias] = useState([
    { label: 'Normal', value: 1 },
    { label: 'Interdisciplinar', value: 2 },
  ]);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);

  const [dadosAvaliacao, setDadosAvaliacao] = useState();
  const inicial = {
    categoria: 1,
  };

  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const obterDisciplinas = async () => {
    const disciplinas = await api.get(
      `v1/professores/${usuario.rf}/turmas/${turmaId}/disciplinas`
    );
    if (disciplinas.data) setListaDisciplinas(disciplinas.data);
  };

  useEffect(() => {
    setdataAvaliacao(window.moment(diaAvaliacao));
    setDadosAvaliacao(inicial);
    obterDisciplinas();
  }, []);

  const [descricaoAvaliacao, setDescricaoAvaliacao] = useState('');
  const textEditorRef = useRef(null);

  const aoTrocarTextEditor = valor => {
    setDescricaoAvaliacao(valor);
  };

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
                onClick={clicouBotaoCadastrar}
                border
                bold
              />
            </Grid>
            <Form>
              <Div className="row">
                <Grid cols={12} className="mb-4">
                  <RadioGroupButton
                    id="categoria"
                    name="categoria"
                    label="Categoria"
                    opcoes={listaCategorias}
                    form={form}
                  />
                </Grid>
              </Div>
              <Div className="row">
                <Grid cols="4" className="mb-4">
                  <SelectComponent
                    id="componente-curricular"
                    name="componente-curricular"
                    label="Componente curricular"
                    lista={listaDisciplinas}
                    valueOption="codigoComponenteCurricular"
                    valueText="nome"
                    placeholder="Disciplina"
                    form={form}
                  />
                </Grid>
                <Grid cols="4" className="mb-4">
                  <SelectComponent
                    id="componente-curricular"
                    name="componente-curricular"
                    label="Tipo de Atividade Avaliativa"
                    lista={[]}
                    valueOption=""
                    valueText=""
                    placeholder="Atividade Avaliativa"
                    form={form}
                  />
                </Grid>
                <Grid cols="4" className="mb-4">
                  <Label text="Nome da Atividade Avaliativa" />
                  <CampoTexto
                    name="nome-atividade"
                    id="nome-atividade"
                    maxlength={100}
                    placeholder="Nome"
                    type="input"
                    form={form}
                    icon
                  />
                </Grid>
              </Div>
              <Div className="row">
                <Grid cols={12}>
                  <Label text="Descrição" />
                  <TextEditor
                    ref={textEditorRef}
                    id="textEditor"
                    onBlur={aoTrocarTextEditor}
                    value={descricaoAvaliacao}
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

export default CadastroAvaliacao;
