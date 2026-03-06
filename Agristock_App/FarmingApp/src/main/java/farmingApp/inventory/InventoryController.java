package farmingApp.inventory;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.Map;

@RestController                //gestioneaza cereri client si raspunde cu date JSON/Text
@RequestMapping("/inventory")  //adresa de baza
@CrossOrigin
public class InventoryController {

    @Autowired
    private InventoryService inventoryService;

    @GetMapping      //get endpoint
    public List<Inventory> getAllInventory() {
        return inventoryService.getAllInventory();
    }

    @GetMapping("/low-stock") //get endpoint stocuri epuizate
    public List<Inventory> getLowStock() {
        return inventoryService.getLowStockInventory();
    }

    @GetMapping("/high-stock") //get endpoint stocuri suficiente
    public List<Inventory> getHighStock() {
        return inventoryService.getHighStockInventory();
    }

    @PutMapping("/{id}/quantity") //PUT endpoint pt update cantitate ,scris asa pt a putea actualiza si doar cantitatea
    public ResponseEntity<Inventory> updateQuantity(@PathVariable int id, @RequestBody Map<String, Double> payload) {
        if (!payload.containsKey("quantity")) {
            return ResponseEntity.badRequest().build();
        }
        Double newQuantity = payload.get("quantity");
        try {
            Inventory updated = inventoryService.updateQuantity(id, newQuantity);
            return ResponseEntity.ok(updated);
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }

    @DeleteMapping("/{id}") //DELETE endpoint
    public ResponseEntity<Void> deleteInventory(@PathVariable int id) {
        try {
            inventoryService.deleteInventory(id);
            return ResponseEntity.noContent().build();
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }
}