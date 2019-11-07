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
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';

const CadastroAula = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
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

  const [valoresIniciais, setValoresIniciais] = useState({});
  const inicial = {
    tipoAula: 1,
    disciplinaId: '',
    quantidadeTexto: '',
    quantidadeRadio: 1,
    dataAula: '',
    recorrenciaAula: '',
    quantidade: 0,
    tipoCalendarioId: '',
    ueId: '',
    turmaId: '',
  };

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
  ];

  const opcoesQuantidadeAulas = [
    { label: '1', value: 1 },
    { label: '2', value: 2 },
  ];

  const opcoesRecorrencia = [
    { label: 'Aula única', value: 1 },
    { label: 'Repetir no Bimestre atual', value: 2 },
    { label: 'Repetir em todos os Bimestres', value: 3 },
  ];

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
      }
      if (novoRegistro) {
        setValoresIniciais(inicial);
      }
    };
    if (turmaId) {
      obterDisciplinas();
      validarConsultaModoEdicaoENovo();
    }
  }, []);

  useEffect(() => {
    montaValidacoes();
  }, []);

  const montaValidacoes = (quantidadeRadio, quantidadeTexto, form) => {
    const val = {
      tipoAula: Yup.string().required('Tipo obrigatório'),
      disciplinaId: Yup.string().required('Disciplina obrigatório'),
      dataAula: momentSchema.required('Hora obrigatória'),
      recorrenciaAula: Yup.string().required('Recorrência obrigatória'),
      quantidadeTexto: Yup.number()
        .positive('Valor inválido')
        .integer(),
    };

    if (quantidadeRadio > 0) {
      quantidadeRadio = Yup.string().required('Quantidade obrigatória');
      form.setFieldValue('quantidadeTexto', '');
    } else if (quantidadeTexto > 0) {
      form.setFieldValue('quantidadeRadio', '');
    } else {
      quantidadeRadio = Yup.string().required('Quantidade obrigatória');
      if (form) {
        form.setFieldValue('quantidadeRadio', 1);
        form.setFieldValue('quantidadeTexto', '');
      }
    }

    setValidacoes(Yup.object(val));
  };

  const validarConsultaModoEdicaoENovo = async () => {
    if (match && match.params && match.params.id) {
      setNovoRegistro(false);
      setBreadcrumbManual(
        match.url,
        'Cadastro de Aula',
        '/calendario-escolar/calendario-professor'
      );
      setIdAula(match.params.id);
      consultaPorId(match.params.id);
    } else {
      setNovoRegistro(true);
      setDataAula(window.moment());
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
      const val = {
        tipoAula: aula.data.tipoAula,
        disciplinaId: String(aula.data.disciplinaId),
        dataAula: aula.data.dataAula ? window.moment(aula.data.dataAula) : '',
        recorrenciaAula: aula.data.recorrenciaAula,
        id: aula.data.id,
        tipoCalendarioId: aula.data.tipoCalendarioId,
        ueId: aula.data.ueId,
        turmaId: aula.data.turmaId,
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
        // TODO Salvar
        history.push('/calendario-escolar/calendario-professor');
      }
    } else {
      history.push('/calendario-escolar/calendario-professor');
    }
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const onClickCadastrar = async valoresForm => {
    if (valoresForm.quantidadeRadio && valoresForm.quantidadeRadio > 0) {
      valoresForm.quantidade = valoresForm.quantidadeRadio;
    } else if (valoresForm.quantidadeTexto && valoresForm.quantidadeTexto > 0) {
      valoresForm.quantidade = valoresForm.quantidadeTexto;
    }

    if (novoRegistro) {
      valoresForm.tipoCalendarioId = match.params.tipoCalendarioId;
      valoresForm.ueId = ueId;
      valoresForm.turmaId = turmaId;
    }

    const cadastrado = idAula
      ? await api.put(`v1/calendarios/professores/aulas/${idAula}`, valoresForm)
      : await api
          .post('v1/calendarios/professores/aulas', valoresForm)
          .catch(e => erros(e));

    if (cadastrado && cadastrado.status == 200) {
      sucesso('Aula cadastrada com sucesso');
      // TODO - Voltar para o calendario quando ele existir!
      history.push('/calendario-escolar/calendario-professor');
    }
    console.log(valoresForm);
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
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
        const excluir = await api
          .delete(`v1/calendarios/professores/aulas/${idAula}`)
          .catch(e => erros(e));
        if (excluir) {
          sucesso('Aula excluída com sucesso.');
          // TODO - Voltar para o calendario quando ele existir!
          history.push('/calendario-escolar/calendario-professor');
        }
      }
    }
  };

  return (
    <>
      <Cabecalho
        pagina={`Cadastro de Aula - ${
          dataAula ? dataAula.format('dddd') : ''
        }, ${dataAula ? dataAula.format('DD/MM/YYYY') : ''} `}
      />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row pb-3">
                <div className="col-md-12 pb-2 d-flex justify-content-end">
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
                    type="submit"
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-12 col-md-5 col-lg-3 col-xl-3 mb-2">
                  <RadioGroupButton
                    id="tipo-aula"
                    label="Tipo de aula"
                    form={form}
                    opcoes={opcoesTipoAula}
                    name="tipoAula"
                    valorInicial
                    onChange={onChangeCampos}
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
                    onChange={onChangeCampos}
                    label="Disciplina"
                    placeholder="Disciplina"
                    disabled={
                      !!(
                        listaDisciplinas &&
                        listaDisciplinas.length &&
                        listaDisciplinas.length == 1
                      )
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
                    valorInicial
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
                    valorInicial
                    onChange={onChangeCampos}
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
