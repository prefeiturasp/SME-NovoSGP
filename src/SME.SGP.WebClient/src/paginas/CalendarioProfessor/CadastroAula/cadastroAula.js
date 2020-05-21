import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
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
  Auditoria,
} from '~/componentes';
import servicoCadastroAula from '~/servicos/Paginas/CalendarioProfessor/CadastroAula/ServicoCadastroAula';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
import servicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import CampoNumeroFormik from '~/componentes/campoNumeroFormik/campoNumeroFormik';
import { aulaDto } from '~/dtos/aulaDto';

import { Container } from './cadastroAula.css';
import modalidade from '~/dtos/modalidade';
import ExcluirAula from './excluirAula';

function CadastroDeAula({ match, location }) {
  const { id, tipoCalendarioId } = match.params;

  const [validacoes, setValidacoes] = useState({
    disciplinaId: Yup.string().required('Informe o componente curricular'),
    dataAula: Yup.string()
      .required('Informe a data da aula')
      .typeError('Informe a data da aula'),
    quantidade: Yup.number()
      .typeError('O valor informado deve ser um número')
      .nullable()
      .required('Informe a quantidade de aulas'),
    recorrenciaAula: Yup.string().required('Informe o tipo de recorrência'),
    tipoAula: Yup.string().required('Informe o tipo de aula'),
  });

  const turmaSelecionada = useSelector(store => store.usuario.turmaSelecionada);
  const [edicao, setEdicao] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [exibirModalExclusao, setExibirModalExclusao] = useState(false);
  const [carregandoDados, setCarregandoDados] = useState(false);
  const [editandoAulaExistente, setEditandoAulaExistente] = useState(false);
  const [controlaGrade, setControlaGrade] = useState(true);
  const [grade, setGrade] = useState({
    quantidadeAulasGrade: 0,
    quantidadeAulasRestante: 0,
  });
  const [gradeAtingida, setGradeAtingida] = useState(false);

  const { diaAula } = queryString.parse(location.search);
  const aulaInicial = {
    ...aulaDto,
    dataAula: window.moment(diaAula),
    turmaId: turmaSelecionada.turma,
    ueId: turmaSelecionada.unidadeEscolar,
    tipoCalendarioId,
  };
  const [aula, setAula] = useState(aulaInicial);

  const [quantidadeBloqueada, setQuantidadeBloqueada] = useState(false);
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

  const obterComponenteSelecionadoPorId = useCallback(
    componenteCurricularId => {
      return listaComponentes.find(
        c => c.codigoComponenteCurricular === Number(componenteCurricularId)
      );
    },
    [listaComponentes]
  );

  const ehRegenciaEja = useCallback(
    componenteSelecionado => {
      return (
        componenteSelecionado != null &&
        componenteSelecionado.regencia &&
        turmaSelecionada != null &&
        turmaSelecionada.modalidade == modalidade.EJA
      );
    },
    [turmaSelecionada]
  );

  const navegarParaCalendarioProfessor = () => {
    history.push('/calendario-escolar/calendario-professor');
  };

  const obterAula = useCallback(() => {
    if (id) {
      setEdicao(true);
      setCarregandoDados(true);
      servicoCadastroAula
        .obterPorId(id)
        .then(resposta => {
          setEditandoAulaExistente(true);
          resposta.data.dataAula = window.moment(resposta.data.dataAula);
          setAula(resposta.data);
          const componenteSelecionado = obterComponenteSelecionadoPorId(
            aula.disciplinaId
          );
          if (ehRegenciaEja(componenteSelecionado)) {
            setAula(aulaState => {
              return {
                ...aulaState,
                quantidade: 5,
              };
            });
          }
        })
        .catch(e => {
          erros(e);
          navegarParaCalendarioProfessor();
          setCarregandoDados(false);
        });
    } else setAula(aulaInicial);
  }, [id, turmaSelecionada.turma]);

  const defineGradeRegenteEja = () => {
    setAula(aulaState => {
      return {
        ...aulaState,
        quantidade: 5,
      };
    });
  };

  const defineGrade = (dadosGrade, componenteSelecionado) => {
    setGrade(dadosGrade);
    const quantidade = dadosGrade.quantidadeAulasRestante;
    if (id) {
      if (ehRegenciaEja(componenteSelecionado)) {
        defineGradeRegenteEja();
      } else
        setValidacoes(validacoesState => {
          return {
            ...validacoesState,
            quantidade: Yup.number()
              .typeError('O valor informado deve ser um número')
              .nullable()
              .required('Informe a quantidade de aulas')
              .max(
                aula.quantidade + quantidade,
                `A quantidade máxima de aulas permitidas é ${aula.quantidade +
                  quantidade}.`
              ),
          };
        });
    } else if (quantidade === 1) {
      setAula(aulaState => {
        return {
          ...aulaState,
          quantidade,
        };
      });
    } else if (ehRegenciaEja(componenteSelecionado)) {
      defineGradeRegenteEja();
    }
  };

  const removeGrade = () => {
    setGrade({
      quantidadeAulasGrade: 0,
      quantidadeAulasRestante: 0,
    });
    setGradeAtingida(false);
    setControlaGrade(false);
    setValidacoes(validacoesState => {
      return {
        ...validacoesState,
        quantidade: Yup.number()
          .typeError('O valor informado deve ser um número')
          .nullable()
          .required('Informe a quantidade de aulas'),
      };
    });
  };

  const carregarGrade = useCallback(
    (componenteSelecionado, dataAula) => {
      if (componenteSelecionado && dataAula) {
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
              defineGrade(respostaGrade.data, componenteSelecionado);
            } else removeGrade();
          })
          .catch(e => {
            erros(e);
          })
          .finally(() => setCarregandoDados(false));
      }
    },
    [turmaSelecionada.turma, turmaSelecionada.modalidade]
  );

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
        navegarParaCalendarioProfessor();
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
          const componenteSelecionado = respostaComponentes.data[0];
          let { quantidade } = aula;
          if (ehRegenciaEja(componenteSelecionado)) {
            quantidade = 5;
          }
          setAula(aulaState => {
            return {
              ...aulaState,
              disciplinaId: String(
                componenteSelecionado.codigoComponenteCurricular
              ),
              quantidade,
            };
          });
          if (!id) carregarGrade(componenteSelecionado, aula.dataAula);
        }
      })
      .catch(e => erros(e))
      .finally(() => setCarregandoDados(false));
  }, []);

  const obterDataFormatada = () => {
    if (aula.dataAula) {
      const data = window.moment.isMoment(aula.dataAula)
        ? aula.dataAula
        : window.moment(aula.dataAula);
      return `${data.format('dddd')}, ${data.format('DD/MM/YYYY')}`;
    }
    return '';
  };

  const onChangeComponente = componenteCurricularId => {
    setModoEdicao(true);
    const componenteSelecionado = obterComponenteSelecionadoPorId(
      componenteCurricularId
    );
    setAula(aulaState => {
      return {
        ...aulaState,
        disciplinaId: componenteSelecionado
          ? String(componenteSelecionado.codigoComponenteCurricular)
          : null,
        disciplinaCompartilhadaId: componenteSelecionado?.compartilhada
          ? componenteSelecionado.componenteCurricularId
          : 0,
      };
    });
    carregarGrade(componenteSelecionado, aula.dataAula);
  };

  const onClickCancelar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        obterAula();
        setModoEdicao(false);
      }
    }
  };

  const onChangeDataAula = data => {
    setModoEdicao(true);
    setAula(aulaState => {
      return { ...aulaState, dataAula: data };
    });
    const componenteSelecionado = obterComponenteSelecionadoPorId(
      aula.disciplinaId
    );
    carregarGrade(componenteSelecionado, data);
  };

  const onChangeTipoAula = e => {
    setModoEdicao(true);
    const ehAulaNormal = e.target.value === 1;
    setControlaGrade(ehAulaNormal);

    let tipoRecorrencia = aula.recorrenciaAula;
    const componente = obterComponenteSelecionadoPorId(aula.disciplinaId);

    if (!ehAulaNormal && !ehRegenciaEja(componente)) {
      tipoRecorrencia = recorrencia.AULA_UNICA;
      setControlaGrade(false);
    } else {
      carregarGrade(componente, aula.dataAula);
    }
    setAula(aulaState => {
      return {
        ...aulaState,
        tipoAula: e.target.value,
        recorrenciaAula: tipoRecorrencia,
      };
    });
  };

  const onChangeQuantidadeAula = quantidade => {
    setModoEdicao(true);
    setAula(aulaState => {
      return {
        ...aulaState,
        quantidade,
      };
    });
  };

  const onChangeRecorrencia = e => {
    setModoEdicao(true);
    setAula(aulaState => {
      return {
        ...aulaState,
        recorrenciaAula: e.target.value,
      };
    });
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas, saindo desta página elas serão perdidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        navegarParaCalendarioProfessor();
        setModoEdicao(false);
      }
    } else navegarParaCalendarioProfessor();
  };

  useEffect(() => {
    obterAula();
  }, [obterAula]);

  useEffect(() => {
    carregarComponentesCurriculares(turmaSelecionada.turma);
  }, [carregarComponentesCurriculares, turmaSelecionada.turma]);

  useEffect(() => {
    if (aula.id && listaComponentes.length && aula.dataAula) {
      const componenteSelecionado = obterComponenteSelecionadoPorId(
        aula.disciplinaId
      );
      carregarGrade(componenteSelecionado, aula.dataAula);
    }
  }, [
    listaComponentes,
    aula.id,
    aula.disciplinaId,
    aula.dataAula,
    carregarGrade,
    obterComponenteSelecionadoPorId,
  ]);

  useEffect(() => {
    if (aula.disciplinaId && !edicao && grade.quantidadeAulasRestante == 0) {
      setGradeAtingida(true);
    } else {
      setGradeAtingida(false);
    }
  }, [edicao, grade.quantidadeAulasRestante, aula.disciplinaId]);

  useEffect(() => {
    let bloqueado = false;
    if (!carregandoDados) {
      if (
        (gradeAtingida && controlaGrade) ||
        !aula.disciplinaId ||
        grade.quantidadeAulasRestante == 1
      )
        bloqueado = true;
      else {
        const componenteSelecionado = obterComponenteSelecionadoPorId(
          aula.disciplinaId
        );
        if (ehRegenciaEja(componenteSelecionado) && aula.tipoAula == 1)
          bloqueado = true;
      }
      setQuantidadeBloqueada(bloqueado);
    }
  }, [
    carregandoDados,
    gradeAtingida,
    controlaGrade,
    aula.disciplinaId,
    aula.tipoAula,
  ]);

  return (
    <Container>
      <Loader loading={carregandoDados}>
        <ExcluirAula
          idAula={id}
          visivel={exibirModalExclusao}
          dataAula={obterDataFormatada()}
          nomeComponente={() => {
            const componente = obterComponenteSelecionadoPorId(
              aula.disciplinaId
            );
            return componente?.nome;
          }}
          onFecharModal={() => {
            setExibirModalExclusao(false);
            navegarParaCalendarioProfessor();
          }}
        />
        <div className="col-md-12">
          {controlaGrade && gradeAtingida && !editandoAulaExistente && (
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
        <Cabecalho pagina={`Cadastro de Aula - ${obterDataFormatada()}`} />
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
                <Form className="col-md-12 mb-8">
                  <div className="row">
                    <div className="col-md-3 pb-2 d-flex justify-content-start">
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
                    <div className="col-md-9 pb-2 d-flex justify-content-end">
                      <Button
                        id={shortid.generate()}
                        label="Voltar"
                        icon="arrow-left"
                        color={Colors.Azul}
                        border
                        className="mr-2"
                        onClick={onClickVoltar}
                      />
                      <Button
                        id={shortid.generate()}
                        label="Cancelar"
                        color={Colors.Roxo}
                        border
                        className="mr-2"
                        onClick={onClickCancelar}
                        disabled={!modoEdicao}
                      />
                      <Button
                        id={shortid.generate()}
                        label="Excluir"
                        color={Colors.Vermelho}
                        border
                        className="mr-2"
                        onClick={() => setExibirModalExclusao(true)}
                        disabled={!id}
                      />

                      <Button
                        id={shortid.generate()}
                        label={id ? 'Salvar' : 'Cadastrar'}
                        color={Colors.Roxo}
                        border
                        bold
                        className="mr-2"
                        onClick={() => form.handleSubmit()}
                        disabled={
                          (controlaGrade &&
                            gradeAtingida &&
                            !editandoAulaExistente) ||
                          !aula.disciplinaId
                        }
                      />
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-xs-12 col-md-3 col-lg-3">
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
                        disabled={
                          editandoAulaExistente || listaComponentes.length === 1
                        }
                        onChange={onChangeComponente}
                      />
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-xs-12 col-md-3 col-lg-3">
                      <CampoNumeroFormik
                        label="Quantidade de aulas"
                        id="quantidade-aula"
                        name="quantidade"
                        form={form}
                        min={1}
                        max={5}
                        onChange={onChangeQuantidadeAula}
                        disabled={quantidadeBloqueada}
                      />
                    </div>
                    <div className="col-xs-12 col-md-6 col-lg-6">
                      <RadioGroupButton
                        id="recorrencia-aula"
                        label="Recorrência"
                        opcoes={opcoesRecorrencia}
                        name="recorrenciaAula"
                        form={form}
                        onChange={onChangeRecorrencia}
                        desabilitado={
                          editandoAulaExistente || aula.tipoAula === 2
                        }
                      />
                    </div>
                  </div>
                </Form>
              )}
            </Formik>
          </div>
          <Auditoria
            alteradoEm={aula.alteradoEm}
            alteradoPor={aula.alteradoPor}
            alteradoRf={aula.alteradoRF}
            criadoEm={aula.criadoEm}
            criadoPor={aula.criadoPor}
            criadoRf={aula.criadoRF}
            ignorarMarginTop
          />
        </Card>
      </Loader>
    </Container>
  );
}

export default CadastroDeAula;
