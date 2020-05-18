import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import { Cabecalho } from '~/componentes-sgp';
import history from '~/servicos/history';
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
import { erros, sucesso } from '~/servicos/alertas';
import servicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import CampoNumeroFormik from '~/componentes/campoNumeroFormik/campoNumeroFormik';
import { aulaDto } from '~/dtos/aulaDto';

// import { Container } from './styles';

function CadastroDeAula({ match }) {
  const { id } = match.params;

  const [validacoes, setValidacoes] = useState(
    Yup.object({
      disciplinaId: Yup.string().required('Informe o componente curricular'),
      dataAula: Yup.string().required('Informe a data da aula'),
      quantidade: Yup.number()
        .typeError('O valor informado deve ser um número')
        .nullable()
        .required('Informe a quantidade de aulas'),
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
  const [controlaGrade, setControlaGrade] = useState(false);
  const [grade, setGrade] = useState();

  const [aula, setAula] = useState({
    ...aulaDto,
    dataAula: window.moment(diaAula),
    turmaId: turmaSelecionada.turma,
  });

  const [listaComponentes, setListaComponentes] = useState([]);

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
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

  const obterComponenteSelecionadoPorId = componenteCurricularId => {
    return listaComponentes.find(
      c => c.codigoComponenteCurricular === Number(componenteCurricularId)
    );
  };

  const carregarGrade = componenteCurricularId => {
    const componenteSelecionado = obterComponenteSelecionadoPorId(
      componenteCurricularId
    );
    if (componenteSelecionado) {
      servicoCadastroAula
        .obterGradePorComponenteETurma(
          turmaSelecionada.turma,
          componenteCurricularId,
          componenteSelecionado.regencia
        )
        .then(respostaGrade => {
          if (respostaGrade.status === 200) {
            setGrade(respostaGrade.data);
            if (
              !editandoAulaExistente &&
              respostaGrade.data.quantidadeRestante == 1
            ) {
              setAula(aulaState => {
                return {
                  ...aulaState,
                  quantidade: 1,
                };
              });
            }
          } else setGrade();
        })
        .catch(e => {
          erros(e);
        });
    }
  };

  const salvar = valoresForm => {
    const componente = obterComponenteSelecionadoPorId(
      valoresForm.disciplinaId
    );
    if (componente) valoresForm.disciplinaNome = componente.nome;
    servicoCadastroAula
      .salvar(id, valoresForm)
      .then(resposta => {
        history.push('/calendario-escolar/calendario-professor');
        sucesso(resposta.mensagens[0]);
      })
      .catch(e => erros(e));
  };

  const carregarComponentesCurriculares = useCallback(idTurma => {
    servicoDisciplina
      .obterDisciplinasPorTurma(idTurma)
      .then(respostaComponentes => {
        setListaComponentes(respostaComponentes.data);
        if (respostaComponentes.data.length === 1) {
          setAula(aulaState => {
            return {
              ...aulaState,
              disciplinaId: String(
                respostaComponentes.data[0].codigoComponenteCurricular
              ),
            };
          });
        }
      })
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
                  <div className="col-md-2 pb-2 d-flex justify-content-start">
                    <CampoData
                      placeholder="Data da aula"
                      label="Data da aula"
                      formatoData="DD/MM/YYYY"
                      name="dataAula"
                      id="dataAula"
                      form={form}
                    />
                  </div>
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <SelectComponent
                      id="disciplinaId"
                      name="disciplinaId"
                      lista={listaComponentes}
                      label="Componente Curricular"
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      placeholder="Selecione um componente curricular"
                      form={form}
                      disabled={
                        editandoAulaExistente || listaComponentes.length === 1
                      }
                      onChange={valor => {
                        carregarGrade(valor);
                      }}
                    />
                  </div>
                  <div className="col-md-6 pb-2 d-flex justify-content-end">
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
                  <div className="col-xs-12 col-md-2 col-lg-2">
                    <CampoNumeroFormik
                      label="Quantidade de aulas"
                      id="quantidade-aula"
                      name="quantidade"
                      form={form}
                      min={0}
                      max={5}
                      disabled={controlaGrade && grade.quantidadeRestante === 0}
                    />
                  </div>
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="tipo-aula"
                      label="Tipo de aula"
                      opcoes={opcoesTipoAula}
                      name="tipoAula"
                      form={form}
                      onChange={tipo => setControlaGrade(tipo === 1)}
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
