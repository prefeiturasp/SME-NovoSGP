import * as Yup from 'yup';

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
            if (opcaoAtual?.questaoComplementar) {
              montaValidacoes(opcaoAtual.questaoComplementar);
            }
          });
        }

        if (
          questaoAtual.obrigatorio &&
          arrayCampos.find(questaoId => questaoId === String(questaoAtual.id))
        ) {
          camposComValidacao[questaoAtual.id] = Yup.string()
            .nullable()
            .required('Campo obrigatório');
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
