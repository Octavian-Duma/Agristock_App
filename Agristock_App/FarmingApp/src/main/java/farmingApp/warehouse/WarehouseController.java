package farmingApp.warehouse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/warehouses")
@CrossOrigin
public class WarehouseController {

    @Autowired
    private WarehouseService warehouseService;

    @GetMapping //get
    public List<Warehouse> getAllWarehouses() {
        return warehouseService.getAllWarehouses();
    }
    @PostMapping //create
    public Warehouse createWarehouse(@RequestBody Warehouse warehouse) {
        return warehouseService.createWarehouse(warehouse);
    }
}