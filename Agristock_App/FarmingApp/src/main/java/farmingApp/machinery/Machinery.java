package farmingApp.machinery;

import farmingApp.warehouse.Warehouse;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Machinery {

    @Id //cheie primara cu ordonare automata
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private String name;

    @Enumerated(EnumType.STRING)  //status dat prin enum
    private MachineryStatus status;

    @Column(length = 500)
    private String statusDescription;

    @ManyToOne       //mai multe utilaje intr-un singur depozit un utilaj are automat un depozit
    @JoinColumn(name = "warehouse_id", nullable = false) // foreign key
    private Warehouse warehouse;

    public Machinery(String name, String status, String statusDescription, Warehouse warehouse) {
        this.name = name;
        this.status = MachineryStatus.valueOf(status);
        this.statusDescription = statusDescription;
        this.warehouse = warehouse;
    }
}