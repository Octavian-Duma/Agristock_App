package farmingApp.inventory;

import com.fasterxml.jackson.annotation.JsonProperty;
import farmingApp.product.Product;
import farmingApp.warehouse.Warehouse;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity //creeaza tabel SQL
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Inventory {

    @Id                                                  //cheia primara
    @GeneratedValue(strategy = GenerationType.IDENTITY)  //generarea automata a numerotarii
    private Integer id;

    @ManyToOne                                           //mai multe stocuri intr-un depozit
    @JoinColumn(name = "warehouse_id", nullable = false) // warehouse_id -foreign key la depozit
    private Warehouse warehouse;

    @ManyToOne                                           //mai multe intrari de stoc pt acelasi produs
    @JoinColumn(name = "product_id", nullable = false)   //prouct_id -foreign key la produs
    private Product product;


    @JsonProperty("current_quantity_kg")  // camp double pt cantitate
    private Double currentQuantityKg;


    public Inventory(Warehouse warehouse, Product product, Double currentQuantityKg) {
        this.warehouse = warehouse;
        this.product = product;
        this.currentQuantityKg = currentQuantityKg;
    }
}