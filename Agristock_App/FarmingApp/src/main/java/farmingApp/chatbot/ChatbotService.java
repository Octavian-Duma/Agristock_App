package farmingApp.chatbot;

import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;    //face cereri HTTP
import java.util.Map;

@Service  //logica de business disponibila prin autowired
public class ChatbotService {

    private final WebClient client;
    private final String ollamaModel = "llama3";//modelul ollama

    public ChatbotService() {
        this.client = WebClient.builder()
                .baseUrl("http://localhost:11434")   //adresa ollama
                .build();
    }

    public String askOllama(String prompt) {
        Map<String, Object> request = Map.of(        //construim JSON catre AI
                "model", ollamaModel,
                "prompt", prompt,
                "stream", false                 //false-asa trimitem rasp complet la final
        );

        try {
            OllamaResponse result = client.post()    //trimitere si asteptare
                    .uri("/api/generate")        //endpoint
                    .bodyValue(request)              //json request
                    .retrieve()                      //send de la buton
                    .bodyToMono(OllamaResponse.class)//transformare JSON in obiect java DTO
                    .block();                        //wait fortat pana primim raspuns
            return result.response;
        } catch (Exception e) {
            e.printStackTrace();
            return "A aparut o problema la conexiunea cu Ollama";
        }
    }
}