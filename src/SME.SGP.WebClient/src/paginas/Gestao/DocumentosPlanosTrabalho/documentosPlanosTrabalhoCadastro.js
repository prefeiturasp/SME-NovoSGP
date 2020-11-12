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
import { confirmar, erros, sucesso } from '~/servicos';
import history from '~/servicos/history';
import ServicoDocumentosPlanosTrabalho from '~/servicos/Paginas/Gestao/DocumentosPlanosTrabalho/ServicoDocumentosPlanosTrabalho';
import FiltroHelper from '~componentes-sgp/filtro/helper';

const DocumentosPlanosTrabalhoCadastro = ({ match }) => {
  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);

  const [listaDres, setListaDres] = useState([]);

  const [listaUes, setListaUes] = useState([]);

  const [listaTipoDocumento, setListaTipoDocumento] = useState([]);

  const [listaClassificacao, setListaClassificacao] = useState([]);

  const [idDocumentosPlanoTrabalho, setIdDocumentosPlanoTrabalho] = useState(0);

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const [valoresIniciais, setValoresIniciais] = useState({
    anoLetivo: null,
  });

  const [urlBuscarDres, setUrlBuscarDres] = useState();

  const [modoEdicao, setModoEdicao] = useState(false);

  const [exibirLoader, setExibirLoader] = useState(false);

  const [carregouAnosLetivos, setCarregouAnosLetivos] = useState(false);
  const [carregouTiposDocumento, setCarregouTiposDocumento] = useState(false);

  const montarUrlBuscarDres = anoLetivo => {
    setUrlBuscarDres();
    setUrlBuscarDres(`v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`);
  };

  const validacoes = () => {
    return Yup.object({
      anoLetivo: Yup.string().required('Campo obrigatório'),
      dreId: Yup.string().required('Campo obrigatório'),
      ueId: Yup.string().required('Campo obrigatório'),
      tipoDocumentoId: Yup.string().required('Campo obrigatório'),
      classificacaoId: Yup.string().required('Campo obrigatório'),
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
      if (!match?.params?.id) {
        valoresIniciais[nomeCampo] = valor;
        setValoresIniciais(valoresIniciais);
      }
    },
    [valoresIniciais, match]
  );

  const obterAnosLetivos = useCallback(async () => {
    setExibirLoader(true);
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
    setExibirLoader(false);
    setCarregouAnosLetivos(true);
  }, [anoAtual, atualizaValoresIniciais]);

  const obterTiposDocumento = useCallback(async () => {
    const resposta = await ServicoDocumentosPlanosTrabalho.obterTiposDeDocumentos().catch(
      e => erros(e)
    );

    if (resposta?.data?.length) {
      setListaTipoDocumento(resposta.data);
      if (resposta.data.length === 1) {
        const tipo = resposta.data[0];
        atualizaValoresIniciais('tipoDocumentoId', String(tipo.id));

        if (tipo.classificacoes.length === 1) {
          setListaClassificacao(tipo.classificacoes);
          const classificacao = tipo.classificacoes[0];
          atualizaValoresIniciais('classificacaoId', String(classificacao.id));
        }
      }
    }
    setCarregouTiposDocumento(true);
  }, [atualizaValoresIniciais]);

  useEffect(() => {
    obterAnosLetivos();
    obterTiposDocumento();
  }, [obterAnosLetivos, obterTiposDocumento]);

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
      const lista = listaTipoDocumento.find(
        item => String(item.id) === String(tipo)
      );
      classificacaoPorTipo = lista.classificacoes;
    }
    setListaClassificacao(classificacaoPorTipo);

    if (classificacaoPorTipo?.length === 1) {
      form.setFieldValue('classificacaoId', String(classificacaoPorTipo[0].id));
    } else {
      form.setFieldValue('classificacaoId', '');
    }
    setModoEdicao(true);
  };

  const onSubmitFormulario = async valores => {
    const { ueId, usuarioId, classificacaoId, tipoDocumentoId } = valores;

    const ueSelecionada = listaUes.find(
      item => String(item.valor) === String(ueId)
    );

    const params = {
      arquivoCodigo: '8c3b3eda-f61c-42db-b794-e960b66f6f9c',
      ueId: ueSelecionada.id,
      tipoDocumentoId,
      classificacaoId,
      usuarioId,
    };

    setExibirLoader(true);
    const resposta = await ServicoDocumentosPlanosTrabalho.salvarDocumento(
      params
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (resposta && resposta.status === 200) {
      sucesso('Registro salvo com sucesso');
      history.push(RotasDto.DOCUMENTOS_PLANOS_TRABALHO);
    }
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
    <Loader loading={exibirLoader}>
      <Cabecalho pagina="Upload de documentos e planos de trabalho" />
      {carregouAnosLetivos &&
      carregouTiposDocumento &&
      valoresIniciais.anoLetivo ? (
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
                        valueOption="id"
                        valueText="tipoDocumento"
                        onChange={valor => onChangeTipoDocumento(valor, form)}
                        placeholder="Tipo de documento"
                        form={form}
                        name="tipoDocumentoId"
                        disabled={listaTipoDocumento?.length === 1}
                      />
                    </div>
                    <div className="col-sm-12 col-md-6 col-lg-6 col-xl- mb-2">
                      <SelectComponent
                        id="select-classificacao-documento"
                        label="Classificação"
                        lista={listaClassificacao}
                        valueOption="id"
                        valueText="classificacao"
                        onChange={() => setModoEdicao(true)}
                        placeholder="Classificação do documento"
                        form={form}
                        name="classificacaoId"
                        disabled={listaClassificacao?.length === 1}
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
      ) : (
        ''
      )}
    </Loader>
  );
};

DocumentosPlanosTrabalhoCadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

DocumentosPlanosTrabalhoCadastro.defaultProps = {
  match: {},
};

export default DocumentosPlanosTrabalhoCadastro;
