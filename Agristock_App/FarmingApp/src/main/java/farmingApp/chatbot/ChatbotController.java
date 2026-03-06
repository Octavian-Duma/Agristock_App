package farmingApp.chatbot;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController               //gestioneaza cereri client si raspunde cu date JSON/Text
@RequestMapping("/chatbot")   //tot ce vine pe /chatbot ajunge aici
@CrossOrigin                  //permit acces clientilor chiar daca sunt pe porturi diferite

public class ChatbotController {
    @Autowired                // Dependency Injection legam controllerul de service si repository
    private ChatbotService chatbotService;


    @PostMapping              //metoda chat răspunde doar la cereri de tip POST
    public String chat(@RequestBody ChatbotRequest request) {

        String prompt = request.getPrompt();
        return chatbotService.askOllama(prompt);
    }
}