package farmingApp.inventory;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import java.util.List;

public interface InventoryRepository extends JpaRepository<Inventory, Integer> {

    //custom query pentru stocuri reduse/suficiente

    @Query("SELECT i FROM Inventory i WHERE i.currentQuantityKg < 1000")
    List<Inventory> findLowStockItems();

    @Query("SELECT i FROM Inventory i WHERE i.currentQuantityKg > 10000")
    List<Inventory> findHighStockItems();
}