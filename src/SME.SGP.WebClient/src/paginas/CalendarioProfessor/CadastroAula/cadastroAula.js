import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import { Cabecalho } from '~/componentes-sgp';
import {
  Card,
  SelectComponent,
  Loader,
  CampoData,
  RadioGroupButton,
  Button,
  Colors,
} from '~/componentes';
import servicoCadastroAula from '~/servicos/Paginas/CalendarioProfessor/CadastroAula/ServicoCadastroAula';
import { erros } from '~/servicos/alertas';
import servicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

// import { Container } from './styles';

function CadastroDeAula({ match }) {
  const { id } = match.params;

  const [validacoes, setValidacoes] = useState(
    Yup.object({
      disciplinaId: Yup.string().required('Informe o componente curricular'),
      dataAula: Yup.string().required('Informe a data da aula'),
      quantidade: Yup.string().required('Informe a quantidade de aulas'),
      recorrenciaAula: Yup.string().required('Informe o tipo de recorrência'),
      tipoAula: Yup.string().required('Informe o tipo de aula'),
    })
  );

  const turmaSelecionada = useSelector(store => store.usuario.turmaSelecionada);
  const diaAula = useSelector(
    state => state.calendarioProfessor.diaSelecionado
  );
  const [carregandoDados, setCarregandoDados] = useState(false);
  const [editandoAulaExistente, setEditandoAulaExistente] = useState(false);
  const [aula, setAula] = useState({
    dataAula: window.moment(diaAula),
    disciplinaId: '',
    quantidade: 1,
    recorrenciaAula: 1,
    tipoAula: 1,
  });

  const [listaComponentes, setListaComponentes] = useState([]);
  const [componenteSelecionado, setComponenteSelecionado] = useState([]);

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
  ];

  const opcoesQuantidadeAulas = [
    {
      label: '1',
      value: 1,
    },
    {
      label: '2',
      value: 2,
    },
  ];

  const recorrencia = {
    AULA_UNICA: 1,
    REPETIR_BIMESTRE_ATUAL: 2,
    REPETIR_TODOS_BIMESTRES: 3,
  };

  const opcoesRecorrencia = [
    { label: 'Aula única', value: recorrencia.AULA_UNICA },
    {
      label: 'Repetir no Bimestre atual',
      value: recorrencia.REPETIR_BIMESTRE_ATUAL,
    },
    {
      label: 'Repetir em todos os Bimestres',
      value: recorrencia.REPETIR_TODOS_BIMESTRES,
    },
  ];

  const salvar = valoresForm => {
    debugger;
    //servicoCadastroAula.salvar(valoresForm);
  };

  const carregarComponentesCurriculares = useCallback(idTurma => {
    servicoDisciplina
      .obterDisciplinasPorTurma(idTurma)
      .then(respostaComponentes =>
        setListaComponentes(respostaComponentes.data)
      )
      .catch(e => erros(e))
      .finally(() => setCarregandoDados(false));
  }, []);

  useEffect(() => {
    if (id) {
      setEditandoAulaExistente(true);
      setCarregandoDados(true);
      servicoCadastroAula
        .obterPorId(id)
        .then(resposta => {
          resposta.data.dataAula = window.moment(resposta.data.dataAula);
          setAula(resposta.data);
          console.log(resposta.data);
          setCarregandoDados(true);
          carregarComponentesCurriculares(resposta.data.turmaId);
        })
        .catch(e => {
          erros(e);
          setCarregandoDados(false);
        });
    } else {
      carregarComponentesCurriculares(turmaSelecionada.turma);
    }
  }, [carregarComponentesCurriculares, id, turmaSelecionada.turma]);

  return (
    <Loader loading={carregandoDados}>
      <Cabecalho pagina="Cadastro de Aula - FORMATAR DATA" />
      <Card>
        <div className="col-xs-12 col-md-12 col-lg-12">
          <Formik
            enableReinitialize
            initialValues={aula}
            validationSchema={validacoes}
            onSubmit={salvar}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <div className="row">
                  <div className="col-md-4 pb-2 d-flex justify-content-start">
                    <CampoData
                      placeholder="Data da aula"
                      formatoData="DD/MM/YYYY"
                      name="dataAula"
                      id="dataAula"
                      form={form}
                    />
                  </div>
                  <div className="col-md-8 pb-2 d-flex justify-content-end">
                    <Button
                      id={shortid.generate()}
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-2"
                    />
                    <Button
                      id={shortid.generate()}
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      className="mr-2"
                    />
                    <Button
                      id={shortid.generate()}
                      label="Excluir"
                      color={Colors.Vermelho}
                      border
                      className="mr-2"
                    />

                    <Button
                      id={shortid.generate()}
                      label="Cadastrar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-2"
                      onClick={() => form.handleSubmit()}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="tipo-aula"
                      label="Tipo de aula"
                      opcoes={opcoesTipoAula}
                      name="tipoAula"
                      form={form}
                    />
                  </div>
                  <div className="col-xs-12 col-md-6 col-lg-6">
                    <SelectComponent
                      id="disciplinaId"
                      name="disciplinaId"
                      lista={listaComponentes}
                      label="Componente Curricular"
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      placeholder="Selecione um componente curricular"
                      form={form}
                      disabled={editandoAulaExistente}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="quantidade-aula"
                      label="Quantidade de aulas"
                      opcoes={opcoesQuantidadeAulas}
                      name="quantidade"
                      form={form}
                    />
                  </div>
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="recorrencia-aula"
                      label="Recorrência"
                      opcoes={opcoesRecorrencia}
                      name="recorrenciaAula"
                      form={form}
                    />
                  </div>
                </div>
              </Form>
            )}
          </Formik>
        </div>
      </Card>
    </Loader>
  );
}

export default CadastroDeAula;
