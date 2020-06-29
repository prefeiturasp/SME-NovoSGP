pipeline {
    agent {
      node { 
        label 'dockerdotnet2'
      }
    }
    
    options {
      buildDiscarder(logRotator(numToKeepStr: '5', artifactNumToKeepStr: '5'))
      disableConcurrentBuilds()
      skipDefaultCheckout()  
    }
    
        
    stages {
      stage('CheckOut') {
        steps {
          checkout scm  
        }
       }
       
      // stage('Início Análise Código') {
      //     when {
      //       branch 'development'
      //     }
      //       steps {
      //           //sh 'echo Analise SonarQube API'
      //           //sh 'dotnet-sonarscanner begin /k:"SME-NovoSGP" /d:sonar.host.url="http://sonar.sme.prefeitura.sp.gov.br" /d:sonar.login="346fd763d9581684b9271a03d8ef5a16fe92622b" /d:sonar.cs.opencover.reportsPaths="teste/SME.SGP.Aplicacao.Teste/coverage.opencover.xml,teste/SME.SGP.Dominio.Servicos.Teste/coverage.opencover.xml,teste/SME.SGP.Dominio.Teste/coverage.opencover.xml,teste/SME.SGP.Dominio.Servicos.Teste/coverage.opencover.xml,teste/SME.SGP.Integracao.Teste/coverage.opencover.xml" /d:sonar.coverage.exclusions="**Test*.cs"'

      //       // anlise codigo frontend
      //           // sh 'echo Analise SonarQube FRONTEND'
      //           // sh 'sonar-scanner \
      //           // -Dsonar.projectKey=SME-NovoSGP-WebClient \
      //           // -Dsonar.sources=src/SME.SGP.WebClient \
      //           // -Dsonar.host.url=http://sonar.sme.prefeitura.sp.gov.br \
      //           // -Dsonar.login=1ab3b0eb51a0f51c846c13f2f5a0255fd5d7583e'
      //       }
      //  } 
         
      stage('Build projeto') {
            steps {
            sh "echo executando build de projeto"
            sh 'dotnet build'
            }
        }
        
            
      stage('Testes') {
            steps {
            //Executa os testes
               sh 'dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
            }
        }
        
      //         stage('Fim Análise Código') {
      //     when {
      //       branch 'development'
      //     }
      //       steps {
      //           // sh 'echo Fim SonarQube API'
      //           // sh 'dotnet-sonarscanner end /d:sonar.login="346fd763d9581684b9271a03d8ef5a16fe92622b"'
      //       }
      //  }

      stage('Deploy DEV') {
        when {
          branch 'development'
        }
          steps {
            sh 'echo Deploying desenvolvimento'
                
        // Start JOB Rundeck para build das imagens Docker e push SME Registry
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                               
              //JOB DE BUILD
              jobId: "743ccbae-bd30-4ac6-b2a3-2f0d1c64e937",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para update de deploy Kubernetes DEV
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "f6c3e74c-6411-466a-84a7-921d637c2645",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }
		
		  stage('Deploy DEV-rc2') {
            when {
                branch 'development-r2'
            }
            steps {
                 
                 sh 'echo Deploying desenvolvimento RC2'
                
        // Start JOB Rundeck para build das imagens Docker e push SME Registry
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                               
              //JOB DE BUILD
              jobId: "29e93baa-a956-46ac-b805-23862ddc6863",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para update de deploy Kubernetes DEV
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "7b136020-3ddd-4fce-9ac7-4fc9c4b9b2af",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }
        
       
      
      stage('Deploy HOM') {
            when {
                branch 'release'
            }
            steps {
                 timeout(time: 24, unit: "HOURS") {
               
                 telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
                 input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marlon_goncalves, allan_santos, everton_nogueira, marcos_costa, bruno_alevato, robson_silva'
            }
                 sh 'echo Deploying homologacao'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                
               
              //JOB DE BUILD
              jobId: "397ce3f8-0af7-4d26-b65b-19f09ccf6c82",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para update de imagens no host homologação 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "124f1ff0-d903-40fd-9455-fc91907293a7",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }
	    
	    stage('Deploy HOM-R2') {
            when {
                branch 'release-r2'
            }
            steps {
            //     timeout(time: 24, unit: "HOURS") {
               
            //    telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
            //     input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marcos_costa,danieli_paula,everton_nogueira,marlon_goncalves'
            //}
                 sh 'echo Deploying homologacao'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
                
               
              //JOB DE BUILD
              jobId: "e15cd478-1155-40a2-842c-11d1de0512eb",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para update de imagens no host homologação 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "b45bb11d-889a-4783-b70d-4406ea6817d7",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
      
       
            }
        }    


        stage('Deploy PROD') {

            when {
                branch 'master'
            }
            steps {
                 timeout(time: 24, unit: "HOURS") {
               
                 telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Requer uma aprovação para deploy !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n")
                 input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: 'marlon_goncalves, allan_santos, everton_nogueira, marcos_costa, bruno_alevato, robson_silva'
            }
                 sh 'echo Deploy produção'
                
        // Start JOB Rundeck para build das imagens Docker e push registry SME
      
          script {
           step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
            
               
              //JOB DE BUILD
              jobId: "b6ff0cbf-6267-41af-bb56-5cdc3eb86902",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
           }
                
       //Start JOB Rundeck para deploy em produção 
         
         script {
            step([$class: "RundeckNotifier",
              includeRundeckLogs: true,
              jobId: "6a3d314b-672b-4fe3-9759-0b08847eb27e",
              nodeFilters: "",
              //options: """
              //     PARAM_1=value1
               //    PARAM_2=value2
              //     PARAM_3=
              //     """,
              rundeckInstance: "Rundeck-SME",
              shouldFailTheBuild: true,
              shouldWaitForRundeckJob: true,
              tags: "",
              tailLog: true])
         }
      
       
            }
        }
     
}

    
post {
        always {
          echo 'One way or another, I have finished'
        }
        success {
          telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME} - Esta ok !!!\n Consulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)\n\n Uma nova versão da aplicação esta disponivel!!!")
        }
        unstable {
          telegramSend("O Build ${BUILD_DISPLAY_NAME} <${env.BUILD_URL}> - Esta instavel ...\nConsulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)")
        }
        failure {
          telegramSend("${JOB_NAME}...O Build ${BUILD_DISPLAY_NAME}  - Quebrou. \nConsulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)")
        }
        changed {
          echo 'Things were different before...'
        }
        aborted {
          telegramSend("O Build ${BUILD_DISPLAY_NAME} - Foi abortado.\nConsulte o log para detalhes -> [Job logs](${env.BUILD_URL}console)")
        }
    }
}