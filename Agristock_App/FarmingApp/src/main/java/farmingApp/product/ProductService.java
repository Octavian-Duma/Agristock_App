package farmingApp.product;

import farmingApp.inventory.Inventory;
import farmingApp.inventory.InventoryRepository;
import farmingApp.warehouse.Warehouse;
import farmingApp.warehouse.WarehouseRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Service
public class ProductService {

    @Autowired
    private ProductRepository productRepository;
    @Autowired
    private InventoryRepository inventoryRepository;
    @Autowired
    private WarehouseRepository warehouseRepository;

    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    public List<String> getAllCategories() { //get categorii pt un dropdown
        return productRepository.findAll().stream()//luam toate produsele
                .map(Product::getCategory) //pastram doar categoriile
                .distinct() //eliminam duplicatele
                .sorted()   //sortam alfabetic
                .collect(Collectors.toList());//punem in lista noua
    }

    public Product createProductWithStock(Map<String, Object> payload) { //tratam optional adaugarea cantitatii
        String name = (String) payload.get("name");         //extragem nume
        String category = (String) payload.get("category"); //extragem categorie

        Product product = new Product(name, category);
        product = productRepository.save(product);     //salvam produsul

        if (payload.get("warehouseId") != null && payload.get("initialQuantity") != null) { //verificam daca contine info despre stoc
            try {
                Object whIdObj = payload.get("warehouseId");
                Integer warehouseId = whIdObj instanceof Integer ? (Integer) whIdObj : Integer.parseInt(whIdObj.toString());//Obiectul whIdObj este deja un numar intreg (Integer)?,daca da il folosim,daca nu il convertim
                Object qtyObj = payload.get("initialQuantity");
                Double quantity = qtyObj instanceof Double ? (Double) qtyObj : Double.parseDouble(qtyObj.toString()); // verificam daca e double , iar daca nu este il convertim

                //facem conersii corecte din JSON

                Warehouse warehouse = warehouseRepository.findById(warehouseId) //cautam depozitul
                        .orElseThrow(() -> new RuntimeException("Depozit negasit"));

                Inventory inventory = new Inventory(warehouse, product, quantity);
                inventoryRepository.save(inventory); //salvam inventory
            } catch (Exception e) {
                System.err.println("Eroare la crearea stocului initial: " + e.getMessage());
            }
        }

        return product;
    }
}