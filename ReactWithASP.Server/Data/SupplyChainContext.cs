using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReactWithASP.Server.Models.TestDbs.SupplyChain;

namespace ReactWithASP.Server.Data
{
    public class SupplyChainContext : DbContext
    {
        public SupplyChainContext(DbContextOptions<SupplyChainContext> options) : base(options) 
        {
            
        }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var inventory_items = JsonConvert.DeserializeObject<List<Inventory>>(File.ReadAllText("TestData/InventoryTestData.json"));

            foreach (var element in inventory_items)
            {
                if (element.release_date.Kind == DateTimeKind.Unspecified)
                {
                    element.release_date = DateTime.SpecifyKind(element.release_date, DateTimeKind.Utc);
                }
                else if (element.release_date.Kind == DateTimeKind.Local)
                {
                    element.release_date = element.release_date.ToUniversalTime();
                }
            }

            modelBuilder.Entity<Inventory>().HasData(inventory_items);

            var product_items = JsonConvert.DeserializeObject<List<Products>>(File.ReadAllText("TestData/ProductsTestData.json"));

            foreach (var element in product_items)
            {
                if (element.release_date.Kind == DateTimeKind.Unspecified)
                {
                    element.release_date = DateTime.SpecifyKind(element.release_date, DateTimeKind.Utc);
                }
                else if (element.release_date.Kind == DateTimeKind.Local)
                {
                    element.release_date = element.release_date.ToUniversalTime();
                }
            }

            modelBuilder.Entity<Products>().HasData(product_items);


            base.OnModelCreating(modelBuilder);
        }
    }
}
