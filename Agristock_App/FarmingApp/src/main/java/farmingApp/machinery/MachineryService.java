package farmingApp.machinery;

import farmingApp.warehouse.Warehouse;
import farmingApp.warehouse.WarehouseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Map;

@Service
public class MachineryService {

    @Autowired
    private MachineryRepository machineryRepository;

    @Autowired
    private WarehouseService warehouseService;

    public List<Machinery> getAllMachinery() {
        return machineryRepository.findAll();
    }

    public Machinery createMachinery(Map<String, Object> payload) {
        String name = (String) payload.get("name");
        String statusStr = (String) payload.get("status");
        String description = (String) payload.get("description");

        Object warehouseIdObj = payload.get("warehouseId");
        Integer warehouseId = warehouseIdObj instanceof Integer ? (Integer) warehouseIdObj : Integer.parseInt(warehouseIdObj.toString());

        Warehouse warehouse = warehouseService.getWarehouseById(warehouseId);

        MachineryStatus status;
        try {
            status = MachineryStatus.valueOf(statusStr.toUpperCase());
        } catch (IllegalArgumentException e) {
            status = MachineryStatus.FUNCTIONAL;
        }

        Machinery newMachinery = new Machinery(name, status.name(), description, warehouse);
        return machineryRepository.save(newMachinery);
    }

    public Machinery updateStatus(int id, String newStatusStr, String newDescription, Integer newWarehouseId) {
        Machinery machinery = machineryRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Utilaj inexistent"));

        try {
            MachineryStatus statusEnum = MachineryStatus.valueOf(newStatusStr.toUpperCase());
            machinery.setStatus(statusEnum);
        } catch (IllegalArgumentException e) {
            throw new RuntimeException("Status invalid: " + newStatusStr);
        }

        machinery.setStatusDescription(newDescription);

        if (newWarehouseId != null) {
            Warehouse newWarehouse = warehouseService.getWarehouseById(newWarehouseId);
            machinery.setWarehouse(newWarehouse);
        }


        return machineryRepository.save(machinery);
    }

    public void deleteMachinery(int id) {
        if (machineryRepository.existsById(id)) {
            machineryRepository.deleteById(id);
        } else {
            throw new RuntimeException("Utilaj negasit");
        }
    }

}