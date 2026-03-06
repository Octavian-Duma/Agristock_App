package farmingApp.inventory;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class InventoryService {

    @Autowired
    private InventoryRepository inventoryRepository;

    public List<Inventory> getAllInventory() {
        return inventoryRepository.findAll();
    }

    public List<Inventory> getLowStockInventory() {
        return inventoryRepository.findLowStockItems();
    }

    public List<Inventory> getHighStockInventory() {
        return inventoryRepository.findHighStockItems();
    }


    public Inventory updateQuantity(int id, double newQuantity) {
        Inventory inventory = inventoryRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Stoc negasit"));
        inventory.setCurrentQuantityKg(newQuantity);

        return inventoryRepository.save(inventory);
    }
    public void deleteInventory(int id) {
        if (inventoryRepository.existsById(id)) {
            inventoryRepository.deleteById(id);
        } else {
            throw new RuntimeException("Stoc negasit");
        }
    }
}