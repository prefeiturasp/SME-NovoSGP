import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { Label } from '~/componentes';
import tipoQuestao from '~/dtos/tipoQuestao';
import AtendimentoClinicoTabela from '~/paginas/Relatorios/AEE/Encaminhamento/Cadastro/Componentes/AtendimentoClinico/atendimentoClinicoTabela';
import UploadArquivosEncaminhamento from '~/paginas/Relatorios/AEE/Encaminhamento/Cadastro/Componentes/UploadArquivosEncaminhamento/uploadArquivosEncaminhamento';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import ServicoQuestionarioDinamico from '~/servicos/Componentes/ServicoQuestionarioDinamico';
import CampoDinamicoCombo from './Componentes/campoDinamicoCombo';
import CampoDinamicoComboMultiplaEscolha from './Componentes/campoDinamicoComboMultiplaEscolha';
import CampoDinamicoRadio from './Componentes/campoDinamicoRadio';
import CampoDinamicoTexto from './Componentes/campoDinamicoTexto';
import InformacoesEscolares from './Componentes/InformacoesEscolares/informacoesEscolares';
import QuestionarioDinamicoFuncoes from './Funcoes/QuestionarioDinamicoFuncoes';
import QuestionarioDinamicoValidacoes from './Validacoes/QuestionarioDinamicoValidacoes';

const QuestionarioDinamico = props => {
  const dispatch = useDispatch();

  const {
    dados,
    dadosQuestionarioAtual,
    desabilitarCampos,
    codigoAluno,
    codigoTurma,
    anoLetivo,
  } = props;

  const [valoresIniciais, setValoresIniciais] = useState();

  const [refForm, setRefForm] = useState({});

  const obterForm = () => refForm;

  useEffect(() => {
    if (refForm) {
      ServicoQuestionarioDinamico.adicionarFormsQuestionarioDinamico(
        () => obterForm(),
        dados.questionarioId,
        dadosQuestionarioAtual,
        dados.id
      );
    }
  }, [refForm]);

  const montarValoresIniciais = useCallback(() => {
    const valores = {};

    const montarDados = questaoAtual => {
      const resposta = questaoAtual?.resposta;

      let valorRespostaAtual = '';

      if (resposta?.length) {
        switch (questaoAtual?.tipoQuestao) {
          case tipoQuestao.Radio:
            valorRespostaAtual = resposta[0].opcaoRespostaId;
            break;
          case tipoQuestao.Combo:
            valorRespostaAtual = String(resposta[0].opcaoRespostaId || '');
            break;
          case tipoQuestao.ComboMultiplaEscolha:
            valorRespostaAtual = resposta.map(r => String(r.opcaoRespostaId));
            break;
          case tipoQuestao.Texto:
            valorRespostaAtual = resposta[0].texto;
            break;
          case tipoQuestao.AtendimentoClinico:
            valorRespostaAtual = resposta[0].texto
              ? JSON.parse(resposta[0].texto)
              : '';
            break;
          case tipoQuestao.Upload:
            if (resposta?.length) {
              valorRespostaAtual = resposta
                ?.map(item => {
                  const { arquivo } = item;
                  if (arquivo) {
                    return {
                      uid: arquivo.codigo,
                      xhr: arquivo.codigo,
                      name: arquivo.nome,
                      status: 'done',
                      arquivoId: arquivo.id,
                    };
                  }
                  return '';
                })
                .filter(a => !!a);
            } else {
              valorRespostaAtual = [];
            }
            break;
          default:
            break;
        }
      }

      if (
        valorRespostaAtual?.length &&
        questaoAtual?.tipoQuestao === tipoQuestao.ComboMultiplaEscolha
      ) {
        const idQuestaoComResposta = valorRespostaAtual.find(valorSalvo => {
          const opcaoResposta = questaoAtual?.opcaoResposta.find(
            q => String(q.id) === String(valorSalvo)
          );

          if (opcaoResposta?.questaoComplementar?.resposta?.length) {
            return true;
          }
          return false;
        });

        if (idQuestaoComResposta) {
          const questaoComplmentarComResposta = questaoAtual?.opcaoResposta.find(
            q => String(q.id) === String(idQuestaoComResposta)
          );

          if (questaoComplmentarComResposta?.questaoComplementar) {
            montarDados(questaoComplmentarComResposta.questaoComplementar);
          }
        }
      } else if (
        valorRespostaAtual &&
        questaoAtual?.tipoQuestao !== tipoQuestao.Upload &&
        questaoAtual?.tipoQuestao !== tipoQuestao.Texto
      ) {
        const opcaoAtual = questaoAtual?.opcaoResposta.find(
          item => String(item.id) === String(valorRespostaAtual)
        );

        if (opcaoAtual?.questaoComplementar) {
          montarDados(opcaoAtual.questaoComplementar);
        }
      }

      valores[questaoAtual.id] = valorRespostaAtual;
    };

    dadosQuestionarioAtual.forEach(questaoAtual => {
      montarDados(questaoAtual);
    });

    setValoresIniciais({ ...valores });
  }, [dadosQuestionarioAtual]);

  useEffect(() => {
    if (dadosQuestionarioAtual?.length) {
      montarValoresIniciais();
    }
  }, [dadosQuestionarioAtual, montarValoresIniciais]);

  const campoAtendimentoClinico = params => {
    const { questaoAtual, label, form } = params;

    return (
      <div className="col-md-12 mb-3">
        <AtendimentoClinicoTabela
          desabilitado={desabilitarCampos}
          label={label}
          form={form}
          questaoAtual={questaoAtual}
        />
      </div>
    );
  };

  const labelPersonalizado = (textolabel, observacaoText) => (
    <Label text={textolabel} observacaoText={observacaoText} />
  );

  const montarCampos = (questaoAtual, form, ordemAnterior) => {
    const ordemLabel = ordemAnterior
      ? `${ordemAnterior}.${questaoAtual.ordem}`
      : questaoAtual.ordem;

    const textoLabel = `${ordemLabel} - ${questaoAtual.nome}`;
    const label = labelPersonalizado(textoLabel, questaoAtual?.observacao);

    let campoQuestaoComplementar = null;

    const valorAtualSelecionado = form.values[questaoAtual.id];

    if (questaoAtual?.tipoQuestao === tipoQuestao.ComboMultiplaEscolha) {
      if (valorAtualSelecionado?.length) {
        const idOpcaoRespostaComComplementarObrigatoria = QuestionarioDinamicoFuncoes.obterIdOpcaoRespostaComComplementarObrigatoria(
          valorAtualSelecionado,
          questaoAtual
        );

        if (idOpcaoRespostaComComplementarObrigatoria) {
          const opcaoResposta = questaoAtual?.opcaoResposta.find(
            item =>
              String(item.id) ===
              String(idOpcaoRespostaComComplementarObrigatoria)
          );

          if (opcaoResposta?.questaoComplementar) {
            campoQuestaoComplementar = montarCampos(
              opcaoResposta.questaoComplementar,
              form,
              ordemLabel
            );
          }
        } else {
          const idOpcaoRespostaComComplementarNaoObrigatoria = QuestionarioDinamicoFuncoes.obterIdOpcaoRespostaComComplementarNaoObrigatoria(
            valorAtualSelecionado,
            questaoAtual
          );

          const opcaoResposta = QuestionarioDinamicoFuncoes.obterOpcaoRespostaPorId(
            questaoAtual?.opcaoResposta,
            idOpcaoRespostaComComplementarNaoObrigatoria
          );

          if (opcaoResposta?.questaoComplementar) {
            campoQuestaoComplementar = montarCampos(
              opcaoResposta.questaoComplementar,
              form,
              ordemLabel
            );
          }
        }
      }
    } else if (valorAtualSelecionado) {
      const opcaoResposta = QuestionarioDinamicoFuncoes.obterOpcaoRespostaPorId(
        questaoAtual?.opcaoResposta,
        valorAtualSelecionado
      );

      if (opcaoResposta?.questaoComplementar) {
        campoQuestaoComplementar = montarCampos(
          opcaoResposta.questaoComplementar,
          form,
          ordemLabel
        );
      }
    }

    const params = {
      questaoAtual,
      form,
      label,
    };

    let campoAtual = null;
    switch (questaoAtual?.tipoQuestao) {
      case tipoQuestao.Radio:
        campoAtual = (
          <CampoDinamicoRadio
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valorAtual => {
              QuestionarioDinamicoFuncoes.onChangeCamposComOpcaoResposta(
                questaoAtual,
                form,
                valorAtual
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
            }}
          />
        );
        break;
      case tipoQuestao.Combo:
        campoAtual = (
          <CampoDinamicoCombo
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valorAtual => {
              QuestionarioDinamicoFuncoes.onChangeCamposComOpcaoResposta(
                questaoAtual,
                form,
                valorAtual
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
            }}
          />
        );
        break;
      case tipoQuestao.ComboMultiplaEscolha:
        campoAtual = (
          <CampoDinamicoComboMultiplaEscolha
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valoresSelecionados => {
              QuestionarioDinamicoFuncoes.onChangeCampoComboMultiplaEscolha(
                questaoAtual,
                form,
                valoresSelecionados
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
            }}
          />
        );
        break;
      case tipoQuestao.Texto:
        campoAtual = (
          <CampoDinamicoTexto
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
          />
        );
        break;
      case tipoQuestao.InformacoesEscolares:
        campoAtual = (
          <div className="col-md-12 mb-3">
            <InformacoesEscolares
              codigoAluno={codigoAluno}
              codigoTurma={codigoTurma}
              anoLetivo={anoLetivo}
            />
          </div>
        );
        break;
      case tipoQuestao.AtendimentoClinico:
        campoAtual = campoAtendimentoClinico(params);
        break;
      case tipoQuestao.Upload:
        campoAtual = (
          <UploadArquivosEncaminhamento
            dados={params}
            desabilitado={desabilitarCampos}
          />
        );
        break;
      default:
        break;
    }

    return (
      <>
        {campoAtual || ''}
        {campoQuestaoComplementar || ''}
      </>
    );
  };

  const montarQuestionarioAtual = (data, form) => {
    const campos = data.map(questaoAtual => {
      return (
        <div className="row" key={questaoAtual.id}>
          {montarCampos(questaoAtual, form, '')}
        </div>
      );
    });

    return campos;
  };

  return dados?.questionarioId &&
    dadosQuestionarioAtual?.length &&
    valoresIniciais ? (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={() =>
        QuestionarioDinamicoValidacoes.obterValidationSchema(
          dadosQuestionarioAtual,
          refForm
        )
      }
      validateOnChange
      validateOnBlur
      ref={refFormik => setRefForm(refFormik)}
    >
      {form => (
        <Form className="col-md-12">
          {montarQuestionarioAtual(dadosQuestionarioAtual, form)}
        </Form>
      )}
    </Formik>
  ) : (
    ''
  );
};

QuestionarioDinamico.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  dadosQuestionarioAtual: PropTypes.oneOfType([PropTypes.any]),
  desabilitarCampos: PropTypes.bool,
  codigoAluno: PropTypes.oneOfType([PropTypes.any]),
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  anoLetivo: PropTypes.oneOfType([PropTypes.any]),
};

QuestionarioDinamico.defaultProps = {
  dados: {},
  dadosQuestionarioAtual: {},
  desabilitarCampos: false,
  codigoAluno: '',
  codigoTurma: '',
  anoLetivo: null,
};

export default QuestionarioDinamico;
