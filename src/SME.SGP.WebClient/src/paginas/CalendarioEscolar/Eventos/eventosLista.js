import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import * as Yup from 'yup';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import history from '~/servicos/history';

const EventosLista = () => {
  const [listaCalendarioEscolar, setListaCalendarioEscolar] = useState([]);
  const [tipoCalendario, setTipoCalendario] = useState('');
  const [nomeEvento, setNomeEvento] = useState('');
  const [listaTipoEvento, setListaTipoEvento] = useState([]);
  const [tipoEvento, setTipoEvento] = useState([]);

  const [refForm, setRefForm] = useState();
  const [valoresIniciais] = useState({
    dataInicio: '',
    dataFim: '',
  });
  const [validacoes] = useState(
    Yup.object({
      dataInicio: momentSchema.test('validaInicio', 'Data obrigatória', function() {
        const dataInicio = this.parent['dataInicio'];
        const dataFim = this.parent['dataFim'];
        if (!dataInicio && dataFim) {
          return false;
        } else {
          return true;
        }
      }),
      dataFim: momentSchema.test('validaFim', 'Data obrigatória', function() {
        const dataInicio = this.parent['dataInicio'];
        const dataFim = this.parent['dataFim'];
        if (dataInicio && !dataFim) {
          return false;
        } else {
          return true;
        }
      })
    })
  );

  useEffect(() => {
    setListaTipoEvento([
      { id: 1, nome: 'Tipo evento 01' },
      { id: 2, nome: 'Tipo evento 02' },
      { id: 3, nome: 'Tipo evento 03' },
    ]);

    async function consultaTipoCalendario() {
      const tiposCalendario = await api.get('v1/tipo-calendario');
      if (
        tiposCalendario &&
        tiposCalendario.data &&
        tiposCalendario.data.length
      ) {
        tiposCalendario.data.map(item => {
          item.id = String(item.id);
          item.descricaoTipoCalendario = `${item.anoLetivo} - ${item.nome} - ${item.descricaoPeriodo}`;
        });
        setListaCalendarioEscolar(tiposCalendario.data);
      } else {
        setListaCalendarioEscolar([]);
      }
    }

    consultaTipoCalendario();
  }, []);

  useEffect(() => {
    validaFiltrar();
  }, [tipoCalendario, nomeEvento, tipoEvento]);

  const onClickVoltar = () => {
    console.log('onClickVoltar');
  };

  const onClickExcluir = () => {
    console.log('onClickExcluir');
  };

  const onClickNovo = () => {
    history.push('eventos/novo');
  };

  const onChangeTipoCalendario = tipo => {
    setTipoCalendario(tipo);
  };

  const onChangeNomeEvento = e => {
    setNomeEvento(e.target.value);
  };

  const onChangeTipoEvento = tipoEvento => {
    setTipoEvento(tipoEvento);
  };

  const onFiltrar = valoresForm => {
    // TODO - Chamar endpoint
    const params = {
      tipoCalendario,
      nomeEvento,
      tipoEvento,
      dataInicio: valoresForm.dataInicio,
      dataFim: valoresForm.dataFim,
    };
    console.log(params);
  };

  const validaFiltrar = () => {
    if (refForm) {
      refForm.validateForm().then(() => refForm.handleSubmit(e => e));
    }
  };

  return (
    <>
      <Cabecalho pagina="Evento do Calendário Escolar" />
      <Card>
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
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mr-2"
            onClick={onClickExcluir}
          />
          <Button
            label="Novo"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickNovo}
          />
        </div>

        <Formik
          ref={refFormik => setRefForm(refFormik)}
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={valores => onFiltrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row">
                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 pb-2">
                  <SelectComponent
                    label="Tipo Calendário"
                    name="select-tipo-calendario"
                    id="select-tipo-calendario"
                    lista={listaCalendarioEscolar}
                    valueOption="id"
                    valueText="nome"
                    onChange={onChangeTipoCalendario}
                    valueSelect={tipoCalendario || []}
                    placeholder="Selecione um calendário"
                  />
                </div>
                <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 pb-2">
                  <CampoTexto
                    label="Nome do evento"
                    placeholder="Digite o nome do evento"
                    onChange={onChangeNomeEvento}
                    value={nomeEvento}
                  />
                </div>
                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2">
                  <SelectComponent
                    label="Tipo Evento"
                    name="select-tipo-evento"
                    id="select-tipo-evento"
                    lista={listaTipoEvento}
                    valueOption="id"
                    valueText="nome"
                    onChange={onChangeTipoEvento}
                    valueSelect={tipoEvento || []}
                    placeholder="Selecione um tipo"
                  />
                </div>

                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2">
                  <CampoData
                    label="Data início"
                    formatoData="DD/MM/YYYY"
                    name="dataInicio"
                    onChange={validaFiltrar}
                    placeholder="Data início"
                    form={form}
                  />
                </div>
                <div className="col-sm-12 col-md-2 col-lg-2 col-xl-2 pb-2">
                  <CampoData
                    label="Data fim"
                    formatoData="DD/MM/YYYY"
                    name="dataFim"
                    onChange={validaFiltrar}
                    placeholder="Data fim"
                    form={form}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default EventosLista;
