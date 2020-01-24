import { DreDropDown, UeDropDown } from 'componentes-sgp';
import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import { CampoData, Loader, momentSchema } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import modalidade from '~/dtos/modalidade';
import api from '~/servicos/api';
import history from '~/servicos/history';

const PeriodoFechamentoReaberturaForm = () => {
  const usuarioStore = useSelector(store => store.usuario);

  const [listaTipoCalendarioEscolar, setListaTipoCalendarioEscolar] = useState(
    []
  );
  const [listaBimestres, setListaBimestres] = useState([]);
  const [desabilitarTipoCalendario, setDesabilitarTipoCalendario] = useState(
    false
  );
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [idFechamentoReabertura, setIdFechamentoReabertura] = useState(0);
  const [validacoes, setValidacoes] = useState({});
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [auditoria, setAuditoria] = useState([]);

  const [valoresIniciais, setValoresIniciais] = useState({
    tipoCalendarioId: undefined,
    dreId: undefined,
    ueId: undefined,
    dataInicio: '',
    dataFim: '',
    descricao: '',
    bimestres: [],
  });

  const montarListaBimestres = tipoModalidade => {
    const listaNova = [
      {
        valor: 1,
        descricao: 'Primeiro Bimestre',
      },
      {
        valor: 2,
        descricao: 'Segundo Bimestre',
      },
    ];

    if (tipoModalidade != modalidade.EJA) {
      listaNova.push(
        {
          valor: 3,
          descricao: 'Terceiro Bimestre',
        },
        {
          valor: 4,
          descricao: 'Quarto Bimestre',
        }
      );
    }

    listaNova.push({
      valor: 5,
      descricao: 'Todos',
    });
    setListaBimestres(listaNova);
  };

  useEffect(() => {
    async function consultaTipos() {
      setCarregandoTipos(true);
      const listaTipo = await api.get('v1/calendarios/tipos');
      if (listaTipo && listaTipo.data && listaTipo.data.length) {
        listaTipo.data.map(item => {
          item.id = String(item.id);
          item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
        });
        setListaTipoCalendarioEscolar(listaTipo.data);
        if (listaTipo.data.length === 1) {
          setDesabilitarTipoCalendario(true);
          const valores = {
            tipoCalendarioId: listaTipo.data[0].id,
            dreId: undefined,
            ueId: undefined,
            dataInicio: '',
            dataFim: '',
            descricao: '',
            bimestres: [],
          };
          setValoresIniciais(valores);
          montarListaBimestres(listaTipo.data[0].modalidade);
        } else {
          setDesabilitarTipoCalendario(false);
        }
      } else {
        setListaTipoCalendarioEscolar([]);
      }
      setCarregandoTipos(false);
    }
    consultaTipos();
  }, []);

  useEffect(() => {
    const montaValidacoes = () => {
      const val = {
        tipoCalendarioId: Yup.string().required('Calendário obrigatório'),
        descricao: Yup.string().required('Descrição obrigatória'),
        dataInicio: momentSchema.required('Data obrigatória'),
        dataFim: momentSchema.required('Data obrigatória'),
        bimestres: Yup.string().required('Bimestre obrigatório'),
      };

      if (!usuarioStore.possuiPerfilSme) {
        val.dreId = Yup.string().required('DRE obrigatória');
      }

      if (!usuarioStore.possuiPerfilSmeOuDre) {
        val.ueId = Yup.string().required('UE obrigatória');
      }

      setValidacoes(Yup.object(val));
    };

    montaValidacoes();
  }, [usuarioStore.possuiPerfilSme, usuarioStore.possuiPerfilSmeOuDre]);

  const onClickVoltar = () => {
    history.push('/calendario-escolar/periodo-fechamento-reabertura');
  };

  const onClickExcluir = () => {};
  const onClickCancelar = () => {};

  const obterBimestresSalvar = valoresForm => {
    const todosBimestres = valoresForm.bimestres.find(item => item == '5');
    if (todosBimestres) {
      const calendarioSelecionado = listaTipoCalendarioEscolar.find(
        item => item.id == valoresForm.tipoCalendarioId
      );
      if (
        calendarioSelecionado &&
        calendarioSelecionado.modalidade &&
        calendarioSelecionado.modalidade == modalidade.EJA
      ) {
        return ['1', '2'];
      }
      return ['1', '2', '3', '4'];
    }

    return valoresForm.bimestres;
  };
  const onClickCadastrar = valoresForm => {
    const prametrosParaSalvar = { ...valoresForm };
    prametrosParaSalvar.bimestres = obterBimestresSalvar(prametrosParaSalvar);

    console.log(prametrosParaSalvar);
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const onChangeTipoCalendario = (tipoId, form) => {
    if (tipoId) {
      const calendarioSelecionado = listaTipoCalendarioEscolar.find(
        item => item.id == tipoId
      );
      if (calendarioSelecionado) {
        montarListaBimestres(calendarioSelecionado.modalidade);
      }
    }
    form.setFieldValue('bimestres', []);
  };

  return (
    <>
      <Cabecalho pagina="Período de Fechamento (Reabertura)" />
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
            <Form className="col-md-12">
              <div className="row mb-4">
                <div className="col-md-12 d-flex justify-content-end pb-4">
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
                  />
                  <Button
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    onClick={onClickExcluir}
                  />
                  <Button
                    label={`${
                      idFechamentoReabertura > 0 ? 'Alterar' : 'Cadastrar'
                    }`}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <Loader loading={carregandoTipos} tip="">
                    <div style={{ maxWidth: '300px' }}>
                      <SelectComponent
                        label="Calendário"
                        form={form}
                        name="tipoCalendarioId"
                        id="tipoCalendarioId"
                        lista={listaTipoCalendarioEscolar}
                        valueOption="id"
                        valueText="descricaoTipoCalendario"
                        disabled={desabilitarTipoCalendario}
                        placeholder="Selecione um tipo de calendário"
                        onChange={valor => onChangeTipoCalendario(valor, form)}
                      />
                    </div>
                  </Loader>
                </div>
                <div className="col-sm-6 col-md-6 col-lg-6 col-xl-6 mb-2">
                  <DreDropDown
                    label="Diretoria Regional de Educação (DRE)"
                    form={form}
                    desabilitado={false}
                  />
                </div>
                <div className="col-sm-6 col-md-6 col-lg-6 col-xl-6 mb-2">
                  <UeDropDown
                    dreId={form.values.dreId}
                    label="Unidade Escolar (UE)"
                    form={form}
                    url="v1/dres"
                    desabilitado={false}
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <CampoTexto
                    label="Descrição"
                    name="descricao"
                    id="descricao"
                    type="textarea"
                    form={form}
                  />
                </div>
                <div className="col-sm-2 col-md-2 col-lg-2 col-xl-2 mb-2">
                  <CampoData
                    label="Início"
                    form={form}
                    name="dataInicio"
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                  />
                </div>
                <div className="col-sm-2 col-md-2 col-lg-2 col-xl-2 mb-2">
                  <CampoData
                    label="Fim"
                    form={form}
                    name="dataFim"
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                  />
                </div>
                <div className="col-sm-4 col-md-4 col-lg-4 col-xl-4 mb-2">
                  <SelectComponent
                    form={form}
                    label="Bimestre"
                    name="bimestres"
                    id="bimestres"
                    lista={listaBimestres}
                    onChange={valor => {
                      if (valor.includes('5')) {
                        form.setFieldValue('bimestres', ['5']);
                      }
                    }}
                    valueOption="valor"
                    valueText="descricao"
                    placeholder="Selecione bimestre(s)"
                    multiple
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
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
          />
        ) : (
          ''
        )}
      </Card>
    </>
  );
};

export default PeriodoFechamentoReaberturaForm;
