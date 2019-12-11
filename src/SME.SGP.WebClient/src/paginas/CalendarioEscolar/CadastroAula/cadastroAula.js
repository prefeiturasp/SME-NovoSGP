import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import RadioGroupButton from '~/componentes/radioGroupButton';
import SelectComponent from '~/componentes/select';
import { confirmar, erros, sucesso, erro } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import RotasDTO from '~/dtos/rotasDto';
import { ModalConteudoHtml } from '~/componentes';

const CadastroAula = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const permissaoTela = useSelector(
    store => store.usuario.permissoes[RotasDTO.CALENDARIO_PROFESSOR]
  );
  const diaAula = useSelector(
    store => store.calendarioProfessor.diaSelecionado
  );
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const ueId = turmaSelecionada ? turmaSelecionada.unidadeEscolar : 0;

  const [dataAula, setDataAula] = useState();
  const [idAula, setIdAula] = useState(0);
  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(!match.params.id);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [validacoes, setValidacoes] = useState({});
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [quantidadeMaximaAulas, setQuantidadeMaximaAulas] = useState(0);
  const [controlaQuantidadeAula, setControlaQuantidadeAula] = useState(true);
  const [refForm, setRefForm] = useState({});
  const [refFormRecorrencia, setRefFormRecorrencia] = useState({});
  const [ehReposicao, setEhReposicao] = useState(false);
  const [quantidadeRecorrencia, setQuantidadeRecorrencia] = useState(0);
  const [
    visualizarFormExcRecorrencia,
    setVisualizarFormExcRecorrencia,
  ] = useState(false);

  const [valoresIniciais, setValoresIniciais] = useState({});
  const inicial = {
    tipoAula: 1,
    disciplinaId: undefined,
    quantidadeTexto: '',
    quantidadeRadio: 0,
    dataAula: '',
    recorrenciaAula: '',
    quantidade: 0,
    tipoCalendarioId: '',
    ueId: '',
    turmaId: '',
    dataAulaCompleta: window.moment(diaAula),
  };

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
  ];

  const opcoesQuantidadeAulas = [
    {
      label: '1',
      value: 1,
      disabled: quantidadeMaximaAulas < 1 && controlaQuantidadeAula,
    },
    {
      label: '2',
      value: 2,
      disabled: quantidadeMaximaAulas < 2 && controlaQuantidadeAula,
    },
  ];

  const recorrencia = {
    AULA_UNICA: 1,
    REPETIR_BIMESTRE_ATUAL: 2,
    REPETIR_TODOS_BIMESTRES: 3,
  };

  const [opcoesRecorrencia, setOpcoesRecorrencia] = useState([
    { label: 'Aula única', value: recorrencia.AULA_UNICA },
    {
      label: 'Repetir no Bimestre atual',
      value: recorrencia.REPETIR_BIMESTRE_ATUAL,
    },
    {
      label: 'Repetir em todos os Bimestres',
      value: recorrencia.REPETIR_TODOS_BIMESTRES,
    },
  ]);

  const [opcoesExcluirRecorrencia, setOpcoesExcluirRecorrencia] = useState([
    { label: 'Somente o dia', value: 1 },
    { label: 'Bimestre atual', value: 2 },
    { label: 'Todos os bimestres', value: 3 },
  ]);

  const valoresIniciaisExclusao = {
    tipoRecorrenciaExclusao: recorrencia.AULA_UNICA,
  };

  useEffect(() => {
    const obterDisciplinas = async () => {
      const disciplinas = await api.get(
        `v1/professores/${usuario.rf}/turmas/${turmaId}/disciplinas`
      );
      setListaDisciplinas(disciplinas.data);

      if (disciplinas.data && disciplinas.data.length == 1) {
        inicial.disciplinaId = String(
          disciplinas.data[0].codigoComponenteCurricular
        );
        if (Object.keys(refForm).length > 0) {
          onChangeDisciplinas(
            disciplinas.data[0].codigoComponenteCurricular,
            refForm
          );
        }
      }
      if (novoRegistro) {
        setValoresIniciais(inicial);
      }
    };
    if (turmaId) {
      obterDisciplinas();
      validarConsultaModoEdicaoENovo();
    }
  }, [refForm]);

  useEffect(() => {
    montaValidacoes();
  }, []);

  const montaValidacoes = (quantidadeRadio, quantidadeTexto, form) => {
    const validacaoQuantidade = Yup.number()
      .positive('Valor inválido')
      .integer();
    const val = {
      tipoAula: Yup.string().required('Tipo obrigatório'),
      disciplinaId: Yup.string().required('Disciplina obrigatório'),
      dataAula: momentSchema.required('Hora obrigatória'),
      dataAulaCompleta: momentSchema.required('Data obrigatória'),
      recorrenciaAula: Yup.string().required('Recorrência obrigatória'),
      quantidadeTexto: controlaQuantidadeAula
        ? validacaoQuantidade.lessThan(
            quantidadeMaximaAulas + 1,
            `Valor não pode ser maior que ${quantidadeMaximaAulas}`
          )
        : validacaoQuantidade,
    };

    if (quantidadeRadio > 0) {
      quantidadeRadio = Yup.string().required('Quantidade obrigatória');
      form.setFieldValue('quantidadeTexto', '');
    } else if (quantidadeTexto > 0) {
      form.setFieldValue('quantidadeRadio', '');
    } else {
      quantidadeRadio = Yup.string().required('Quantidade obrigatória');
      if (form) {
        form.setFieldValue('quantidadeTexto', '');
      }
    }

    setValidacoes(Yup.object(val));
  };

  const validarConsultaModoEdicaoENovo = async () => {
    setBreadcrumbManual(
      match.url,
      'Cadastro de Aula',
      '/calendario-escolar/calendario-professor'
    );

    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setIdAula(match.params.id);
      consultaPorId(match.params.id);
    } else {
      setNovoRegistro(true);
      setDataAula(window.moment(diaAula));
      // TODO
    }
  };

  const consultaPorId = async id => {
    const aula = await api
      .get(`v1/calendarios/professores/aulas/${id}`)
      .catch(e => {
        if (
          e &&
          e.response &&
          e.response.data &&
          Array.isArray(e.response.data)
        ) {
          erros(e);
        }
      });
    setNovoRegistro(false);
    if (aula && aula.data) {
      setDataAula(window.moment(aula.data.dataAula));
      const respRecorrencia = await api.get(
        `v1/calendarios/professores/aulas/${id}/recorrencias/serie`
      );
      const dadosRecorrencia = respRecorrencia.data;
      if (
        respRecorrencia &&
        dadosRecorrencia &&
        dadosRecorrencia.recorrenciaAula !== recorrencia.AULA_UNICA
      ) {
        setQuantidadeRecorrencia(dadosRecorrencia.quantidadeAulasRecorrentes);
        setOpcoesRecorrencia([
          ...getRecorrenciasHabilitadas(opcoesRecorrencia, dadosRecorrencia),
        ]);
        setOpcoesExcluirRecorrencia([
          ...getRecorrenciasHabilitadas(
            opcoesExcluirRecorrencia,
            dadosRecorrencia
          ),
        ]);
      }
      const val = {
        tipoAula: aula.data.tipoAula,
        disciplinaId: String(aula.data.disciplinaId),
        dataAula: aula.data.dataAula ? window.moment(aula.data.dataAula) : '',
        recorrenciaAula: recorrencia.AULA_UNICA,
        id: aula.data.id,
        tipoCalendarioId: aula.data.tipoCalendarioId,
        ueId: aula.data.ueId,
        turmaId: aula.data.turmaId,
        dataAulaCompleta: window.moment(aula.data.dataAula),
      };
      if (aula.data.quantidade > 0 && aula.data.quantidade < 3) {
        val.quantidadeRadio = aula.data.quantidade;
        val.quantidadeTexto = '';
      } else if (aula.data.quantidade > 0 && aula.data.quantidade > 2) {
        val.quantidadeTexto = aula.data.quantidade;
      }
      setValoresIniciais(val);
      setAuditoria({
        criadoPor: aula.data.criadoPor,
        criadoRf: aula.data.criadoRF > 0 ? aula.data.criadoRF : '',
        criadoEm: aula.data.criadoEm,
        alteradoPor: aula.data.alteradoPor,
        alteradoRf: aula.data.alteradoRF > 0 ? aula.data.alteradoRF : '',
        alteradoEm: aula.data.alteradoEm,
      });
      setExibirAuditoria(true);
    }
  };

  const getRecorrenciasHabilitadas = (opcoesRecorrencia, dadosRecorrencia) => {
    opcoesRecorrencia.forEach(item => {
      if (
        item.value === dadosRecorrencia.recorrenciaAula ||
        item.value === recorrencia.AULA_UNICA
      ) {
        item.disabled = false;
      } else {
        item.disabled = true;
      }
    });
    return opcoesRecorrencia;
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        resetarTela(form);
      }
    }
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?',
        'Sim',
        'Não'
      );

      if (confirmado) {
        onClickCadastrar(refForm.state.values);
      } else {
        history.push('/calendario-escolar/calendario-professor');
      }
    } else {
      history.push('/calendario-escolar/calendario-professor');
    }
  };

  const resetarTela = form => {
    form.resetForm();
    setControlaQuantidadeAula(true);
    setQuantidadeMaximaAulas(0);
    setModoEdicao(false);
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onChangeDisciplinas = async (id, form) => {
    onChangeCampos();
    let quantidade = 0;
    form.setFieldValue('quantidadeTexto', '');
    const resultado = await api.get(
      `v1/grades/aulas/turmas/${turmaId}/disciplinas/${id}`,
      {
        params: {
          data: dataAula ? dataAula.format('YYYY-MM-DD') : '',
        },
      }
    );
    if (resultado) {
      if (resultado.status === 200) {
        setControlaQuantidadeAula(true);
        quantidade = resultado.data.quantidadeAulasRestante;
        await setQuantidadeMaximaAulas(quantidade);
        if (quantidade > 0) {
          form.setFieldValue('quantidadeRadio', 1);
        } else {
          form.setFieldValue('quantidadeRadio', '');
          form.setFieldValue('quantidadeTexto', '');
        }
      } else if (resultado.status === 204) {
        setControlaQuantidadeAula(false);
      }
    }
    quantidade > 0 ? montaValidacoes(1, 0, form) : montaValidacoes(0, 1, form);
  };

  const onClickCadastrar = async valoresForm => {
    if (
      quantidadeRecorrencia > 1 &&
      valoresForm.recorrenciaAula !== recorrencia.AULA_UNICA
    ) {
      const confirmado = await confirmar(
        'Atenção',
        '',
        `Você tem certeza que deseja alterar ${quantidadeRecorrencia} ocorrências desta aula a partir desta data?`,
        'Sim',
        'Não'
      );
      if (confirmado) {
        await salvar(valoresForm);
        history.push('/calendario-escolar/calendario-professor');
      }
    } else {
      await salvar(valoresForm);
    }
  };

  const salvar = async valoresForm => {
    const data = valoresForm.dataAulaCompleta.format('YYYY-MM-DD');
    const hora = valoresForm.dataAula.format('HH:mm:SS');
    valoresForm.dataAula = window.moment(`${data}T${hora}`).format();

    if (valoresForm.quantidadeRadio && valoresForm.quantidadeRadio > 0) {
      valoresForm.quantidade = valoresForm.quantidadeRadio;
    } else if (valoresForm.quantidadeTexto && valoresForm.quantidadeTexto > 0) {
      valoresForm.quantidade = valoresForm.quantidadeTexto;
    }

    if (novoRegistro) {
      valoresForm.tipoCalendarioId = match.params.tipoCalendarioId;
      valoresForm.ueId = ueId;
      valoresForm.turmaId = turmaId;
      valoresForm.dataAula = dataAula;
    }

    const cadastrado = idAula
      ? await api
          .put(`v1/calendarios/professores/aulas/${idAula}`, valoresForm)
          .then(resp => resp)
          .catch(err => err)
      : await api
          .post('v1/calendarios/professores/aulas', valoresForm)
          .then(resp => resp)
          .catch(err => err);

    if (cadastrado && cadastrado.status === 200) {
      if (cadastrado.data) sucesso(cadastrado.data.mensagens[0]);
      history.push('/calendario-escolar/calendario-professor');
    }

    if (cadastrado && cadastrado.response) {
      erro(
        cadastrado.response.status === 601
          ? cadastrado.response.data.mensagens
          : 'Houve uma falha ao salvar a aula, por favor contate o suporte'
      );
    }
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      if (quantidadeRecorrencia > 1) {
        setVisualizarFormExcRecorrencia(true);
      } else {
        const confirmado = await confirmar(
          `Excluir aula  - ${dataAula.format('dddd')}, ${dataAula.format(
            'DD/MM/YYYY'
          )} `,
          'Você tem certeza que deseja excluir esta aula?',
          'Deseja continuar?',
          'Excluir',
          'Cancelar'
        );
        if (confirmado) {
          excluir(recorrencia.AULA_UNICA);
        }
      }
    }
  };

  const excluir = async tipoRecorrencia => {
    const excluir = await api
      .delete(
        `v1/calendarios/professores/aulas/${idAula}/recorrencias/${tipoRecorrencia}`
      )
      .catch(e => erros(e));
    if (excluir) {
      if (tipoRecorrencia === recorrencia.AULA_UNICA) {
        sucesso('Aula excluída com sucesso.');
      } else {
        if (excluir.status === 200) sucesso(excluir.data.mensagens[0]);
      }
      history.push('/calendario-escolar/calendario-professor');
    }
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(inicial);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const getDataFormatada = () => {
    const titulo = `${dataAula ? dataAula.format('dddd') : ''}, ${
      dataAula ? dataAula.format('DD/MM/YYYY') : ''
    } `;
    return titulo;
  };

  return (
    <>
      <Cabecalho pagina={`Cadastro de Aula - ${getDataFormatada()}`} />
      <Card>
        <ModalConteudoHtml
          key="reiniciarSenha"
          visivel={visualizarFormExcRecorrencia}
          onConfirmacaoPrincipal={() =>
            excluir(refFormRecorrencia.state.values.tipoRecorrenciaExclusao)
          }
          onConfirmacaoSecundaria={() => setVisualizarFormExcRecorrencia(false)}
          onClose={() => {}}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          titulo={`Excluir aula - ${getDataFormatada()}`}
          closable={false}
        >
          <Formik
            enableReinitialize
            initialValues={valoresIniciaisExclusao}
            validationSchema={validacoes}
            ref={refFormik => setRefFormRecorrencia(refFormik)}
            onSubmit={() => {}}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <div className="row justify-content-start">
                  <div
                    className="col-sm-12 col-md-12"
                    style={{ paddingTop: '10px' }}
                  >
                    <p>{`Essa aula se repete por ${quantidadeRecorrencia}${
                      quantidadeRecorrencia > 1 ? ' vezes' : ' vez'
                    } em seu planejamento.`}</p>
                    <p>Qual opção de exclusão você deseja realizar?</p>
                  </div>
                  <div className="col-sm-12 col-md-12 d-block">
                    <RadioGroupButton
                      form={form}
                      id="tipo-recorrencia-exclusao"
                      label="Realizar exclusão"
                      opcoes={opcoesExcluirRecorrencia}
                      name="tipoRecorrenciaExclusao"
                      onChange={() => {}}
                    />
                  </div>
                </div>
              </Form>
            )}
          </Formik>
        </ModalConteudoHtml>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          ref={refFormik => setRefForm(refFormik)}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row pb-3">
                <div className="col-md-2 pb-2 d-flex justify-content-start">
                  <CampoData
                    form={form}
                    placeholder="Data da aula"
                    formatoData="DD/MM/YYYY"
                    label=""
                    name="dataAulaCompleta"
                    onChange={onChangeCampos}
                  />
                </div>
                <div className="col-md-10 pb-2 d-flex justify-content-end">
                  <Button
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={onClickVoltar}
                  />
                  <Button
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    className="mr-2"
                    onClick={() => onClickCancelar(form)}
                    disabled={!modoEdicao}
                  />
                  <Button
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    hidden={novoRegistro}
                    onClick={onClickExcluir}
                  />
                  <Button
                    label={novoRegistro ? 'Cadastrar' : 'Alterar'}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    disabled={
                      (novoRegistro && !permissaoTela.podeIncluir) ||
                      (!novoRegistro && !permissaoTela.podeAlterar)
                    }
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-12 col-md-5 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    desabilitado={!novoRegistro}
                    id="tipo-aula"
                    label="Tipo de aula"
                    form={form}
                    opcoes={opcoesTipoAula}
                    name="tipoAula"
                    onChange={e => {
                      setEhReposicao(e.target.value === 2);
                      setValoresIniciais({
                        ...valoresIniciais,
                        tipoAula: e.target.value,
                        recorrenciaAula: e.target.value === 2 ? 1 : '',
                      });
                      onChangeCampos();
                      montaValidacoes(0, e.target.value, form);
                      setControlaQuantidadeAula(ehReposicao);
                    }}
                  />
                </div>
                <div className="col-sm-12 col-md-7 col-lg-9 col-xl-6 mb-2">
                  <SelectComponent
                    id="disciplina"
                    form={form}
                    name="disciplinaId"
                    lista={listaDisciplinas}
                    valueOption="codigoComponenteCurricular"
                    valueText="nome"
                    onChange={e => onChangeDisciplinas(e, form)}
                    label="Disciplina"
                    placeholder="Disciplina"
                    disabled={
                      !!(
                        listaDisciplinas &&
                        listaDisciplinas.length &&
                        listaDisciplinas.length == 1
                      ) || !novoRegistro
                    }
                  />
                </div>
                <div className="col-sm-12 col-md-4 col-lg-4 col-xl-3 pb-2">
                  <CampoData
                    form={form}
                    label="Horário do início da aula"
                    placeholder="Formato 24 horas"
                    formatoData="HH:mm"
                    name="dataAula"
                    onChange={onChangeCampos}
                    somenteHora
                  />
                </div>
                <div className="col-sm-12 col-md-8 col-lg-8 col-xl-5 mb-2 d-flex justify-content-start">
                  <RadioGroupButton
                    id="quantidade-aulas"
                    label="Quantidade de Aulas"
                    form={form}
                    opcoes={opcoesQuantidadeAulas}
                    name="quantidadeRadio"
                    onChange={e => {
                      onChangeCampos();
                      montaValidacoes(e.target.value, 0, form);
                    }}
                    className="text-nowrap"
                  />
                  <div className="mt-4 ml-2 mr-2 text-nowrap">
                    ou informe a quantidade
                  </div>
                  <CampoTexto
                    form={form}
                    name="quantidadeTexto"
                    className="mt-3"
                    style={{ width: '70px' }}
                    id="quantidadeTexto"
                    desabilitado={
                      quantidadeMaximaAulas < 3 && controlaQuantidadeAula
                    }
                    onChange={e => {
                      onChangeCampos();
                      montaValidacoes(0, e.target.value, form);
                    }}
                    icon
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-7 mb-2">
                  <RadioGroupButton
                    id="recorrencia"
                    label="Recorrência"
                    form={form}
                    opcoes={opcoesRecorrencia}
                    name="recorrenciaAula"
                    desabilitado={ehReposicao}
                    onChange={e => {
                      onChangeCampos();
                      montaValidacoes(
                        form.values.quantidadeRadio,
                        form.values.quantidadeTexto,
                        form
                      );
                    }}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        {exibirAuditoria ? (
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
      </Card>
    </>
  );
};

export default CadastroAula;
