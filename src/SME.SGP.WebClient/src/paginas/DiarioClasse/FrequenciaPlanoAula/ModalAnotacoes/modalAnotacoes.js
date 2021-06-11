import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import shortid from 'shortid';
import * as Yup from 'yup';
import { Auditoria, Colors, Loader, ModalConteudoHtml } from '~/componentes';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import Button from '~/componentes/button';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import SelectComponent from '~/componentes/select';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import ServicoAnotacaoFrequenciaAluno from '~/servicos/Paginas/DiarioClasse/ServicoAnotacaoFrequenciaAluno';
import { EditorAnotacao } from './modalAnotacoes.css';

const ModalAnotacoesFrequencia = props => {
  const {
    exibirModal,
    onCloseModal,
    dadosModalAnotacao,
    dadosListaFrequencia,
    ehInfantil,
    aulaId,
    componenteCurricularId,
    desabilitarCampos,
  } = props;

  const [showModal, setShowModal] = useState(exibirModal);
  const [carregandoMotivosAusencia, setCarregandoMotivosAusencia] = useState(
    exibirModal
  );
  const [listaMotivoAusencia, setListaMotivoAusencia] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    id: 0,
    anotacao: '',
    motivoAusenciaId: undefined,
    auditoria: {},
  });

  const [validacoes] = useState(
    Yup.object().shape(
      {
        anotacao: Yup.string()
          .nullable()
          .when('motivoAusenciaId', (motivoAusenciaId, schema) => {
            return motivoAusenciaId
              ? schema.notRequired()
              : schema.required('Anotação obrigatória');
          }),
        motivoAusenciaId: Yup.string()
          .nullable()
          .when('anotacao', (anotacao, schema) => {
            return anotacao
              ? schema.notRequired()
              : schema.required('Motivo ausência obrigatório');
          }),
      },
      ['motivoAusenciaId', 'anotacao']
    )
  );

  const [dadosEstudanteOuCrianca, setDadosEstudanteOuCrianca] = useState({});

  const obterAnotacao = useCallback(async () => {
    const resultado = await ServicoAnotacaoFrequenciaAluno.obterAnotacao(
      dadosModalAnotacao.codigoAluno,
      aulaId
    ).catch(e => erros(e));

    if (resultado && resultado.data) {
      resultado.data.motivoAusenciaId = resultado.data.motivoAusenciaId
        ? String(resultado.data.motivoAusenciaId)
        : undefined;
      setValoresIniciais(resultado.data);
    }
  }, [aulaId, dadosModalAnotacao]);

  const obterListaMotivosAusencia = async () => {
    const retorno = await ServicoAnotacaoFrequenciaAluno.obterMotivosAusencia().catch(
      e => erros(e)
    );
    if (retorno && retorno.data) {
      setListaMotivoAusencia(retorno.data);
    } else {
      setListaMotivoAusencia([]);
    }
    setCarregandoMotivosAusencia(false);
  };

  const montarDadosAluno = useCallback(() => {
    const aluno = {
      ...dadosModalAnotacao,
      nome: dadosModalAnotacao.nomeAluno,
      numeroChamada: dadosModalAnotacao.numeroAlunoChamada,
      dataNascimento: dadosModalAnotacao.dataNascimento,
      codigoEOL: dadosModalAnotacao.codigoAluno,
    };
    setDadosEstudanteOuCrianca(aluno);
  }, [dadosModalAnotacao]);

  useEffect(() => {
    if (dadosModalAnotacao) {
      obterAnotacao();
      montarDadosAluno();
      obterListaMotivosAusencia();
    }
  }, [dadosModalAnotacao, obterAnotacao, montarDadosAluno]);

  const fecharAposSalvarExcluir = (salvou, excluiu) => {
    const linhaEditada = dadosListaFrequencia.find(
      item => item.codigoAluno === dadosModalAnotacao.codigoAluno
    );
    const index = dadosListaFrequencia.indexOf(linhaEditada);
    if (salvou) {
      dadosListaFrequencia[index].possuiAnotacao = true;
    } else if (excluiu) {
      dadosListaFrequencia[index].possuiAnotacao = false;
    }
    onCloseModal();
  };

  const onClickExcluir = async id => {
    const retorno = await ServicoAnotacaoFrequenciaAluno.deletarAnotacao(
      id
    ).catch(e => erros(e));
    if (retorno && retorno.status === 200) {
      sucesso('Anotação excluída com sucesso');
      fecharAposSalvarExcluir(false, true);
    }
  };

  const onClickEditar = async valores => {
    const { anotacao, motivoAusenciaId, id } = valores;
    const params = {
      motivoAusenciaId,
      id,
      anotacao,
    };
    const retorno = await ServicoAnotacaoFrequenciaAluno.alterarAnotacao(
      params
    ).catch(e => erros(e));
    if (retorno && retorno.status === 200) {
      sucesso('Anotação alterada com sucesso');
      fecharAposSalvarExcluir(true, false);
    }
  };

  const onClickSalvar = async valores => {
    const { codigoAluno } = dadosModalAnotacao;
    const { anotacao, motivoAusenciaId } = valores;
    const params = {
      motivoAusenciaId,
      aulaId,
      componenteCurricularId,
      anotacao,
      codigoAluno,
      ehInfantil,
    };
    const retorno = await ServicoAnotacaoFrequenciaAluno.salvarAnotacao(
      params
    ).catch(e => {
      erros(e);
      onCloseModal();
    });
    if (retorno && retorno.status === 200) {
      sucesso('Anotação salva com sucesso');
      fecharAposSalvarExcluir(true, false);
    }
  };

  const validaAntesDoSubmit = form => {
    if (!desabilitarCampos) {
      const arrayCampos = Object.keys(valoresIniciais);
      arrayCampos.forEach(campo => {
        form.setFieldTouched(campo, true, true);
      });
      form.validateForm().then(() => {
        if (form.isValid || Object.keys(form.errors).length === 0) {
          form.handleSubmit(e => e);
        }
      });
    }
  };

  const validaAntesDeExcluir = async id => {
    if (!desabilitarCampos) {
      setShowModal(false);
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Você tem certeza que deseja excluir este registro?'
      );
      if (confirmado) {
        onClickExcluir(id);
      } else {
        setShowModal(true);
      }
    }
  };

  const validaAntesDeFechar = async () => {
    if (modoEdicao && !desabilitarCampos) {
      setShowModal(false);
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        if (refForm) {
          validaAntesDoSubmit(refForm.getFormikContext());
        }
      } else {
        onCloseModal();
      }
    } else {
      onCloseModal();
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  return dadosEstudanteOuCrianca ? (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="inserir-anotacao"
      visivel={showModal}
      titulo={`Anotações ${ehInfantil ? 'da criança' : 'do estudante'}`}
      onClose={() => validaAntesDeFechar()}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <Formik
        ref={f => setRefForm(f)}
        enableReinitialize
        initialValues={valoresIniciais}
        validationSchema={validacoes}
        onSubmit={valores => {
          if (valores.id) {
            onClickEditar(valores);
          } else {
            onClickSalvar(valores);
          }
        }}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <div className="col-md-12">
              <DetalhesAluno
                dados={dadosEstudanteOuCrianca}
                exibirBotaoImprimir={false}
                exibirFrequencia={false}
                permiteAlterarImagem={false}
              />
            </div>
            <div className="col-md-12 mt-2">
              <Loader loading={carregandoMotivosAusencia} tip="">
                <SelectComponent
                  form={form}
                  id="motivo-ausencia"
                  name="motivoAusenciaId"
                  lista={listaMotivoAusencia}
                  valueOption="valor"
                  valueText="descricao"
                  onChange={onChangeCampos}
                  placeholder="Selecione um motivo"
                  disabled={desabilitarCampos}
                />
              </Loader>
            </div>
            <div className="col-md-12 mt-2">
              <EditorAnotacao>
                <JoditEditor
                  form={form}
                  value={form.values.anotacao}
                  name="anotacao"
                  onChange={v => {
                    if (valoresIniciais.anotacao !== v) {
                      onChangeCampos();
                    }
                  }}
                  readonly={desabilitarCampos}
                />
              </EditorAnotacao>
            </div>
            <div className="row">
              <div
                className="col-md-12 d-flex justify-content-start"
                style={{ marginTop: '-15px' }}
              >
                {valoresIniciais &&
                valoresIniciais.auditoria &&
                valoresIniciais.auditoria.criadoPor ? (
                  <Auditoria
                    criadoPor={valoresIniciais.auditoria.criadoPor}
                    criadoEm={valoresIniciais.auditoria.criadoEm}
                    alteradoPor={valoresIniciais.auditoria.alteradoPor}
                    alteradoEm={valoresIniciais.auditoria.alteradoEm}
                    alteradoRf={valoresIniciais.auditoria.alteradoRF}
                    criadoRf={valoresIniciais.auditoria.criadoRF}
                  />
                ) : (
                  ''
                )}
              </div>
              <div className="col-md-12 d-flex justify-content-end">
                <Button
                  key="btn-voltar-anotacao"
                  id="btn-voltar-anotacao"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  onClick={validaAntesDeFechar}
                  className="mr-3 mt-2 padding-btn-confirmacao"
                />
                <Button
                  key="btn-excluir-anotacao"
                  id="btn-excluir-anotacao"
                  label="Excluir"
                  color={Colors.Vermelho}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={() => validaAntesDeExcluir(form.values.id)}
                  disabled={
                    desabilitarCampos ||
                    (dadosModalAnotacao && !dadosModalAnotacao.possuiAnotacao)
                  }
                />
                <Button
                  id="btn-salvar-anotacao"
                  key="btn-salvar-anotacao"
                  label={
                    valoresIniciais && valoresIniciais.id ? 'Alterar' : 'Salvar'
                  }
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={() => validaAntesDoSubmit(form)}
                  disabled={!modoEdicao || desabilitarCampos}
                />
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </ModalConteudoHtml>
  ) : (
    ''
  );
};

ModalAnotacoesFrequencia.propTypes = {
  exibirModal: PropTypes.bool,
  onCloseModal: PropTypes.func,
  dadosModalAnotacao: PropTypes.oneOfType([PropTypes.object]),
  dadosListaFrequencia: PropTypes.oneOfType([PropTypes.array]),
  ehInfantil: PropTypes.bool,
  aulaId: PropTypes.oneOfType([PropTypes.any]),
  componenteCurricularId: PropTypes.oneOfType([PropTypes.any]),
  desabilitarCampos: PropTypes.bool,
};

ModalAnotacoesFrequencia.defaultProps = {
  exibirModal: false,
  onCloseModal: () => {},
  dadosModalAnotacao: {},
  dadosListaFrequencia: [],
  ehInfantil: false,
  aulaId: '',
  componenteCurricularId: '',
  desabilitarCampos: false,
};

export default ModalAnotacoesFrequencia;
