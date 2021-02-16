import * as Yup from 'yup';
import tipoQuestao from '~/dtos/tipoQuestao';

class QuestionarioDinamicoValidacoes {
  obterValidationSchema = (dadosQuestionarioAtual, form) => {
    if (dadosQuestionarioAtual?.length && form?.state?.values) {
      const camposComValidacao = {};

      let arrayCampos = [];

      const camposValidar = form?.state?.values;
      if (camposValidar && Object.keys(camposValidar)?.length) {
        arrayCampos = Object.keys(camposValidar);
      }

      const montaValidacoes = questaoAtual => {
        if (questaoAtual?.opcaoResposta?.length) {
          questaoAtual.opcaoResposta.forEach(opcaoAtual => {
            if (opcaoAtual?.questoesComplementares?.length) {
              opcaoAtual.questoesComplementares.forEach(q => {
                montaValidacoes(q);
              });
            }
          });
        }

        if (
          questaoAtual.obrigatorio &&
          arrayCampos.find(questaoId => questaoId === String(questaoAtual.id))
        ) {
          if (questaoAtual.tipoQuestao === tipoQuestao.Periodo) {
            // TODO Fazer validação para campos datas!
            //   camposComValidacao[
            //     questaoAtual.id
            //   ].periodoInicio = momentSchema.required('Campo obrigatório');
            //   camposComValidacao[
            //     questaoAtual.id
            //   ].periodoFim = momentSchema.required('Campo obrigatório');
          } else {
            camposComValidacao[questaoAtual.id] = Yup.string()
              .nullable()
              .required('Campo obrigatório');
          }
        }
      };

      if (arrayCampos?.length) {
        dadosQuestionarioAtual.forEach(questaoAtual => {
          montaValidacoes(questaoAtual);
        });

        return Yup.object(camposComValidacao);
      }
    }
    return {};
  };
}

export default new QuestionarioDinamicoValidacoes();
