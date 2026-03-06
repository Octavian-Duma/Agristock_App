package farmingApp.machinery;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface MachineryRepository extends JpaRepository<Machinery, Integer> {

    List<Machinery> findByStatus(MachineryStatus status);
}