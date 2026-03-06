package farmingApp.product;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Product {

    @Id  //primary key
    @GeneratedValue(strategy = GenerationType.IDENTITY) //ordonare automata
    private Integer id;

    private String name;
    private String category;

    public Product(String name, String category) {
        this.name = name;
        this.category = category;
    }
}