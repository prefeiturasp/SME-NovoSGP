import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import Alert from '~/componentes/alert';
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
  const { id, tipoCalendarioId } = match.params;

  const [validacoes, setValidacoes] = useState({
    disciplinaId: Yup.string().required('Informe o componente curricular'),
    dataAula: Yup.string().required('Informe a data da aula'),
    quantidade: Yup.number()
      .typeError('O valor informado deve ser um número')
      .nullable()
      .required('Informe a quantidade de aulas'),
    recorrenciaAula: Yup.string().required('Informe o tipo de recorrência'),
    tipoAula: Yup.string().required('Informe o tipo de aula'),
  });

  const turmaSelecionada = useSelector(store => store.usuario.turmaSelecionada);
  const diaAula = useSelector(
    state => state.calendarioProfessor.diaSelecionado
  );

  const [carregandoDados, setCarregandoDados] = useState(false);
  const [editandoAulaExistente, setEditandoAulaExistente] = useState(false);
  const [controlaGrade, setControlaGrade] = useState(true);
  const [gradeAtingida, setGradeAtingida] = useState(false);

  const [aula, setAula] = useState({
    ...aulaDto,
    dataAula: window.moment(diaAula),
    turmaId: turmaSelecionada.turma,
    ueId: turmaSelecionada.unidadeEscolar,
    tipoCalendarioId,
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

  const carregarGrade = (componenteSelecionado, dataAula) => {
    if (componenteSelecionado) {
      setCarregandoDados(true);
      servicoCadastroAula
        .obterGradePorComponenteETurma(
          turmaSelecionada.turma,
          componenteSelecionado.codigoComponenteCurricular,
          dataAula,
          componenteSelecionado.regencia
        )
        .then(respostaGrade => {
          if (respostaGrade.status === 200) {
            if (!editandoAulaExistente) {
              const quantidade = respostaGrade.data.quantidadeAulasRestante;
              if (quantidade === 1) {
                setAula(aulaState => {
                  return {
                    ...aulaState,
                    quantidade,
                  };
                });
              }
              if (quantidade > 0) {
                setValidacoes(validacoesState => {
                  return {
                    ...validacoesState,
                    quantidade: Yup.number()
                      .typeError('O valor informado deve ser um número')
                      .nullable()
                      .required('Informe a quantidade de aulas')
                      .max(
                        quantidade,
                        `O máximo de aulas permitidas deve ser ${quantidade}.`
                      ),
                  };
                });
                setGradeAtingida(false);
              } else {
                setGradeAtingida(true);
              }
            }
          } else {
            setGradeAtingida(false);
            setValidacoes(validacoesState => {
              return {
                ...validacoesState,
                quantidade: Yup.number()
                  .typeError('O valor informado deve ser um número')
                  .nullable()
                  .required('Informe a quantidade de aulas'),
              };
            });
          }
        })
        .catch(e => {
          erros(e);
        })
        .finally(() => setCarregandoDados(false));
    }
  };

  const onChangeComponente = componenteCurricularId => {
    const componenteSelecionado = obterComponenteSelecionadoPorId(
      componenteCurricularId
    );
    setAula(aulaState => {
      return {
        ...aulaState,
        disciplinaId: String(componenteSelecionado?.codigoComponenteCurricular),
        disciplinaCompartilhadaId: componenteSelecionado?.compartilhada
          ? componenteSelecionado.componenteCurricularId
          : 0,
      };
    });
    carregarGrade(componenteSelecionado, aula.dataAula);
  };

  const salvar = valoresForm => {
    const componente = obterComponenteSelecionadoPorId(
      valoresForm.disciplinaId
    );
    if (componente) valoresForm.disciplinaNome = componente.nome;
    setCarregandoDados(true);
    servicoCadastroAula
      .salvar(id, valoresForm)
      .then(resposta => {
        sucesso(resposta.data.mensagens[0]);
        history.push('/calendario-escolar/calendario-professor');
      })
      .catch(e => erros(e))
      .finally(() => setCarregandoDados(false));
  };

  const carregarComponentesCurriculares = useCallback(idTurma => {
    setCarregandoDados(true);
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

  const onChangeDataAula = data => {
    setAula(aulaState => {
      return { ...aulaState, dataAula: data };
    });
    const componente = obterComponenteSelecionadoPorId(aula.disciplinaId);
    carregarGrade(componente, data);
  };

  const onChangeTipoAula = e => {
    setControlaGrade(e.target.value === 1);
    setAula(aulaState => {
      return { ...aulaState, tipoAula: e.target.value };
    });
    const componente = obterComponenteSelecionadoPorId(aula.disciplinaId);
    carregarGrade(componente, aula.dataAula);
  };

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
      <div className="col-md-12">
        {controlaGrade && gradeAtingida && (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'cadastro-aula-quantidade-maxima',
              mensagem:
                'Não é possível criar aula normal porque o limite da grade curricular foi atingido',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        )}
      </div>
      <Cabecalho pagina="Cadastro de Aula - FORMATAR DATA" />
      <Card>
        <div className="col-xs-12 col-md-12 col-lg-12">
          <Formik
            enableReinitialize
            initialValues={aula}
            validationSchema={Yup.object(validacoes)}
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
                      onChange={onChangeDataAula}
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
                      onChange={onChangeComponente}
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
                      disabled={controlaGrade && gradeAtingida}
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
                      min={1}
                      max={5}
                      disabled={controlaGrade && gradeAtingida}
                    />
                  </div>
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="tipo-aula"
                      label="Tipo de aula"
                      opcoes={opcoesTipoAula}
                      name="tipoAula"
                      form={form}
                      onChange={onChangeTipoAula}
                      desabilitado={editandoAulaExistente}
                    />
                  </div>
                  <div className="col-xs-12 col-md-4 col-lg-4">
                    <RadioGroupButton
                      id="recorrencia-aula"
                      label="Recorrência"
                      opcoes={opcoesRecorrencia}
                      name="recorrenciaAula"
                      form={form}
                      desabilitado={editandoAulaExistente}
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
