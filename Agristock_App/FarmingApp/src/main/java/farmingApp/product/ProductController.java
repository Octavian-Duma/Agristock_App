package farmingApp.product;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/products")
@CrossOrigin
public class ProductController {

    @Autowired
    private ProductService productService;

    @GetMapping
    public List<Product> getAllProducts() {
        return productService.getAllProducts();
    }

    @GetMapping("/categories") //get pentru un dropdown
    public List<String> getCategories() {
        return productService.getAllCategories();
    }

    @PostMapping
    public Product createProduct(@RequestBody Map<String, Object> payload) {
        return productService.createProductWithStock(payload);
    }
}