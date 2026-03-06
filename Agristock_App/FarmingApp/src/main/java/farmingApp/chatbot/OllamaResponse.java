package farmingApp.chatbot;

import lombok.Data;
@Data
public class OllamaResponse {     //DTO pentru Ollama response
    String model;
    String created_at;
    String response;
    boolean done;
}