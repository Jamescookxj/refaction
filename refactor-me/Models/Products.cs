using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
 
namespace refactor_me.Models
{   
    // Product data access class
    public class Products
    {
        public List<Product> Items { get; private set; }

        public   Products()
        {}

        //1. Transfer data from a SqlDataReader to a product list
        public List<Product> DataReaderToProductLsit(SqlDataReader datareader)
        {
            List<Product> resultdata = new List<Product>();
            while (datareader.Read())
            {
                Product item = new Product(0);
                item.Id = Guid.Parse(datareader["Id"].ToString());
                item.Name = datareader["Name"].ToString();
                item.Description = (DBNull.Value == datareader["Description"]) ? null : datareader["Description"].ToString();
                item.Price = decimal.Parse(datareader["Price"].ToString());
                item.DeliveryPrice = decimal.Parse(datareader["DeliveryPrice"].ToString());
                resultdata.Add(item);
            }
            return resultdata;
        }
        public List<ProductOption> DataReaderToProductOptionList(SqlDataReader datareader)
        {
            List<ProductOption> resultdata = new List<ProductOption>();
            while (datareader.Read())
            {
                ProductOption item = new ProductOption();
                item.ProductId = Guid.Parse(datareader["ProductId"].ToString());
                item.Id= Guid.Parse(datareader["Id"].ToString());
                item.Name = datareader["Name"].ToString();
                item.Description = (DBNull.Value == datareader["Description"]) ? null : datareader["Description"].ToString();               
                resultdata.Add(item);
            }
            return resultdata;
        }

        //2. Get all product records
        public List<Product> GetAllProducts()
        {          
            DAL dal = new DAL();
            SqlDataReader datareader = dal.QueryAllFromAtable("product");
            List<Product> resultdata = new List<Product>();
            resultdata= DataReaderToProductLsit(datareader);
            return resultdata;
        }

        //3. Query particular data with 'name' field's value  
        public List<Product> GetProductsByName(string name)
        {
            DAL dal = new DAL();
            SqlDataReader datareader= dal.QueryAtableByAFieldValue("product", "Name", name);
            List<Product> resultdata = new List<Product>();
            resultdata = DataReaderToProductLsit(datareader);
            return resultdata;
        }

        //4. Query particular data with id value  
        public List<Product> QueryTableByID(Guid idvalue)
        {
            DAL dal = new DAL();
            SqlDataReader datareader = dal.QueryTableByID("product",idvalue);
            List<Product> resultdata = new List<Product>();
            resultdata = DataReaderToProductLsit(datareader);
            return resultdata;
        }

        //5. Insert a new product
        public int InsertNewProduct(Product product)
        {
            DAL dal = new DAL();
            Guid ids =   Guid.NewGuid();
            int res;
            if (product.Name != null && product.Name.Trim().Length>0)
            {
                string valuestring ="'" +ids.ToString() + "','" + product.Name + "','" + product.Description + "','" + product.Price + "','" + product.DeliveryPrice+"'";
                res = dal.InsertARecord("product", "id, name, description, price, deliveryprice", valuestring);
            }
            else res = 0;            
            return res;
        }

        //6. Update a new product
        public int UpdateProduct(Product product)
        {
            DAL dal = new DAL();
            Guid ids = Guid.NewGuid();
            int res=0;
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            myDic.Add("name",  product.Name.ToString());
            myDic.Add("description", product.Description.ToString());
            myDic.Add("price", product.Price.ToString());
            myDic.Add("deliveryprice", product.DeliveryPrice.ToString());
            res = dal.UpdateARecord("product", product.Id.ToString(), myDic);
            return res;
        }

        //7. Delete a product
        public int DeteteAProduct(Guid id)
        {
            DAL dal = new DAL();            
            int res = 0;
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            myDic.Add("lower(ProductId)=", id.ToString().ToLower().Trim());
            dal.DeleteRecords("productoption", myDic);
            res = dal.DeleteARecord("product", id.ToString());
            return res;
        }

        //8. Return all options of  a product with product id
        public List<ProductOption> ReturnProductOptions(Guid productId)
        {
            DAL dal = new DAL();
            SqlDataReader datareader = dal.QueryAtableByAFieldValue("ProductOption", "ProductId", productId.ToString());
            List<ProductOption> resultdata = new List<ProductOption>();
            resultdata = DataReaderToProductOptionList(datareader);
            return resultdata;
        }

        //9. Finds the specified product option for the specified product
        public ProductOption ReturnProductOptionById(Guid productId, Guid id)
        {
            DAL dal = new DAL();
            Dictionary<string, string> conditiondic = new Dictionary<string, string>();
            conditiondic.Add("lower(ProductId)=", "lower('" + productId.ToString()+"')");
            conditiondic.Add("lower(Id)=", "lower('" + id.ToString() + "')");
            SqlDataReader datareader = dal.QueryRecordsByCondition("ProductOption", "ProductId", conditiondic);
            List<ProductOption> resultdata = new List<ProductOption>();
            resultdata = DataReaderToProductOptionList(datareader);
            if (resultdata.Count>0)
            {
                return resultdata[0];
            }          
            return null;
        }

        //10. Add a new product option
        public int InsertNewProductOption(ProductOption productoption)
        {
            DAL dal = new DAL();
            Guid ids = Guid.NewGuid();
            int res;
            if (productoption.Name != null && productoption.Name.Trim().Length > 0)
            {
                string valuestring = "'" + ids.ToString() + "','" + productoption.ProductId + "','" + productoption.Name + "','" + productoption.Description + "'";
                res = dal.InsertARecord("ProductOption", "id,ProductId , name, description", valuestring);
            }
            else res = 0;
            return res;
        }

        //11. Update a new product option
        public int UpdateProductOption(ProductOption option)
        {
            DAL dal = new DAL();
            Guid ids = Guid.NewGuid();
            int res = 0;
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            myDic.Add("Name", option.Name.ToString());
            myDic.Add("Description", option.Description.ToString());
           // myDic.Add("id", option.Id.ToString());
           // myDic.Add("productid", option.ProductId.ToString());
            res = dal.UpdateARecord("productoption", option.Id.ToString(), myDic);
            return res;
        }

        //12. Delete a product option
        public int DeteteAProductOption(Guid id)
        {
            DAL dal = new DAL();
            int res = 0;
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            myDic.Add("lower(Id)=", "'"+id.ToString().ToLower().Trim()+"'");
            res= dal.DeleteRecords("productoption", myDic);            
            return res;
        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        public Guid GetNewProductID()
        {
             return Guid.NewGuid();
        }

        public Product(int IsNewInput)
        {
            if (IsNewInput == 0) { IsNew = true; }
            else if(IsNewInput == 1) { IsNew = false; }         
        }

        public Product(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            Price = decimal.Parse(rdr["Price"].ToString());
            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew ? 
                new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})", conn) : 
                new SqlCommand($"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from product where id = '{Id}'", conn);
            cmd.ExecuteNonQuery();
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            if (productId.ToString().Trim().Length == 36)
            {
                LoadProductOptions($"where productid = '{productId}'");
            }
           
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
        }


    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew ?
                new SqlCommand($"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')", conn) :
                new SqlCommand($"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from productoption where id = '{Id}'", conn);
            cmd.ExecuteReader();
        }
    }
}