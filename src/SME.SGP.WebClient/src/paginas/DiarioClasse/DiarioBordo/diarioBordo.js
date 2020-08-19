import { Form, Formik } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import { Auditoria, CampoData, Loader, PainelCollapse } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import AlertaPeriodoEncerrado from '~/componentes-sgp/Calendario/componentes/MesCompleto/componentes/Dias/componentes/DiaCompleto/componentes/AlertaPeriodoEncerrado';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import Editor from '~/componentes/editor/editor';
import ModalMultiLinhas from '~/componentes/modalMultiLinhas';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoDiarioBordo from '~/servicos/Paginas/DiarioClasse/ServicoDiarioBordo';
import ServicoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoFrequencia';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import ModalSelecionarAula from './modalSelecionarAula';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import AlertaPermiteSomenteTurmaInfantil from '~/componentes-sgp/AlertaPermiteSomenteTurmaInfantil/alertaPermiteSomenteTurmaInfantil';

const DiarioBordo = () => {
  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;
  const permissoesTela = usuario.permissoes[RotasDto.DIARIO_BORDO];

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [
    listaComponenteCurriculare,
    setListaComponenteCurriculare,
  ] = useState();
  const [
    componenteCurricularSelecionado,
    setComponenteCurricularSelecionado,
  ] = useState();
  const [dataSelecionada, setDataSelecionada] = useState();
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [listaDatasAulas, setListaDatasAulas] = useState([]);
  const [diasParaHabilitar, setDiasParaHabilitar] = useState();
  const [errosValidacao, setErrosValidacao] = useState([]);
  const [mostrarErros, setMostarErros] = useState(false);
  const [auditoria, setAuditoria] = useState('');
  const [turmaInfantil, setTurmaInfantil] = useState(false);
  const [refForm, setRefForm] = useState({});
  const [temPeriodoAberto, setTemPeriodoAberto] = useState(true);
  const [aulaSelecionada, setAulaSelecionada] = useState();
  const [aulasParaSelecionar, setAulasParaSelecionar] = useState([]);
  const [exibirModalSelecionarAula, setExibirModalSelecionarAula] = useState(
    false
  );
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const inicial = {
    aulaId: 0,
    planejamento: '',
    reflexoesReplanejamento: '',
    devolutivas: '',
  };
  const [valoresIniciais, setValoresIniciais] = useState(inicial);

  const validacoes = Yup.object({
    planejamento: Yup.string()
      .required('Campo planejamento é obrigatório')
      .min(
        200,
        'Você precisa preencher o planejamento com no mínimo 200 caracteres'
      ),
  });

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = !ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );

    const soConsulta = verificaSomenteConsulta(
      permissoesTela,
      naoSetarSomenteConsultaNoStore
    );
    setSomenteConsulta(soConsulta);
    const desabilitar =
      auditoria && auditoria.id > 0
        ? soConsulta || !permissoesTela.podeAlterar
        : soConsulta || !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);

    if (!temPeriodoAberto) {
      setDesabilitarCampos(true);
    }
  }, [
    auditoria,
    permissoesTela,
    temPeriodoAberto,
    modalidadesFiltroPrincipal,
    turmaSelecionada,
  ]);

  const resetarTela = useCallback(
    form => {
      setValoresIniciais(inicial);
      if (form && form.resetForm) {
        form.resetForm();
      }
      setDataSelecionada('');
      setAulaSelecionada();
      setModoEdicao(false);
      setAuditoria();
      setTemPeriodoAberto(true);
    },
    [inicial]
  );

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);

    if (!turmaInfantil) {
      resetarTela(refForm);
    }
  }, [turmaSelecionada, modalidadesFiltroPrincipal, turmaInfantil]);

  useEffect(() => {
    setListaDatasAulas();
    setDiasParaHabilitar();
    resetarTela(refForm);
  }, [turmaSelecionada.turma]);

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
    ).catch(e => erros(e));

    if (componentes.data && componentes.data.length) {
      setListaComponenteCurriculare(componentes.data);

      if (componentes.data.length === 1) {
        const componente = componentes.data[0];
        setComponenteCurricularSelecionado(
          String(componente.codigoComponenteCurricular)
        );
      }
    }

    setCarregandoGeral(false);
  }, [turmaId]);

  useEffect(() => {
    if (turmaId && turmaInfantil) {
      obterComponentesCurriculares();
    } else {
      setListaComponenteCurriculare([]);
      setComponenteCurricularSelecionado(undefined);
      resetarTela();
    }
  }, [turmaId, obterComponentesCurriculares, turmaInfantil]);

  const obterDatasDeAulasDisponiveis = useCallback(async () => {
    setCarregandoGeral(true);
    const datasDeAulas = await ServicoFrequencia.obterDatasDeAulasPorCalendarioTurmaEComponenteCurricular(
      turmaId,
      componenteCurricularSelecionado
    ).catch(e => {
      setCarregandoGeral(false);
      erros(e);
    });

    setCarregandoGeral(false);
    if (datasDeAulas && datasDeAulas.data && datasDeAulas.data.length) {
      setListaDatasAulas(datasDeAulas.data);
      const habilitar = datasDeAulas.data.map(item =>
        window.moment(item.data).format('YYYY-MM-DD')
      );
      setDiasParaHabilitar(habilitar);
    } else {
      setListaDatasAulas();
      setDiasParaHabilitar();
    }
  }, [turmaId, componenteCurricularSelecionado]);

  useEffect(() => {
    if (turmaId && componenteCurricularSelecionado) {
      obterDatasDeAulasDisponiveis();
    }
  }, [turmaId, componenteCurricularSelecionado, obterDatasDeAulasDisponiveis]);

  const onChangeComponenteCurricular = valor => {
    if (!valor) {
      setDiasParaHabilitar([]);
    }
    setDataSelecionada('');
    setComponenteCurricularSelecionado(valor);
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const obterDiarioBordo = async aulaId => {
    setCarregandoGeral(true);
    const retorno = await ServicoDiarioBordo.obterDiarioBordo(aulaId).catch(e =>
      erros(e)
    );
    setCarregandoGeral(false);

    if (retorno && retorno.data) {
      const valInicial = {
        aulaId: aulaId || 0,
        planejamento: retorno.data.planejamento || '',
        reflexoesReplanejamento: retorno.data.reflexoesReplanejamento || '',
        devolutivas: retorno.data.devolutivas || '',
      };
      setTemPeriodoAberto(retorno.data.temPeriodoAberto);
      setValoresIniciais(valInicial);
      if (retorno.data.auditoria) {
        setAuditoria(retorno.data.auditoria);
      }
    }
  };

  const salvarDiarioDeBordo = async (valores, form, clicouBtnSalvar) => {
    setCarregandoGeral(true);
    const params = {
      aulaId: aulaSelecionada.aulaId,
      planejamento: valores.planejamento,
      reflexoesReplanejamento: valores.reflexoesReplanejamento,
    };
    const retorno = await ServicoDiarioBordo.salvarDiarioBordo(
      params,
      auditoria.id
    ).catch(e => erros(e));
    setCarregandoGeral(false);
    let salvouComSucesso = false;
    if (retorno && retorno.status === 200) {
      sucesso('Diário de bordo salvo com sucesso.');
      if (clicouBtnSalvar) {
        setModoEdicao(false);
        resetarTela();
      }
      salvouComSucesso = true;
    }
    return salvouComSucesso;
  };

  const validaAntesDoSubmit = (form, clicouBtnSalvar) => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    return form.validateForm().then(() => {
      if (Object.keys(form.errors).length > 0) {
        setErrosValidacao([form.errors.planejamento]);
        setMostarErros(true);
      } else {
        setErrosValidacao([]);
        setMostarErros(false);
      }

      if (form.isValid || Object.keys(form.errors).length === 0) {
        return salvarDiarioDeBordo(form.values, form, clicouBtnSalvar);
      }
      return false;
    });
  };

  const obterAulasDataSelecionada = useCallback(
    async data => {
      if (listaDatasAulas) {
        const aulaDataSelecionada = listaDatasAulas.find(item => {
          return (
            window.moment(item.data).format('DD/MM/YYYY') ===
            window.moment(data).format('DD/MM/YYYY')
          );
        });

        return aulaDataSelecionada;
      }
      return null;
    },
    [listaDatasAulas]
  );

  const validaSeTemIdAula = useCallback(
    async (data, form) => {
      form.resetForm();
      setValoresIniciais(inicial);
      setModoEdicao(false);
      setAulaSelecionada();
      setAuditoria();
      const aulasDataSelecionada = await obterAulasDataSelecionada(data);

      if (aulasDataSelecionada && aulasDataSelecionada.aulas.length === 1) {
        // Quando for Professor ou CJ podem visualizar somente uma aula por data selecionada!
        const aulaDataSelecionada = aulasDataSelecionada.aulas[0];
        if (aulaDataSelecionada) {
          setAulaSelecionada(aulaDataSelecionada);
          obterDiarioBordo(aulaDataSelecionada.aulaId);
        } else {
          resetarTela(form);
        }
      } else if (
        aulasDataSelecionada &&
        aulasDataSelecionada.aulas.length > 1
      ) {
        // Quando for CP, Diretor ou usuários da DRE e SME podem visualizar mais aulas por data selecionada!
        setAulasParaSelecionar(aulasDataSelecionada.aulas);
        setExibirModalSelecionarAula(true);
      } else {
        resetarTela(form);
      }
    },
    [obterAulasDataSelecionada]
  );

  const onChangeData = async (data, form) => {
    if (modoEdicao) {
      const confirmarParaSalvar = await pergutarParaSalvar();
      if (confirmarParaSalvar) {
        const salvoComSucesso = await validaAntesDoSubmit(form);
        if (salvoComSucesso) {
          await validaSeTemIdAula(data, form);
          setDataSelecionada(data);
        }
      } else {
        await validaSeTemIdAula(data, form);
        setDataSelecionada(data);
      }
    } else {
      await validaSeTemIdAula(data, form);
      setDataSelecionada(data);
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao && valoresIniciais && valoresIniciais.aulaId) {
      setModoEdicao(true);
    }
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        form.resetForm();
        setModoEdicao(false);
      }
    }
  };

  const onClickVoltar = async form => {
    if (modoEdicao && turmaInfantil && !desabilitarCampos) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        const salvou = await validaAntesDoSubmit(form);
        if (salvou) {
          history.push(URL_HOME);
        }
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onCloseErros = () => {
    setErrosValidacao([]);
    setMostarErros(false);
  };

  const onClickFecharModal = () => {
    setExibirModalSelecionarAula(false);
  };

  const onClickSelecionarAula = aula => {
    setExibirModalSelecionarAula(false);
    if (aula) {
      setAulaSelecionada(aula);
      obterDiarioBordo(aula.aulaId);
    }
  };

  return (
    <Loader loading={carregandoGeral} className="w-100 my-2">
      {!turmaSelecionada.turma ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'diario-bordo-selecione-turma',
            mensagem: 'Você precisa escolher uma turma',
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      {turmaSelecionada.turma ? <AlertaPermiteSomenteTurmaInfantil /> : ''}
      <AlertaPeriodoEncerrado exibir={!temPeriodoAberto && !somenteConsulta} />
      <ModalMultiLinhas
        key="erros-diario-bordo"
        visivel={mostrarErros}
        onClose={onCloseErros}
        type="error"
        conteudo={errosValidacao}
        titulo="Erros diário de bordo"
      />
      <ModalSelecionarAula
        visivel={exibirModalSelecionarAula}
        aulasParaSelecionar={aulasParaSelecionar}
        onClickFecharModal={onClickFecharModal}
        onClickSelecionarAula={onClickSelecionarAula}
      />
      <Cabecalho pagina="Diário de bordo" />
      <Card>
        <div className="col-md-12">
          <Formik
            enableReinitialize
            onSubmit={(v, form) => {
              salvarDiarioDeBordo(v, form);
            }}
            validationSchema={
              valoresIniciais && valoresIniciais.aulaId ? validacoes : {}
            }
            initialValues={valoresIniciais}
            validateOnBlur
            validateOnChange
            ref={refFormik => setRefForm(refFormik)}
          >
            {form => (
              <Form>
                <div className="row">
                  <div className="col-md-12 d-flex justify-content-end pb-4">
                    <Button
                      id="btn-voltar-ata-diario-bordo"
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-3"
                      onClick={() => onClickVoltar(form)}
                    />
                    <Button
                      id="btn-cancelar-ata-diario-bordo"
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-3"
                      onClick={() => onClickCancelar(form)}
                      disabled={!modoEdicao || desabilitarCampos}
                    />
                    <Button
                      id="btn-gerar-ata-diario-bordo"
                      label="Salvar"
                      color={Colors.Roxo}
                      border
                      bold
                      onClick={() => validaAntesDoSubmit(form, true)}
                      disabled={
                        !modoEdicao || !turmaInfantil || desabilitarCampos
                      }
                    />
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
                    <SelectComponent
                      id="disciplina"
                      name="disciplinaId"
                      lista={listaComponenteCurriculare || []}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      valueSelect={componenteCurricularSelecionado}
                      onChange={onChangeComponenteCurricular}
                      placeholder="Selecione um componente curricular"
                      disabled={
                        !turmaInfantil ||
                        (listaComponenteCurriculare &&
                          listaComponenteCurriculare.length === 1)
                      }
                    />
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
                    <CampoData
                      valor={dataSelecionada}
                      onChange={data => onChangeData(data, form)}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                      desabilitado={
                        !turmaInfantil ||
                        !listaComponenteCurriculare ||
                        !componenteCurricularSelecionado ||
                        !diasParaHabilitar
                      }
                      diasParaHabilitar={diasParaHabilitar}
                    />
                  </div>
                </div>
                <div className="row">
                  {turmaInfantil &&
                  componenteCurricularSelecionado &&
                  dataSelecionada &&
                  aulaSelecionada ? (
                    <>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                        <PainelCollapse defaultActiveKey="1">
                          <PainelCollapse.Painel
                            temBorda
                            header="Planejamento"
                            key="1"
                          >
                            <Editor
                              form={form}
                              name="planejamento"
                              id="editor-planejamento"
                              onChange={v => {
                                if (valoresIniciais.planejamento !== v) {
                                  onChangeCampos();
                                }
                              }}
                              desabilitar={desabilitarCampos}
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                        <PainelCollapse defaultActiveKey="2">
                          <PainelCollapse.Painel
                            temBorda
                            header="Reflexões e Replanejamentos"
                            key="2"
                          >
                            <Editor
                              form={form}
                              name="reflexoesReplanejamento"
                              id="editor-reflexoes-replanejamentos"
                              onChange={v => {
                                if (
                                  valoresIniciais.reflexoesReplanejamento !== v
                                ) {
                                  onChangeCampos();
                                }
                              }}
                              desabilitar={desabilitarCampos}
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <PainelCollapse>
                          <PainelCollapse.Painel temBorda header="Devolutivas">
                            <CampoTexto
                              id="editor-devolutivas"
                              name="devolutivas"
                              type="textarea"
                              form={form}
                              desabilitado
                            />
                          </PainelCollapse.Painel>
                        </PainelCollapse>
                      </div>
                    </>
                  ) : (
                    ''
                  )}
                  {auditoria ? (
                    <Auditoria
                      criadoEm={auditoria.criadoEm}
                      criadoPor={auditoria.criadoPor}
                      criadoRf={auditoria.criadoRf}
                      alteradoPor={auditoria.alteradoPor}
                      alteradoEm={auditoria.alteradoEm}
                      alteradoRf={auditoria.alteradoRf}
                    />
                  ) : (
                    ''
                  )}
                </div>
              </Form>
            )}
          </Formik>
        </div>
      </Card>
    </Loader>
  );
};

export default DiarioBordo;
