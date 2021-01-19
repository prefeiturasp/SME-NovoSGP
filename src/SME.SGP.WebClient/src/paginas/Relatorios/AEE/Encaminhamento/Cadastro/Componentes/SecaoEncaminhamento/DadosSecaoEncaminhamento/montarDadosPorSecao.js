import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { CampoTexto, RadioGroupButton, SelectComponent } from '~/componentes';
import { RotasDto } from '~/dtos';
import tipoQuestao from '~/dtos/tipoQuestao';
import AtendimentoClinicoTabela from '~/paginas/Relatorios/AEE/Encaminhamento/Cadastro/Componentes/AtendimentoClinico/atendimentoClinicoTabela';
import { setEncaminhamentoAEEEmEdicao } from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros, setBreadcrumbManual } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import InformacoesEscolares from '../../IndicativosEstudante/indicativosEstudante';
import UploadArquivosEncaminhamento from '../../UploadArquivosEncaminhamento/uploadArquivosEncaminhamento';

const MontarDadosPorSecao = props => {
  const dispatch = useDispatch();

  const { dados, match } = props;

  const [dadosQuestionarioAtual, setDadosQuestionarioAtual] = useState();
  const [valoresIniciais, setValoresIniciais] = useState();
  const tiposQuestaoPorIdQuestao = {};

  const [refForm, setRefForm] = useState({});

  useEffect(() => {
    const encaminhamentoId = match?.params?.id;
    if (encaminhamentoId) {
      setBreadcrumbManual(
        match.url,
        'Editar Encaminhamento',
        `${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}`
      );
    }
  }, [match]);

  const obterQuestionario = useCallback(async questionarioId => {
    const encaminhamentoId = match?.params?.id;
    dispatch(setEncaminhamentoAEEEmEdicao(false));
    const resposta = await ServicoEncaminhamentoAEE.obterQuestionario(
      questionarioId,
      encaminhamentoId
    ).catch(e => erros(e));

    if (resposta?.data) {
      setDadosQuestionarioAtual(resposta.data);
    } else {
      setDadosQuestionarioAtual();
    }
  }, []);

  useEffect(() => {
    if (dados?.questionarioId) {
      obterQuestionario(dados?.questionarioId);
    } else {
      setDadosQuestionarioAtual();
    }
  }, [dados, obterQuestionario]);

  const obterForm = () => refForm;

  useEffect(() => {
    if (refForm) {
      ServicoEncaminhamentoAEE.addFormsSecoesEncaminhamentoAEE(
        () => obterForm(),
        dados.questionarioId,
        dadosQuestionarioAtual,
        tiposQuestaoPorIdQuestao
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
          case tipoQuestao.Texto:
            valorRespostaAtual = resposta[0].texto;
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

  const campoRadio = params => {
    const { questaoAtual, form, label } = params;

    const opcoes = questaoAtual?.opcaoResposta.map(item => {
      return { label: item.nome, value: item.id };
    });

    return (
      <div className="col-md-12 mb-3">
        <RadioGroupButton
          id={String(questaoAtual.id)}
          name={String(questaoAtual.id)}
          label={label}
          form={form}
          opcoes={opcoes}
          onChange={() => {
            dispatch(setEncaminhamentoAEEEmEdicao(true));
          }}
        />
      </div>
    );
  };

  const campoCombo = params => {
    const { questaoAtual, form, label } = params;

    const lista = questaoAtual?.opcaoResposta.map(item => {
      return { desc: item.nome, valor: item.id };
    });

    return (
      <>
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-3">
          <SelectComponent
            id={String(questaoAtual.id)}
            name={String(questaoAtual.id)}
            label={label}
            placeholder={questaoAtual.nome}
            form={form}
            lista={lista}
            valueOption="valor"
            valueText="desc"
            onChange={() => {
              dispatch(setEncaminhamentoAEEEmEdicao(true));
            }}
          />
        </div>
      </>
    );
  };

  const campoTexto = params => {
    const { questaoAtual, form, label } = params;

    return (
      <div className="col-md-12 mb-3">
        <CampoTexto
          id={String(questaoAtual.id)}
          name={String(questaoAtual.id)}
          label={label}
          form={form}
          type="textarea"
          maxLength={99999}
          onChange={() => {
            dispatch(setEncaminhamentoAEEEmEdicao(true));
          }}
        />
      </div>
    );
  };

  const campoAtendimentoClinico = params => {
    const { questaoAtual, label, form } = params;

    return (
      <div className="col-md-12 mb-3">
        <AtendimentoClinicoTabela
          label={label}
          form={form}
          questaoAtual={questaoAtual}
        />
      </div>
    );
  };

  const montarCampos = (questaoAtual, form, ordemAnterior) => {
    const ordemLabel = ordemAnterior
      ? `${ordemAnterior}.${questaoAtual.ordem}`
      : questaoAtual.ordem;

    const label = `${ordemLabel} - ${questaoAtual.nome}`;

    let campoQuestaoComplementar = null;

    const valorAtualSelecionado = form.values[questaoAtual.id];

    if (valorAtualSelecionado) {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        item => String(item.id) === String(valorAtualSelecionado)
      );

      if (opcaoAtual?.questaoComplementar) {
        campoQuestaoComplementar = montarCampos(
          opcaoAtual.questaoComplementar,
          form,
          ordemLabel
        );
      }
    } else {
      const opcaoAtual = questaoAtual?.opcaoResposta[0];

      if (opcaoAtual?.questaoComplementar) {
        campoQuestaoComplementar = montarCampos(
          opcaoAtual.questaoComplementar,
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

    tiposQuestaoPorIdQuestao[questaoAtual.id] = questaoAtual.tipoQuestao;

    let campoAtual = null;
    switch (questaoAtual?.tipoQuestao) {
      case tipoQuestao.Radio:
        campoAtual = campoRadio(params);
        break;
      case tipoQuestao.Combo:
        campoAtual = campoCombo(params);
        break;
      case tipoQuestao.Texto:
        campoAtual = campoTexto(params);
        break;
      case tipoQuestao.InformacoesEscolares:
        campoAtual = (
          <div className="col-md-12 mb-3">
            <InformacoesEscolares dados={params} />
          </div>
        );
        break;
      case tipoQuestao.AtendimentoClinico:
        campoAtual = campoAtendimentoClinico(params);
        break;
      case tipoQuestao.Upload:
        campoAtual = <UploadArquivosEncaminhamento dados={params} />;
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
      validationSchema={{}}
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

MontarDadosPorSecao.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  match: PropTypes.oneOfType([PropTypes.object]),
};

MontarDadosPorSecao.defaultProps = {
  dados: {},
  match: {},
};

export default MontarDadosPorSecao;
