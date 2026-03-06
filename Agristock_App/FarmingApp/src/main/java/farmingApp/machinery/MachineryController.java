package farmingApp.machinery;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/machinery")
@CrossOrigin
public class MachineryController {

    @Autowired
    private MachineryService machineryService;

    @GetMapping //citire
    public List<Machinery> getAllMachinery() {
        return machineryService.getAllMachinery();
    }

    @PostMapping //creare
    public Machinery createMachinery(@RequestBody Map<String, Object> payload) {
        return machineryService.createMachinery(payload);
    }

    //update
    @PutMapping("/{id}/status")   //Permite frontend-ului trimiterea unui un JSON care are statusul, descrierea si eventual un ID nou de depozit
    public ResponseEntity<Machinery> updateStatus(@PathVariable int id, @RequestBody Map<String, Object> payload) { // evit DTO ,dar conversia se face manual
        String newStatus = (String) payload.get("status");   //extragere si conversie string
        String newDescription = (String) payload.get("description"); //specific descrierea string

        Integer newWarehouseId = null;    // depozit nou optional
        if (payload.get("warehouseId") != null) {
            newWarehouseId = Integer.parseInt(payload.get("warehouseId").toString());
            //convertesc in string si apoi in integer pt a trimite corect id-ul
        }
        //executie update
        try {
            Machinery updated = machineryService.updateStatus(id, newStatus, newDescription, newWarehouseId);
            return ResponseEntity.ok(updated);
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }


    @DeleteMapping("/{id}") //stergere
    public ResponseEntity<Void> deleteMachinery(@PathVariable int id) {
        try {
            machineryService.deleteMachinery(id);
            return ResponseEntity.noContent().build();
        } catch (RuntimeException e) {
            return ResponseEntity.notFound().build();
        }
    }
}