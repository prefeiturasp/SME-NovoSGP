import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import * as Yup from 'yup';
import { Loader, Localizador, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import DreDropDown from '~/componentes-sgp/DreDropDown/';
import UeDropDown from '~/componentes-sgp/UeDropDown/';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import { RotasDto } from '~/dtos';
import { confirmar } from '~/servicos';
import history from '~/servicos/history';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const DocumentosPlanosTrabalhoCadastro = ({ match }) => {
  const [carregandoAnos, setCarregandoAnos] = useState(false);
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);

  const [listaDres, setListaDres] = useState([]);

  const [listaUes, setListaUes] = useState([]);

  const [listaTipoDocumento, setListaTipoDocumento] = useState([]);

  const [listaClassificacao, setListaClassificacao] = useState([]);

  const [idDocumentosPlanoTrabalho, setIdDocumentosPlanoTrabalho] = useState(0);

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const [valoresIniciais, setValoresIniciais] = useState({
    anoLetivo: anoAtual,
    dreId: '',
    ueId: '',
    tipoDocumento: '',
    classificacaoDocumento: '',
    professorRf: '',
  });

  const [urlBuscarDres, setUrlBuscarDres] = useState();

  const [modoEdicao, setModoEdicao] = useState(false);

  const montarUrlBuscarDres = anoLetivo => {
    setUrlBuscarDres();
    setUrlBuscarDres(`v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`);
  };

  const validacoes = () => {
    return Yup.object({
      anoLetivo: Yup.string().required('Campo obrigatório'),
      dreId: Yup.string().required('Campo obrigatório'),
      ueId: Yup.string().required('Campo obrigatório'),
      tipoDocumento: Yup.string().required('Campo obrigatório'),
      classificacaoDocumento: Yup.string().required('Campo obrigatório'),
      professorRf: Yup.string().required('Campo obrigatório'),
    });
  };

  useEffect(() => {
    if (match?.params?.id) {
      setIdDocumentosPlanoTrabalho(match.params.id);
    }
  }, [match]);

  const atualizaValoresIniciais = useCallback(
    (nomeCampo, valor) => {
      valoresIniciais[nomeCampo] = valor;
      setValoresIniciais(valoresIniciais);
    },
    [valoresIniciais]
  );

  const obterAnosLetivos = useCallback(async () => {
    setCarregandoAnos(true);
    let anosLetivos = [];

    const anosLetivoComHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: true,
    });
    const anosLetivoSemHistorico = await FiltroHelper.obterAnosLetivos({
      consideraHistorico: false,
    });

    anosLetivos = anosLetivos.concat(anosLetivoComHistorico);

    anosLetivoSemHistorico.forEach(ano => {
      if (!anosLetivoComHistorico.find(a => a.valor === ano.valor)) {
        anosLetivos.push(ano);
      }
    });

    if (!anosLetivos.length) {
      anosLetivos.push({
        desc: anoAtual,
        valor: anoAtual,
      });
    }

    if (anosLetivos && anosLetivos.length) {
      const temAnoAtualNaLista = anosLetivos.find(
        item => String(item.valor) === String(anoAtual)
      );
      if (temAnoAtualNaLista) {
        atualizaValoresIniciais('anoLetivo', anoAtual);
        montarUrlBuscarDres(anoAtual);
      } else {
        atualizaValoresIniciais('anoLetivo', String(anosLetivos[0].valor));
        montarUrlBuscarDres(String(anosLetivos[0].valor));
      }
    }

    setListaAnosLetivo(anosLetivos);
    setCarregandoAnos(false);
  }, [anoAtual, atualizaValoresIniciais]);

  const obterTiposDocumento = () => {
    // TODO MOCK!
    const mockTipoDocumento = [
      {
        valor: '1',
        desc: 'Documento',
        classificacao: [
          { valor: 'PEA ', desc: 'PEA ' },
          { valor: 'PPP', desc: 'PPP' },
        ],
      },
      {
        valor: '2',
        desc: 'Plano de Trabalho',
        classificacao: [
          { valor: 'PAEE', desc: 'PAEE' },
          { valor: 'PAP', desc: 'PAP' },
          { valor: 'POA', desc: 'POA' },
          { valor: 'POED', desc: 'POED' },
          { valor: 'POEI', desc: 'POEI' },
          { valor: 'POSL', desc: 'POSL' },
        ],
      },
    ];
    setListaTipoDocumento(mockTipoDocumento);
  };

  useEffect(() => {
    obterAnosLetivos();
    obterTiposDocumento();
  }, [obterAnosLetivos]);

  const onChangeAnoLetivo = form => {
    form.setFieldValue('dreId', '');
    form.setFieldValue('ueId', '');
    setModoEdicao(true);
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja voltar para tela de listagem agora?'
      );
      if (confirmado) {
        history.push(RotasDto.DOCUMENTOS_PLANOS_TRABALHO);
      }
    }
  };

  const onChangeTipoDocumento = (tipo, form) => {
    let classificacaoPorTipo = [];
    if (tipo) {
      const lista = listaTipoDocumento.find(item => item.valor === tipo);
      classificacaoPorTipo = lista.classificacao;
    }
    setListaClassificacao(classificacaoPorTipo);

    if (classificacaoPorTipo?.length === 1) {
      form.setFieldValue('classificacaoDocumento', classificacaoPorTipo[0]);
    } else {
      form.setFieldValue('classificacaoDocumento', '');
    }
    setModoEdicao(true);
  };

  const onSubmitFormulario = form => {
    console.log(form);
  };

  const resetarTela = form => {
    form.resetForm();
    setModoEdicao(false);
    montarUrlBuscarDres(valoresIniciais.anoLetivo);
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

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.submitForm(form);
      }
    });
  };

  return (
    <>
      <Cabecalho pagina="Upload de documentos e planos de trabalho" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={onSubmitFormulario}
          validateOnBlur
          validateOnChange
        >
          {form => (
            <Form>
              <div className="col-md-12">
                <div className="row">
                  <div className="col-md-12 d-flex justify-content-end pb-4 justify-itens-end">
                    <Button
                      id="btn-voltar"
                      label="Voltar"
                      icon="arrow-left"
                      color={Colors.Azul}
                      border
                      className="mr-2"
                      onClick={onClickVoltar}
                    />
                    <Button
                      id="btn-cancelar"
                      label="Cancelar"
                      color={Colors.Roxo}
                      border
                      className="mr-2"
                      onClick={() => onClickCancelar(form)}
                      disabled={!modoEdicao}
                    />
                    <Button
                      id="btn-novo"
                      label={
                        idDocumentosPlanoTrabalho ? 'Alterar' : 'Cadastrar'
                      }
                      color={Colors.Roxo}
                      border
                      bold
                      onClick={() => validaAntesDoSubmit(form)}
                      disabled={!modoEdicao}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
                    <Loader loading={carregandoAnos} tip="">
                      <SelectComponent
                        id="select-ano-letivo"
                        label="Ano Letivo"
                        lista={listaAnosLetivo}
                        valueOption="valor"
                        valueText="desc"
                        disabled={
                          listaAnosLetivo && listaAnosLetivo.length === 1
                        }
                        onChange={() => onChangeAnoLetivo(form)}
                        placeholder="Ano letivo"
                        form={form}
                        name="anoLetivo"
                        allowClear={false}
                      />
                    </Loader>
                  </div>
                </div>
                <div className="row">
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                    <DreDropDown
                      id="select-dre"
                      label="Diretoria Regional de Educação (DRE)"
                      url={urlBuscarDres}
                      form={form}
                      onChange={(_, lista, changeManual) => {
                        setListaDres(lista);
                        if (changeManual) {
                          setModoEdicao(true);
                        }
                      }}
                    />
                  </div>
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                    <UeDropDown
                      id="select-ue"
                      label="Unidade Escolar (UE)"
                      dreId={form.values.dreId}
                      form={form}
                      url={`v1/abrangencias/false/dres/${form.values.dreId}/ues?anoLetivo=${form.values.anoLetivo}`}
                      temParametros
                      onChange={(_, lista, changeManual) => {
                        setListaUes(lista);
                        if (changeManual) {
                          setModoEdicao(true);
                        }
                      }}
                    />
                  </div>

                  <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 mb-2">
                    <SelectComponent
                      id="select-tipos-documento"
                      label="Tipo de documento"
                      lista={listaTipoDocumento}
                      valueOption="valor"
                      valueText="desc"
                      onChange={valor => onChangeTipoDocumento(valor, form)}
                      placeholder="Tipo de documento"
                      form={form}
                      name="tipoDocumento"
                    />
                  </div>
                  <div className="col-sm-12 col-md-6 col-lg-6 col-xl- mb-2">
                    <SelectComponent
                      id="select-classificacao-documento"
                      label="Classificação"
                      lista={listaClassificacao}
                      valueOption="valor"
                      valueText="desc"
                      onChange={() => setModoEdicao(true)}
                      placeholder="Classificação do documento"
                      form={form}
                      name="classificacaoDocumento"
                    />
                  </div>
                  <div className="col-md-12 mb-2">
                    <div className="row pr-3">
                      <Localizador
                        dreId={form.values.dreId}
                        anoLetivo={form.values.anoLetivo}
                        showLabel
                        form={form}
                        onChange={valor => {
                          const campos = Object.keys(valor);
                          const onChangeManual = campos.find(
                            item => item === 'professorNome'
                          );
                          if (onChangeManual === 'professorNome') {
                            setModoEdicao(true);
                          }
                        }}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

DocumentosPlanosTrabalhoCadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

DocumentosPlanosTrabalhoCadastro.defaultProps = {
  match: {},
};

export default DocumentosPlanosTrabalhoCadastro;
