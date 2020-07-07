import React, { useState, useEffect, useCallback, useRef } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import * as Yup from 'yup';
import Alert from '~/componentes/alert';
import { Cabecalho } from '~/componentes-sgp';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

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
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import RotasDto from '~/dtos/rotasDto';
import AlterarAula from './alterarAula';
import { RegistroMigrado } from '~/componentes-sgp/registro-migrado';

function CadastroDeAula({ match, location }) {
  const { id, tipoCalendarioId } = match.params;
  const permissoesTela = useSelector(state => state.usuario.permissoes);
  const somenteConsulta = verificaSomenteConsulta(
    permissoesTela[RotasDto.CADASTRO_DE_AULA]
  );
  const refForm = useRef();

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
  const [somenteLeitura, setSomenteLeitura] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [exibirModalExclusao, setExibirModalExclusao] = useState(false);
  const [exibirModalAlteracao, setExibirModalAlteracao] = useState(false);
  const [carregandoDados, setCarregandoDados] = useState(false);
  const [controlaGrade, setControlaGrade] = useState(true);
  const [gradeAtingida, setGradeAtingida] = useState(false);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [emManutencao, setEmManutencao] = useState(false);
  const [desabilitarBtnSalvar, setDesabilitarBtnSalvar] = useState(false);

  const { diaAula } = queryString.parse(location.search);
  const aulaInicial = {
    ...aulaDto,
    dataAula: window.moment(diaAula),
    turmaId: turmaSelecionada.turma,
    ueId: turmaSelecionada.unidadeEscolar,
    tipoCalendarioId,
  };

  const [recorrenciaAulaEmEdicao, setRecorrenciaAulaEmEdicao] = useState({
    aulaId: id,
    existeFrequenciaOuPlanoAula: false,
    quantidadeAulasRecorrentes: 0,
    recorrenciaAula: 1,
  });

  const [aula, setAula] = useState(aulaInicial);

  const [quantidadeBloqueada, setQuantidadeBloqueada] = useState(false);
  const [listaComponentes, setListaComponentes] = useState([]);
  const [recorrenciaAulaOriginal, setRecorrenciaAulaOriginal] = useState();

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
  ];

  const [recorrenciaInicial, setRecorrenciaInicial] = useState(1);

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
      disabled: id && recorrenciaInicial !== recorrencia.REPETIR_BIMESTRE_ATUAL
    },
    {
      label: 'Repetir em todos os Bimestres',
      value: recorrencia.REPETIR_TODOS_BIMESTRES,
      disabled: id && recorrenciaInicial !== recorrencia.REPETIR_TODOS_BIMESTRES
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

  const obterAula = useCallback(async () => {
    const carregarComponentesCurriculares = async idTurma => {
      setCarregandoDados(true);
      const respostaComponentes = await servicoDisciplina
        .obterDisciplinasPorTurma(idTurma)
        .catch(e => erros(e))
        .finally(() => setCarregandoDados(false));

      if (respostaComponentes && respostaComponentes.status == 200) {
        setListaComponentes(respostaComponentes.data);
        return respostaComponentes.data;
      }
    };
    const componentes = await carregarComponentesCurriculares(
      turmaSelecionada.turma
    );
    if (id) {
      setCarregandoDados(true);
      servicoCadastroAula
        .obterPorId(id)
        .then(resposta => {
          const respostaAula = resposta.data;
          respostaAula.dataAula = window.moment(respostaAula.dataAula);
          setRecorrenciaInicial(respostaAula.recorrenciaAulaPai ?? respostaAula.recorrenciaAula);
          setAula(respostaAula);
          setRegistroMigrado(respostaAula.migrado);
          setEmManutencao(respostaAula.emManutencao);
          servicoCadastroAula
            .obterRecorrenciaPorIdAula(id, aula.recorrenciaAula)
            .then(resposta => {
              setRecorrenciaAulaEmEdicao(resposta.data);
            })
            .catch(e => erros(e));
          if (componentes) {
            const componenteSelecionado = componentes.find(
              c => c.codigoComponenteCurricular == respostaAula.disciplinaId
            );
            carregarGrade(
              componenteSelecionado,
              respostaAula.dataAula,
              respostaAula.tipoAula,
              respostaAula.tipoAula == 1
            );
          }
        })
        .catch(e => {
          erros(e);
          navegarParaCalendarioProfessor();
          setCarregandoDados(false);
        });
    } else if (componentes && componentes.length == 1) {
      setAula({
        ...aulaInicial,
        disciplinaId: String(componentes[0].codigoComponenteCurricular),
      });

      carregarGrade(
        componentes[0],
        aulaInicial.dataAula,
        aulaInicial.tipoAula,
        aulaInicial.tipoAula == 1
      );
    }
  }, [id, turmaSelecionada.turma]);

  const defineGradeRegistroNovoComValidacoes = quantidadeAulasRestante => {
    setValidacoes(validacoesState => {
      return {
        ...validacoesState,
        quantidade: Yup.number()
          .typeError('O valor informado deve ser um número')
          .nullable()
          .required('Informe a quantidade de aulas')
          .max(
            quantidadeAulasRestante,
            `A quantidade máxima de aulas permitidas é ${quantidadeAulasRestante}.`
          ),
      };
    });

    if (quantidadeAulasRestante == 0) {
      setQuantidadeBloqueada(true);
      setGradeAtingida(true);
      setControlaGrade(true);
    }
  };

  const defineGradeEdicaoComValidacoes = quantidadeAulasRestante => {
    setValidacoes(validacoesState => {
      return {
        ...validacoesState,
        quantidade: Yup.number()
          .typeError('O valor informado deve ser um número')
          .nullable()
          .required('Informe a quantidade de aulas')
          .max(
            quantidadeAulasRestante,
            `A quantidade máxima de aulas permitidas é ${quantidadeAulasRestante}.`
          ),
      };
    });
  };

  const removeGrade = () => {
    refForm.current.handleReset();
    setControlaGrade(false);
    setQuantidadeBloqueada(false);
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

  const defineGrade = useCallback(
    (dadosGrade, tipoAula, aplicarGrade) => {
      refForm.current.handleReset();
      const { quantidadeAulasRestante, podeEditar } = dadosGrade;
      setGradeAtingida(quantidadeAulasRestante == 0);
      if (tipoAula == 1) {
        if (aplicarGrade) {
          setQuantidadeBloqueada(!podeEditar);
          if (!id) {
            if (quantidadeAulasRestante === 1 || !podeEditar) {
              // defineGrade limite 1 aula
              setQuantidadeBloqueada(true);
              setAula(aulaState => {
                return {
                  ...aulaState,
                  quantidade: quantidadeAulasRestante,
                };
              });
            }
            // define grade registro novo com validações
            defineGradeRegistroNovoComValidacoes(quantidadeAulasRestante);
          } else {
            // define grade para edição
            defineGradeEdicaoComValidacoes(quantidadeAulasRestante);
          }
        } else {
          removeGrade();
        }
      } else removeGrade();
    },
    [id]
  );

  const carregarGrade = useCallback(
    (componenteSelecionado, dataAula, tipoAula, aplicarGrade) => {
      if (componenteSelecionado && dataAula) {
        setCarregandoDados(true);
        servicoCadastroAula
          .obterGradePorComponenteETurma(
            turmaSelecionada.turma,
            componenteSelecionado.codigoComponenteCurricular,
            dataAula,
            id || 0,
            componenteSelecionado.regencia,
            tipoAula
          )
          .then(respostaGrade => {
            setDesabilitarBtnSalvar(false);
            if (respostaGrade.status === 200) {
              const { grade } = respostaGrade.data;
              if (grade) {
                defineGrade(grade, tipoAula, aplicarGrade);
              } else {
                removeGrade();
              }
            } else {
              removeGrade();
            }
          })
          .catch(e => {
            setDesabilitarBtnSalvar(true);
            if (
              e &&
              e.response &&
              e.response.data &&
              e.response.data.mensagens
            ) {
              erros(e);
            }
          })
          .finally(() => setCarregandoDados(false));
      }
    },
    [turmaSelecionada.turma, defineGrade, id]
  );

  const validaPerguntaAntesSalvar = async () => {
    const quantidade = recorrenciaAulaEmEdicao.quantidadeAulasRecorrentes;
    return confirmar(
      'Atenção',
      '',
      `Você tem certeza que deseja alterar ${quantidade} ocorrências desta aula a partir desta data?`
    );
  };

  const salvar = async valoresForm => {
    let salvarRegistro = true;
    if (
      id &&
      aula.tipoAula === 1 &&
      (aula.recorrenciaAula === 2 || aula.recorrenciaAula === 3)
    ) {
      salvarRegistro = await validaPerguntaAntesSalvar();
    }

    if (salvarRegistro) {
      const componente = obterComponenteSelecionadoPorId(
        valoresForm.disciplinaId
      );
      if (componente) valoresForm.disciplinaNome = componente.nome;
      setCarregandoDados(true);
      servicoCadastroAula
        .salvar(id, valoresForm, valoresForm.regencia || false)
        .then(resposta => {
          resposta.data.mensagens.forEach(mensagem => sucesso(mensagem));
          navegarParaCalendarioProfessor();
        })
        .catch(e => erros(e))
        .finally(() => setCarregandoDados(false));
    }
  };

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
    carregarGrade(
      componenteSelecionado,
      aula.dataAula,
      aula.tipoAula,
      aula.tipoAula == 1
    );
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
    carregarGrade(componenteSelecionado, data, aula.tipoAula, controlaGrade);
  };

  const onChangeTipoAula = e => {
    setModoEdicao(true);
    const ehAulaNormal = e.target.value === 1;
    setControlaGrade(ehAulaNormal);

    let tipoRecorrencia = aula.recorrenciaAula;
    const componente = obterComponenteSelecionadoPorId(aula.disciplinaId);

    if (!ehAulaNormal) {
      tipoRecorrencia = recorrencia.AULA_UNICA;
      setQuantidadeBloqueada(false);
    }
    carregarGrade(componente, aula.dataAula, e.target.value, ehAulaNormal);
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
    if (id) {
      servicoCadastroAula
        .obterRecorrenciaPorIdAula(id, e.target.value)
        .then(resposta => {
          setRecorrenciaAulaEmEdicao(resposta.data);
        })
        .catch(e => erros(e));
    }
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

  const onClickExcluir = async () => {
    if (recorrenciaAulaEmEdicao.recorrenciaAula == 1) {
      let mensagem = 'Você tem certeza que deseja excluir esta aula?';
      if (recorrenciaAulaEmEdicao.existeFrequenciaOuPlanoAula) {
        mensagem +=
          ' Obs: Esta aula ou sua recorrência possui frequência ou plano de aula registrado, ao excluí-la estará excluindo esse registro também';
      }
      const confirmado = await confirmar(
        `Excluir aula - ${obterDataFormatada()}`,
        mensagem,
        'Deseja Continuar?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const componenteSelecionado = obterComponenteSelecionadoPorId(
          aula.disciplinaId
        );
        if (componenteSelecionado) {
          setCarregandoDados(true);
          servicoCadastroAula
            .excluirAula(id, aula.recorrenciaAula, componenteSelecionado.nome)
            .then(resposta => {
              sucesso(resposta.data.mensagens[0]);
              navegarParaCalendarioProfessor();
            })
            .catch(e => erros(e))
            .finally(() => setCarregandoDados(false));
        }
      }
    } else {
      setExibirModalExclusao(true);
    }
  };

  useEffect(() => {
    setBreadcrumbManual(
      match.url,
      'Cadastro de Aula',
      '/calendario-escolar/calendario-professor'
    );
    obterAula();
  }, [obterAula, match.url]);

  useEffect(() => {
    if (!carregandoDados && aula.somenteLeitura) {
      setSomenteLeitura(true);
    }
  }, [carregandoDados, aula.somenteLeitura]);

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
          recorrencia={recorrenciaAulaEmEdicao}
          onFecharModal={() => {
            setExibirModalExclusao(false);
            navegarParaCalendarioProfessor();
          }}
          onCancelar={() => setExibirModalExclusao(false)}
        />
        <AlterarAula
          visivel={exibirModalAlteracao}
          dataAula={obterDataFormatada()}
          nomeComponente={() => {
            const componente = obterComponenteSelecionadoPorId(
              aula.disciplinaId
            );
            return componente?.nome;
          }}
          recorrencia={recorrenciaAulaEmEdicao}
          recorrenciaSelecionada={aula.recorrenciaAula}
          onFecharModal={(salvar) => {
            setExibirModalAlteracao(false);
            if (salvar) {
              refForm.current.handleSubmit();
            }
          }}
          onCancelar={() => setExibirModalAlteracao(false)}
        />

        <div className="col-md-12">
          {controlaGrade && gradeAtingida && !id && (
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
        <div className="col-md-12">
          {somenteLeitura && (
            <Alert
              alerta={{
                tipo: 'warning',
                id: 'somente-leitura',
                mensagem: 'Você possui permissão somente de leitura nesta aula',
                estiloTitulo: { fontSize: '18px' },
              }}
              className="mb-2"
            />
          )}
        </div>
        <Cabecalho pagina={`Cadastro de Aula - ${obterDataFormatada()}`}>
          {registroMigrado && (
            <div className="col-md-2 float-right">
              <RegistroMigrado>Registro Migrado</RegistroMigrado>
            </div>
          )}
        </Cabecalho>
        <div className="col-md-12">
          {emManutencao && (
            <Alert
              alerta={{
                tipo: 'warning',
                id: 'em-manutencao',
                mensagem: 'Registro em manutenção',
                estiloTitulo: { fontSize: '18px' },
              }}
              className="mb-2"
            />
          )}
        </div>
        <Card>
          <div className="col-xs-12 col-md-12 col-lg-12">
            <Formik
              enableReinitialize
              initialValues={aula}
              validationSchema={Yup.object(validacoes)}
              onSubmit={salvar}
              validateOnChange
              validateOnBlur
              ref={refForm}
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
                        disabled={somenteConsulta || !modoEdicao}
                      />
                      <Button
                        id={shortid.generate()}
                        label="Excluir"
                        color={Colors.Vermelho}
                        border
                        className="mr-2"
                        onClick={onClickExcluir}
                        disabled={somenteConsulta || !id || somenteLeitura}
                      />

                      <Button
                        id={shortid.generate()}
                        label={id ? 'Salvar' : 'Cadastrar'}
                        color={Colors.Roxo}
                        border
                        bold
                        className="mr-2"
                        onClick={() => {
                          if (aula.recorrenciaAula == recorrencia.AULA_UNICA && !recorrenciaAulaEmEdicao.existeFrequenciaOuPlanoAula) {
                            form.handleSubmit();
                          }
                          else {
                            setExibirModalAlteracao(true);
                          }
                        }}
                        disabled={
                          somenteConsulta ||
                          (controlaGrade && gradeAtingida && !id) ||
                          !aula.disciplinaId ||
                          somenteLeitura ||
                          desabilitarBtnSalvar ||
                          !modoEdicao
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
                        desabilitado={!!id}
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
                        disabled={!!id || listaComponentes.length === 1}
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
                        desabilitado={aula.tipoAula === 2}
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
